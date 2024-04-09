using System;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace OrderV2.Controls
{
    public partial class RenameHeaderText : Form
    {
        #region Ctor
        public RenameHeaderText(RadGridView gridSamples, string currentHeader)
        {
            InitializeComponent();

            this.gridSamples = gridSamples;
            _currentHeader = currentHeader;
            this.txtNewHeader.Text = currentHeader;
            txtNewHeader.Focus();
            txtNewHeader.Select();
        }
        #endregion

        #region Fields
        public event Action<string> HeaderChanged;
        private readonly RadGridView gridSamples;
        private readonly string _currentHeader;
        #endregion

        #region Buttons

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            //Close window
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var list = gridSamples.Columns.Select(column => column.HeaderText).ToList();
            //אם קיימת עמודה עם שם זהה
            if (list.Contains(txtNewHeader.Text) && txtNewHeader.Text != _currentHeader)
            {
                radLabel1.Visible = true;
                return;
            }
            if (HeaderChanged != null)
                //Save changes in DB
                HeaderChanged(txtNewHeader.Text);

            //Close window
            Close();
        }

        #endregion
    }
}
