using ActiveDirectorySearch.Searches;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace ActiveDirectorySearch
{
    public partial class Main : Form
    {
        private IContainer components = null;
        internal ToolTip ToolTip1;
        internal ErrorProvider ErrorProvider1;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.dgvAD = new System.Windows.Forms.DataGridView();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ErrorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.cboSearchType = new System.Windows.Forms.ComboBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.cboDomain = new System.Windows.Forms.ComboBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer4
            // 
            resources.ApplyResources(this.splitContainer4, "splitContainer4");
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.tbFilter);
            this.splitContainer4.Panel1.Controls.Add(this.Label5);
            this.splitContainer4.Panel1.Controls.Add(this.dgvAD);
            this.splitContainer4.Panel1.Controls.Add(this.lblResultCount);
            // 
            // splitContainer4.Panel2
            // 
            resources.ApplyResources(this.splitContainer4.Panel2, "splitContainer4.Panel2");
            // 
            // tbFilter
            // 
            resources.ApplyResources(this.tbFilter, "tbFilter");
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.TextChanged += new System.EventHandler(this.FilterTextChanged);
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.Name = "Label5";
            // 
            // dgvAD
            // 
            this.dgvAD.AllowUserToAddRows = false;
            resources.ApplyResources(this.dgvAD, "dgvAD");
            this.dgvAD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAD.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAD.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAD.Name = "dgvAD";
            this.dgvAD.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAD.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAD.TabStop = false;
            this.dgvAD.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvAdCellMouseDown);
            // 
            // lblResultCount
            // 
            resources.ApplyResources(this.lblResultCount, "lblResultCount");
            this.lblResultCount.Name = "lblResultCount";
            // 
            // ToolTip1
            // 
            this.ToolTip1.ShowAlways = true;
            this.ToolTip1.Tag = "";
            // 
            // ErrorProvider1
            // 
            this.ErrorProvider1.ContainerControl = this;
            // 
            // splitContainer3
            // 
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.cboSearchType);
            this.splitContainer3.Panel1.Controls.Add(this.GroupBox1);
            this.splitContainer3.Panel1.Controls.Add(this.btnHelp);
            this.splitContainer3.Panel1.Controls.Add(this.btnCheck);
            this.splitContainer3.Panel1.Controls.Add(this.GroupBox2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            // 
            // cboSearchType
            // 
            resources.ApplyResources(this.cboSearchType, "cboSearchType");
            this.cboSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchType.FormattingEnabled = true;
            this.cboSearchType.Name = "cboSearchType";
            this.cboSearchType.SelectedIndexChanged += new System.EventHandler(this.SearchTypeChanged);
            // 
            // GroupBox1
            // 
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // btnHelp
            // 
            resources.ApplyResources(this.btnHelp, "btnHelp");
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.HelpClick);
            // 
            // btnCheck
            // 
            resources.ApplyResources(this.btnCheck, "btnCheck");
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.CheckClick);
            // 
            // GroupBox2
            // 
            resources.ApplyResources(this.GroupBox2, "GroupBox2");
            this.GroupBox2.Controls.Add(this.cboDomain);
            this.GroupBox2.Controls.Add(this.lblPassword);
            this.GroupBox2.Controls.Add(this.Label2);
            this.GroupBox2.Controls.Add(this.tbPassword);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.TabStop = false;
            // 
            // cboDomain
            // 
            this.cboDomain.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDomain.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDomain.FormattingEnabled = true;
            resources.ApplyResources(this.cboDomain, "cboDomain");
            this.cboDomain.Name = "cboDomain";
            this.cboDomain.TextChanged += new System.EventHandler(this.DomainTextChanged);
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            // 
            // Label2
            // 
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.Name = "Label2";
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // Main
            // 
            this.AcceptButton = this.btnCheck;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer3);
            this.Name = "Main";
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        private SplitContainer splitContainer3;
        internal GroupBox GroupBox1;
        internal Button btnHelp;
        internal Button btnCheck;
        internal GroupBox GroupBox2;
        internal ComboBox cboDomain;
        internal Label lblPassword;
        internal Label Label2;
        internal TextBox tbPassword;
        private SplitContainer splitContainer4;
        internal Label lblResultCount;
        internal DataGridView dgvAD;
        internal TextBox tbFilter;
        internal Label Label5;
        internal ComboBox cboSearchType;
    }
}
