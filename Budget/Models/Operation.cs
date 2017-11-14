using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Budget
{
    [Table("operation_t")]
    public class Operation
    {
        [Key]
        [Column("OP_ID")]
        public int Id { get; set; }

        [Column("OP_AC_ID")]
        public string AcccountId { get; set; }

        [Column("OP_NAME")]
        public string Name { get; set; }

        [Column("OP_LOGIN")]
        public string Login { get; set; }

        [Column("OP_ICON")]
        public string Icon { get; set; }
    }
}