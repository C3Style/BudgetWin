using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MySql.Data.Entity;

namespace Budget
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BudgetContext : DbContext
    {
        public string user = "chris";
        public string account = "0983 4930";

        public DbSet<Operation> Operations { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Recurrence> Recurrences { get; set; }
        public DbSet<TransactionType> Types { get; set; }
    }
}