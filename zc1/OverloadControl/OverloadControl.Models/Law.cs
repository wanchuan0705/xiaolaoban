using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 法律法规表
    /// </summary>
   public class Law
    {
        public int Id { get; set; }
      
        public string Content { get; set; }
        public List<Law_Case> law_Cases { get; set; } 

        public List<Law_LawType> law_LawTypes { get; set; }

      
    }
}
