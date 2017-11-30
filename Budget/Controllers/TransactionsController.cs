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
using Budget;
using System.Web;

namespace Budget.Controllers
{
    public class TransactionsController : ApiController
    {
        private BudgetContext db = new BudgetContext();

        [Route("api/Transactions/GetTransactionBloc/{date}")]
        public IQueryable<TransactionDTO> GetTransactionBloc(DateTime date)
        {
            return db.Recurrences
                .Join(db.Transactions, re => re.TransactionId, tr => tr.Id, (re, tr) => new { Tr = tr, Re = re })
                .Join(db.Operations, retr => retr.Tr.OperationId, op => op.Id, (retr, op) => new { ReTr = retr, Op = op })
                .Where(x => x.ReTr.Tr.Login == db.user)
                .Where(x => x.ReTr.Tr.AcccountId == db.account)
                .Where(x => x.ReTr.Re.Month == date.Month)
                .Where(x => x.ReTr.Re.Year == date.Year)
                .Include(x => x.ReTr.Tr.Operation)
                .Include(x => x.ReTr.Tr.Type)
                .Select(x => new TransactionDTO
                {
                    TransactionId = x.ReTr.Tr.Id,
                    OperationId = x.Op.Id,
                    OperationName = x.Op.Name,
                    OperationIcon = x.Op.Icon,
                    TransactionAmount = x.ReTr.Tr.Amount,
                    TransactionType = x.ReTr.Tr.TypeId,
                    TransactionDate = x.ReTr.Tr.Date,
                    TransactionRemark = x.ReTr.Tr.Remark
                });
        }

        [Route("api/Transactions/GetDebitByOperation/{date}")]
        public IQueryable<TransactionDTO> GetDebitByOperation(DateTime date)
        {
            return db.Recurrences
                .Join(db.Transactions, re => re.TransactionId, tr => tr.Id, (re, tr) => new { Tr = tr, Re = re })
                .Join(db.Operations, retr => retr.Tr.OperationId, op => op.Id, (retr, op) => new { ReTr = retr, Op = op })
                .Where(x => x.ReTr.Tr.Login == db.user)
                .Where(x => x.ReTr.Tr.AcccountId == db.account)
                .Where(x => x.ReTr.Re.Month == date.Month)
                .Where(x => x.ReTr.Re.Year == date.Year)
                .Where(x => x.ReTr.Tr.Type.Id == (int)TransactionType.TypeValues.Debit)
                .Include(x => x.ReTr.Tr.Operation)
                .Include(x => x.ReTr.Tr.Type)
                .GroupBy(x => new
                {
                    x.Op.Id,
                    x.Op.Name,
                }
                )
                .Select(x => new TransactionDTO
                {
                    OperationId = x.Key.Id,
                    OperationName = x.Key.Name,
                    TransactionAmount = x.Sum(s => s.ReTr.Tr.Amount)
                });
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(int id, Transaction transaction)
        {
            transaction.AcccountId = db.account;
            transaction.Login = db.user;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.Id)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            transaction.AcccountId = db.account;
            transaction.Login = db.user;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(transaction);
            db.SaveChanges();

            Recurrence newRecurrence = new Recurrence()
            {
                IsPaid = false,
                Month = transaction.Date.Month,
                Year = transaction.Date.Year,
                TransactionId = transaction.Id
            };

            db.Recurrences.Add(newRecurrence);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.Id == id) > 0;
        }
    }
}
 