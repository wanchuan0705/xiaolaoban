using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 法律法规-案件中间表
    /// </summary>
   public class Law_Case
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int LawId { get; set; }

        public Case Case { get; set; }
        public Law Law { get; set; }
    }
}
