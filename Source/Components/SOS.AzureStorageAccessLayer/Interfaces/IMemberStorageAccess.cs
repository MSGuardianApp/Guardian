using SOS.AzureStorageAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOS.AzureStorageAccessLayer
{
    public interface IMemberStorageAccess
    {
        PhoneValidation CreatePhoneValidatorEntry(PhoneValidation PhValidator, ref bool IsReused);

        void SavePhoneValidationRecord(PhoneValidation PhValidator);

        PhoneValidation LoadPhoneValidation(string LiveID, string PhoneNumber);

        bool DeletePhoneValidationEntry(PhoneValidation PhValidationToDelete);

        void SaveGroupAdmin(AdminUser grpAdmin);

        List<PhoneValidation> GetAllPhoneValidationData();

        int PhoneValidationCount();

        void DeleteHistoryForProfile(string ProfileID);

        Task DeleteLocationHistoryForProfileAsync(string ProfileID);

        Task DeleteSessionHistoryForProfileAsync(string ProfileID);

        #region Methods for OPS tools

        List<PhoneValidation> GetAllPendingVerificationProfiles(int days);

        List<object> GetSecurityTokenByNumber(string number);

        #endregion
    }

}
