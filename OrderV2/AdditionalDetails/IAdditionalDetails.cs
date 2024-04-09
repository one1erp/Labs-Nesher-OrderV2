using System;
using DAL;

namespace OrderV2.AdditionalDetails
{
    interface IAdditionalDetails
    {
        void Init();

        void Clear();

        void DisplaySdgDetails(Sdg sdg);

        void UpdateSdg(Sdg sdg);

        bool ValidateSdg();

        void InsertSdg(Sdg newSdg);

        event Action<string> SamplingDateCopied;

        event Action<string> SamplingTimeCopied;

        event Action<bool> HasChange;
    }

}
