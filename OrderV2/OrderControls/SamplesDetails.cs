using System;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
//using Association_Tests;
using Association_Tests;
using DAL;
using LSSERVICEPROVIDERLib;
using OrderV2.Controls;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Common;
using One1.Controls;
using Telerik.WinControls.UI.Localization;
using XmlService;

namespace OrderV2.OrderControls
{
    public partial class SamplesDetails : UserControl, IDetailsPanel
    {

        #region fields

        // public INautilusServiceProvider ServiceProvider;
        public INautilusUser _ntlsUser;
        public LabInfo _currentLab;
        private List<Sample> EditedSamples;
        private GridLayout gridLayout;
        public event Action SelectionChanged;
        public event Action<IDataLayer> DalChanged;
        private bool limsUser;

        #endregion

        #region Ctor

        public SamplesDetails()
        {
            InitializeComponent();
            RadGridLocalizationProvider.CurrentProvider = new HebrewRadGridLocalizationProvider();
        }

        #endregion

        #region Implement IDetailsPanel

        public INautilusServiceProvider ServiceProvider { get; set; }
        public bool IsUpdatedState { get; set; }
        public Client CurrentClient { get; set; }
        public Operator CurrentOperator { get; set; }
        public Workstation CurrentWorkstation { get; set; }
        public IDataLayer dal { get; set; }
        public Sdg CurrentSdg { get; set; }
        public ListDatas ListData { get; set; }
        public event Action<bool> HasChange;


        public void Initilaize()
        {
            gridLayout = new GridLayout(ListData);
            gridLayout.RemoveFromCOA += (gridLayout_RemoveFromCOA);

            //lims_sys הרשאה  רק ל 
            if (ServiceProvider != null)
            {
                //     btnDeleteXml.Enabled = true;// Utils.GetNautilusUser(ServiceProvider).GetOperatorName() == "lims_sys";
            }
        }

        /// <summary>
        /// Display new Grid
        /// </summary>
        public void DisplayNew()
        {

            ClearGridView();
            //load grid with special columns per client
            LoadGridColumns();

        }

        /// <summary>
        /// Display sdg data
        /// </summary>
        /// <param name="sdg">Enterd sdg</param>
        public void DisplaySdgDetails(Sdg sdg)
        {

            var samples = sdg.Samples.OrderBy(x => x.SampleId).ToList();
            //save samples in local fields
            EditedSamples = samples;
            _currentLab = sdg.LabInfo;
            LoadGridColumns();
            //
            gridLayout.PopulateData(samples, gridSamples);
            spinCount.Value = samples.Where(x => x.Status != "X").Count();
            btnDeleteXml.Enabled = false;
        }

        /// <summary>
        /// Clear data from controls
        /// </summary>
        public void Clear()
        {

            ClearGridView();
            EditedSamples = null;
            spinCount.Value = 0;
            _currentLab = null;
        }

        /// <summary>
        /// Checks mandatory Fields
        /// </summary>
        /// <returns></returns>
        public bool ValidateSdg()
        {
            //  נועד לעקיפה של באג שלא מוסיף שורה כאשר עומדים על שדה לקריאה בלבד 
            var cc = gridSamples.CurrentCell;

            if (cc != null && cc.ColumnInfo.ReadOnly)
            {
                cc.ColumnInfo.ReadOnly = false;
                gridSamples.BeginEdit();
                gridSamples.EndEdit();
                cc.ColumnInfo.ReadOnly = true;
            }
            else
            {

                gridSamples.EndEdit();
            }

            try
            {


                //שדות חובה
                foreach (GridViewRowInfo row in gridSamples.Rows)
                {

                    var cell = row.Cells["ProductName"];

                    var cb = cell.ColumnInfo as GridViewComboBoxColumn;

                    List<Product> products = (List<Product>)cb.DataSource;


                    if (cell.Value == null || products.FirstOrDefault(x => x.Name == cell.Value.ToString()) == null)
                    {
                        row.Cells["ProductName"].Style.DrawFill = true;
                        return false;
                    }

                }
                return true;

            }
            catch (Exception e)
            {
                Logger.WriteLogFile(e);
                return false;
            }
        }

        /// <summary>
        /// Add samples to new sdg
        /// </summary>
        /// <param name="newSdg"></param>
        public void InsertSdg(Sdg newSdg)
        {


            foreach (GridViewRowInfo row in gridSamples.Rows)
            {
                //Add sample under sdg
                var sampleId = AddSample(newSdg);
                if (sampleId != "0")//If login success
                {
                    //Get new sample from DB
                    Sample newSample = dal.GetSampleByKey(Int32.Parse(sampleId));
                    //Update other data in new sample
                    EditSampleProperties(row, newSample);
                }
            }
            // Save grid layout
            SaveClientLayout();
            UpdateSdgSpecification(newSdg);
        }

        /// <summary>
        /// Add or update samples under enterd sdg
        /// </summary>
        public void UpdateSdg()
        {

            foreach (GridViewRowInfo row in gridSamples.Rows)
            {
                //Get sample name
                var nameValue = row.Cells["Name"].Value;

                if (nameValue == null) //if is new sample
                {
                    var sampleId = AddSample(CurrentSdg);
                    if (sampleId != "0") //If login success
                    {
                        Sample newSample = dal.GetSampleByKey(Int32.Parse(sampleId));
                        //Update sample in other data
                        EditSampleProperties(row, newSample);
                    }
                }
                else // Update older sample
                {
                    string sampleName = nameValue.ToString();
                    var sample = EditedSamples.FirstOrDefault(s => s.Name == sampleName);
                    EditSampleProperties(row, sample);
                }
            }
            // Save grid layout
            SaveClientLayout();
            UpdateSdgSpecification(CurrentSdg);

        }

