using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    public class Law_LawType
    {
        public int Id { get; set; }
        public int LawTypeId { get; set; }
        public LawType lawType { get; set; }
        public int LawId { get; set; }
        public Law law { get; set; }
    }
}
