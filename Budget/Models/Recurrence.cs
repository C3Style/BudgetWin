using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Budget
{
    [Table("recurrence_t")]
    public class Recurrence
    {
        [Key]
        [Column("RE_ID")]
        public int Id { get; set; }

        [Column("RE_TR_ID")]
        [ForeignKey(nameof(Transaction))]
        public int TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }

        [Column("RE_PAID")]
        public bool IsPaid { get; set; }

        [Column("RE_MONTH")]
        public int Month { get; set; }

        [Column("RE_YEAR")]
        public int Year { get; set; }
    }
}