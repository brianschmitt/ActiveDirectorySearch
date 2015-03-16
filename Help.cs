namespace ActiveDirectorySearch
{
    using System;
    using System.Windows.Forms;

    public partial class Help
    {
        public Help()
        {
            InitializeComponent();
            lblHelp.Text = Resources.HelpText;
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
