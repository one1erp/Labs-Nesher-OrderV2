using System;
using System.Windows.Forms;
using Common;
using DAL;

namespace OrderV2.AdditionalDetails
{
    public partial class WaterCtrl : UserControl, IAdditionalDetails
    {

        #region Fields

        public event Action<bool> HasChange;
        private ListDatas ListData;
        public event Action<string> SamplingDateCopied;
        public event Action<string> SamplingTimeCopied;
        public bool IsUpdatedState { get; set; }

        #endregion

        #region Ctor

        public WaterCtrl(ListDatas listDatas)
        {
            InitializeComponent();
            this.ListData = listDatas;

        }

        #endregion


        public void Init()
        {
            GetHealthMinistryPhrase();
            recivedTimePicker.TimePickerElement.MaskedEditBox.TextBoxItem.BackColor = NautilusDesign.MandatoryColor;
            testTimePicker.TimePickerElement.MaskedEditBox.TextBoxItem.BackColor = NautilusDesign.MandatoryColor;
            testDateTimePicker.DateTimePickerElement.TextBoxElement.BackColor = NautilusDesign.MandatoryColor;
            spinWaterTemperature.SpinElement.BackColor = NautilusDesign.MandatoryColor;
            samplingTimePicker.TimePickerElement.MaskedEditBox.TextBoxItem.BackColor = NautilusDesign.MandatoryColor;
            SamplingDateTimePicker.DateTimePickerElement.TextBoxElement.BackColor = NautilusDesign.MandatoryColor;

        }

        public void Clear()
        {
            recivedTimePicker.Value = default(DateTime);
            testDateTimePicker.Value = default(DateTime);
            testTimePicker.Value = default(DateTime);
            spinWaterTemperature.Value = default(decimal);
            samplingTimePicker.Value = default(DateTime);
            SamplingDateTimePicker.Value = default(DateTime);
            ddlHealthMinstry.Text = "";
            ddlHealthMinstry.SelectedIndex = -1;

        }

        #region Get Data

        private void GetHealthMinistryPhrase()
        {
            ddlHealthMinstry.DisplayMember = "PhraseName";
            if (ListData != null)
                ddlHealthMinstry.DataSource = ListData.MinistryHealthList;
            ddlHealthMinstry.SelectedIndex = -1;
        }



        #endregion

        public void InsertSdg(Sdg newSdg)
        {
            //שעת קבלה
            if (recivedTimePicker.Value.HasValue && recivedTimePicker.Value != default(DateTime))
            {
                var time = recivedTimePicker.Value.GetValueOrDefault().ToShortTimeString();
                newSdg.U_TXT_RECIVED_TIME = time;
            }

            //טמפרטורה
            newSdg.WaterTemperature = spinWaterTemperature.Value;

            //שעת ותאריך  דיגום
            string samplingTime = null;
            if (samplingTimePicker.Value.HasValue && samplingTimePicker.Value != default(DateTime))
                samplingTime = samplingTimePicker.Value.GetValueOrDefault().ToShortTimeString();
            string date = SamplingDateTimePicker.Value.ToShortDateString();
            string dt = date + " " + samplingTime;
            newSdg.U_TXT_SAMPLING_TIME = dt;

            //נפה מחוז
            newSdg.U_MINISTRY_OF_HEALTH = ddlHealthMinstry.Text;

            //תאריך ושעת בדיקה
            UpdateTestDateTime(newSdg);
        }

        public event Action ControlClosed;

        public void DisplaySdgDetails(Sdg sdg)
        {

            try
            {

                //שעת קבלה
                if (sdg.U_TXT_RECIVED_TIME != null)
                    recivedTimePicker.Value = Convert.ToDateTime(sdg.U_TXT_RECIVED_TIME);

                //טמפרטורה
                if (sdg.WaterTemperature != null) spinWaterTemperature.Value = (decimal)sdg.WaterTemperature;


                //תאריך ושעת דיגום
                if (!string.IsNullOrEmpty(sdg.U_TXT_SAMPLING_TIME))
                {
                    SamplingDateTimePicker.Value = Convert.ToDateTime(sdg.U_TXT_SAMPLING_TIME);
                    var dtValue = sdg.U_TXT_SAMPLING_TIME;
                    var st = dtValue.Substring(dtValue.Length - 6, 6);
                    samplingTimePicker.Value = Convert.ToDateTime(st);
                }

                //נפה מחוז
                ddlHealthMinstry.SelectedText = sdg.U_MINISTRY_OF_HEALTH;

                if (sdg.U_TEST_DATE_TIME != null)
                {
                    var d = Convert.ToDateTime(sdg.U_TEST_DATE_TIME);
                    testDateTimePicker.Value = d;
                    testTimePicker.Value = d;
                }

            }
            catch (Exception e)
            {
                Logger.WriteLogFile(e);
                One1.Controls.CustomMessageBox.Show("שגיאה בהצגת הנתונים בחלון פרטים נוספים!");
            }

        }

