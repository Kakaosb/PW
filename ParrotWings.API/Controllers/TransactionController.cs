using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using ParrotWings.API;
using ParrotWings.API.Models;
using ParrotWings.API.Models.ModelHelpers;

namespace ParrotWings.API.Controllers
{
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        private ParrotWingsContext db = new ParrotWingsContext();

       [Route("GetUsers")]
        public string GetUsers(string term)
        {
            try
            {
                var currentUserName = User.Identity.Name;

                var users = db.Users
                    .Where(el => el.Name.StartsWith(term)
                        && el.Name != currentUserName)
                    .Select(el => new
                    {
                        el.Id,
                        el.Name
                    })
                    .ToList();

                return JsonConvert.SerializeObject(users);
            }
            catch (Exception)
            {
                return "Server Error. Contact your administrator.";
            }
        }

        // GET: api/Transaction
        [Route("GetTransactionLogs")]
        public string GetTransactionLogs()
        {
            try
            {
                var currentUserName = User.Identity.Name;

                var entityCurrentUser = db.Users
                    .FirstOrDefault(el => el.Name == currentUserName);

                if (entityCurrentUser == null) return "Account Error. Contact your administrator.";

                var userId = entityCurrentUser.Id;

                var list = db.TransactionLogs
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

                return JsonConvert.SerializeObject(logsList);

            }
            catch (Exception)
            {
                return "Server Error. Contact your administrator.";
            }
        }

        // GET: api/Transaction/5
        [ResponseType(typeof(TransactionLog))]
        public IHttpActionResult GetTransactionLog(int id)
        {
            TransactionLog transactionLog = db.TransactionLogs.Find(id);
            if (transactionLog == null)
            {
                return NotFound();
            }

            return Ok(transactionLog);
        }

        // PUT: api/Transaction/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransactionLog(int id, TransactionLog transactionLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transactionLog.Id)
            {
                return BadRequest();
            }

            db.Entry(transactionLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Transaction
        [HttpPost]
        [ResponseType(typeof(TransactionLog))]
        public IHttpActionResult PostTransactionLog(TransactionLog transactionLog)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var currentUserName = User.Identity.Name;

                var entityCurrentUser = db.Users
                    .FirstOrDefault(el => el.Name == currentUserName);

                var recipient = db.Users
                    .FirstOrDefault(el => el.Id == transactionLog.RecipientId);

                if (entityCurrentUser == null || recipient == null) return BadRequest("One of the parties not found. Contact your administrator.");
                if (entityCurrentUser.Id == transactionLog.RecipientId) return BadRequest("You can not send PW to yourself");
                if (entityCurrentUser.Balance < transactionLog.Sum) return BadRequest("You don't have enough PW");

                entityCurrentUser.Balance -= transactionLog.Sum;
                recipient.Balance += transactionLog.Sum;

                transactionLog.SenderId = entityCurrentUser.Id;
                transactionLog.CreateDateTime = DateTime.Now;
                transactionLog.TotalRecipient = recipient.Balance;
                transactionLog.TotalSender = entityCurrentUser.Balance;

                db.TransactionLogs.Add(transactionLog);

                db.SaveChanges();

                return Ok(entityCurrentUser.Balance.ToString());

            }
            catch (Exception)
            {
                return BadRequest("Server Error. Contact your administrator.");
            }
        }

        // DELETE: api/Transaction/5
        [ResponseType(typeof(TransactionLog))]
        public IHttpActionResult DeleteTransactionLog(int id)
        {
            TransactionLog transactionLog = db.TransactionLogs.Find(id);
            if (transactionLog == null)
            {
                return NotFound();
            }

            db.TransactionLogs.Remove(transactionLog);
            db.SaveChanges();

            return Ok(transactionLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionLogExists(int id)
        {
            return db.TransactionLogs.Count(e => e.Id == id) > 0;
        }
    }
}