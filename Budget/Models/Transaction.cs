using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Budget
{
    [Table("transaction_t")]
    public class Transaction
    {
        [Key]
        [Column("TR_ID")]
        public int Id { get; set; }

        [Column("TR_TY_ID")]
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public virtual TransactionType Type { get; set; }

        [Column("TR_OP_ID")]
        [ForeignKey(nameof(Operation))]
        public int OperationId { get; set; }

        public virtual Operation Operation { get; set; }

        [Column("TR_DATE")]
        public DateTime Date { get; set; }

        [Column("TR_REMARK")]
        public string Remark { get; set; }

        [Column("TR_AMOUNT")]
        public float Amount { get; set; }

        [Column("TR_LOGIN")]
        public string Login { get; set; }

        [Column("TR_AC_ID")]
        public string AcccountId { get; set; }
    }
}