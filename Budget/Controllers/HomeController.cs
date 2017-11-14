using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace Budget.Controllers
{
    public class HomeController : Controller
    {
        private BudgetContext db = new BudgetContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var operations = db.Operations.ToList();
            return View();
        }

        public ActionResult Operation()
        {
            ViewBag.Title = "Opération";
            return View();
        }
       
        public ActionResult Transaction()
        {
            ViewBag.Title = "Transaction";
            return View();
        }
    }
}
