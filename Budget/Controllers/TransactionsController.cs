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

        [Route("api/Transactions/GetOperationBloc")]
        public IQueryable<OperationDTO> GetOperationBloc()
        {
            DateTime date = DateTime.Now.AddYears(-7);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return db.Recurrences
                .Join(db.Transactions, re => re.TransactionId, tr => tr.Id, (re, tr) => new { Transaction = tr, Recurrence = re })
                .Join(db.Operations, retr => retr.Transaction.OperationId, op => op.Id, (retr, op) => new { Transaction = retr.Transaction, Operation = op })
                .Where(x => x. Transaction.AcccountId == db.account)
                .Where(x => x.Transaction.Login == db.user)
                .Where(x => x.Transaction.Date >= firstDayOfMonth && x.Transaction.Date <= lastDayOfMonth)
                .Include(x => x.Transaction.Operation)
                .GroupBy(x => new
                {
                    x.Operation.Id,
                    x.Operation.Name,
                    x.Operation.Icon,
                }
                )
                .Select(x => new OperationDTO
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Icon = x.Key.Icon,
                    Total = x.Sum(s => s.Transaction.Amount)
                });
        }

        // GET: api/Transactions
        public IQueryable<OperationDTO> GetTransactions()
        {
            DateTime date = DateTime.Now.AddYears(-7);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return db.Transactions
                .Join(db.Operations, tr => tr.OperationId, op => op.Id, (tr, op) => new { Transaction = tr, Operation = op })
                .Where(x => x.Transaction.AcccountId == db.account)
                .Where(x => x.Transaction.Login == db.user)
                .Where(x => x.Transaction.Date >= firstDayOfMonth && x.Transaction.Date <= lastDayOfMonth)
                .Include(x => x.Transaction.Operation)
                .GroupBy(x => new
                {
                    x.Operation.Id,
                    x.Operation.Name,
                    x.Operation.Icon,
                }
                )
                .Select(x => new OperationDTO
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Icon = x.Key.Icon,
                    Total = x.Sum(s => s.Transaction.Amount)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(transaction);
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