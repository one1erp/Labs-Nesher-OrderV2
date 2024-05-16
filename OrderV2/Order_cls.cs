using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Common;
using DAL;
using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;
using One1.Controls;
using OrderV2.Controls;
using Telerik.WinControls.UI;
using XmlService;

namespace OrderV2
{

    [ComVisible(true)]
    [ProgId("OrderV2.Order_cls")]
    public partial class Order_cls : UserControl, IExtensionWindow
    {

        #region Ctor

        public static bool DEBUG;
        public Order_cls()
        {

            InitializeComponent();
            BackColor = Color.FromName("Control");

        }

        #endregion

        #region private members

        private INautilusServiceProvider serviceProvider;
        private INautilusDBConnection _ntlsCon;
        private IExtensionWindowSite2 _ntlsSite;
        private IDataLayer dal;
        private List<IDetailsPanel> panels;
        private ListDatas listData;
        public static bool hasChanges = false;
        private NautilusUser _ntlsUser;

        private Operator currentOperator;
        private Workstation currentWorkstation;

        //For run from entity extension
        public string InputData
        {
            get { return txtEnterName.Text; }
            set { txtEnterName.Text = value; }
        }

        public bool RunFromWindow { get; set; }


        #endregion

        #region Implementation of IExtensionWindow

        public bool CloseQuery()
        {
            samplesDetails.SaveClientLayout();
            if (dal != null) dal.Close();
            CloseSamplesColumnChooser();

            //    this.Dispose();
            return true;
        }

        public void CloseSamplesColumnChooser()
        {
            if (samplesDetails != null) samplesDetails.CloseColumnChooser();

        }
        public void Internationalise()
        {
        }

        public void SetSite(object site)
        {
            _ntlsSite = (IExtensionWindowSite2)site;
            _ntlsSite.SetWindowInternalName("Order V2");
            _ntlsSite.SetWindowRegistryName("Order V2");
            _ntlsSite.SetWindowTitle("Order V2");
        }

        public void PreDisplay()
        {
            if (!DEBUG)
            {
                Utils.CreateConstring(_ntlsCon);
                dal = new DataLayer();
            }
            else
            {
                dal = new MockDataLayer();
            }
            Initialize(dal);
            this.Cursor = Cursors.Default;

        }


        private void SetCurrentOperator()
        {
            if (_ntlsUser != null)
            {

                var u = _ntlsUser.GetOperatorName();
                currentOperator = listData.GetOperatorByName(u);
                var wid = _ntlsUser.GetWorkstationId();
                currentWorkstation = dal.getWorkStaitionById((long)wid);


            }
        }

        public WindowButtonsType GetButtons()
        {
            return WindowButtonsType.windowButtonsNone;
        }


        public void SetServiceProvider(object serviceProvider)
        {
            this.serviceProvider = serviceProvider as NautilusServiceProvider;
            //  _processXml = Utils.GetXmlProcessor(this.serviceProvider);
            _ntlsCon = Utils.GetNtlsCon(this.serviceProvider);
            _ntlsUser = Utils.GetNautilusUser(this.serviceProvider);

        }

        public void SetParameters(string parameters)
        {
        }

        public void Setup()
        {
        }

        public WindowRefreshType DataChange()
        {
            return WindowRefreshType.windowRefreshNone;
        }

        public WindowRefreshType ViewRefresh()
        {
            return WindowRefreshType.windowRefreshNone;
        }

        public void refresh()
        {
        }

        public bool SaveData()
        {
            //panels.ForEach(x => x.Clear());
            return true;
        }

        public void SaveSettings(int hKey)
        {
        }

        public void RestoreSettings(int hKey)
        {
        }

        #endregion

        #region  Events
        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewOrder_Click(object sender, EventArgs e)
        {

            //Check if has changes
            if (hasChanges)
            {
                var dr = MessageBox.Show("אתה עלול לאבד נתונים,האם ברצונך להמשיך?", "Nautilus",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)

                    return;
            }


            //Cancel Last changes
            CancelChanges();

            //Close column chooser
            samplesDetails.CloseColumnChooser();

            //Clear data from all panels
            ClearPanelStatus();
            panels.ForEach(panel =>
                           {

                               panel.CurrentSdg = null;
                               panel.CurrentClient = null;
                               panel.IsUpdatedState = false;
                               panel.Clear();
                           });

            clientDetails.DisplayNew();
            clientDetails.OnClientChanged += clientDetails_OnClientChanged;
            orderDetails.Enabled = false;
            samplesDetails.Enabled = false;
            samplesDetails.DisplayNew();
            PanelHasChange(false);


        }
        /// <summary>
        /// Display Order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TxtEnterName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (hasChanges)
                {
                    var dr = MessageBox.Show("אתה עלול לאבד נתונים,האם ברצונך להמשיך?", "Nautilus",
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.No)
                        return;
                }

