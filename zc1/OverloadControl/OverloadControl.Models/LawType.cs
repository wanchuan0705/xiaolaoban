using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    public class LawType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Law_LawType> law_LawTypes { get; set; }
    }
}
