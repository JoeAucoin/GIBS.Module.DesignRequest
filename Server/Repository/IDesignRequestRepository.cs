using GIBS.Module.DesignRequest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.DesignRequest.Repository
{
    public interface IDesignRequestRepository
    {
        IEnumerable<Models.DesignRequest> GetDesignRequests(int ModuleId);
        Models.DesignRequest GetDesignRequest(int DesignRequestId);
        Models.DesignRequest GetDesignRequest(int DesignRequestId, bool tracking);
        Models.DesignRequest AddDesignRequest(Models.DesignRequest DesignRequest);
        Models.DesignRequest UpdateDesignRequest(Models.DesignRequest DesignRequest);
        void DeleteDesignRequest(int DesignRequestId);

        // New methods for pagination
        IEnumerable<Models.DesignRequest> GetDesignRequests(int moduleId, int page, int pageSize);
        int CountDesignRequests(int moduleId);

        // FileToRequest methods
        IEnumerable<Models.FileToRequest> GetFileToRequests(int DesignRequestId);
        Models.FileToRequest GetFileToRequest(int FileToRequestId);
        Models.FileToRequest AddFileToRequest(Models.FileToRequest FileToRequest);
        Models.FileToRequest UpdateFileToRequest(Models.FileToRequest FileToRequest);
        void DeleteFileToRequest(int FileToRequestId);

        // UserCredit Methods
        IEnumerable<Models.UserCredit> GetUserCredits(int ModuleId);
        Models.UserCredit GetUserCredit(int UserCreditId);
        Models.UserCredit GetUserCreditByUser(int ModuleId, int UserId);
        Models.UserCredit AddUserCredit(Models.UserCredit UserCredit);
        Models.UserCredit UpdateUserCredit(Models.UserCredit UserCredit);
        void DeleteUserCredit(int UserCreditId);

        // CreditPackage Methods
        IEnumerable<Models.CreditPackage> GetCreditPackages(int ModuleId);
        Models.CreditPackage GetCreditPackage(int CreditPackageId);
        Models.CreditPackage AddCreditPackage(Models.CreditPackage CreditPackage);
        Models.CreditPackage UpdateCreditPackage(Models.CreditPackage CreditPackage);
        void DeleteCreditPackage(int CreditPackageId);

        // CreditTransaction Methods
        IEnumerable<Models.CreditTransaction> GetCreditTransactions(int ModuleId);
        IEnumerable<Models.CreditTransaction> GetCreditTransactionsByUser(int ModuleId, int UserId);
        Models.CreditTransaction GetCreditTransaction(int TransactionId);
        Models.CreditTransaction AddCreditTransaction(Models.CreditTransaction CreditTransaction);
        Models.CreditTransaction UpdateCreditTransaction(Models.CreditTransaction CreditTransaction);
        void DeleteCreditTransaction(int TransactionId);

        // PaymentRecord Methods
        IEnumerable<Models.PaymentRecord> GetPaymentRecords(int ModuleId);
        IEnumerable<Models.PaymentRecord> GetPaymentRecordsByUser(int ModuleId, int UserId);
        Models.PaymentRecord GetPaymentRecord(int PaymentId);
        Models.PaymentRecord AddPaymentRecord(Models.PaymentRecord PaymentRecord);
        Models.PaymentRecord UpdatePaymentRecord(Models.PaymentRecord PaymentRecord);
        void DeletePaymentRecord(int PaymentId);


    }
}