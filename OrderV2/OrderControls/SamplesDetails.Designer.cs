using Telerik.WinControls.UI;

namespace OrderV2.OrderControls
{
    partial class SamplesDetails
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
            this.groupBox3 = new Telerik.WinControls.UI.RadGroupBox();
            this.btnDeleteXml = new Telerik.WinControls.UI.RadButton();
            this.gridSamples = new Telerik.WinControls.UI.RadGridView();
            this.btnPrintAll = new Telerik.WinControls.UI.RadButton();
            this.btnAssociationTests = new Telerik.WinControls.UI.RadButton();
            this.btnChangeXml = new Telerik.WinControls.UI.RadButton();
            this.btnChooseFields = new Telerik.WinControls.UI.RadButton();
            this.spinCount = new Telerik.WinControls.UI.RadSpinEditor();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox3)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteXml)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSamples)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSamples.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrintAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAssociationTests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChangeXml)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChooseFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinCount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.groupBox3.Controls.Add(this.btnDeleteXml);
            this.groupBox3.Controls.Add(this.gridSamples);
            this.groupBox3.Controls.Add(this.btnPrintAll);
            this.groupBox3.Controls.Add(this.btnAssociationTests);
            this.groupBox3.Controls.Add(this.btnChangeXml);
            this.groupBox3.Controls.Add(this.btnChooseFields);
            this.groupBox3.Controls.Add(this.spinCount);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox3.GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Office;
            this.groupBox3.HeaderText = "פרטי דוגמאות:";
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 22, 3, 2);
            this.groupBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // 
            // 
            this.groupBox3.RootElement.Padding = new System.Windows.Forms.Padding(3, 22, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(1624, 404);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "פרטי דוגמאות:";
            // 
            // btnDeleteXml
            // 
            this.btnDeleteXml.Enabled = false;
            this.btnDeleteXml.Location = new System.Drawing.Point(204, 28);
            this.btnDeleteXml.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteXml.Name = "btnDeleteXml";
            this.btnDeleteXml.Size = new System.Drawing.Size(147, 30);
            this.btnDeleteXml.TabIndex = 36;
            this.btnDeleteXml.Text = "מחק   XML";
            this.btnDeleteXml.EnabledChanged += new System.EventHandler(this.btnDeleteXml_EnabledChanged);
            this.btnDeleteXml.Click += new System.EventHandler(this.btnDeleteLayoutXml_Click);
            // 
            // gridSamples
            // 
            this.gridSamples.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSamples.Location = new System.Drawing.Point(7, 63);
            this.gridSamples.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            // 
            // gridSamples
            // 
            this.gridSamples.MasterTemplate.AddNewRowPosition = Telerik.WinControls.UI.SystemRowPosition.Bottom;
            this.gridSamples.MasterTemplate.AllowDeleteRow = false;
            this.gridSamples.MasterTemplate.EnableSorting = false;
            this.gridSamples.Name = "gridSamples";
            this.gridSamples.Size = new System.Drawing.Size(1611, 341);
            this.gridSamples.TabIndex = 35;
            this.gridSamples.RowFormatting += new Telerik.WinControls.UI.RowFormattingEventHandler(this.gridSamples_RowFormatting);
            this.gridSamples.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.gridSamples_CellFormatting);
            this.gridSamples.CellEditorInitialized += new Telerik.WinControls.UI.GridViewCellEventHandler(this.GridSamples_CellEditorInitialized);
            this.gridSamples.CellEndEdit += new Telerik.WinControls.UI.GridViewCellEventHandler(this.gridSamples_CellEndEdit);
            this.gridSamples.ValueChanged += new System.EventHandler(this.gridSamples_ValueChanged);
            this.gridSamples.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.gridSamples_UserAddedRow);
            this.gridSamples.UserDeletingRow += new Telerik.WinControls.UI.GridViewRowCancelEventHandler(this.gridSamples_UserDeletingRow);
            this.gridSamples.UserDeletedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.gridSamples_UserDeletedRow);
            this.gridSamples.CommandCellClick += new Telerik.WinControls.UI.CommandCellClickEventHandler(this.gridSamples_CommandCellClick);
            this.gridSamples.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.gridSamples_ContextMenuOpening);
            this.gridSamples.LayoutLoaded += new Telerik.WinControls.UI.LayoutLoadedEventHandler(this.gridSamples_LayoutLoaded);
            // 
            // btnPrintAll
            // 
            this.btnPrintAll.Enabled = false;
            this.btnPrintAll.Location = new System.Drawing.Point(32, 28);
            this.btnPrintAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPrintAll.Name = "btnPrintAll";
            this.btnPrintAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnPrintAll.Size = new System.Drawing.Size(147, 30);
            this.btnPrintAll.TabIndex = 34;
            this.btnPrintAll.Text = "הדפס הכל";
            this.btnPrintAll.Click += new System.EventHandler(this.btnPrintAll_Click);
            // 
            // btnAssociationTests
            // 
            this.btnAssociationTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssociationTests.Enabled = false;
            this.btnAssociationTests.Location = new System.Drawing.Point(985, 26);
            this.btnAssociationTests.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAssociationTests.Name = "btnAssociationTests";
            this.btnAssociationTests.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnAssociationTests.Size = new System.Drawing.Size(147, 30);
            this.btnAssociationTests.TabIndex = 34;
            this.btnAssociationTests.Text = "שיוך בדיקות";
            this.btnAssociationTests.Click += new System.EventHandler(this.btnAssociationTests_Click);
            // 
            // btnChangeXml
            // 
            this.btnChangeXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeXml.Location = new System.Drawing.Point(985, 26);
            this.btnChangeXml.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChangeXml.Name = "btnChangeXml";
            this.btnChangeXml.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnChangeXml.Size = new System.Drawing.Size(147, 30);
            this.btnChangeXml.TabIndex = 34;
            this.btnChangeXml.Text = "שיוך בדיקות";
            // 
            // btnChooseFields
            // 
            this.btnChooseFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseFields.Location = new System.Drawing.Point(1168, 26);
            this.btnChooseFields.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChooseFields.Name = "btnChooseFields";
            this.btnChooseFields.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnChooseFields.Size = new System.Drawing.Size(147, 30);
            this.btnChooseFields.TabIndex = 34;
            this.btnChooseFields.Text = "בחירת שדות";
            this.btnChooseFields.Click += new System.EventHandler(this.btnChooseFields_Click);
            // 
            // spinCount
            // 
            this.spinCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.spinCount.Location = new System.Drawing.Point(1351, 31);
            this.spinCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spinCount.Name = "spinCount";
            this.spinCount.ReadOnly = true;
            this.spinCount.Size = new System.Drawing.Size(60, 24);
            this.spinCount.TabIndex = 21;
            this.spinCount.TabStop = false;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label16.Location = new System.Drawing.Point(1455, 33);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label16.Size = new System.Drawing.Size(88, 17);
            this.label16.TabIndex = 20;
            this.label16.Text = "כמות דוגמאות:";
            // 
            // SamplesDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SamplesDetails";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1624, 404);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox3)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteXml)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSamples.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSamples)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrintAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAssociationTests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChangeXml)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChooseFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinCount)).EndInit();
            this.ResumeLayout(false);

        }

     

     

        #endregion

        private RadGroupBox groupBox3;
        private Telerik.WinControls.UI.RadButton btnAssociationTests;
        private Telerik.WinControls.UI.RadButton btnChangeXml;

        private Telerik.WinControls.UI.RadButton btnChooseFields;
        private Telerik.WinControls.UI.RadSpinEditor spinCount;
        private System.Windows.Forms.Label label16;
        private RadButton btnPrintAll;
        private RadGridView gridSamples;
        private RadButton btnDeleteXml;


    }
}
