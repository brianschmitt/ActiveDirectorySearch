namespace ActiveDirectorySearch
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using ActiveDirectorySearch;
    using Searches;
    using System.Text;

    public partial class Main
    {
        private readonly IEnumerable<ISearch> _searchPlugins;
        private AdSearcher _activeDirectorySearcher;

        private int _gridColumn = -1;
        private int _gridRow = -1;

        private int _splitDistance = 80;

        public Main()
        {
            InitializeComponent();
            SetToolTips();
            GetDomains();

            _searchPlugins = Common.GetAvailableSearches();

            cboSearchType.DataSource = _searchPlugins.Select(s => s.Name).ToList();
            cboSearchType.SelectedIndex = cboSearchType.FindStringExact("Group Member Search");

            var contextMenuStrip = new ContextMenuStrip();
            var toolStripMenuItem = new ToolStripMenuItem("Copy");
            var toolStripMenuItem2 = new ToolStripMenuItem("Search");
            var toolStripMenuItem3 = new ToolStripMenuItem("Export CSV");

            contextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                toolStripMenuItem,
                toolStripMenuItem2,
                toolStripMenuItem3
            });

            toolStripMenuItem.Click += CopyClick;
            toolStripMenuItem2.Click += SearchClick;
            toolStripMenuItem3.Click += ExportClick;
            contextMenuStrip.ShowImageMargin = false;

            dgvAD.ContextMenuStrip = contextMenuStrip;
        }

        private void UpdateInput(ISearch searchFields)
        {
            GroupBox1.Controls.Clear();
            GroupBox1.Height = 0;
            var posY = 25;
            const int posX = 30;
            const int offset = 30;
            const int inputWidth = 320;
            var defaultInputSet = false;

            foreach (var input in searchFields.Terms.Where(s => s.Visible))
            {
                var label = new Label { AutoSize = true, Location = new Point(posX, posY + 3), Text = input.Label};
                GroupBox1.Controls.Add(label);

                var searchInput = new ComboBox
                    {
                        Location = new Point(posX + label.Width, posY),
                        Size = new Size(inputWidth, posY),
                        Name = input.Id
                    };
                ToolTip1.SetToolTip(searchInput, Resources.HelpSearch);
                GroupBox1.Controls.Add(searchInput);

                var contextMenuStrip = new ContextMenuStrip();
                var toolStripMenuItem = new ToolStripMenuItem("Clear Saved");

                contextMenuStrip.Items.AddRange(new ToolStripItem[]
                {
                    toolStripMenuItem
                });

                contextMenuStrip.ShowImageMargin = false;

                searchInput.ContextMenuStrip = contextMenuStrip;

                posY += offset;

                if (defaultInputSet)
                {
                    continue;
                }
                ActiveControl = searchInput;
                defaultInputSet = true;
            }

            foreach (var checkBox in searchFields.Options.Where(s => s.Visible).Select(input => new CheckBox
                {
                    AutoSize = true,
                    Location = new Point(posX, posY),
                    Text = input.Label,
                    Name = input.Id,
                    UseVisualStyleBackColor = true
                }))
            {
                posY += offset;
                GroupBox1.Controls.Add(checkBox);
            }

            _splitDistance = GroupBox1.Height + offset + 5;
            if (_splitDistance < 80)
            {
                _splitDistance = 80;
            }

            splitContainer3.SplitterDistance = _splitDistance;
        }

        private static void AdsBadLogin(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.InvalidPassword, Resources.BadLogin, MessageBoxButtons.OK);
        }

        private void HelpClick(object sender, EventArgs e)
        {
            var help = new Help();
            help.Show();
        }

        private void DgvAdCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            _gridRow = e.RowIndex;
            _gridColumn = e.ColumnIndex;
        }

        private void CopyClick(object sender, EventArgs e)
        {
            if (dgvAD.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                Clipboard.SetDataObject(dgvAD.GetClipboardContent());
            }
        }

        private void SearchClick(object sender, EventArgs e)
        {
            if (_gridRow < 0 || _gridColumn < 0)
            {
                return;
            }
            foreach (var ctrl in GroupBox1.Controls.Cast<Control>().Where(ctrl => ctrl.GetType() == typeof(ComboBox)))
            {
                ctrl.Text = dgvAD.Rows[_gridRow].Cells[_gridColumn].Value.ToString();
                break;
            }

            CheckClick(sender, e);
        }

        private void ExportClick(object sender, EventArgs e)
        {
            const string delimt = ",";
            var sb = new StringBuilder();

            var headers = dgvAD.Columns.Cast<DataGridViewColumn>();
            sb.AppendLine(string.Join(delimt, headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));

            foreach (DataGridViewRow row in dgvAD.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>();
                sb.AppendLine(string.Join(delimt, cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
            }

            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = Resources.Main_ExportClick_CSV_files,
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, sb.ToString());
            }
        }

        private void CheckClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ClearValues();
            var search = _searchPlugins.FirstOrDefault(s => s.Name == cboSearchType.Text);
            if (search != null)
            {
                foreach (var item in search.Terms)
                {
                    item.Value = GroupBox1.Controls.Find(item.Id, false).First().Text;
                }

                foreach (var item in search.Options)
                {
                    var control = (CheckBox)GroupBox1.Controls.Find(item.Id, false).First();
                    item.Value = control.Checked;
                }

                if (search.IsValid() && DomainIsValid())
                {
                    if (_activeDirectorySearcher == null)
                    {
                        _activeDirectorySearcher = new AdSearcher(cboDomain.Text, Environment.UserName, tbPassword.Text);
                        _activeDirectorySearcher.BadLogOn += AdsBadLogin;
                    }

                    search.Search(_activeDirectorySearcher);
                    BindData();

                    var genAttrib = GetGeneralAdAttributesForUser();
                    ShowMessage(genAttrib);
                }
                else
                {
                    if (search.Errors.Any())
                    {
                        var msg = string.Join(Environment.NewLine, search.Errors.ToArray());
                        MessageBox.Show(msg, Resources.Main_CheckClick_Error, MessageBoxButtons.OK);
                    }
                }
            }

            Cursor = Cursors.Arrow;
        }

        private bool DomainIsValid()
        {
            if (!(cboDomain.Text != Environment.UserDomainName
                  & string.IsNullOrWhiteSpace(tbPassword.Text)))
            {
                return true;
            }

            ErrorProvider1.SetError(tbPassword, string.Format(Resources.ErrorPassword, Environment.UserDomainName));
            return false;
        }

        private void FilterTextChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            var search = _searchPlugins.FirstOrDefault(s => s.Name == cboSearchType.Text);
            if (search != null && search.Results != null && search.Results.Any())
            {
                dgvAD.DataSource = (
                     from r in search.Results.Filter(tbFilter.Text)
                     select r).ToList();
            }

            lblResultCount.Text = string.Format("{0} Results", dgvAD.RowCount);
        }

        private void SetToolTips()
        {
            ToolTip1.SetToolTip(tbFilter, Resources.HelpFilter);
            ToolTip1.SetToolTip(cboDomain, Resources.HelpDomain);
            ToolTip1.SetToolTip(tbPassword, string.Format(Resources.HelpPassword, Environment.UserDomainName));
        }

        private void GetDomains()
        {
            cboDomain.DataSource = Common.GetDomains();
            cboDomain.Text = Environment.UserDomainName;
        }

        private void ClearValues()
        {
            ErrorProvider1.Clear();
            dgvAD.DataSource = null;
            dgvAD.Refresh();
            splitContainer4.Panel2.Controls.Clear();
        }

        private void ShowMessage(IEnumerable<dynamic> searchResult)
        {
            splitContainer4.Panel2.Controls.Clear();

            if (searchResult == null)
            {
                return;
            }

            var userDetails = searchResult;

            const int posX = 0;
            var posY = 0;
            const int textBoxHeight = 210;
            const int textBoxWidth = 230;
            foreach (var user in userDetails.GroupBy(u => u.Parent))
            {
                var user1 = user.FirstOrDefault();
                var results = searchResult
                               .Where(s => user1 != null && s.Parent == user1.Parent)
                               .Select(r => string.Format("{0}: {1}", r.PropertyName, r.CommonName));

                var msg = string.Join(Environment.NewLine, results.ToArray());

                var userDetailsText = new TextBox
                    {
                        Width = textBoxWidth,
                        Height = textBoxHeight,
                        Location = new Point(posX, posY),
                        Multiline = true,
                        Name = "genericDetails",
                        Text = msg
                    };

                splitContainer4.Panel2.Controls.Add(userDetailsText);

                var prop = searchResult.FirstOrDefault(r => user1 != null && (r.Parent == user1.Parent && r.PropertyName == "thumbnailphoto"));
                if (prop != null)
                {
                    var ba = (byte[])prop.Value;
                    var img = Converter.ToImage(ba);
                    var picBox = new PictureBox
                    {
                        Location = new Point(textBoxWidth, posY),
                        SizeMode = PictureBoxSizeMode.AutoSize,
                        Name = prop.CommonName,
                        Image = img
                    };
                    splitContainer4.Panel2.Controls.Add(picBox);
                }

                posY += textBoxHeight;
            }
        }

        private IEnumerable<dynamic> GetGeneralAdAttributesForUser()
        {
            var searchResult = _searchPlugins.FirstOrDefault(s => s.Name == "User Detail");
            var searchTerm = string.Empty;
            foreach (var item in searchResult.Terms.Where(t => t.Visible = true))
            {
                foreach (var ctrl in GroupBox1.Controls.Cast<Control>().Where(ctrl => ctrl.GetType() == typeof(ComboBox)))
                {
                    searchTerm = ctrl.Text;
                    break;
                }

                item.Value = searchTerm;
            }

            searchResult.Search(_activeDirectorySearcher);
            return searchResult.Results;
        }

        private void DomainTextChanged(object sender, EventArgs e)
        {
            _activeDirectorySearcher = null;
            var visible = cboDomain.Text.ToUpper() != Environment.UserDomainName;
            lblPassword.Visible = visible;
            tbPassword.Visible = visible;
        }

        private void SearchTypeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboSearchType.Text))
            {
                return;
            }

            var search = _searchPlugins.Where(s => s.Name == cboSearchType.Text);
            UpdateInput(search.First());
            ClearValues();
        }
    }
}
