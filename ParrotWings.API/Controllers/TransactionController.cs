using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using ParrotWings.API.Models;
using ParrotWings.API.Models.ModelHelpers;
using ParrotWings.API.Models.ResponseModels;
using ParrotWings.API.Services;

namespace ParrotWings.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        private ParrotWingsContext db = new ParrotWingsContext();

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

        // GET: api/Transaction
        [Route("GetTransactionLogs")]
        public IHttpActionResult GetTransactionLogs()
        {
            try
            {
                var trService = new TrasactionService(db, User.Identity.Name);

                var logsList = trService.GetTransactionLogList();

                return Ok(JsonConvert.SerializeObject(logsList));

            }
            catch (Exception)
            {
                return BadRequest("Server Error. Contact your administrator.");
            }
        }

        // POST: api/Transaction
        [HttpPost]
        [ResponseType(typeof(TransactionLog))]
        public IHttpActionResult PostTransactionLog(TransactionLog transactionLog)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var trService = new TrasactionService(db, User.Identity.Name);

                var checkedResult = trService.CheckParties(transactionLog.RecipientId);
                if (checkedResult != "Ok")
                    return BadRequest(checkedResult);

                checkedResult = trService.CheckSolvency(transactionLog.Sum);
                if (checkedResult != "Ok")
                    return BadRequest(checkedResult);

                trService.CreateTransactionLog(transactionLog, out var recipientName);

                var response = new TransactionResponse()
                {
                    Total = transactionLog.TotalSender,
                    Sum = transactionLog.Sum * (-1),
                    TransferType = TransferType.Credit.ToString(),
                    OperationDate = transactionLog.CreateDateTime.ToString("G"),
                    UserName = recipientName
                };

                return Ok(JsonConvert.SerializeObject(response));

            }
            catch (Exception e)
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