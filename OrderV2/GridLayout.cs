using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using DAL;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace OrderV2
{
    internal class GridLayout
    {
        private readonly ListDatas _listData;
        public event Action<string, string> RemoveFromCOA;
        private const string productCategoy = "Category";


        public GridLayout(ListDatas listData)
        {
            _listData = listData;
        }

        /// <summary>
        ///Save layout and convert it to string
        /// </summary>
        /// <param name="gridView"></param>
        /// <returns></returns>
        public byte[] SaveLayout(RadGridView gridView)
        {


            //     return Common.UiHelperMethods.ConvertGridDesignToString(gridView);
            return Common.UiHelperMethods.ConvertGridToByteArrray(gridView);

        }

        /// <summary>
        /// Load layout from telerik xml
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gridView"></param>
        internal void LoadLayout(string value, RadGridView gridView)
        {


            Common.UiHelperMethods.LoadGridLayout(value, gridView);

        }
        internal void LoadLayout(byte[] value, RadGridView gridView)
        {


            Common.UiHelperMethods.LoadGridLayout(value, gridView);

        }

        /// <summary>
        /// Build grid Columns
        /// </summary>
        /// <param name="gridSamples">Specified grid</param>
        internal void BuildCoulmns(RadGridView gridSamples)
        {
            gridSamples.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;

            //אסור לשנות את שמות העמודות 
            //הם מקבילים לשמות משתנים ב
            //sample partial class

            // Build columns where not saved xml per client


            var autoIncrement = new GridViewTextBoxColumn();
            autoIncrement.Name = "AutoIncrement";
            autoIncrement.HeaderText = "מספר סידורי";
            autoIncrement.AllowResize = false;
            autoIncrement.ReadOnly = true;
            autoIncrement.IsPinned = true;
            autoIncrement.Width = 44;
            gridSamples.Columns.Add(autoIncrement);


            var sampleName = new GridViewTextBoxColumn();
            sampleName.Name = "Name";
            sampleName.HeaderText = "מספר פנימי";
            sampleName.ReadOnly = true;
            gridSamples.Columns.Add(sampleName);


            var sampleDescription = new GridViewTextBoxColumn();
            sampleDescription.Name = "Description";
            sampleDescription.HeaderText = "תיאור";
            sampleDescription.ReadOnly = false;
            gridSamples.Columns.Add(sampleDescription);

            var products = new GridViewComboBoxColumn();
            products.Name = "ProductName";
            products.HeaderText = "מוצר";
            products.Width = 44;

            products.DisplayMember = "Name";
            products.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            products.DataSource = _listData.CategoryProducts;
            gridSamples.Columns.Add(products);
            //mandatory field
            products.AllowHide = false;


            var productDescription = new GridViewTextBoxColumn();
            productDescription.Name = "ProductDescription";
            productDescription.HeaderText = "תיאור מוצר";
            productDescription.ReadOnly = true;
            gridSamples.Columns.Add(productDescription);

            var realProducts = new GridViewComboBoxColumn();
            realProducts.Name = "RealProductName";
            realProducts.HeaderText = "שם מוצר";
            realProducts.Width = 44;

            realProducts.DisplayMember = "Name";
            realProducts.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            realProducts.DataSource = _listData.RealProducts;
            gridSamples.Columns.Add(realProducts);


            var dateProduction = new GridViewDateTimeColumn();
            dateProduction.Name = "DateProduction";
            dateProduction.HeaderText = "תאריך ייצור";
            dateProduction.FormatString = "{0:dd/MM/yyyy}";

            gridSamples.Columns.Add(dateProduction);


            var batch = new GridViewTextBoxColumn();
            batch.Name = "Batch";
            batch.HeaderText = "Batch";
            gridSamples.Columns.Add(batch);

            var temperature = new GridViewDecimalColumn();
            temperature.Minimum = -9999999;
            temperature.Maximum = 9999999999;
            temperature.Name = "SamplingTemperature";
            temperature.HeaderText = "טמפ' דיגום";
            gridSamples.Columns.Add(temperature);

            var sampledByOperator = new GridViewComboBoxColumn();
            sampledByOperator.Name = "SampledByOperatorName";
            sampledByOperator.HeaderText = "נדגם ע\"י";
            sampledByOperator.DisplayMember = "Name";

            sampledByOperator.AutoCompleteMode = AutoCompleteMode.Suggest;
            sampledByOperator.DataSource = _listData.OperatorsByRole;
            sampledByOperator.AllowHide = false;
            gridSamples.Columns.Add(sampledByOperator);

            //var sampledOn = new GridViewDateTimeColumn();
            //sampledOn.Name = "SampledOn";
            //sampledOn.HeaderText = "תאריך דיגום";
            //sampledOn.Format = DateTimePickerFormat.Custom;
            ////sampledOn.FormatString = "{0:F}";
            //gridSamples.Columns.Add(sampledOn);

            var TextualSamplingTime = new GridViewTextBoxColumn();
            TextualSamplingTime.Name = "TextualSamplingTime";
            TextualSamplingTime.HeaderText = "תאריך דיגום";
            gridSamples.Columns.Add(TextualSamplingTime);

            var TextualSamplingTime2 = new GridViewTextBoxColumn();
            TextualSamplingTime2.Name = "TextualSamplingTime2";
            TextualSamplingTime2.HeaderText = "שעת דיגום";
            gridSamples.Columns.Add(TextualSamplingTime2);

            var externalReference = new GridViewTextBoxColumn();
            externalReference.Name = "ExternalReference";
            externalReference.HeaderText = "מספור לקוח";
            externalReference.FieldName = "ExternalReference";
            gridSamples.Columns.Add(externalReference);

            BuildFcsColumns(gridSamples);

            var tabMethod = new GridViewComboBoxColumn();
            tabMethod.Name = "TabMethod";
            tabMethod.HeaderText = "שיטת בדיקה";
            tabMethod.Width = 44;

            tabMethod.DisplayMember = "PhraseName";
            tabMethod.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tabMethod.DataSource = _listData.TabMethodList;
            gridSamples.Columns.Add(tabMethod);

            var pointCode = new GridViewTextBoxColumn();
            pointCode.Name = "PointCode";
            pointCode.HeaderText = "קוד נקודה כספת";
            gridSamples.Columns.Add(pointCode);

            var comments = new GridViewTextBoxColumn();
            comments.Name = "Comments";
            comments.HeaderText = "הערות";
            gridSamples.Columns.Add(comments);

            var print = new GridViewCommandColumn();
            print.Name = "print";
            print.UseDefaultText = true;
            print.DefaultText = "הדפס";
            print.HeaderText = "הדפס";
            print.ReadOnly = true;
            print.IsVisible = false;

            gridSamples.Columns.Add(print);


            BuildSpecialColumn(gridSamples);
        }

        /// <summary>
        /// Build special Columns
        /// </summary>
        /// <param name="gridSamples">Specified grid</param>
        private void BuildSpecialColumn(RadGridView gridSamples)
        {

            // לכל לקוח יכול להיות שדות מיוחדים עבורו
            // לכך נבנה במערכת 20 שדות, 5 מכל סוג 
            // המשתמש יוכל לשנות את שם השדה בגריד, ולעדכן בו נתונים


            //סוגי השדות המיוחדים
            string[] fieldTypes = { "Bool", "Text", "Num", "Date" };

            // field כל השדות המיוחדים מתחילים ב 
            List<PropertyInfo> propertyInfo =
                typeof(Sample).GetProperties().Where(x => x.Name.StartsWith("Field")).ToList();


            foreach (string fieldType in fieldTypes)
            {
                string type = fieldType;
                PropertyInfo[] clientFields = propertyInfo.Where(x => x.Name.Contains(type)).ToArray();

                for (int i = 0; i < clientFields.Length; i++)
                {
                    //Add column according field type 
                    GridViewDataColumn newColumn = null;
                    switch (fieldType)
                    {
                        case "Bool":
                            newColumn = new GridViewCheckBoxColumn();
                            break;
                        case "Text":
                            newColumn = new GridViewTextBoxColumn();
                            break;
                        case "Date":
                            newColumn = new GridViewDateTimeColumn();
                            //Set short format
                            newColumn.FormatString = "{0:dd/MM/yyyy}";

                            break;
                        case "Num":
                            newColumn = new GridViewDecimalColumn();
                            break;
                    }
                    if (newColumn != null)
                    {

                        //Set header text
                        newColumn.HeaderText = fieldType + " " + (i + 1);

                        //Set in column chooser
                        newColumn.IsVisible = false;
                        //Set name
                        newColumn.Name = clientFields[i].Name;
                        //Add it grid
                        gridSamples.Columns.Add(newColumn);
                    }
                }
            }

            //Design columns
            SetColumnProperties(gridSamples);
            RegisterIsVisibleEvent(gridSamples);
        }

        /// <summary>
        ///Design columns         
        /// </summary>
        /// <param name="gridSamples">Specified grid</param>
        private void SetColumnProperties(RadGridView gridSamples)
        {

            foreach (GridViewDataColumn column in gridSamples.Columns)
            {

                column.MinWidth = 105;
            }
        }


        internal void RegisterIsVisibleEvent(RadGridView gridSamples)
        {

            foreach (GridViewDataColumn column in gridSamples.Columns)
            {
                column.RadPropertyChanged += column_RadPropertyChanged;

            }
        }

        void column_RadPropertyChanged(object sender, RadPropertyChangedEventArgs e)
        {
            if (String.Equals(e.Property.Name, "IsVisible"))
            {
                var s = (GridViewColumn)sender;
                if (s.Name == "print") return;
                if (s.IsVisible)
                {
                    //    MessageBox.Show(sender.ToString() + " is now being made visible");
                }
                else
                {

                    //הסרה מתעודה
                    if (RemoveFromCOA != null) RemoveFromCOA(s.Name, s.HeaderText);



                    if (s.Name.StartsWith("Field"))
                    {
                        //הורדת המילה field
                        var nn = s.Name.Remove(0, 5);
                        //הוספת רווח
                        s.HeaderText = nn.Replace(nn[nn.Length - 1].ToString(), " " + nn[nn.Length - 1]);
                    }
                    //   MessageBox.Show(sender.ToString() + " is now being made not visible");
                }
            }
        }

        /// <summary>
        /// Populate grid with data
        /// </summary>
        /// <param name="list"></param>
        /// <param name="gridSamples"></param>
        internal void PopulateData(List<Sample> list, RadGridView gridSamples)
        {
            if (gridSamples == null)
            {
                // Handle the case where gridSamples is null
                return;
            }
            foreach (Sample sample in list)
            {
                GridViewRowInfo newRow = gridSamples.Rows.AddNew();
                if (newRow != null && gridSamples != null)
                {
                    newRow.Cells["AutoIncrement"].Value = gridSamples.Rows.Count;
                    newRow.Cells["Name"].Value = sample.Name;
                    newRow.Cells["Description"].Value = sample.Description;
                    newRow.Cells["Batch"].Value = sample.Batch;
                    newRow.Cells["SamplingTemperature"].Value = sample.SamplingTemperature;
                    newRow.Cells["ExternalReference"].Value = sample.ExternalReference;
                    newRow.Cells["Comments"].Value = sample.Comments;
                    newRow.Cells["DateProduction"].Value = sample.DateProduction;
                }

               

                if (newRow.Cells["ContainerNumber"] == null)
                {
                    BuildFcsColumns(gridSamples);
                }


                if (sample.ContainerNumber != null) newRow.Cells["ContainerNumber"].Value = sample.ContainerNumber;
                if (sample.DelFileNum != null) newRow.Cells["DelFileNum"].Value = sample.DelFileNum;
                if (sample.TextualSamplingTime != null) newRow.Cells["TextualSamplingTime"].Value = sample.TextualSamplingTime;
                if (sample.TextualSamplingTime2 != null) newRow.Cells["TextualSamplingTime2"].Value = sample.TextualSamplingTime2;
                if (sample.PointCode != null) newRow.Cells["PointCode"].Value = sample.PointCode;
                //     if (sample.SampledOn != null) newRow.Cells["SampledOn"].Value = sample.SampledOn;
                if (sample.TabMethod != null) newRow.Cells["TabMethod"].Value = sample.TabMethod;
                if (sample.SampledByOperator != null) newRow.Cells["SampledByOperatorName"].Value = sample.SampledByOperator.Name;
                if (sample.RealProduct != null && newRow.Cells["RealProductName"] != null) newRow.Cells["RealProductName"].Value = sample.RealProduct.Name;

                if (sample.Product != null)
                {
                    newRow.Cells["ProductName"].Value = sample.Product.Name;
                    newRow.Cells["ProductDescription"].Value = sample.Product.Description;
                }



                //Populate special fields by reflection
                List<PropertyInfo> propertyInfo =
                    typeof(Sample).GetProperties().Where(x => x.Name.StartsWith("Field")).ToList();

                foreach (PropertyInfo info in propertyInfo)
                {
                    object newValue = info.GetValue(sample, null);
                    if (newRow.Cells[info.Name] != null)//If column exists
                    {

                        if (newRow.Cells[info.Name].ColumnInfo is GridViewCheckBoxColumn) //If is boolean
                        {
                            if (newValue != null)
                                //convert from nautilus Boolean
                                newValue = newValue.ToString() == "T";
                        }
                        //Set value
                        newRow.Cells[info.Name].Value = newValue;
                    }
                }
            }
        }

        private static void BuildFcsColumns(RadGridView gridSamples)
        {
            var containerNum = new GridViewTextBoxColumn();
            containerNum.Name = "ContainerNumber";
            containerNum.HeaderText = "מספר מכולה";
            gridSamples.Columns.Add(containerNum);

            var delFileNum = new GridViewTextBoxColumn();
            delFileNum.Name = "DelFileNum";
            delFileNum.HeaderText = "מספר תיק";
            gridSamples.Columns.Add(delFileNum);
        }


        private static object[,] CopyRows2(RadGridView radGridView)
        {
            if (radGridView.SelectedRows.Count < 1) return null;
            object[,] clipboard = new object[radGridView.SelectedRows.Count, radGridView.SelectedRows[0].Cells.Count];

            for (int i = 0; i < radGridView.SelectedRows.Count; ++i)
            {
                for (int j = 0; j < radGridView.SelectedRows[i].Cells.Count; ++j)
                {
                    var value = radGridView.SelectedRows[i].Cells[j].Value;
                    clipboard[i, j] = value != null ? value.ToString() : null;
                }

            }


            return clipboard;
        }

        private static void PasteRows2(RadGridView radGridView, object[,] clipboard)
        {
            radGridView.GridElement.BeginUpdate();
            try
            {


                for (int i = 0; i < clipboard.GetLength(0); ++i)
                {
                    var row = radGridView.Rows.AddNew();

                    for (int j = 0; j < clipboard.GetLength(1); ++j)
                    {
                        row.Cells[j].Value = clipboard[i, j];
                    }
                    row.Cells["autoIncrement"].Value = radGridView.Rows.Count;
                    row.Cells["Name"].Value = null;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Incompatible DataSources");
            }
            radGridView.GridElement.EndUpdate();

        }

        public static void CopyPasteRowForGridSample(RadGridView radGrid)
        {
            var cb = CopyRows2(radGrid);
            if (cb != null)
                PasteRows2(radGrid, cb);
        }



    }
}