        public void Exit()
        {
            //Closing column chooser
            CloseColumnChooser();
        }


        #endregion

        #region private methods

        /// <summary>
        /// Load grid's samples
        /// </summary>
        private void LoadGridColumns()
        {
            try
            {


                if (CurrentClient != null && _currentLab != null)
                {
                    var xmlDal = new DataLayer();

                    xmlDal.Connect();

                    var xs = xmlDal.GetXmlStorage("CLIENT_SAMPLE_DETAILS", CurrentClient.ClientId, _currentLab.LabInfoId);
                    if (xs != null)
                    //  ששמור ללקוח XML טעינה של טבלת הדוגמאות לפי ה
                    {
                        if (xs.XmlData != null)
                        {
                            var xml = xs.XmlData;
                            gridLayout.LoadLayout(xml, gridSamples);
                            gridLayout.RegisterIsVisibleEvent(gridSamples);
                            return;
                        }
                    }
                    xmlDal.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogFile(ex);
            }
            //xml טעינה דיפולטית  אם לא קיים 
            gridLayout.BuildCoulmns(gridSamples);
        }
        /// <summary>
        /// Print samples of sdg
        /// </summary>
        /// <param name="CurrentSdg"></param>
        public void PrintAll(Sdg CurrentSdg)
        {
            if (CurrentSdg != null)
            {
                //בודק האם וכמה הדפסת מדבקות נכשלו
                var printSamples = new List<bool>();
                bool allCanceled = true;
                foreach (Sample sample in CurrentSdg.Samples.OrderBy(x => x.SampleId))
                {

                    if (sample.Status != "X") //מדפיס רק דגימות שאינם מבוטלות
                    {


                        var fireEvent = new FireEventXmlHandler(ServiceProvider);
                        fireEvent.CreateFireEventXml("SAMPLE", sample.SampleId, "Print Sample Label");
                        var sucess = fireEvent.ProcssXml();

                        printSamples.Add(sucess);
                        if (!sucess)
                        {
                            //Write error to log
                            Logger.WriteLogFile(fireEvent.ErrorResponse, true);
                        }
                        allCanceled = false;
                    }
                }
                //מספר המדבקות שנכשלו
                var notPrintedCount = printSamples.Where(isPrinted => !isPrinted).Count();
                if (notPrintedCount > 0 && !allCanceled)//הודעה כמה הדפסות נכשלו
                {
                    var s = string.Format(" נכשלה הדפסת {0} מדבקות", notPrintedCount);
                    CustomMessageBox.Show(s, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (allCanceled)
                {
                    CustomMessageBox.Show("כל הדוגמאות מבוטלות,לא ניתן להדפיס", MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                }
            }
        }



        /// <summary>
        /// Get selected product from Row
        /// </summary>
        /// <param name="row">Specified row</param>
        /// <returns>Selected product</returns>
        private Product GetSelectedProduct(GridViewRowInfo row, string colName, List<Product> products)//TODO:add parameter cell name 
        {
            if (row.Cells[colName] == null) return null;
            var value = row.Cells[colName].Value;
            if (value != null)
            {
                var productName = value.ToString();
                Product p = products.Where(x => productName != null && x.Name == productName).FirstOrDefault();
                return p;
            }
            return null;
        }
        /// <summary>
        /// Get selected Operator from Row
        /// </summary>
        /// <param name="row">Specified row</param>
        /// <returns>Selected Operator</returns>
        private Operator GetSelectedOperator(GridViewRowInfo row)
        {
            var value = row.Cells["SampledByOperatorName"].Value;
            if (value != null)
            {
                var operatorName = value;
                Operator p = ListData.OperatorsByRole.FirstOrDefault(x => operatorName != null && x.Name == operatorName);
                return p;
            }
            return null;
        }
        /// <summary>
        ///Create login xml 
        /// </summary>
        /// <param name="newSdg">Added sdg</param>
        /// <returns>New sample ID</returns>
        private string AddSample(Sdg newSdg)
        {
            LoginXmlHandler login = new LoginXmlHandler(ServiceProvider, "ORDER - LOGIN SAMPLE");
            login.CreateLoginChildXml("SDG", newSdg.Name, "SAMPLE", newSdg.LabInfo.SampleWorkflow.Name, FindBy.Name);
            var sucsess = login.ProcssXml();
            if (!sucsess)
            {

                //Write error to log
                Logger.WriteLogFile(login.ErrorResponse, true);
                string s = "לא נקלטה דוגמה כראוי" + "\n" + "אנא פנה לתמיכה";
                CustomMessageBox.Show(s, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "0";
            }
            var sampleId = login.GetValueByTagName("SAMPLE_ID");
            return sampleId;

        }

        /// <summary>
        /// Clear grid
        /// </summary>
        private void ClearGridView()
        {
            gridSamples.BeginEdit();
            gridSamples.Columns.Clear();
            gridSamples.Rows.Clear();
            gridSamples.DataSource = null;
            gridSamples.EndEdit();
        }

        /// <summary>
        /// Update sample from grid columns
        /// </summary>
        /// <param name="row">Specified row</param>
        /// <param name="sample">Specified sample</param>
        private void EditSampleProperties(GridViewRowInfo row, Sample sample)
        {
            sample.Product = GetSelectedProduct(row, "ProductName", ListData.CategoryProducts);
            sample.RealProduct = GetSelectedProduct(row, "RealProductName", ListData.RealProducts);
            sample.Batch = (string)(row.Cells["Batch"].Value ?? null);
            sample.Description = (string)(row.Cells["Description"].Value ?? null);
            sample.SamplingTemperature = row.Cells["SamplingTemperature"].Value as decimal?;
            var comments = row.Cells["Comments"].Value;
            if (comments != null) sample.Comments = comments.ToString();
            sample.Client = CurrentClient;
            sample.DateProduction = (DateTime?)row.Cells["DateProduction"].Value;
            var ef = row.Cells["ExternalReference"].Value;
            if (ef != null)
                sample.ExternalReference = ef.ToString();
            sample.SampledByOperator = GetSelectedOperator(row);
            //       sample.SampledOn = (DateTime?)row.Cells["SampledOn"].Value;
            if (row.Cells["TextualSamplingTime"] != null)
            {
                sample.TextualSamplingTime = (string)row.Cells["TextualSamplingTime"].Value ?? null;
            }
            if (row.Cells["TextualSamplingTime2"] != null)
            {
                sample.TextualSamplingTime2 = (string)row.Cells["TextualSamplingTime2"].Value ?? null;
            }
            if (sample.TabMethod != null)
                sample.TabMethod = (string)(row.Cells["TabMethod"].Value ?? null);

            //var cn = row.Cells["ContainerNumber"].Value;
            //if (cn != null)
            //    sample.ContainerNumber = cn.ToString();
            //sample.SampledByOperator = GetSelectedOperator(row);
            if (row.Cells["ContainerNumber"] != null)
            {
                sample.ContainerNumber = (string)row.Cells["ContainerNumber"].Value ?? null;
            }
            if (row.Cells["DelFileNum"] != null)
            {
                sample.DelFileNum = (string)row.Cells["DelFileNum"].Value ?? null;
            }
            if (row.Cells["PointCode"] != null)
            {
                sample.PointCode = (string)row.Cells["PointCode"].Value ?? null;
            }

            //Set special fields
            var propertyInfo = typeof(Sample).GetProperties().Where(x => x.Name.StartsWith("Field")).ToList();
            foreach (PropertyInfo info in propertyInfo)
            {
                if (row.Cells[info.Name] != null)
                {
                    var value = row.Cells[info.Name].Value;
                    if (value != null)
                    {
                        if (value is bool)
                        {
                            value = NautilsuBoolean.ConvertBack(Convert.ToBoolean(value));

                        }
                        info.SetValue(sample, value, null);
                    }
                }
            }

        }

        private void UpdateSdgSpecification(Sdg sdg)
        {
            if (sdg.LabInfo != null && sdg.LabInfo.Name == "Water")
            {
                var sample = sdg.Samples.FirstOrDefault();
                if (sample != null)
                {
                    var product = sample.Product;
                    if (product != null)
                    {
                        var es = dal.GetSpecification("PRODUCT", product.ProductId);

                        if (es != null && es.Count > 0)
                        {
                            var s = es.FirstOrDefault().Specification;
                            if (s != null)
                            {
                                sdg.U_SPECIFICATION = s.DESCRIPTION;

                            }
                        }
                    }
                }
            }


        }



        /// <summary>
        /// Save grid layout
        /// </summary>
        public void SaveClientLayout()
        {
            try
            {


                if (CurrentSdg == null || CurrentClient == null || CurrentSdg.LabInfoId == null) return;

                //שומר את העיצוב והעמודות עבור הלקוח       
                var xmlDal = new DataLayer();
                xmlDal.Connect();
                var clientId = CurrentClient.ClientId;
                var newLayoutXml = UiHelperMethods.ConvertGridToByteArrray(gridSamples);
                var xs = xmlDal.GetXmlStorage("CLIENT_SAMPLE_DETAILS", clientId, (long)CurrentSdg.LabInfoId);
                if (xs == null)
                {
                    xs = new XmlStorage();
                    xs.TableName = "CLIENT_SAMPLE_DETAILS";
                    xs.EntityId = clientId;
                    xs.XmlData = newLayoutXml;
                    xs.LAB_ID = CurrentSdg.LabInfoId;
                    xmlDal.AddXmlStorage(xs);
                }
                else
                {
                    xs.XmlData = newLayoutXml;
                }
                xmlDal.SaveChanges();
                xmlDal.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in save xml");
                Logger.WriteLogFile(ex);
                if (CurrentClient != null)
                    if (CurrentSdg != null)
                        Logger.WriteLogFile("Error in save xml " + "CLIENT_SAMPLE_DETAILS, clientId is " + CurrentClient.ClientId + "Lab id is " + CurrentSdg.LabInfoId + "sdg id is " + CurrentSdg.SdgId, false);
            }
        }



        #endregion

        #region Buttons

        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            PrintAll(CurrentSdg);
        }

        private void btnAssociationTests_Click(object sender, EventArgs e)
        {

            if (CurrentSdg != null && CurrentSdg.Samples.Count > 0)
            {
                CloseColumnChooser();
                var associationTestsWindow = new AssociationTestsForm(CurrentSdg, _ntlsUser, ServiceProvider);
                if (!associationTestsWindow.IsDisposed)//בודק אם המסך לא נסגר בעקבות תנאים שנמצאים באתחול
                    associationTestsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("לא שויכו דוגמאות להזמנה.");
            }

        }

        /// <summary>
        /// Open column chooser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseFields_Click(object sender, EventArgs e)
        {
            gridSamples.ColumnChooser.Show();
        }

        #endregion

        #region Internal methods
        internal void CopyOperatorName(Operator obj)
        {
            foreach (GridViewRowInfo row in gridSamples.Rows)
            {
                if (IsCanceled(row.Cells["Name"].Value) != true)
                    row.Cells["SampledByOperatorName"].Value = obj.Name;
            }
        }

        internal void CopyTemperature(decimal tempValue)
        {
            //Copy temperature 
            foreach (GridViewRowInfo row in gridSamples.Rows)
            {
                if (IsCanceled(row.Cells["Name"].Value) != true)
                    row.Cells["SamplingTemperature"].Value = tempValue;
            }
        }

        internal void CopySamplingDate(string samplingTime)
        {

            foreach (GridViewRowInfo row in gridSamples.Rows)
            {
                if (IsCanceled(row.Cells["Name"].Value) != true)
                    row.Cells["TextualSamplingTime"].Value = samplingTime;
            }
        }

        internal void CopySamplingTime(string samplingTime)
        {
            foreach (GridViewRowInfo row in gridSamples.Rows)
            {
                if (IsCanceled(row.Cells["Name"].Value) != true)
                    row.Cells["TextualSamplingTime2"].Value = samplingTime;
            }
        }

        internal void ReDisplay(Sdg sdg)
        {
            var samples = sdg.Samples.OrderBy(x => x.SampleId).ToList();
            EditedSamples = samples;
            DisplayNew();
            gridLayout.PopulateData(samples, gridSamples);
            VisibleCommand(true);

        }

        public void CloseColumnChooser()
        {
            if (gridSamples != null && gridSamples.ColumnChooser != null)
                gridSamples.ColumnChooser.Close();
        }

        internal void VisibleCommand(bool flag)
        {
            if (gridSamples.Columns.Count > 0)
                gridSamples.Columns["print"].IsVisible = flag;

            btnPrintAll.Enabled = flag;
            btnAssociationTests.Enabled = flag;
            btnDeleteXml.Enabled = flag;

        }
        #endregion

        #region contex menu item


        private GridHeaderCellElement headerCell;
        private bool hasColumnInCOA;

        private void gridSamples_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            //if is cell context menu
            GridDataCellElement cell = e.ContextMenuProvider as GridDataCellElement;
            if (cell != null)
            {

                //Checks if current column is read only
                bool visible = !cell.ColumnInfo.ReadOnly;

                //Build new context menu
                var menuItem1 = new RadMenuItem("העתקה מאקסל");
                menuItem1.Visibility = visible ? ElementVisibility.Visible : ElementVisibility.Collapsed;
                menuItem1.Click += PasteFromExcellClick;
                var menuItem2 = new RadMenuItem("ביטול דוגמה");
                menuItem2.Click += DeleteSampleItem_Click;
                RadMenuItem menuItem3 = new RadMenuItem("מחק ערכים מכל הטור");
                menuItem3.Visibility = visible ? ElementVisibility.Visible : ElementVisibility.Collapsed;
                menuItem3.Click += (ClearColumnValue_CLick);
                RadMenuItem menuItem4 = new RadMenuItem("העתק שורה");
                menuItem4.Visibility = visible ? ElementVisibility.Visible : ElementVisibility.Collapsed;
                menuItem4.Click += new EventHandler(CopyPasteRowForGridSample);
                RadMenuItem menuItem5 = new RadMenuItem("העתק ערך לכל הטור");
                menuItem5.Visibility = visible ? ElementVisibility.Visible : ElementVisibility.Collapsed;
                menuItem5.Click += CopyColumnValue_CLick;
                e.ContextMenu.Items.Add(menuItem1);
                e.ContextMenu.Items.Add(menuItem2);
                e.ContextMenu.Items.Add(menuItem3);
                e.ContextMenu.Items.Add(menuItem4);
                e.ContextMenu.Items.Add(menuItem5);


            }
            else
            {
                if (CurrentClient == null) return;
                //if is header Context menu
                headerCell = e.ContextMenuProvider as GridHeaderCellElement;
                if (headerCell != null)
                {
                    var coulmnName = headerCell.Data.Name;
                    //var headerName = headerCell.ColumnInfo.HeaderText;
                    //    if (CurrentClient.DefaultCOA_column != null)
                    hasColumnInCOA = HasColumnInCOA(coulmnName);//האם השדה מופיע כבר בתעודה

                    //שדות שלא יכולים להתווסף לתעודה
                    var notAddToCoa = (headerCell.ColumnInfo.Name == "Name" || headerCell.ColumnInfo.Name == "Description" || headerCell.ColumnInfo.Name == "AutoIncrement");

                    RadMenuItem renameMenuItem = new RadMenuItem();
                    //אפשר לשנות כותרת  רק לשדות מיוחדים
                    bool isSpecialColumn = coulmnName.StartsWith("Field");
                    renameMenuItem.Visibility = isSpecialColumn ? ElementVisibility.Visible : ElementVisibility.Collapsed;
                    renameMenuItem.Text = "שנה כותרת";
                    renameMenuItem.Click += Rename_Click;

                    if (_currentLab != null)
                    {
                        RadMenuItem toCoaMenuItem = new RadMenuItem();
                        toCoaMenuItem.Text = hasColumnInCOA ? "הסר מתעודה" : "הוסף לתעודה";
                        toCoaMenuItem.Click += ToCOA_Click;
                        toCoaMenuItem.Visibility = !notAddToCoa
                            ? ElementVisibility.Visible
                            : ElementVisibility.Collapsed;
                        e.ContextMenu.Items.Add(toCoaMenuItem);
                    }
                    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
                    e.ContextMenu.Items.Add(separator);
                    //          e.ContextMenu.Items.Add(toCoaMenuItem);
                    e.ContextMenu.Items.Add(renameMenuItem);

                }
                else
                {
                    e.ContextMenu = null;
                }

            }

        }

        private bool HasColumnInCOA(string coulmnName)
        {
            if (_currentLab == null || CurrentClient == null) return false;

            string[] splited = null;
            var dalClientData = new DataLayer();
            dalClientData.Connect();


            U_CLIENT_DATA cd = dalClientData.GetClientData(CurrentClient.ClientId, _currentLab.LabInfoId);

            if (cd != null && !string.IsNullOrEmpty(cd.U_COA_COLUMNS))
                splited = cd.U_COA_COLUMNS.Split(';');


            if (splited != null)
            {
                foreach (string s in splited)
                {
                    var b = s.Split('@');
                    if (b.Length > 0 && b[0] == coulmnName)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        void CopyPasteRowForGridSample(object sender, EventArgs e)
        {
            GridLayout.CopyPasteRowForGridSample(gridSamples);

        }

        /// <summary>
        /// Delete column  value from all rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClearColumnValue_CLick(object sender, EventArgs e)
        {

            //Get column index
            var index = gridSamples.CurrentCell.ColumnIndex;


            for (int i = 0; i < gridSamples.Rows.Count; i++)
            {
                if (!IsCanceled(i))

                    //Delte value from cell
                    gridSamples.Rows[i].Cells[index].Value = null;

            }
        }

        void CopyColumnValue_CLick(object sender, EventArgs e)
        {

            //Get column index
            var index = gridSamples.CurrentCell.ColumnIndex;
            if (gridSamples.CurrentCell.Value == null) return;

            for (int i = 0; i < gridSamples.Rows.Count; i++)
            {
                if (!IsCanceled(i) && IsdDropDownValue(gridSamples.Rows[i].Cells[index], gridSamples.CurrentCell.Value.ToString()))

                    //Delete value from cell
                    gridSamples.Rows[i].Cells[index].Value = gridSamples.CurrentCell.Value;
            }
        }

        void Rename_Click(object sender, EventArgs e)
        {

            //Open rename header window
            var textWindow = new RenameHeaderText(gridSamples, headerCell.ColumnInfo.HeaderText);
            textWindow.StartPosition = FormStartPosition.CenterParent;
            textWindow.HeaderChanged += TextWindow_HeaderChanged;
            textWindow.ShowDialog();
        }

        void TextWindow_HeaderChanged(string newHeader)
        {
            var oldHeader = headerCell.ColumnInfo.HeaderText;
            headerCell.ColumnInfo.HeaderText = newHeader;


            try
            {
                if (hasColumnInCOA) //אם העמודה הזו מופיעה בתעודה
                {


                    var dalClientData = new DataLayer();
                    dalClientData.Connect();
                    U_CLIENT_DATA cd;
                    cd = dalClientData.GetClientData(CurrentClient.ClientId, _currentLab.LabInfoId);
                    var newStr = cd.U_COA_COLUMNS.Replace(oldHeader, newHeader);
                    cd.U_COA_COLUMNS = newStr;
                    dalClientData.SaveChanges();
                    dalClientData.Close();
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Error");
                Logger.WriteLogFile(exception);
            }
        }
        void gridLayout_RemoveFromCOA(string columnName, string headerText)
        {

            if (HasColumnInCOA(columnName))
            {
                RemoveFromCoa(columnName, headerText);
            }

        }

        void ToCOA_Click(object sender, EventArgs e)
        {

            //אם נלחץ הסר מתעודה
            if (hasColumnInCOA)
            {
                //  RemoveFromCoa(/*client*/);
                RemoveFromCoa(headerCell.ColumnInfo.Name, headerCell.ColumnInfo.HeaderText);
            }
            else //אם נלחץ הוסף מתעודה
            {
                if (_currentLab == null)
                {
                    MessageBox.Show("אנא בחר מעבדה!");
                }
                var dalClientData = new DataLayer();
                dalClientData.Connect();
                U_CLIENT_DATA cd;
                cd = dalClientData.GetClientData(CurrentClient.ClientId, _currentLab.LabInfoId);
                if (cd == null)
                {
                    cd = new U_CLIENT_DATA();
                    cd.NAME = CurrentClient.Name + "," + _currentLab.Name;
                    cd.U_CLIENT_ID = CurrentClient.ClientId;
                    cd.U_LAB_ID = _currentLab.LabInfoId;
                    cd.VERSION = "1";
                    cd.VERSION_STATUS = "A";
                    dalClientData.AddClientData(cd);
                }
                cd.U_COA_COLUMNS += headerCell.ColumnInfo.Name + "@" + headerCell.ColumnInfo.HeaderText + ";";
                dalClientData.SaveChanges();
                dalClientData.Close();

                MessageBox.Show("השדה נוסף לתעודה");
            }



        }
        /// <summary>
        /// הורדת שדה שלא יופיע במסמך תעודה
        /// </summary>
        /// <param name="client"></param>
        private void RemoveFromCoa(string columnName, string headerText)
        {


            try
            {



                string newValue = null;
                var dalClientData = new DataLayer();
                dalClientData.Connect();

                var cd = dalClientData.GetClientData(CurrentClient.ClientId, _currentLab.LabInfoId);
                if (cd != null)
                {
                    newValue = cd.U_COA_COLUMNS.Replace(columnName + "@" + headerText + ";", "");
                    cd.U_COA_COLUMNS = newValue;
                    dalClientData.SaveChanges();
                    dalClientData.Close();
                    One1.Controls.CustomMessageBox.Show("השדה הוסר מתעודה");

                }

            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR  IN RemoveFromCoa");
                Logger.WriteLogFile(e);

            }
        }

        private void DeleteSampleItem_Click(object sender, EventArgs e)
        {

            var currentCell = gridSamples.CurrentCell;


            var sampleName = currentCell.RowInfo.Cells["Name"].Value;
            //  אם הוא כבר נמצא במערכת  אז יוחלף לו סטטוס
            if (sampleName != null)
            {
                var sample = EditedSamples.FirstOrDefault(x => x.Name == sampleName.ToString());
                if (sample == null) return;
                sample.Status = "X";
                currentCell.RowElement.ForeColor = Color.Red;
                Font currentFont = currentCell.RowElement.Font;
                currentCell.RowElement.Enabled = false;
                currentCell.RowElement.Font = new Font(currentFont.FontFamily, currentFont.Size, FontStyle.Strikeout);
                if (HasChange != null) HasChange(true);
            }
            else
            {
                //אם הוא לא נמצא במערכת השורה תימחק
                gridSamples.Rows.Remove(gridSamples.CurrentRow);
            }
            spinCount.Value = spinCount.Value - 1;
        }



        private void PasteFromExcellClick(object sender, EventArgs e)
        {
            PasteRows(sender, e);
        }

        private void PasteRows(object sender, EventArgs e)
        {


            //Get location
            var row = gridSamples.CurrentCell.RowIndex;
            var col = gridSamples.CurrentCell.ColumnIndex;

            bool addedNewRows = row == -1;


            IDataObject dataObj = Clipboard.GetDataObject();
            if (dataObj != null)
            {
                var clipBoard = dataObj.GetData(DataFormats.Text).ToString();



                if (clipBoard != String.Empty)
                {
                    if (gridSamples.TableElement != null) gridSamples.TableElement.BeginUpdate();
                    var lineSeparator = new String[] { Environment.NewLine };
                    string[] lines = clipBoard.Split(lineSeparator, StringSplitOptions.RemoveEmptyEntries);
                    int k = 0;
                    try
                    {
                        //  int k = 0;
                        DataRow newRow;
                        for (int j = 0; j < lines.Length; j++)
                        {
                            var cellVal = lines[j].Split('\t');

                            if (addedNewRows)
                            {
                                var dataRowInfo = new GridViewDataRowInfo(this.gridSamples.MasterView);
                                gridSamples.Rows.Add(dataRowInfo);
                                dataRowInfo.Cells["autoIncrement"].Value = gridSamples.Rows.Count;
                                row = gridSamples.Rows.Count - 1;

                            }

                            for (k = 0; k < cellVal.Length; k++)
                            {

                                if (!IsCanceled(row) && IsdDropDownValue(gridSamples.Rows[row].Cells[col + k], cellVal[k]))
                                {
                                    gridSamples.Rows[row].Cells[col + k].Value = cellVal[k];
                                }
                                if (!addedNewRows)
                                {
                                    row++;
                                }
                            }


                        }

                    }
                    catch (Exception ex)
                    {


                        var errorMessage = String.Format("ערך מוטעה בשורה {0} עמודה {1}", row + 1, col + k + 1);
                        MessageBox.Show("errorMessage " + errorMessage + "  ex :" + ex);

                    }
                    finally
                    {
                        gridSamples.TableElement.EndUpdate();
                        //clear last copy to prevent double copy.
                        dataObj = null;


                    }

                }
            }
        }
        /// <summary>
        /// DROP DOWN האם הערך שהועתק תואם לערכים שנמצאים ב 
        /// </summary>
        /// <param name="gridViewCellInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsdDropDownValue(GridViewCellInfo gridViewCellInfo, string value)
        {
            if (gridViewCellInfo.ViewTemplate.CurrentColumn is GridViewComboBoxColumn)
            {
                //רק אם הערך מתאים 

                if (gridViewCellInfo.ColumnInfo.Name == "ProductName")
                {
                    var products =
                        (List<Product>)
                        ((GridViewComboBoxColumn)((gridViewCellInfo.ViewTemplate).CurrentColumn)).DataSource;
                    Product p = products.Where(product => product.Name == value).FirstOrDefault();
                    if (p != null)
                    {
                        //כאשר מועתק מאקסל מוצר יתמלא גם התיאור מוצר
                        gridViewCellInfo.RowInfo.Cells["ProductDescription"].Value = p.Description;
                    }
                    return p != null;
                }
                else if (gridViewCellInfo.ColumnInfo.Name == "SampledByOperatorName")
                {
                    var operators =
                        (List<Operator>)
                        ((GridViewComboBoxColumn)((gridViewCellInfo.ViewTemplate).CurrentColumn)).DataSource;
                    return operators.Any(o => o.Name == value);
                }
            }
            return true;



        }


        /// <summary>
        /// האם הדגימה אינה מבוטלת
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsCanceled(int row)
        {
            //   if (IsCanceled( gridSamples.Rows[row].Cells["Name"].Value)!=null)

            var sampleName = gridSamples.Rows[row].Cells["Name"].Value;
            if (EditedSamples == null)
            {
                return false;//לא קיימות דגימות
            }
            //Get specified sample
            var sample = EditedSamples.Where(x => x.Name == sampleName).FirstOrDefault();

            if ((sample != null && sample.Status != "X") || sample == null)
            {
                return false;
            }
            return true;


        }
        /// <summary>
        /// האם הדגימה קיית במערכת,ומה הסטטוס שלה.
        /// </summary>
        /// <param name="sampleName">שם הדגימה המבוקשת</param>
        /// <returns>True if sample is Canceled</returns>
        private bool? IsCanceled(object sampleName)
        {
            //TODO:להשתמש בפונקציה הזאת בכל מקום שבודקים אם ניתן לערוך

            if (EditedSamples == null || sampleName == null)
            {
                return null;//דגימה שעוד לאא נקלטה במערכת
            }

            Sample sample = EditedSamples.Where(x => x.Name == sampleName.ToString()).FirstOrDefault();
            if (sample == null)
            {
                return null;
            }
            if (sample.Status == "X")
            {
                return true;
            }


            return false;//X דגימה קיימת ואינה בסטטוס 
        }
        #endregion

        #region grid events

        private void btnDeleteLayoutXml_Click(object sender, EventArgs e)
        {
            if (CurrentClient == null)
            {
                CustomMessageBox.Show("אין אפשרות למחוק XML," + "\n" + " אנא בחר לקוח", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (_currentLab == null)
            {
                CustomMessageBox.Show("אין אפשרות למחוק XML," + "\n" + " אנא בחר מעבדה ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var xmlStoragedal = new DataLayer();
            xmlStoragedal.Connect();
            XmlStorage xs = xmlStoragedal.GetXmlStorage("CLIENT_SAMPLE_DETAILS", CurrentClient.ClientId, (long)_currentLab.LabInfoId);
            U_CLIENT_DATA clientData = xmlStoragedal.GetClientData(CurrentClient.ClientId, (long)_currentLab.LabInfoId);
            if (xs != null)
            {
                if (clientData != null)
                {
                    //מחיקה גם של שדות נוספים מתעודה(גרם להרבה בעיות)
                    clientData.U_COA_COLUMNS = string.Empty;
                }
                xs.XmlData = null;
                xmlStoragedal.SaveChanges();
            }
            xmlStoragedal.Close();

            CustomMessageBox.Show("XML נמחק עבור" + " " + _currentLab.LabHebrewName + "," + CurrentClient.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            DisplayNew();
        }

        private void ResetValue()
        {

        }
        private void gridSamples_RowFormatting(object sender, RowFormattingEventArgs e)
        {

            if (gridSamples.RowCount > 0 && ((e.RowElement).Data).Cells.Count > 0)
            {

                ResetRowValue(e);
                var sampleName = ((e.RowElement).Data).Cells["Name"].Value;
                if (sampleName == null)//Is new sample
                {
                    ////ערכים התחלתיים
                    //e.RowElement.Enabled = true;
                    //e.RowElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
                    //e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                    //e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    //e.RowElement.ResetValue(VisualElement.FontProperty, ValueResetFlags.Local);
                    //e.RowElement.ResetValue(VisualElement.ForeColorProperty, ValueResetFlags.Local);
                    //e.RowElement.ResetValue(RadElement.EnabledProperty, ValueResetFlags.Local);


                    return;
                }

                var sample = EditedSamples.FirstOrDefault(x => x.Name == sampleName.ToString());


                if (sample != null)
                {
                    //צבע הרקע לפי הסטטוס של הדגימה
                    e.RowElement.DrawFill = true;
                    switch (sample.Status)
                    {
                        case "V":
                            e.RowElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
                            e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                            e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                            e.RowElement.ResetValue(VisualElement.FontProperty, ValueResetFlags.Local);
                            e.RowElement.ResetValue(VisualElement.ForeColorProperty, ValueResetFlags.Local);
                            e.RowElement.ResetValue(RadElement.EnabledProperty, ValueResetFlags.Local);
                            break;
                        case "P":
                            e.RowElement.BackColor = Color.Yellow;
                            break;
                        case "C":
                            e.RowElement.BackColor = Color.Red;
                            break;
                        case "I":
                            e.RowElement.BackColor = Color.Azure;
                            break;
                        case "A":
                            e.RowElement.BackColor = Color.Green;
                            break;
                        case "X":
                            e.RowElement.ForeColor = Color.Red;
                            Font currentFont = e.RowElement.Font;
                            e.RowElement.Font = new Font(currentFont.FontFamily, currentFont.Size, FontStyle.Strikeout);
                            e.RowElement.Enabled = false;
                            break;

                    }
                }
            }

        }

        private void ResetRowValue(RowFormattingEventArgs e)
        {
            e.RowElement.Enabled = true;
            e.RowElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
            e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            e.RowElement.ResetValue(VisualElement.FontProperty, ValueResetFlags.Local);
            e.RowElement.ResetValue(VisualElement.ForeColorProperty, ValueResetFlags.Local);
            e.RowElement.ResetValue(RadElement.EnabledProperty, ValueResetFlags.Local);
        }

        private void gridSamples_CommandCellClick(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count > 0)
            {
                var colValue = gridSamples.SelectedRows.First().Cells["Name"];
                if (colValue.Value != null)
                {
                    var sampleName = colValue.Value.ToString();
                    Sample sample = dal.GetSampleByName(sampleName);
                    //לא להדפיס דגימה מבוטלת
                    if (sample.Status == "X") return;

                    //Fire event xml
                    var fireEvent = new FireEventXmlHandler(ServiceProvider);
                    fireEvent.CreateFireEventXml("SAMPLE", sample.SampleId, "Print Sample Label");
                    var succes = fireEvent.ProcssXml();
                    if (!succes)
                    {
                        Logger.WriteLogFile(fireEvent.ErrorResponse, true);
                        CustomMessageBox.Show("נכשלה הדפסת מדבקה.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }


        }

        private void gridSamples_ValueChanged(object sender, EventArgs e)
        {
            //מדווח למסך הכללי על כל שינוי של אחד הערכים
            if (HasChange != null) HasChange(true);
        }

        private void gridSamples_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            //Set identity
            var count = gridSamples.Rows.Count;
            e.Row.Cells["autoIncrement"].Value = count;
            spinCount.Value = count;
        }

        private void gridSamples_LayoutLoaded(object sender, LayoutLoadedEventArgs e)
        {

            //When design load from xml ,Populate comboBoxex whit data
            var grid = sender as RadGridView;
            var ProductCol = grid.Columns["ProductName"] as GridViewComboBoxColumn;
            if (ProductCol != null)
            {
                ProductCol.DisplayMember = "Name";
                ProductCol.DataSource = ListData.CategoryProducts;
            }
            var realProductCol = grid.Columns["RealProductName"] as GridViewComboBoxColumn;
            if (realProductCol != null)
            {
                realProductCol.DisplayMember = "Name";
                realProductCol.DataSource = ListData.RealProducts;
            }

            var operatorCol = grid.Columns["SampledByOperatorName"] as GridViewComboBoxColumn;
            if (operatorCol != null)
            {
                operatorCol.DisplayMember = "Name";
                operatorCol.DataSource = ListData.OperatorsByRole;
            }
            var tabMetohdCol = grid.Columns["TabMethod"] as GridViewComboBoxColumn;
            if (tabMetohdCol != null)
            {
                tabMetohdCol.DisplayMember = "PhraseName";
                tabMetohdCol.DataSource = ListData.TabMethodList;
            }
        }

        private void gridSamples_CellEndEdit(object sender, GridViewCellEventArgs e)
        {

            //כאשר בוחרים מוצר הוא ממלא את העמודה שלידו בתיאור מוצר
            if (e.Column.Name != "ProductName") return;
            if (e.Value != null)
            {
                Product p =
                    ListData.CategoryProducts.FirstOrDefault(x => x.Name == e.Value.ToString());
                if (p != null)
                {
                    if (e.Row != null)
                        if (e.Row.Cells.Count > 0)
                        {
                            var pd = e.Row.Cells["ProductDescription"];
                            pd.Value = p.Description;

                        }
                }
            }
        }

        private void gridSamples_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //Set tooltip(Not working in nautilus)
            //if (e.CellElement.Value != null)
            //{
            //    e.CellElement.ToolTipText = e.CellElement.Value.ToString();
            //}

            //Mark mandatory columns
            if (e.Column.Name == "ProductName")// || e.Column.Name == "SampledByOperatorName")
            {
                e.CellElement.DrawFill = true;
                e.CellElement.BackColor = NautilusDesign.MandatoryColor;
            }
        }



        #endregion

        private void btnDeleteXml_EnabledChanged(object sender, EventArgs e)
        {
            //ASHI
            //הכפתור יהיה זמין רק אחרי לחיצה על הזמנה חדשה
            //לא במצב עדכון ולא במצב הצגת הזמנה

            var b = ((System.Windows.Forms.Control)(sender)).Enabled;
            if (b)
            {
                if (gridSamples.RowCount > 0)
                {
                    btnDeleteXml.Enabled = false;
                }
            }
        }

        private void gridSamples_UserDeletedRow(object sender, GridViewRowEventArgs e)
        {
            MessageBox.Show("gridSamples_UserDeletedRow");
        }

        private void gridSamples_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            MessageBox.Show("gridSamples_UserDeletingRow");
        }



        internal void LabChanged(LabInfo lab)
        {
            //ashi 22.3.15
            _currentLab = lab;

            DisplayNew();


            if (lab.GroupID == null) return;
            ListData.SetListByGroup(lab.GroupID.Value);
            ListData.SetOperatorsByRole(lab.Name + " Sampler");
            SetProductDataSource();
            SetOperatorDataSource();

            if (HasChange != null) HasChange(true);
            btnDeleteXml.Enabled = true;
        }

        private void SetOperatorDataSource()
        {
            GridViewDataColumn col = gridSamples.Columns.SingleOrDefault(c => c.Name == "SampledByOperatorName");
            if (col != null)
            {
                var comboBoxColumn = col as GridViewComboBoxColumn;
                if (comboBoxColumn != null)
                {
                    comboBoxColumn.DataSource = ListData.OperatorsByRole;
                }
            }
        }

        public void SetProductDataSource()
        {


            GridViewDataColumn col = gridSamples.Columns.SingleOrDefault(c => c.Name == "ProductName");
            if (col != null)
            {
                var comboBoxColumn = col as GridViewComboBoxColumn;
                if (comboBoxColumn != null)
                {
                    comboBoxColumn.DataSource = ListData.CategoryProducts;
                }
            }
        }
        public IDataLayer DALDEBUG()
        {
            if (Order_cls.DEBUG)
            {
                return new MockDataLayer();


            }
            else
            {
                return new DataLayer();
            }
        }

        #region auto complete column

        void GridSamples_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

            RadDropDownListEditor dropDownEditor = this.gridSamples.ActiveEditor as RadDropDownListEditor;
            if (dropDownEditor != null)
            {
                RadDropDownListEditorElement dropDownEditorElement = (RadDropDownListEditorElement)dropDownEditor.EditorElement;

                dropDownEditorElement.AutoCompleteMode = AutoCompleteMode.Suggest;
                dropDownEditorElement.AutoCompleteSuggest = new CustomAutoSuggestHelper(dropDownEditorElement);
                dropDownEditorElement.DropDownStyle = RadDropDownStyle.DropDown;
            }
        }

        public class CustomAutoSuggestHelper : AutoCompleteSuggestHelper
        {
            public CustomAutoSuggestHelper(RadDropDownListElement owner)
                : base(owner)
            {
            }

            protected override bool DefaultFilter(RadListDataItem item)
            {
                return item.Text.Contains(this.Filter);
            }
        }
        #endregion

    }

}
