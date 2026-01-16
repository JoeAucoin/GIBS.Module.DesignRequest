using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.DesignRequest.Repository
{
    public class DesignRequestRepository : IDesignRequestRepository, ITransientService
    {
        private readonly IDbContextFactory<DesignRequestContext> _factory;

        public DesignRequestRepository(IDbContextFactory<DesignRequestContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.DesignRequest> GetDesignRequests(int ModuleId)
        {
            //using var db = _factory.CreateDbContext();
            //return db.DesignRequest.Where(item => item.ModuleId == ModuleId).ToList();
            using var db = _factory.CreateDbContext();
            return db.DesignRequest
                .Include(item => item.Files)
                .Include(item => item.Notes) // <-- Add this line
                .Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.DesignRequest GetDesignRequest(int DesignRequestId)
        {
            return GetDesignRequest(DesignRequestId, true);
        }

        public Models.DesignRequest GetDesignRequest(int DesignRequestId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.DesignRequest.Find(DesignRequestId);
            }
            else
            {
                return db.DesignRequest.AsNoTracking().FirstOrDefault(item => item.DesignRequestId == DesignRequestId);
            }
        }

        public Models.DesignRequest AddDesignRequest(Models.DesignRequest DesignRequest)
        {
            using var db = _factory.CreateDbContext();
            db.DesignRequest.Add(DesignRequest);
            db.SaveChanges();
            return DesignRequest;
        }

        public Models.DesignRequest UpdateDesignRequest(Models.DesignRequest DesignRequest)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(DesignRequest).State = EntityState.Modified;
            db.SaveChanges();
            return DesignRequest;
        }

        public void DeleteDesignRequest(int DesignRequestId)
        {
            using var db = _factory.CreateDbContext();
            Models.DesignRequest DesignRequest = db.DesignRequest.Find(DesignRequestId);
            db.DesignRequest.Remove(DesignRequest);
            db.SaveChanges();
        }

        // New method implementations for pagination
        public IEnumerable<Models.DesignRequest> GetDesignRequests(int moduleId, int page, int pageSize)
        {
            using var db = _factory.CreateDbContext();
            return db.DesignRequest
                .Where(r => r.ModuleId == moduleId)
                .OrderByDescending(r => r.DesignRequestId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int CountDesignRequests(int moduleId)
        {
            using var db = _factory.CreateDbContext();
            return db.DesignRequest
                .Count(r => r.ModuleId == moduleId);
        }


        // FileToRequest methods
        public IEnumerable<Models.FileToRequest> GetFileToRequests(int DesignRequestId)
        {
            //using var db = _factory.CreateDbContext();
            //return db.FileToRequest.Where(item => item.DesignRequestId == DesignRequestId).ToList();
            using var db = _factory.CreateDbContext();
            return db.FileToRequest.Where(item => item.DesignRequestId == DesignRequestId).OrderByDescending(f => f.CreatedOn).ToList();
        }

        public Models.FileToRequest GetFileToRequest(int FileToRequestId)
        {
            using var db = _factory.CreateDbContext();
            return db.FileToRequest.Find(FileToRequestId);
        }

        public Models.FileToRequest AddFileToRequest(Models.FileToRequest FileToRequest)
        {
            using var db = _factory.CreateDbContext();
            db.FileToRequest.Add(FileToRequest);
            db.SaveChanges();
            return FileToRequest;
        }

        public Models.FileToRequest UpdateFileToRequest(Models.FileToRequest FileToRequest)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(FileToRequest).State = EntityState.Modified;
            db.SaveChanges();
            return FileToRequest;
        }

        public void DeleteFileToRequest(int FileToRequestId)
        {
            using var db = _factory.CreateDbContext();
            Models.FileToRequest FileToRequest = db.FileToRequest.Find(FileToRequestId);
            db.FileToRequest.Remove(FileToRequest);
            db.SaveChanges();
        }


        public IEnumerable<Models.UserCredit> GetUserCredits(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.UserCredit.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.UserCredit GetUserCredit(int UserCreditId)
        {
            using var db = _factory.CreateDbContext();
            return db.UserCredit.Find(UserCreditId);
        }

        public Models.UserCredit GetUserCreditByUser(int ModuleId, int UserId)
        {
            using var db = _factory.CreateDbContext();
            return db.UserCredit.FirstOrDefault(item => item.ModuleId == ModuleId && item.UserId == UserId);
        }

        public Models.UserCredit AddUserCredit(Models.UserCredit UserCredit)
        {
            using var db = _factory.CreateDbContext();
            db.UserCredit.Add(UserCredit);
            db.SaveChanges();
            return UserCredit;
        }

        public Models.UserCredit UpdateUserCredit(Models.UserCredit UserCredit)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(UserCredit).State = EntityState.Modified;
            db.SaveChanges();
            return UserCredit;
        }

        public void DeleteUserCredit(int UserCreditId)
        {
            using var db = _factory.CreateDbContext();
            var userCredit = db.UserCredit.Find(UserCreditId);
            db.UserCredit.Remove(userCredit);
            db.SaveChanges();
        }

        // CreditPackage Methods
        public IEnumerable<Models.CreditPackage> GetCreditPackages(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.CreditPackage.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.CreditPackage GetCreditPackage(int CreditPackageId)
        {
            using var db = _factory.CreateDbContext();
            return db.CreditPackage.Find(CreditPackageId);
        }

        public Models.CreditPackage AddCreditPackage(Models.CreditPackage CreditPackage)
        {
            using var db = _factory.CreateDbContext();
            db.CreditPackage.Add(CreditPackage);
            db.SaveChanges();
            return CreditPackage;
        }

        public Models.CreditPackage UpdateCreditPackage(Models.CreditPackage CreditPackage)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(CreditPackage).State = EntityState.Modified;
            db.SaveChanges();
            return CreditPackage;
        }

        public void DeleteCreditPackage(int CreditPackageId)
        {
            using var db = _factory.CreateDbContext();
            var creditPackage = db.CreditPackage.Find(CreditPackageId);
            db.CreditPackage.Remove(creditPackage);
            db.SaveChanges();
        }

        // CreditTransaction Methods
        public IEnumerable<Models.CreditTransaction> GetCreditTransactions(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            // Best Practice: Add ModuleId to CreditTransaction model and filtering here: .Where(t => t.ModuleId == ModuleId)
            return db.CreditTransaction.OrderByDescending(t => t.TransactionDate).ToList();
        }

        public IEnumerable<Models.CreditTransaction> GetCreditTransactionsByUser(int ModuleId, int UserId)
        {
            using var db = _factory.CreateDbContext();
            // Best Practice: Add ModuleId to filtering: .Where(t => t.ModuleId == ModuleId && t.UserId == UserId)
            return db.CreditTransaction.Where(t => t.UserId == UserId).OrderByDescending(t => t.TransactionDate).ToList();
        }

        public Models.CreditTransaction GetCreditTransaction(int TransactionId)
        {
            using var db = _factory.CreateDbContext();
            return db.CreditTransaction.Find(TransactionId);
        }

        public Models.CreditTransaction AddCreditTransaction(Models.CreditTransaction CreditTransaction)
        {
            using var db = _factory.CreateDbContext();
            db.CreditTransaction.Add(CreditTransaction);
            db.SaveChanges();
            return CreditTransaction;
        }

        public Models.CreditTransaction UpdateCreditTransaction(Models.CreditTransaction CreditTransaction)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(CreditTransaction).State = EntityState.Modified;
            db.SaveChanges();
            return CreditTransaction;
        }

        public void DeleteCreditTransaction(int TransactionId)
        {
            using var db = _factory.CreateDbContext();
            var transaction = db.CreditTransaction.Find(TransactionId);
            db.CreditTransaction.Remove(transaction);
            db.SaveChanges();
        }

        // PaymentRecord Methods
        public IEnumerable<Models.PaymentRecord> GetPaymentRecords(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.PaymentRecord.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public IEnumerable<Models.PaymentRecord> GetPaymentRecordsByUser(int ModuleId, int UserId)
        {
            using var db = _factory.CreateDbContext();
            return db.PaymentRecord.Where(item => item.ModuleId == ModuleId && item.UserId == UserId).ToList();
        }

        public Models.PaymentRecord GetPaymentRecord(int PaymentId)
        {
            using var db = _factory.CreateDbContext();
            return db.PaymentRecord.Find(PaymentId);
        }

        public Models.PaymentRecord AddPaymentRecord(Models.PaymentRecord PaymentRecord)
        {
            using var db = _factory.CreateDbContext();
            db.PaymentRecord.Add(PaymentRecord);
            db.SaveChanges();
            return PaymentRecord;
        }

        public Models.PaymentRecord UpdatePaymentRecord(Models.PaymentRecord PaymentRecord)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(PaymentRecord).State = EntityState.Modified;
            db.SaveChanges();
            return PaymentRecord;
        }

        public void DeletePaymentRecord(int PaymentId)
        {
            using var db = _factory.CreateDbContext();
            var PaymentRecord = db.PaymentRecord.Find(PaymentId);
            db.PaymentRecord.Remove(PaymentRecord);
            db.SaveChanges();
        }


    }
}