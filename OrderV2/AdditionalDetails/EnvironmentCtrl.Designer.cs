namespace OrderV2.AdditionalDetails
{
    partial class EnvironmentCtrl
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
            this.ddlPriority = new Telerik.WinControls.UI.RadDropDownList();
            this.ddlSamplingType = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.label1 = new Telerik.WinControls.UI.RadLabel();
            this.label2 = new Telerik.WinControls.UI.RadLabel();
            this.startDateTimePicker = new Telerik.WinControls.UI.RadDateTimePicker();
            this.endDateTimePicker = new Telerik.WinControls.UI.RadDateTimePicker();
            this.spinRecivedTemperature = new Telerik.WinControls.UI.RadSpinEditor();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            this.lblHeader = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ddlPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlSamplingType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateTimePicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDateTimePicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinRecivedTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // ddlPriority
            // 
            this.ddlPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlPriority.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlPriority.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ddlPriority.Location = new System.Drawing.Point(64, 106);
            this.ddlPriority.Name = "ddlPriority";
            this.ddlPriority.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ddlPriority.Size = new System.Drawing.Size(200, 20);
            this.ddlPriority.TabIndex = 87;
            this.ddlPriority.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // ddlSamplingType
            // 
            this.ddlSamplingType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSamplingType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlSamplingType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ddlSamplingType.Location = new System.Drawing.Point(64, 173);
            this.ddlSamplingType.Name = "ddlSamplingType";
            this.ddlSamplingType.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ddlSamplingType.Size = new System.Drawing.Size(200, 20);
            this.ddlSamplingType.TabIndex = 88;
            this.ddlSamplingType.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel1.AutoSize = true;
            this.radLabel1.Location = new System.Drawing.Point(383, 106);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(51, 18);
            this.radLabel1.TabIndex = 89;
            this.radLabel1.Text = ": דחיפות ";
            // 
            // radLabel2
            // 
            this.radLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel2.AutoSize = true;
            this.radLabel2.Location = new System.Drawing.Point(372, 172);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(59, 18);
            this.radLabel2.TabIndex = 90;
            this.radLabel2.Text = ": סוג דיגום";
            // 
            // radLabel5
            // 
            this.radLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel5.AutoSize = true;
            this.radLabel5.Location = new System.Drawing.Point(328, 304);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radLabel5.Size = new System.Drawing.Size(100, 18);
            this.radLabel5.TabIndex = 95;
            this.radLabel5.Text = "תאריך סיום דיגום :";
            this.radLabel5.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(314, 238);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(113, 18);
            this.label1.TabIndex = 91;
            this.label1.Text = "תאריך התחלת דיגום :";
            this.label1.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(332, 370);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(96, 18);
            this.label2.TabIndex = 93;
            this.label2.Text = "טמפרטורת קבלה :";
            this.label2.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startDateTimePicker.Location = new System.Drawing.Point(64, 239);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.startDateTimePicker.TabIndex = 96;
            this.startDateTimePicker.TabStop = false;
            this.startDateTimePicker.Value = new System.DateTime(((long)(0)));
            this.startDateTimePicker.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endDateTimePicker.Location = new System.Drawing.Point(64, 305);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.endDateTimePicker.TabIndex = 97;
            this.endDateTimePicker.TabStop = false;
            this.endDateTimePicker.Value = new System.DateTime(((long)(0)));
            this.endDateTimePicker.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // spinRecivedTemperature
            // 
            this.spinRecivedTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.spinRecivedTemperature.DecimalPlaces = 1;
            this.spinRecivedTemperature.Location = new System.Drawing.Point(64, 371);
            this.spinRecivedTemperature.Maximum = new decimal(new int[] {
            -2072745074,
            0,
            0,
            0});
            this.spinRecivedTemperature.Name = "spinRecivedTemperature";
            this.spinRecivedTemperature.ShowUpDownButtons = false;
            this.spinRecivedTemperature.Size = new System.Drawing.Size(200, 20);
            this.spinRecivedTemperature.TabIndex = 98;
            this.spinRecivedTemperature.TabStop = false;
            this.spinRecivedTemperature.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(30, 475);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 24);
            this.btnClose.TabIndex = 99;
            this.btnClose.Text = "סגור";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("David", 20F);
            this.lblHeader.ForeColor = System.Drawing.Color.Blue;
            this.lblHeader.Location = new System.Drawing.Point(140, 17);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblHeader.Size = new System.Drawing.Size(154, 32);
            this.lblHeader.TabIndex = 100;
            this.lblHeader.Text = "פרטים נוספים";
            this.lblHeader.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // EnvironmentCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.spinRecivedTemperature);
            this.Controls.Add(this.startDateTimePicker);
            this.Controls.Add(this.endDateTimePicker);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.ddlSamplingType);
            this.Controls.Add(this.ddlPriority);
            this.Name = "EnvironmentCtrl";
            this.Size = new System.Drawing.Size(456, 518);
            ((System.ComponentModel.ISupportInitialize)(this.ddlPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlSamplingType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDateTimePicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDateTimePicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinRecivedTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadDropDownList ddlPriority;
        private Telerik.WinControls.UI.RadDropDownList ddlSamplingType;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel label1;
        private Telerik.WinControls.UI.RadLabel label2;
        private Telerik.WinControls.UI.RadDateTimePicker startDateTimePicker;
        private Telerik.WinControls.UI.RadDateTimePicker endDateTimePicker;
        private Telerik.WinControls.UI.RadSpinEditor spinRecivedTemperature;
        private Telerik.WinControls.UI.RadButton btnClose;
        private Telerik.WinControls.UI.RadLabel lblHeader;

    }
}
