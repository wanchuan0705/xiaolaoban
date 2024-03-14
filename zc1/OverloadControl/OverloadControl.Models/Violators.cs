using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 违法人员信息表
    /// </summary>
   public class Violators
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string CarNumber { get; set; }
        public string Address { get; set; }
        public string  Age { get; set; }

        //public List<Case> Cases { get; set; } 
    }
}
