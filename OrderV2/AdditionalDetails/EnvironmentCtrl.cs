using System;
using System.Linq;
using System.Windows.Forms;
using Common;
using DAL;
using OrderV2.Controls;

namespace OrderV2.AdditionalDetails
{
    public partial class EnvironmentCtrl : UserControl, IAdditionalDetails
    {
        public EnvironmentCtrl(ListDatas listDatas)
        {
            this.ListData = listDatas;
            InitializeComponent();
        }

        private ListDatas ListData;

        private void GetPrority()
        {
            ddlPriority.DisplayMember = "PhraseDescription";
            if (ListData != null)
                ddlPriority.DataSource = ListData.SdgPriority;
            ddlPriority.SelectedIndex = -1;
        }

        private void GetSamplingType()
        {
            ddlSamplingType.DisplayMember = "PhraseName";
            if (ListData != null)
                ddlSamplingType.DataSource = ListData.SamplingType;
            ddlSamplingType.SelectedIndex = -1;
        }

        public void Init()
        {
            GetPrority();
            GetSamplingType();

            spinRecivedTemperature.SpinElement.BackColor = NautilusDesign.MandatoryColor;
            startDateTimePicker.DateTimePickerElement.TextBoxElement.BackColor = NautilusDesign.MandatoryColor;
        }

        public void Clear()
        {

            startDateTimePicker.Value = default(DateTime);
            endDateTimePicker.Value = default(DateTime);
            spinRecivedTemperature.Value = default(decimal);

            ddlPriority.Text = "";
            ddlPriority.SelectedIndex = -1;
            ddlSamplingType.Text = "";
            ddlSamplingType.SelectedIndex = -1;

            SetDefaults();
        }

        private void SetDefaults()
        {
            var priority = ListData.SdgPriority.FirstOrDefault(x => x.PhraseName == "1");
            if (priority != null)
                ddlPriority.SelectedValue = priority;

            var samplingType = ListData.SamplingType.FirstOrDefault(x => x.PhraseName == "אקראי");
            if (samplingType != null)
                ddlSamplingType.SelectedValue = samplingType;
        }

        public void DisplaySdgDetails(Sdg sdg)
        {
            try
            {


                //תאריך התחלת דיגום
                if (sdg.U_START_SAMPLING != null)
                    startDateTimePicker.Value = sdg.U_START_SAMPLING.Value;

                //תאריך סיום דיגום

                if (sdg.U_END_SAMPLING != null)
                    endDateTimePicker.Value = sdg.U_END_SAMPLING.Value;


                //טמפרטורה
                if (sdg.WaterTemperature != null) spinRecivedTemperature.Value = (decimal)sdg.WaterTemperature;

                //דחיפות
                var p = ListData.SdgPriority.Where(x => x.PhraseName == sdg.U_PRIORITY).FirstOrDefault();
                if (p != null)
                    ddlPriority.SelectedValue = p;
                //סוג דיגום
                ddlSamplingType.SelectedText = sdg.U_SAMPLING_TYPE;


            }
            catch (Exception)
            {

                MessageBox.Show("Error in DisplaySdgDetails");
            }
        }

        public void UpdateSdg(Sdg sdg)
        {

            //תאריך התחלת דיגום
            if (startDateTimePicker.Value != default(DateTime))
                sdg.U_START_SAMPLING = startDateTimePicker.Value;

            //תאריך סיום דיגום
            if (endDateTimePicker.Value != default(DateTime))
                sdg.U_END_SAMPLING = endDateTimePicker.Value;


            //טמפרטורה
            sdg.WaterTemperature = spinRecivedTemperature.Value;

            ////דחיפות
            //sdg.U_PRIORITY = ddlPriority.Text;
            //דחיפות
            var prio = ddlPriority.SelectedValue as PhraseEntry;
            if (prio != null)
                sdg.U_PRIORITY = prio.PhraseName;

            //סוג דיגום
            sdg.U_SAMPLING_TYPE = ddlSamplingType.Text;


        }




        public void InsertSdg(Sdg newSdg)
        {
            //תאריך התחלת דיגום
            if (startDateTimePicker.Value != default(DateTime))
                newSdg.U_START_SAMPLING = startDateTimePicker.Value;

            //תאריך סיום דיגום
            if (endDateTimePicker.Value != default(DateTime))
                newSdg.U_END_SAMPLING = endDateTimePicker.Value;


            //טמפרטורה
            newSdg.WaterTemperature = spinRecivedTemperature.Value;




            //דחיפות
            var prio = ddlPriority.SelectedValue as PhraseEntry;
            if (prio != null)
                newSdg.U_PRIORITY = prio.PhraseName;

            //סוג דיגום
            newSdg.U_SAMPLING_TYPE = ddlSamplingType.Text;
        }


        public bool ValidateSdg()
        {
            //אם אחד מהשדות לא עודכן
            if (startDateTimePicker.Value == default(DateTime)
)
            {

                return false;
            }
            else if (spinRecivedTemperature.Value == default(decimal))
            {
                One1.Controls.CustomMessageBox.Show("לא הוזנה טמפרטורת קבלה.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return true;
        }




        public event Action<string> SamplingDateCopied;
        public event Action<string> SamplingTimeCopied;
        public event Action<bool> HasChange;

        private void btnClose_Click(object sender, EventArgs e)
        {
        
            if (ParentForm != null)
                ParentForm.Hide();
        }

        private void TextChanged(object sender, EventArgs e)
        {
            //התראה למסך הראשי על כל שינוי שנעשה בפרטי ההזמנה
            if (HasChange != null) HasChange(true);
        }
    }
}
