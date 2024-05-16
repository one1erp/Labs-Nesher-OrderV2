using DAL;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var dal = new DataLayer();
            dal.Connect("metadata=res://*/NautilusModel1.csdl|res://*/NautilusModel1.ssdl|res://*/NautilusModel1.msl;provider=Oracle.DataAccess.Client;provider connection string='Data Source=microb;User ID=lims_sys;Password=lims_sys'");
            var a = dal.getContractByClinet(111);
            MessageBox.Show("1");

            var b = dal.getContractByClinet(111);
            MessageBox.Show("2");
        }
    }
}
