using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 案件-处理进度中间表
    /// </summary>
   public class Case_Progress
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int ProgressId { get; set; }
        public string _Content { get; set; }
        public string Time { get; set; }

        public Case Case { get; set; }
        public Progress Progress { get; set; }
    }
}
