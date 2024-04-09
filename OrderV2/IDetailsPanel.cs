using System;
using DAL;
using LSSERVICEPROVIDERLib;


namespace OrderV2
{
    internal interface IDetailsPanel
    {

        INautilusServiceProvider ServiceProvider { get; set; }
        /// <summary>
        /// Order state
        /// </summary>
        bool IsUpdatedState { get; set; }

        /// <summary>
        /// Specified client
        /// </summary>
        Client CurrentClient { get; set; }

        Operator CurrentOperator { get; set; }

        Workstation CurrentWorkstation { get; set; }

        IDataLayer dal { get; set; }

        /// <summary>
        /// Specified sdg
        /// </summary>
        Sdg CurrentSdg { get; set; }


       

        /// <summary>
        /// Lists of all data
        /// </summary>
        ListDatas ListData { get; set; }

        /// <summary>
        /// First time when control laded
        /// </summary>
        void Initilaize();

        /// <summary>
        ///Display details when entered sdg
        /// </summary>
        /// <param name="sdg"></param>
        void DisplaySdgDetails(Sdg sdg);

        /// <summary>
        /// Display panel to  create new order
        /// </summary>
        void DisplayNew();

        /// <summary>
        /// Clear Panel
        /// </summary>
        void Clear();

        /// <summary>
        /// Update sdg details (in update state)
        /// </summary>
        void UpdateSdg();

        /// <summary>
        /// Check mandatory fields
        /// </summary>
        /// <returns></returns>
        bool ValidateSdg();

        /// <summary>
        /// When saved new order
        /// </summary>
        /// <param name="newSdg"></param>
        void InsertSdg(Sdg newSdg);

        /// <summary>
        /// Notification when data changes
        /// </summary>
        event Action<bool> HasChange;

        /// <summary>
        /// Exit from panel
        /// </summary>
        void Exit();




    }

}
