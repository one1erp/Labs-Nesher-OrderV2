using System;
using System.Linq;
using System.Windows.Forms;
using Common;
using DAL;
using LSSERVICEPROVIDERLib;

namespace OrderV2.OrderControls
{
    public partial class ClientDetails : UserControl, IDetailsPanel
    {
        #region fields

        private bool _isUpdated;
        public event Action OnClientChanged;

        #endregion

        #region Ctor
        public ClientDetails()
        {
            InitializeComponent();
        }
        #endregion

        #region Implement IDetailsPanel

        public Client CurrentClient { get; set; }
        public Operator CurrentOperator { get; set; }
      
        public Workstation CurrentWorkstation { get; set; }
        public IDataLayer dal { get; set; }
        public Sdg CurrentSdg { get; set; }
        public ListDatas ListData { get; set; }

        public void Initilaize()
        {
            //Set mandatory fields
            dropDownClients.DropDownListElement.TextBox.BackColor = NautilusDesign.MandatoryColor;

            //Populate drop down
            GetClients();
            //Set event
            dropDownClients.SelectedValueChanged += (dropDownClients_SelectedValueChanged);
        }

        public INautilusServiceProvider ServiceProvider { get; set; }

        public bool IsUpdatedState
        {
            get { return _isUpdated; }
            set
            {
                _isUpdated = value;
                dropDownClients.Enabled = !value;
            }
        }

        public void DisplaySdgDetails(Sdg sdg)
        {

            this.Enabled = true;
            //Set drop down with Client Order
            dropDownClients.SelectedValue = sdg.Client;
            this.CurrentClient = sdg.Client;

        }
        /// <summary>
        /// Clear all data
        /// </summary>
        public void Clear()
        {
            CurrentSdg = null;
            dropDownClients.SelectedIndex = -1;
            ClearClientDetails();
            OnClientChanged = null;

        }


        /// <summary>
        /// Update order with client details
        /// </summary>
        public void UpdateSdg()
        {
            if (!IsUpdatedState) return;
            CurrentSdg.Address = txtAddress.Text;
            CurrentSdg.Phone = txtClientPhone.Text;
            CurrentSdg.Emai = txtEmail.Text;
            CurrentSdg.ContactName = txtContactName.Text;
            CurrentSdg.ContactPhone = txtContactPhone.Text;
        }

        /// <summary>
        /// Cheks mandatory fields
        /// </summary>
        /// <returns></returns>
        public bool ValidateSdg()
        {
            if (dropDownClients.SelectedValue == null)
                return false;
            return true;

        }


        /// <summary>
        /// Update client details new order
        /// </summary>
        /// <param name="newSdg"></param>
        public void InsertSdg(Sdg newSdg)
        {
            newSdg.Phone = txtClientPhone.Text;
            newSdg.Emai = txtEmail.Text;
            newSdg.Address = txtAddress.Text;
            newSdg.ContactName = txtContactName.Text;
            newSdg.ContactPhone = txtContactPhone.Text;
        }

        public event Action<bool> HasChange;
        public void Exit()
        {

        }


        public void DisplayNew()
        {

            this.Enabled = true;
            dropDownClients.Enabled = true;
        }

        #endregion

        #region Events
        void dropDownClients_SelectedValueChanged(object sender, EventArgs e)
        {

            if (dropDownClients.SelectedIndex > -1)
            {


                if (_isUpdated)
                {
                    if (CurrentSdg != null)
                    {
                        //אם ההזמנה במצב עדכון הפרטים יבואו מההזמנה
                        txtAddress.Text = CurrentSdg.Address;
                        txtClientPhone.Text = CurrentSdg.Phone;
                        txtEmail.Text = CurrentSdg.Emai;
                        txtContactName.Text = CurrentSdg.ContactName;
                        txtContactPhone.Text = CurrentSdg.ContactPhone;
                    }

                }
                else
                {

                    this.CurrentClient = (Client)dropDownClients.SelectedItem.DataBoundItem;
                    var wsGroupId = CurrentWorkstation.GROUP_ID;
                    if (wsGroupId != null)
                    {
                        var lab = ListData.Labs.FirstOrDefault(l => l.GroupID == wsGroupId);
                        if (lab != null) SetClientAddress(lab.ClientAddressCode);
                    }
                    else
                    {
                        //אם לתחנה הנוכחית אין גרופ ,פרטי איש הרשר יילקחו מ COMPANY 
                        SetClientAddress("C");
                    }
                    //ADDRESS אם זו הזמנה חדשה פרטי הלקוח בהזמנה יתמלאו מ

                    //Update other panels with selected client  
                    if (OnClientChanged != null) OnClientChanged();

                    HasChange(false);

                }
            }
        }

        private void Text_Changed(object sender, EventArgs e)
        {
            if (HasChange != null) HasChange(true);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Set sdg with client address
        /// </summary>
        private void SetClientAddress(string labLetter)
        {


            Address address = ListData.Addresses.FirstOrDefault(x => x.AddressType == labLetter && x.AddressItemId == CurrentClient.ClientId);
            if (address != null)
            {
                txtContactName.Text = address.ContactMan;
                txtAddress.Text = address.FullAddress;
                txtEmail.Text = address.Email;
                txtContactPhone.Text = address.ContactMobile;
                txtClientPhone.Text = address.Phone;
            }
            else
            {
                //Clean fields from previous client
                ClearClientDetails();
            }


        }

        /// <summary>
        /// Clear client details
        /// </summary>
        private void ClearClientDetails()
        {
            txtContactName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtContactPhone.Text = string.Empty;
            txtClientPhone.Text = string.Empty;

        }
        /// <summary>
        ///Populate drop down
        /// </summary>
        private void GetClients()
        {

            dropDownClients.DisplayMember = "Name";

            dropDownClients.DataSource = ListData.Clients;

            dropDownClients.SelectedIndex = -1;

        }



        #endregion

        private void dropDownClients_PopupOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TOdo:לשאול פה האם אתה מעונין להמשיך
        }


        internal void LabChanged(LabInfo obj)
        {
            if (obj != null && obj.LimsGroup != null
               && obj.LimsGroup.Name != null) SetClientAddress(obj.LimsGroup.Name);
        }

        private void dropDownClients_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }


 
  
    }
}