        public void UpdateSdg(Sdg currentSdg)
        {
            if (recivedTimePicker.Value.HasValue)
            {
                var time = recivedTimePicker.Value.GetValueOrDefault().ToShortTimeString();
                currentSdg.U_TXT_RECIVED_TIME = time;
            }
            currentSdg.WaterTemperature = spinWaterTemperature.Value;

            //  currentSdg.U_TXT_SAMPLING_TIME = txtDateTimeSampling.Text;
            string samplingTime = null;

            if (samplingTimePicker.Value.HasValue && samplingTimePicker.Value != default(DateTime))
            {
                samplingTime = samplingTimePicker.Value.GetValueOrDefault().ToShortTimeString();

            }
            string date = SamplingDateTimePicker.Value.ToShortDateString();
            string dt = date + " " + samplingTime;

            currentSdg.U_TXT_SAMPLING_TIME = dt;


            currentSdg.U_MINISTRY_OF_HEALTH = ddlHealthMinstry.Text;


            //תאריך ושעת בדיקה
            UpdateTestDateTime(currentSdg);
        }



        public bool ValidateSdg()
        {
            //אם אחד מהשדות לא עודכן
            if (recivedTimePicker.Value == default(DateTime)
                || SamplingDateTimePicker.Value == default(DateTime)
                || samplingTimePicker.Value == default(DateTime)
                || testDateTimePicker.Value == default(DateTime)
                 || testTimePicker.Value == default(DateTime))
            {

                return false;
            }
            else if (spinWaterTemperature.Value == default(decimal))
            {
                One1.Controls.CustomMessageBox.Show("לא הוזנה טמפרטורת קבלה.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return true;
        }

        private void UpdateTestDateTime(Sdg sdg)
        {
            if (testTimePicker.Value != null && testDateTimePicker.Value != default(DateTime))
            {
                var date = testDateTimePicker.Value.ToString("yyyy/MM/dd");

                var time = testTimePicker.Value.Value.ToString("HH/mm");

                var d =
                    DateTime.ParseExact(date + " " + time, "yyyy/MM/dd HH/mm",
                        null);
                sdg.U_TEST_DATE_TIME = d;
            }
        }


        #region Events

        private void btnCopyDate_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (ParentForm != null)
                ParentForm.Hide();

        }

        private void txtDateTimeSampling_TextChanged(object sender, EventArgs e)
        {
            //התראה למסך הראשי על כל שינוי שנעשה בפרטי ההזמנה
            if (HasChange != null) HasChange(true);
        }

        #endregion

        private void btnCopySamlpingDate_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("? האם אתה בטוח שברצונך להעתיק את תאריך הדיגום לכל הדוגמאות ", "",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);

            if (dr == DialogResult.Yes)
            {
                var date = SamplingDateTimePicker.Value.ToString("yyyy/MM/dd");
                if (SamplingDateCopied != null) SamplingDateCopied(date);
            }
        }

        private void btnCopySamlpingTime_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("? האם אתה בטוח שברצונך להעתיק את שעת הדיגום לכל הדוגמאות ", "",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);

            if (dr == DialogResult.Yes)
            {
                var time = samplingTimePicker.Value.GetValueOrDefault().ToShortTimeString();
                if (SamplingTimeCopied != null) SamplingTimeCopied(time);
            }
        }



        private void GetWaterSpecifications()
        {
            ddlSpecifications.DisplayMember = "PhraseName";
            if (ListData != null)
                ddlSpecifications.DataSource = ListData.WaterSpecifications;
            ddlSpecifications.SelectedIndex = -1;
        }

        private void SamplingDateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

    }
}