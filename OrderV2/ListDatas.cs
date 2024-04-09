using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DAL;
//using Telerik.WinControls.RichTextBox.Model;

namespace OrderV2
{
    /// <summary>
    /// Load all lists of data for program
    /// </summary>   
    public class ListDatas
    {
        private const string CATEGORY_PRODUCT = "Category";
        public ListDatas(IDataLayer dal)
        {

            Clients = dal.GetClients();
            Addresses = dal.GetAddressesByTable("CLIENT");

            Operators = dal.GetOperatorsIncludeALL().ToList();

            Labs = dal.GetLabs();

            //    Labs=Labs.Where(g=>g.GroupID)
            Products = dal.GetProducts();

            Locations = dal.GetLocations();


            StorageConditionsList = dal.GetPhraseByName("תנאי אחסון").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            OrderStatusList = dal.GetPhraseByName("Order Status").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            MinistryHealthList = dal.GetPhraseByName("Ministry of health district").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            CoaRemarksList = dal.GetPhraseByName("COA Remarks").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            TabMethodList = dal.GetPhraseByName("Tab Method").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            WaterSpecifications = dal.GetPhraseByName("Water specifications").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            SdgPriority = dal.GetPhraseByName("Sdg Priority").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            SamplingType = dal.GetPhraseByName("Sampling type").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();
            CoaTemp = dal.GetPhraseByName("COA Temp").PhraseEntries.OrderBy(o => o.ORDER_NUMBER).ToList();

        }

        //Phrases
        public List<PhraseEntry> StorageConditionsList { get; private set; }
        public List<PhraseEntry> OrderStatusList { get; private set; }
        public List<PhraseEntry> MinistryHealthList { get; private set; }
        public List<PhraseEntry> CoaRemarksList { get; private set; }
        public List<PhraseEntry> TabMethodList { get; private set; }
        public List<PhraseEntry> WaterSpecifications { get; private set; }
        public List<PhraseEntry> SdgPriority { get; private set; }
        public List<PhraseEntry> SamplingType { get; private set; }
        public List<PhraseEntry> CoaTemp{ get; private set;}
      


        //Operators
        public List<Operator> OperatorsByRole { get; private set; }
        public List<Operator> OperatorsByGroup { get; private set; }
        private List<Operator> Operators;

        //Products
        private List<Product> Products;
        public List<Product> CategoryProducts { get; private set; }
        public List<Product> RealProducts { get; private set; }


        public List<LabInfo> Labs { get; private set; }
        public List<Location> Locations { get; private set; }
        public List<Address> Addresses { get; private set; }
        public List<Client> Clients { get; private set; }





        public void SetListByGroup(long groupId)
        {

            CategoryProducts =
                (from item in Products
                 where item.ProductType == CATEGORY_PRODUCT && item.GroupID == groupId
                 select item)
                    .ToList();
            RealProducts =
                (from item in Products
                 where string.IsNullOrEmpty(item.ProductType) && item.GroupID == groupId
                 select item).ToList();

            OperatorsByGroup = new List<Operator>();
            foreach (var pp in Operators)
            {

                foreach (OperatorGroup operatorGroup in pp.OperatorGroups)
                {
                    if (operatorGroup.GroupId == groupId)
                    {
                        if (pp.OperatorRole.Any(role => role.Name == "Operator"))
                        {
                            OperatorsByGroup.Add(pp);
                            break;
                        }

                    }
                }
            }
        }

        public void SetOperatorsByRole(string roleName)
        {
            OperatorsByRole = new List<Operator>();

            foreach (var pp in Operators)
            {
                foreach (LimsRole operatorGroup in pp.OperatorRole)
                {
                    if (operatorGroup.Name == roleName)
                    {


                        OperatorsByRole.Add(pp);
                        break;
                    }
                }
            }


        }




        internal Operator GetOperatorByName(string u)
        {
            return (from item in Operators
                    where item.Name == u
                    select item).SingleOrDefault();
        }
    }
}

