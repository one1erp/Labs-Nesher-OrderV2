using DAL;
using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;
using System.Windows.Forms;

namespace OrderV2.Controls
{
    partial class FCS_Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FCS_Window
            // 
            this.ClientSize = new System.Drawing.Size(508, 420);
            this.Name = "FCS_Window";
            this.Text = "סריקת טופס ממשרד הבריאות";
            this.Load += new System.EventHandler(this.FCS_Window_Load);
            this.ResumeLayout(false);

        }

        #endregion

    
        

    }
}