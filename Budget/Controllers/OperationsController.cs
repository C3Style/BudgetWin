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

namespace Budget.Controllers
{
    public class OperationsController : ApiController
    {
        private BudgetContext db = new BudgetContext();

        // GET: api/Operations
        public IQueryable<Operation> GetOperations([FromUri]string name = null, [FromUri]bool? isCredit = null)
        {
            return db.Operations
                .Where(x => name == null || x.Name.Contains(name))
                .Where(x => isCredit == null || x.IsCredit == isCredit)
                .Where(x => x.AcccountId == db.account)
                .Where(x => x.Login == db.user)
                .OrderBy(x => x.Name);
        }

        // GET: api/Operations/5
        [ResponseType(typeof(Operation))]
        public IHttpActionResult GetOperation(int id)
        {
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return NotFound();
            }

            return Ok(operation);
        }

        // PUT: api/Operations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOperation(int id, Operation operation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != operation.Id)
            {
                return BadRequest();
            }

            db.Entry(operation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationExists(id))
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

        // POST: api/Operations
        [ResponseType(typeof(Operation))]
        public IHttpActionResult PostOperation(Operation operation)
        {
            operation.AcccountId = db.account;
            operation.Login = db.user;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Operations.Add(operation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = operation.Id }, operation);
        }

        // DELETE: api/Operations/5
        [ResponseType(typeof(Operation))]
        public IHttpActionResult DeleteOperation(int id)
        {
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return NotFound();
            }

            db.Operations.Remove(operation);
            db.SaveChanges();

            return Ok(operation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OperationExists(int id)
        {
            return db.Operations.Count(e => e.Id == id) > 0;
        }
    }
}