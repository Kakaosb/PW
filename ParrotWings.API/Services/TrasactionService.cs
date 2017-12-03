using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParrotWings.API.Controllers;
using ParrotWings.API.Models;
using ParrotWings.API.Models.ModelHelpers;

namespace ParrotWings.API.Services
{
    public class TrasactionService : TransactionController
    {
        private static ParrotWingsContext _db;
        private static User _currentUser;
        private static string _currentUserName;

        public TrasactionService(ParrotWingsContext db, string currentUserName)
        {
            _db = db;
            _currentUserName = currentUserName;
            _currentUser = db.Users
                .FirstOrDefault(el => el.Name == _currentUserName);
        }

        public List<OperationsLogTable> GetTransactionLogList()
        {
            var userId = _currentUser.Id;

            var list = _db.TransactionLogs
                .Where(el => el.RecipientId == userId || el.SenderId == userId)
                .ToList();

            var logsList = list
                .Select(el =>
                    new OperationsLogTable()
                    {
                        Sum = el.RecipientId == userId ? el.Sum : el.Sum * (-1),

                        TransferType = el.RecipientId == userId
                            ? TransferType.Debit.ToString()
                            : TransferType.Credit.ToString(),

                        UserName = el.RecipientId == userId ? el.Sender.Name : el.Recipient.Name,
                        OperationDate = el.CreateDateTime.ToString("G"),
                        Total = el.RecipientId == userId ? el.TotalRecipient : el.TotalSender
                    }
                )
                .OrderByDescending(el => el.OperationDate)
                .ToList();

            return logsList;
        }

        public void CreateTransactionLog(TransactionLog transactionLog, out string recipientName)
        {
            var recipient = _db.Users
                .FirstOrDefault(el => el.Id == transactionLog.RecipientId);

            recipientName = recipient.Name;

            _currentUser.Balance -= transactionLog.Sum;
            recipient.Balance += transactionLog.Sum;

            transactionLog.SenderId = _currentUser.Id;
            transactionLog.CreateDateTime = DateTime.Now;
            transactionLog.TotalRecipient = recipient.Balance;
            transactionLog.TotalSender = _currentUser.Balance;

            _db.TransactionLogs.Add(transactionLog);

            _db.SaveChanges();
        }

        public string CheckParties(int recipientId)
        {

          var recipient = _db.Users
                .FirstOrDefault(el => el.Id == recipientId);

            if (_currentUser == null || recipient == null)
                return "One of the parties not found. Contact your administrator.";

            if (_currentUser.Id == recipientId)
                return "You can not send PW to yourself";

            return "Ok";
        }

        public string CheckSolvency(decimal transactionSum)
        {
            if (_currentUser.Balance < transactionSum)
                return "You don't have enough PW";

            return "Ok";
        }
    }
}