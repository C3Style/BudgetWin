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

        [Route("api/Transactions/GetOperationBloc/{date}")]
        public IQueryable<OperationDTO> GetOperationBloc(DateTime date)
        {
            return db.Recurrences
                .Join(db.Transactions, re => re.TransactionId, tr => tr.Id, (re, tr) => new { Tr = tr, Re = re })
                .Join(db.Operations, retr => retr.Tr.OperationId, op => op.Id, (retr, op) => new { ReTr = retr, Op = op })
                .Where(x => x.ReTr.Tr.Login == db.user)
                .Where(x => x.ReTr.Tr.AcccountId == db.account)
                .Where(x => x.ReTr.Re.Month == date.Month)
                .Where(x => x.ReTr.Re.Year == date.Year)
                // .Where(x => x.ReTr.Tr.Type.Id == (int)TransactionType.TypeValues.BudgetDebit)
                .Include(x => x.ReTr.Tr.Operation)
                .Include(x => x.ReTr.Tr.Type)
                .Select(x => new OperationDTO
                {
                    Id = x.ReTr.Tr.Id,
                    Name = x.Op.Name,
                    Icon = x.Op.Icon,
                    Amount = x.ReTr.Tr.Amount,
                    Type = x.ReTr.Tr.TypeId,
                    Remark = x.ReTr.Tr.Remark
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
                    Amount = x.Sum(s => s.Transaction.Amount)
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