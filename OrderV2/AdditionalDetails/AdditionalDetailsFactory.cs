using System;
using System.Windows.Forms;
using DAL;
using OrderV2.Controls;

namespace OrderV2.AdditionalDetails
{
    public partial class AdditionalDetailsFactory : Form//
    {

        private const string WATER = "Water";
        private const string ENVIRONMENT = "Environment";


        public event Action<string> SamplingDateCopied;
        public event Action<string> SamplingTimeCopied;
        public event Action<bool> HasChange;
        private UserControl labControl;
        private string _labName;

        public string LabName
        {
            get { return _labName; }
            set
            {
                this.Controls.Clear();
                bool assign = false;
                _labName = value;

                if (_labName == WATER)
                {
                    _additionalDetails = new WaterCtrl(ListData);

                }
                else if (_labName == ENVIRONMENT)
                {
                    _additionalDetails = new EnvironmentCtrl(ListData);
                }
                else
                {
                    _additionalDetails = null;
                }


                if (_additionalDetails != null)
                {

                    InitControl();
                }
            }
        }

        private void InitControl()
        {
            _additionalDetails.SamplingDateCopied += _additionalDetails_SamplingDateCopied;
            _additionalDetails.SamplingTimeCopied += _additionalDetails_SamplingTimeCopied;
            _additionalDetails.HasChange += _additionalDetails_HasChange;
            _additionalDetails.Init();
            _additionalDetails.Clear();
            var ctrl = _additionalDetails as UserControl;
            ctrl.Dock = DockStyle.Fill;
            this.Controls.Add(ctrl);
        }



        void _additionalDetails_HasChange(bool obj)
        {
            if (HasChange != null) HasChange(obj);
        }

        void _additionalDetails_SamplingDateCopied(string obj)
        {
            if (SamplingDateCopied != null)
                SamplingDateCopied(obj);
        }
        void _additionalDetails_SamplingTimeCopied(string obj)
        {
            if (SamplingTimeCopied != null)
                SamplingTimeCopied(obj);
        }
        private IAdditionalDetails _additionalDetails;

        #region Ctor

        public AdditionalDetailsFactory(ListDatas listDatas)
        {
            InitializeComponent();
            this.ListData = listDatas;

        }

        public ListDatas ListData { get; private set; }

        #endregion


        internal void DisplaySdgDetails(Sdg sdg)
        {
            if (_additionalDetails != null) _additionalDetails.DisplaySdgDetails(sdg);
        }

        internal void Clear()
        {
            if (_additionalDetails != null) _additionalDetails.Clear();
        }

        internal void UpdateSdg(Sdg CurrentSdg)
        {
            if (_additionalDetails != null) _additionalDetails.UpdateSdg(CurrentSdg);
        }

        internal bool ValidateSdg()
        {
            if (_additionalDetails == null) return true;

            return _additionalDetails.ValidateSdg();

        }

        internal void InsertSdg(Sdg newSdg)
        {
            if (_additionalDetails != null) _additionalDetails.InsertSdg(newSdg);
        }


        private void AdditionalDetails_Load(object sender, EventArgs e)
        {

        }





    }
}
