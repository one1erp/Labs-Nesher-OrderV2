using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Common;
using DAL;
using LSSERVICEPROVIDERLib;
using OrderV2.AdditionalDetails;


namespace OrderV2.OrderControls
{
    public partial class OrderDetails : UserControl, IDetailsPanel
    {
        private bool _isUpdatedState;
        public event Action<Operator> OperatorCopied;
        public event Action<decimal> TemperatureCopied;
        public event Action<string> SamplingDateCopied;
        public event Action<string> SamplingTimeCopied;
        public LabInfo ChoosenLab { get; private set; }

        public event Action<LabInfo> OnLabChanged;
        private const string WaterLabName = "Water";
        private const string EnvironmentLabName = "Environment";
        private const string CosmeticsLabName = "Cosmetics";
        private const string FoodLabName = "Food";
        private const string SlaughterLabName = "Slaughter";
        private AdditionalDetailsFactory _additionalDetailsFactory;

        public OrderDetails()
        {
            InitializeComponent();
        }

        #region Implement IDetailsPanel

        public Client CurrentClient { get; set; }
        public Operator CurrentOperator { get; set; }
        public Workstation CurrentWorkstation { get; set; }
        public IDataLayer dal { get; set; }
        public Sdg CurrentSdg { get; set; }
        public ListDatas ListData { get; set; }

        public INautilusServiceProvider ServiceProvider { get; set; }

        public bool IsUpdatedState
        {
            get { return _isUpdatedState; }
            set
            {
                _isUpdatedState = value;

                dropDownLabs.Enabled = !value;

            }
        }

        public void Initilaize()
        {
            _additionalDetailsFactory = new AdditionalDetailsFactory(ListData);
            _additionalDetailsFactory.SamplingDateCopied += AdditionalDetailsFactorySamplingDateCopied;
            _additionalDetailsFactory.SamplingTimeCopied += AdditionalDetailsFactorySamplingTimeCopied;
            _additionalDetailsFactory.HasChange += AdditionalDetailsFactoryHasChange;
            btnAdditionalD.BackColor = NautilusDesign.MandatoryColor;

            //Set mandatory fields
            dropDownLabs.DropDownListElement.TextBox.BackColor = NautilusDesign.MandatoryColor;
            dropDownOperators.DropDownListElement.TextBox.BackColor = NautilusDesign.MandatoryColor;


            //Fill dropdowns
            GetLabs();
            GetTemps();
            GetLocations();
            GetOperators();
            GetStorageCondition();
            //   GetHealthMinistryPhrase();
            GetOperatorsByGroup();
            GetCOARemarks();
            //Set event
            dropDownLabs.SelectedValueChanged += dropDownLabs_SelectedValueChanged;
            //dropDownSampledByLab.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(dropDownSampledByLab_SelectedIndexChanged);

        }

        void AdditionalDetailsFactorySamplingTimeCopied(string obj)
        {
            if (SamplingTimeCopied != null) SamplingTimeCopied(obj);
        }



        void AdditionalDetailsFactorySamplingDateCopied(string obj)
        {
            if (SamplingDateCopied != null) SamplingDateCopied(obj);
        }


        public void DisplayNew()
        {
            //  this.Visible = true;
            Clear();
            //מעדכן את המעבדה הדיפולטית של הלקוח לפי התחנה
            if (CurrentWorkstation != null)
            {
                var wsGroupId = CurrentWorkstation.GROUP_ID;
                if (wsGroupId != null)
                {
                    var lab = ListData.Labs.FirstOrDefault(l => l.GroupID == wsGroupId);
                    dropDownLabs.SelectedValue = lab;
                }

            }


        }