                //Cancel Last change
                CancelChanges();

                GetData(txtEnterName.Text);
            }
        }



        public void GetData(string inputData)
        {

            //dal = new DataLayer();

            //Initialize(dal);


            Sdg sdg = dal.GetSdgByName(inputData);

            if (sdg == null)
            {
                //try to get sample by name
                Sample sample = dal.GetSampleByName(inputData);

                if (sample == null)
                {
                    //try to get sample by id
                    long sampleId;
                    bool parsed = (long.TryParse(inputData, out sampleId));
                    if (parsed)
                    {
                        sample = dal.GetSampleByKey(sampleId);
                    }
                }

                if (sample == null || sample.Sdg == null)
                {
                    CustomMessageBox.Show("SDG not found");
                    panels.ForEach(x => x.Clear());
                    ClearPanelStatus();
                    txtEnterName.Focus();
                    return;
                }
                sdg = sample.Sdg;
            }

            //Init status panel
            SetPanelStauts(sdg);
            btnSaveAndPrint.Enabled = true;
            btnSaveWithoutPrint.Enabled = true;
            //  cbSaveWithoutPrint.Enabled = true;

            //Populate panels with entered sdg
            panels.ForEach(panel =>{ panel.Clear();
                                     panel.IsUpdatedState = true;
                                     panel.dal = dal;
                                     panel.CurrentSdg = sdg;
                                     
                                     panel.CurrentClient = sdg.Client;
                                     panel.DisplaySdgDetails(sdg);
                               });

            samplesDetails.Enabled = true;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveClick(true);
        }

        private void btnSaveWithoutPrint_Click(object sender, EventArgs e)
        {
            SaveClick(false);
        }

        private void SaveClick(bool toPrint)
        {
            if (hasChanges)
            {


                //Check mandatory fields
                bool isValidate = panels.All(x => x.ValidateSdg());

                if (!UserHasGroup())
                {
                    CustomMessageBox.Show("אין למשתמש הרשאה מתאימה", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (isValidate)
                {
                    btnSaveAndPrint.Enabled = false;
                    btnSaveWithoutPrint.Enabled = false;



                    //Checks order mode
                    bool updateState = panels.Any(x => x.IsUpdatedState);

                    if (updateState)
                    {
                        //Update specified sdg
                        panels.ForEach(panel => panel.UpdateSdg());
                        //Redisplay

                        SaveAndLoadSaved(orderDetails.CurrentSdg, toPrint);
                    }

                    else
                    {
                        var workflowName = orderDetails.ChoosenLab.SdgWorkflow.Name;
                        if (orderDetails.ChoosenLab == null || orderDetails.ChoosenLab.SdgWorkflow == null)
                        {
                            CustomMessageBox.Show("לא נבחרה מעבדה או לא הוגדר WORKFLOW  ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //Create new sdg
                        var loginXml = new LoginXmlHandler(serviceProvider, "ORDER - LOGIN SDG");
                        loginXml.CreateLoginXml("SDG", workflowName);
                        var success = loginXml.ProcssXml();//שינוי
                        if (success)
                        {
                            //Get new sdg name
                            var newName = loginXml.GetValueByTagName("NAME");
                            //Get new sdg from DB
                            Sdg newSdg = dal.GetSdgByName(newName);

                            //Add other data
                            panels.ForEach(panel =>
                                               {
                                                   panel.CurrentSdg = newSdg;
                                                   panel.InsertSdg(newSdg);
                                               });
                            //Redisplay
                            SaveAndLoadSaved(newSdg, toPrint);
                            //Enables drop down clients(sefi)
                            clientDetails.IsUpdatedState = true;
                        }
                        else
                        {
                            //Write error to log
                            Logger.WriteLogFile(loginXml.ErrorResponse, true);
                            One1.Controls.CustomMessageBox.Show("יצירת ההזמנה נכשלה,אנא פנה לספק השירות");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("שדות חובה");
                }
            }
        }



        private void BtnExitClick(object sender, EventArgs e)
        {

            try
            {



                if (hasChanges)
                {
                    DialogResult dr = MessageBox.Show("?האם ברצונך לשמור את השינויים", "Nautilus",
                                                      MessageBoxButtons.YesNoCancel);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            //Check mandatory fields before close
                            bool isValidate = panels.All(x => x.ValidateSdg());
                            if (isValidate)
                            {
                                btnSave_Click(null, null);
                                Exit();
                            }
                            else
                                One1.Controls.CustomMessageBox.Show("שדות חובה", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            break;
                        case DialogResult.No:
                            Exit();
                            break;
                    }

                }
                else
                {

                    Exit();
                }
            }
            catch (Exception ex)
            {

                Logger.WriteLogFile(ex);
                CustomMessageBox.Show("Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void splitPanel1_Resize(object sender, EventArgs e)
        {
            //עיצוב כותרת
            lblHeader.Location = new Point(Width / 2 - lblHeader.Width / 2, lblHeader.Location.Y);
        }

        #endregion

        #region private metods

        public void Initialize(IDataLayer dal)
        {


            dal.Connect();

            //load all lists
            listData = new ListDatas(dal);
            SetCurrentOperator();
            //Add all panels to list          
            panels = new List<IDetailsPanel>
            {
                clientDetails,
                orderDetails,
                samplesDetails
            };


            //Init panels data
            panels.ForEach(panel =>
                           {
                               panel.CurrentWorkstation = currentWorkstation;
                               panel.CurrentOperator = currentOperator;
                               panel.ServiceProvider = serviceProvider;
                               panel.ListData = listData;
                               panel.dal = dal;
                               panel.Initilaize();
                               panel.HasChange += PanelHasChange;
                           });
            //Registration to events
            orderDetails.OperatorCopied += orderDetails_OperatorCopied;
            orderDetails.TemperatureCopied += orderDetails_TemperatureCopied;
            orderDetails.OnLabChanged += orderDetails_OnLabChanged;
            orderDetails.SamplingDateCopied += orderDetails_SamplingDateCopied;
            orderDetails.SamplingTimeCopied += orderDetails_SamplingTimeCopied;
            //ShowBtnMsb();
            samplesDetails._ntlsUser = _ntlsUser;
            samplesDetails.DalChanged += samplesDetails_DalChanged;
            PanelHasChange(false);
            txtEnterName.Focus();

        }

        private void ShowBtnMsb()
        {
            btnNewOrderBriut.Visible = (currentWorkstation != null
                    && currentWorkstation.GROUP_ID.HasValue
                    && currentWorkstation.LIMS_GROUP.Name.ToUpper() == "FOOD")
                    || _ntlsUser.GetOperatorName().ToUpper() == "LIMS_SYS";

        }

        void orderDetails_SamplingTimeCopied(string obj)
        {
            samplesDetails.CopySamplingTime(obj);
        }





        void samplesDetails_DalChanged(IDataLayer dal)
        {
            //When dal changed update all references
            panels.Foreach(panel => panel.dal = dal);
        }

        private void Exit()
        {



            if (RunFromWindow) { CloseQuery(); ParentForm.Close(); }
            else
            {
                if (_ntlsSite != null) _ntlsSite.CloseWindow();
            }


        }

        private void SaveAndLoadSaved(Sdg sdg, bool toPrint)
        {

            dal.SaveChanges();
            //הצגה מחדש של ההזמנה נשמרה
            samplesDetails.ReDisplay(panels.FirstOrDefault(x => x.CurrentSdg != null).CurrentSdg);
            if (toPrint)
            {
                //הדפסה של כל המדבקות
                samplesDetails.PrintAll(sdg);
            }
            //Update status panel
            SetPanelStauts(sdg);
            PanelHasChange(false);
            //  panels.Foreach(x=>x.IsUpdatedState=true);

        }

        public void PanelHasChange(bool hasChanges)
        {
            //מנגנון שמנהל את השינויים שנעשו במסך
            //ובהתאם לזאת מתאים את זמינות הפקדים
            if (hasChanges != Order_cls.hasChanges)
            {
                Order_cls.hasChanges = hasChanges;
                btnSaveAndPrint.Enabled = hasChanges;
                btnSaveWithoutPrint.Enabled = hasChanges;
                //   cbSaveWithoutPrint.Enabled = hasChanges;
                samplesDetails.VisibleCommand(!hasChanges);
            }
        }
        /// <summary>
        ///  //ביטול שינויים שנעשו על האוביקט
        /// </summary>

        private void CancelChanges()
        {
            if (panels != null)
            {
                var sdg = panels.First().CurrentSdg;
                if (sdg != null)
                {
                    if (dal != null)
                    {
                        dal.CancelChanges(sdg);
                        foreach (var sample in sdg.Samples)
                        {
                            dal.CancelChanges(sample);

                        }
                    }
                }
            }
        }

        /// <summary>
        ///Occurs when a client is selected
        /// </summary>
        private void clientDetails_OnClientChanged()
        {

            orderDetails.Enabled = true;
            orderDetails.CurrentClient = clientDetails.CurrentClient;

            //Display order details by client
            orderDetails.DisplayNew();
            samplesDetails.Enabled = true;

            //Display grid by Client
            samplesDetails.CurrentClient = clientDetails.CurrentClient;
            samplesDetails.DisplayNew();

        }

        #region Order details Events

        void orderDetails_SamplingDateCopied(string obj)
        {
            samplesDetails.CopySamplingDate(obj);
        }

        void orderDetails_OnLabChanged(LabInfo obj)
        {


            clientDetails.LabChanged(obj);
            samplesDetails.LabChanged(obj);

        }

        void orderDetails_TemperatureCopied(decimal value)
        {
            //Copy temperature for all samples
            samplesDetails.CopyTemperature(value);
        }

        void orderDetails_OperatorCopied(Operator obj)
        {
            if (obj == null) return;
            //Copy operator for all samples
            samplesDetails.CopyOperatorName(obj);
        }
        #endregion

        #endregion

        #region Status panel
        /// <summary>
        /// Update status panel 
        /// </summary>
        /// <param name="sdg">Entered sdg</param>
        private void SetPanelStauts(Sdg sdg)
        {
            txtExternalReference.Text = sdg.ExternalReference;
            if (sdg.LabInfo != null) txtLabName.Text = sdg.LabInfo.LabHebrewName;
            txtOrder.Text = sdg.Name;

            //Calculate order status and color  status
            CalculateorderStatus(sdg);
        }

        /// <summary>
        ///Calculate order status and color  status
        /// </summary>
        /// <param name="sdg">Specified sdg</param>
        private void CalculateorderStatus(Sdg sdg)
        {
            switch (sdg.Status)
            {
                case "V":
                    bool b = false;
                    //האם שויכו בדיקות
                    var NothasTets = sdg.Samples.All(sample => sample.Aliqouts.Count == 0);

                    if (!NothasTets)
                    {
                        txtOrderStatus.Text = GetPhraseDescriptionByName("L");
                        txtOrderStatus.BackColor = Color.Purple;
                    }
                    else
                    {
                        txtOrderStatus.Text = GetPhraseDescriptionByName("V");
                        txtOrderStatus.BackColor = Color.Green;
                    }
                    break;
                case "C":
                    txtOrderStatus.Text =
                        txtOrderStatus.Text = GetPhraseDescriptionByName("C");
                    txtOrderStatus.BackColor = Color.Red;
                    break;
                case "A":
                    txtOrderStatus.Text = GetPhraseDescriptionByName("A");
                    txtOrderStatus.BackColor = Color.Purple;
                    break;
                case "O":
                    txtOrderStatus.Text = GetPhraseDescriptionByName("O");
                    txtOrderStatus.BackColor = Color.Blue;

                    break;

            }
        }
        /// <summary>
        ///Get order status
        /// </summary>
        /// <param name="phraseName">phrase name</param>
        /// <returns>phrase description</returns>
        string GetPhraseDescriptionByName(string phraseName)
        {
            var phraseEntry = listData.OrderStatusList.Where(x => x.PhraseName == phraseName).FirstOrDefault();
            if (phraseEntry != null)
                return phraseEntry.PhraseDescription;
            return null;
        }
        /// <summary>
        /// Reset panel status
        /// </summary>
        private void ClearPanelStatus()
        {
            txtEnterName.Text = "";
            txtExternalReference.Text = "";
            txtLabName.Text = "";
            txtOrder.Text = "";
            txtOrderStatus.Text = "";
            txtOrderStatus.BackColor = txtLabName.BackColor;
        }
        #endregion






        public bool VisibleExitButton
        {
            get
            {
                return this.btnExit.Enabled;
            }
            set { this.btnExit.Enabled = value; }
        }

        public string ConnectionString { get; private set; }

        private Entities context;

        public bool UserHasGroup()
        {
            var groupId = orderDetails.ChoosenLab.GroupID;
            return currentOperator.OperatorGroups.Any(operatorGroup => operatorGroup.GroupId == groupId);
        }

        private void samplesDetails_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void orderDetails_Load(object sender, EventArgs e)
        {

        }

        private void txtEnterName_TextChanged(object sender, EventArgs e)
        {

        }


        #region MyRegion


        OrderV2.Controls.FCS_Window wind = null;

        private void btnNewOrderBriut_Click(object sender, EventArgs e)
        {

            wind = new Controls.FCS_Window(serviceProvider, _ntlsSite ,dal);
            wind.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FcsWin_FormClosing);
            wind.ShowDialog();

        }

        private void FcsWin_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!string.IsNullOrEmpty(wind.CreatedSdgName))
            {
                txtEnterName.Text = wind.CreatedSdgName;
                KeyEventArgs _e = new KeyEventArgs(Keys.Enter);
                TxtEnterName_KeyDown(null, _e);
            }

        }
        #endregion

        private void clientDetails_Load(object sender, EventArgs e)
        {

        }

        private void samplesDetails_Load_1(object sender, EventArgs e)
        {

        }
    }
}
