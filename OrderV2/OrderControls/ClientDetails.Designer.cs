namespace OrderV2.OrderControls
{
    partial class ClientDetails
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.txtEmail = new Telerik.WinControls.UI.RadTextBox();
            this.txtContactPhone = new Telerik.WinControls.UI.RadTextBox();
            this.txtClientPhone = new Telerik.WinControls.UI.RadTextBox();
            this.txtContactName = new Telerik.WinControls.UI.RadTextBox();
            this.txtAddress = new Telerik.WinControls.UI.RadTextBox();
            this.dropDownClients = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropDownClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.txtEmail);
            this.radGroupBox1.Controls.Add(this.txtContactPhone);
            this.radGroupBox1.Controls.Add(this.txtClientPhone);
            this.radGroupBox1.Controls.Add(this.txtContactName);
            this.radGroupBox1.Controls.Add(this.txtAddress);
            this.radGroupBox1.Controls.Add(this.dropDownClients);
            this.radGroupBox1.Controls.Add(this.radLabel5);
            this.radGroupBox1.Controls.Add(this.radLabel4);
            this.radGroupBox1.Controls.Add(this.radLabel3);
            this.radGroupBox1.Controls.Add(this.radLabel6);
            this.radGroupBox1.Controls.Add(this.radLabel2);
            this.radGroupBox1.Controls.Add(this.radLabel1);
            this.radGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox1.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office;
            this.radGroupBox1.HeaderText = "פרטי לקוח";
            this.radGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.radGroupBox1.Name = "radGroupBox1";
            // 
            // 
            // 
            this.radGroupBox1.RootElement.Padding = new System.Windows.Forms.Padding(2, 18, 2, 2);
            this.radGroupBox1.Size = new System.Drawing.Size(590, 140);
            this.radGroupBox1.TabIndex = 4;
            this.radGroupBox1.Text = "פרטי לקוח";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radGroupBox1.GetChildAt(0).GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.SystemColors.ControlDark;
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.Location = new System.Drawing.Point(48, 70);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(125, 20);
            this.txtEmail.TabIndex = 6;
            this.txtEmail.TabStop = false;
            this.txtEmail.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // txtContactPhone
            // 
            this.txtContactPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContactPhone.Location = new System.Drawing.Point(48, 99);
            this.txtContactPhone.Name = "txtContactPhone";
            this.txtContactPhone.Size = new System.Drawing.Size(125, 20);
            this.txtContactPhone.TabIndex = 7;
            this.txtContactPhone.TabStop = false;
            this.txtContactPhone.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // txtClientPhone
            // 
            this.txtClientPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClientPhone.Location = new System.Drawing.Point(345, 70);
            this.txtClientPhone.Name = "txtClientPhone";
            this.txtClientPhone.Size = new System.Drawing.Size(125, 20);
            this.txtClientPhone.TabIndex = 3;
            this.txtClientPhone.TabStop = false;
            this.txtClientPhone.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // txtContactName
            // 
            this.txtContactName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContactName.Location = new System.Drawing.Point(345, 99);
            this.txtContactName.Name = "txtContactName";
            this.txtContactName.Size = new System.Drawing.Size(125, 20);
            this.txtContactName.TabIndex = 4;
            this.txtContactName.TabStop = false;
            this.txtContactName.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Location = new System.Drawing.Point(48, 41);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(125, 20);
            this.txtAddress.TabIndex = 5;
            this.txtAddress.TabStop = false;
            this.txtAddress.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // dropDownClients
            // 
            this.dropDownClients.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dropDownClients.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dropDownClients.Enabled = false;
            this.dropDownClients.Location = new System.Drawing.Point(345, 41);
            this.dropDownClients.Name = "dropDownClients";
            this.dropDownClients.Size = new System.Drawing.Size(125, 22);
            this.dropDownClients.TabIndex = 2;
            this.dropDownClients.Text = "בחר לקוח";
            this.dropDownClients.PopupOpening += new System.ComponentModel.CancelEventHandler(this.dropDownClients_PopupOpening);
            this.dropDownClients.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.dropDownClients_SelectedIndexChanged);
            // 
            // radLabel5
            // 
            this.radLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel5.AutoSize = true;
            this.radLabel5.Location = new System.Drawing.Point(254, 70);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(40, 18);
            this.radLabel5.TabIndex = 1;
            this.radLabel5.Text = "אימייל";
            this.radLabel5.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel4
            // 
            this.radLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel4.AutoSize = true;
            this.radLabel4.Location = new System.Drawing.Point(255, 41);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(39, 18);
            this.radLabel4.TabIndex = 1;
            this.radLabel4.Text = "כתובת";
            this.radLabel4.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel3
            // 
            this.radLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel3.AutoSize = true;
            this.radLabel3.Location = new System.Drawing.Point(519, 99);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(50, 18);
            this.radLabel3.TabIndex = 1;
            this.radLabel3.Text = "איש קשר";
            this.radLabel3.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel6
            // 
            this.radLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel6.AutoSize = true;
            this.radLabel6.Location = new System.Drawing.Point(211, 99);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(83, 18);
            this.radLabel6.TabIndex = 1;
            this.radLabel6.Text = "טלפון איש קשר";
            this.radLabel6.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel2
            // 
            this.radLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel2.AutoSize = true;
            this.radLabel2.Location = new System.Drawing.Point(531, 70);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(38, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "טלפון ";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel1.AutoSize = true;
            this.radLabel1.Location = new System.Drawing.Point(519, 41);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(50, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "שם לקוח";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // ClientDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGroupBox1);
            this.Enabled = false;
            this.Name = "ClientDetails";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(590, 140);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropDownClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadTextBox txtContactName;
        private Telerik.WinControls.UI.RadTextBox txtAddress;
        private Telerik.WinControls.UI.RadDropDownList dropDownClients;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtContactPhone;
        private Telerik.WinControls.UI.RadTextBox txtClientPhone;
        private Telerik.WinControls.UI.RadTextBox txtEmail;
    }
}