        /// <summary>
        /// Populate controls entered sdg
        /// </summary>
        /// <param name="sdg">entered sdg</param>
        public void DisplaySdgDetails(Sdg sdg)
        {
            this.Enabled = true;
            if (sdg.DeliveryDate != null) datePickerRecivedTime.Value = (DateTime)sdg.DeliveryDate;
            if (sdg.LastUpdated != null) datePickerLastUpdated.Value = (DateTime)sdg.LastUpdated;
            txtExternalReference.Text = sdg.ExternalReference;
            dropDownStorageConditions.SelectedText = sdg.StorageConditions;
            if (sdg.Temperature != null) spinTemperature.Value = (decimal)sdg.Temperature;
            RichTxtRemarks.Text = sdg.Comments;
            dropDownLocation.SelectedValue = sdg.Location;
            int i = ListData.CoaTemp.FindIndex(x=> x.PhraseDescription == sdg.U_FOOD_TEMPERATURE);
            if (i != -1)
            {
                dropDownTemp.SelectedValue = ListData.CoaTemp[i];
            }
            

            //new
            DropDownCOARemarks.SelectedText = sdg.U_COA_REMARKS;


            txtSamplingSite.Text = sdg.U_SAMPLING_SITE;



            if (sdg.Location != null) dropDownLocation.SelectedText = sdg.Location.Name;
            RichTxtRemarks.Text = sdg.Comments;
            if (sdg.LabInfo != null)
            {
                dropDownLabs.SelectedValue = sdg.LabInfo;
            }

            //חייב להיות אחרי השמת מעבדה
            if (sdg.SampledByOperator != null)
            {
                dropDownOperators.SelectedValue = sdg.SampledByOperator;
            }

            if (sdg.PerformedOperator != null)
            {
                dropDownPerformedOperators.SelectedValue = sdg.PerformedOperator;
            }

            txtCoaFile.Text = sdg.U_COA_FILE;

            _additionalDetailsFactory.DisplaySdgDetails(sdg);

            ////for water lab
            //if (IsWaterLab(sdg.LabInfo))
            //{
            //    additionalDetails.DisplaySdgDetails(sdg);
            //    //if (sdg.WaterTemperature != null) spinWaterTemperature.Value = (decimal)sdg.WaterTemperature;
            //    //if (sdg.SamplingTime != null)
            //    //{
            //    //    datePickerSamplingTime.Value = (DateTime)sdg.SamplingTime;
            //    //}
            //}
        }

        /// <summary>
        /// Clear all fields
        /// </summary>
        public void Clear()
        {
            datePickerRecivedTime.Value = DateTime.Now;
            datePickerRecivedTime.Enabled = true;
            datePickerLastUpdated.Value = default(DateTime);
            datePickerLastUpdated.Enabled = false;
            txtExternalReference.Text = string.Empty;
            txtCoaFile.Text = string.Empty;
            spinTemperature.Value = 0;
            //spinWaterTemperature.Value = 0;
            //// timeSampledWater.Value = null;
            //datePickerSamplingTime.Value = default(DateTime);
            _additionalDetailsFactory.Clear();
            RichTxtRemarks.Text = string.Empty;
            RichTxtRemarks.Text = string.Empty;

            dropDownTemp.SelectedIndex = -1;
            dropDownOperators.SelectedIndex = -1;
            dropDownLocation.SelectedIndex = -1;
            dropDownLabs.SelectedIndex = -1;
            dropDownStorageConditions.SelectedIndex = -1;
            //new
            DropDownCOARemarks.SelectedIndex = -1;
            DropDownCOARemarks.Text = "";



            txtSamplingSite.Text = string.Empty;
            dropDownPerformedOperators.SelectedIndex = -1;
            panelAdditionalDetails.Visible = false;



        }


        /// <summary>
        /// Update Entered sdg 
        /// </summary>
        public void UpdateSdg()
        {


            CurrentSdg.StorageConditions = dropDownStorageConditions.SelectedText;
            CurrentSdg.Temperature = spinTemperature.Value;
            CurrentSdg.Comments = RichTxtRemarks.Text;
            CurrentSdg.LastUpdated = DateTime.Now;
            CurrentSdg.DeliveryDate = datePickerRecivedTime.Value;
            CurrentSdg.ExternalReference = txtExternalReference.Text;
            PhraseEntry selectedFoodTemp = (PhraseEntry)dropDownTemp.SelectedValue;
            if (selectedFoodTemp != null)
                CurrentSdg.U_FOOD_TEMPERATURE = selectedFoodTemp.PhraseDescription;

            //new
            CurrentSdg.U_COA_REMARKS = DropDownCOARemarks.Text;

            CurrentSdg.U_SAMPLING_SITE = txtSamplingSite.Text;


            if (dropDownOperators.SelectedValue != null)
            {
                CurrentSdg.SampledByOperator = (Operator)dropDownOperators.SelectedValue;
            }
            if (dropDownPerformedOperators.SelectedValue != null)
            {
                CurrentSdg.PerformedOperator = (Operator)dropDownPerformedOperators.SelectedValue;
            }

            if (dropDownLabs.SelectedValue != null)
            {
                CurrentSdg.LabInfo = (LabInfo)dropDownLabs.SelectedValue;
            }
            if (dropDownLocation.SelectedValue != null)
            {
                CurrentSdg.Location = (Location)dropDownLocation.SelectedValue;
            }
            if (dropDownStorageConditions.SelectedValue != null)
            {
                var pe = (PhraseEntry)dropDownStorageConditions.SelectedValue;
                CurrentSdg.StorageConditions = pe.PhraseName;
            }
            CurrentSdg.U_COA_FILE = txtCoaFile.Text;
            _additionalDetailsFactory.UpdateSdg(CurrentSdg);

            //if (IsWaterLab(CurrentSdg.LabInfo))
            //{
            //    additionalDetails.UpdateSdg(CurrentSdg);
            //    //CurrentSdg.WaterTemperature = spinWaterTemperature.Value;

            //    //if (datePickerSamplingTime.Value != default(DateTime))
            //    //    CurrentSdg.SamplingTime = datePickerSamplingTime.Value;// Convert.ToDateTime(d);




            //    //CurrentSdg.U_MINISTRY_OF_HEALTH = dropDownHealthMinstry.Text;
            //    //                CurrentSdg.SamplingTime = timeSampledWater.Value;
            //}

        }

