using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 执法人员-案件表
    /// </summary>
   public class Police_Case
    {
        public int Id { get; set; }
        public int  PoliceId  { get; set; }
        public int CaseId { get; set; }
        public Police Police { get; set; }
        public Case Case { get; set; }
    }
}
