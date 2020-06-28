using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReactMvc.Models
{
    public partial class Word
    {
        [NotMapped]
        public string Number
        {
            get { return NumberRead.ToString(); }
            set { int t = 0; if (int.TryParse(value, out t)) NumberRead = t; }
        }
    }
}