        public bool ValidateSdg()
        {
            ChoosenLab = (LabInfo)dropDownLabs.SelectedValue;
            //Check mandatory fields
            if (dropDownLabs.SelectedValue == null || dropDownOperators.SelectedValue == null)
            {
                return false;
            }
            //וולידציות למעבדת מים
            if (/*IsWaterLab(ChoosenLab) &&*/ _additionalDetailsFactory.ValidateSdg() == false)
            {
                return false;
            }

            DateTime dt = datePickerRecivedTime.Value;
            if (dt.Year < 2000)
            {
                DialogResult dialogResult = MessageBox.Show("שנת קליטה קטנה מ-2000, האם להשאיר כך?", "שנה לא תיקנית", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    DateTime newDate = new DateTime(DateTime.Now.Year, dt.Month, dt.Day);
                    datePickerRecivedTime.Value = newDate;
                }
            }


            return true;

        }


        /// <summary>
        /// Update order details for new sdg
        /// </summary>
        /// <param name="newSdg"></param>
        public void InsertSdg(Sdg newSdg)
        {
            newSdg.Client = CurrentClient;
            newSdg.Comments = RichTxtRemarks.Text;
            //   newSdg.SampledOn = DateTime.Now;
            newSdg.LastUpdated = DateTime.Now;
            newSdg.DeliveryDate = datePickerRecivedTime.Value;
            newSdg.StorageConditions = dropDownStorageConditions.Text;

            //new
            newSdg.U_COA_REMARKS = DropDownCOARemarks.Text;

            newSdg.U_SAMPLING_SITE = txtSamplingSite.Text;
            //
            newSdg.Temperature = spinTemperature.Value;
            newSdg.ExternalReference = txtExternalReference.Text;

            var performedoperator = (Operator)dropDownPerformedOperators.SelectedValue;
            if (performedoperator != null)
                newSdg.PerformedOperator = performedoperator;

            var selectedoperator = (Operator)dropDownOperators.SelectedValue;
            if (selectedoperator != null)
                newSdg.SampledByOperator = selectedoperator;

            var selectedFoodTemp = (PhraseEntry)dropDownTemp.SelectedValue;
            if (selectedFoodTemp != null)
                newSdg.U_FOOD_TEMPERATURE = selectedFoodTemp.PhraseDescription;

            var selectedLocation = (Location)dropDownLocation.SelectedValue;
            if (selectedLocation != null)
                newSdg.Location = selectedLocation;

            newSdg.U_COA_FILE = txtCoaFile.Text;
            var selectedLab = (LabInfo)dropDownLabs.SelectedValue;
            if (selectedLab != null)
            {
                //   newSdg.LabInfoId=selectedLab.LabInfoId;//.LabInfo = selectedLab;
                // CurrentClient.LabInfo = selectedLab;
            }


            _additionalDetailsFactory.InsertSdg(newSdg);

            //if (IsWaterLab(newSdg.LabInfo))
            //{
            //    additionalDetails.InsertSdg(newSdg);
            //    //if (datePickerSamplingTime.Value != default(DateTime))
            //    //    CurrentSdg.SamplingTime = datePickerSamplingTime.Value;// Convert.ToDateTime(d);
            //    //newSdg.U_MINISTRY_OF_HEALTH = dropDownHealthMinstry.Text;
            //    //newSdg.WaterTemperature = spinWaterTemperature.Value;
            //}

        }


        public event Action<bool> HasChange;
        public void Exit()
        {
        }

        #endregion

        #region Fill dropDowns

        private void GetLabs()
        {

            dropDownLabs.DisplayMember = "LabHebrewName";
            dropDownLabs.DataSource = ListData.Labs;
            dropDownLabs.SelectedIndex = -1;
        }

        private void GetTemps()
        {

            dropDownTemp.DisplayMember = "PhraseDescription";
            dropDownTemp.DataSource = ListData.CoaTemp;
            dropDownTemp.SelectedIndex = -1;
        }

