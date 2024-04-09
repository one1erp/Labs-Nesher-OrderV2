using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FcsSampleRequestV2;
using DAL;
using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;

namespace OrderV2.Controls
{
    public partial class FCS_Window : Form
    {

        public string CreatedSdgName { get; private set; }


        public FCS_Window(INautilusServiceProvider serviceProvider, IExtensionWindowSite2 _ntlsSite, IDataLayer dal)
        {

            InitializeComponent();
            var fcsSampleRequest2 = new FcsSampleRequestV2.FcsSampleRequestV2Ctrl(serviceProvider, _ntlsSite, dal);
            fcsSampleRequest2.Init();
            this.Controls.Add(fcsSampleRequest2);
            fcsSampleRequest2.NewSdgCreated += fcsSampleRequest2_SdgCreated;
        }

        void fcsSampleRequest2_SdgCreated(string sdgName)
        {

            //if נוצר sdg

            if (!string.IsNullOrEmpty(sdgName))
            {

            CreatedSdgName = sdgName;

              this.Close();

            }
            else
            {
                CreatedSdgName = ""; ;

            }


        }

        private void FCS_Window_Load(object sender, EventArgs e)
        {

        }
    }
}
