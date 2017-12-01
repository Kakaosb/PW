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
    [RoutePrefix("api/TransactionLogs")]
    public class TransactionLogsController : ApiController
    {
        private ParrotWingsContext db = new ParrotWingsContext();

        // GET: api/TransactionLogs
        [Route("GetTransactionLogs")]
        public string GetTransactionLogs()
        {
            try
            {
                var currentUserName = User.Identity.Name;
                //var currentUserName = "test3 test3";

                var entityCurrentUser = db.Users
                    .FirstOrDefault(el => el.Name == currentUserName);

                var logsList = new List<OperationsLogTable>();

                if (entityCurrentUser != null)
                {
                    var userId = entityCurrentUser.Id;

                    var list = db.TransactionLogs
                        .Where(el => el.RecipientId == userId || el.SenderId == userId)
                        .ToList();

                    logsList = list
                        .Select(el =>
                            new OperationsLogTable()
                            {
                                Sum = el.RecipientId == userId ? el.Sum : el.Sum * (-1),
                                IsDebet = el.RecipientId == userId,
                                UserName = el.RecipientId == userId ? el.Sender.Name : el.Recipient.Name,
                                OperationDate = el.CreateDateTime.ToString("G")
                            }
                        )
                        .OrderByDescending(el => el.OperationDate)
                        .ToList();

                }

                return JsonConvert.SerializeObject(logsList);

            }
            catch (Exception ex)
            {
                return "На сервере произошла ошибка. Обратитесь к администратотру";
            }
        }

        // GET: api/TransactionLogs/5
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

        // PUT: api/TransactionLogs/5
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

        // POST: api/TransactionLogs
        [ResponseType(typeof(TransactionLog))]
        public IHttpActionResult PostTransactionLog(decimal sum, int recipientId)
        {
            try
            {
                var currentUserName = User.Identity.Name;

                var entityCurrentUser = db.Users
                    .FirstOrDefault(el => el.Name == currentUserName);

                var recipient = db.Users
                    .FirstOrDefault(el => el.Id == recipientId);

                if (entityCurrentUser != null & recipient != null)
                {
                    if (entityCurrentUser.Balance < sum)
                    {
                    }
                    else
                    {
                        entityCurrentUser.Balance -= sum;
                        recipient.Balance += sum;

                        db.SaveChanges();
                    }
                }
                else
                {

                }
            }
            catch
            {

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            return Ok();
        }

        // DELETE: api/TransactionLogs/5
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