        private void GetOperators()
        {

            dropDownOperators.DisplayMember = "Name";
            dropDownOperators.DataSource = ListData.OperatorsByRole;
            dropDownOperators.SelectedIndex = -1;
            dropDownOperators.SelectedText = "";
        }


        private void GetOperatorsByGroup()
        {

            dropDownPerformedOperators.DisplayMember = "Name";
            if (ListData.OperatorsByGroup != null)
                dropDownPerformedOperators.DataSource = ListData.OperatorsByGroup;
            dropDownPerformedOperators.SelectedIndex = -1;
        }

        private void GetLocations()
        {
            dropDownLocation.DisplayMember = "Name";
            dropDownLocation.DataSource = ListData.Locations;
            dropDownLocation.SelectedIndex = -1;
        }

        private void GetStorageCondition()
        {
            dropDownStorageConditions.DisplayMember = "PhraseName";
            dropDownStorageConditions.DataSource = ListData.StorageConditionsList;
            dropDownStorageConditions.SelectedIndex = -1;
        }
        private void GetCOARemarks()
        {
            DropDownCOARemarks.DisplayMember = "PhraseName";
            DropDownCOARemarks.DataSource = ListData.CoaRemarksList;
            DropDownCOARemarks.SelectedIndex = -1;
        }

        #endregion

        #region Events
        private void btnCopyTempertature_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("? האם אתה בטוח שברצונך להעתיק את הטמפרטורה לכל הדוגמאות ", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            if (dr == DialogResult.Yes)
            {
                if (TemperatureCopied != null) TemperatureCopied(spinTemperature.Value);
            }
        }

        private void btnCopyOperator_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("? האם אתה בטוח שברצונך להעתיק את שם הדוגם לכל הדוגמאות", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            if (dr == DialogResult.Yes)
            {
                var selectedOperator = (Operator)dropDownOperators.SelectedValue;
                if (OperatorCopied != null && selectedOperator != null) OperatorCopied(selectedOperator);
            }
        }

        private void dropDownLabs_SelectedValueChanged(object sender, EventArgs e)
        {


            //הצגת הפאנל רק כאשר נבחרה מעבדת מים
            if (dropDownLabs.SelectedIndex > -1)
            {
                var lab = (LabInfo)dropDownLabs.SelectedValue;
                OnLabChanged(lab);
                dropDownOperators.DataSource = ListData.OperatorsByRole;
                dropDownOperators.SelectedIndex = -1;
                dropDownPerformedOperators.DataSource = ListData.OperatorsByGroup;
                dropDownPerformedOperators.SelectedIndex = -1;

                if (lab.Name == FoodLabName || lab.Name == SlaughterLabName)
                {
                    dropDownTemp.Visible = true;
                    spinTemperature.Visible = false;
                    radButton2.Visible = false;
                    spinTemperature.SendToBack();
                    dropDownTemp.BringToFront();
                }
                else
                {
                    
                    dropDownTemp.Visible = false;
                    spinTemperature.Visible = true;
                    radButton2.Visible = true;
                    dropDownTemp.SendToBack();
                    spinTemperature.BringToFront();
                }

                //אם נבחרה מעבדת קוסמטיקה והמסך במצב עדכון לא יתאפשר לשנות נדגם ע"י
                if (lab.Name == CosmeticsLabName && _isUpdatedState)
                {
                    dropDownOperators.Enabled = false;
                }
                else
                {
                    dropDownOperators.Enabled = true;

                }
                btnAdditionalD.Text = lab.Name;
                _additionalDetailsFactory.LabName = btnAdditionalD.Text;
                panelAdditionalDetails.Visible = lab.Name == WaterLabName || lab.Name == EnvironmentLabName;




            }
        }

        private void Text_Changed(object sender, EventArgs e)
        {
            //התראה למסך הראשי על כל שינוי שנעשה בפרטי ההזמנה
            if (HasChange != null) HasChange(true);
        }
        void AdditionalDetailsFactoryHasChange(bool obj)
        {
            if (HasChange != null) HasChange(true);

        }
        /// <summary>
        /// Checks if entered sdg is from water lab
        /// </summary>
        /// <param name="lab"></param>
        /// <returns></returns>   
        private bool IsWaterLab(LabInfo lab)
        {
            if (lab == null)
            {
                return false;
            }
            return lab.Name == WaterLabName;
        }

        #endregion



        private void radButton1_Click(object sender, EventArgs e)
        {
            _additionalDetailsFactory.StartPosition = FormStartPosition.CenterParent;
            _additionalDetailsFactory.ShowDialog();
        }


        private void radTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            var vld = Path.GetInvalidFileNameChars();
            if (vld.Contains(e.KeyChar) || e.KeyChar == '.')
            {
                e.Handled = true;
            }
        }







    }
}