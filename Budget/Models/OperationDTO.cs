using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Budget
{
    public class OperationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public float Total { get; set; }
    }
}