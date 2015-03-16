
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace ActiveDirectorySearch
{
    public partial class Help : Form
    {
        private IContainer components = null;
        internal Button OK_Button;
        internal Label lblHelp;
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
            OK_Button = new System.Windows.Forms.Button();
            lblHelp = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // OK_Button
            // 
            OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            OK_Button.Location = new System.Drawing.Point(609, 439);
            OK_Button.Name = "OK_Button";
            OK_Button.Size = new System.Drawing.Size(67, 23);
            OK_Button.TabIndex = 2;
            OK_Button.Text = "OK";
            OK_Button.Click += new System.EventHandler(OkButtonClick);
            // 
            // lblHelp
            // 
            lblHelp.BackColor = System.Drawing.SystemColors.Window;
            lblHelp.Location = new System.Drawing.Point(12, 9);
            lblHelp.Name = "lblHelp";
            lblHelp.Size = new System.Drawing.Size(669, 424);
            lblHelp.TabIndex = 3;
            // 
            // Help
            // 
            AcceptButton = OK_Button;
            ClientSize = new System.Drawing.Size(688, 472);
            Controls.Add(OK_Button);
            Controls.Add(lblHelp);
            Name = "Help";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Help";
            ResumeLayout(false);

        }
        
    }
}
