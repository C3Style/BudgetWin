using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Budget
{
    [Table("type_t")]
    public class TransactionType
    {
        [Key]
        [Column("TY_ID")]
        public int Id { get; set; }

        [Column("TY_NAME")]
        public string Name { get; set; }

        public enum TypeValues
        {
            Debit = 1,
            Credit = 2,
            BudgetDebit = 3,
            BudgetCredit = 4,
        }
    }
}