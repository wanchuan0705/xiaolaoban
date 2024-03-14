using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 案件进度表
    /// </summary>
   public class Progress
    {
        public int Id { get; set; }
        public string ProgressName { get; set; }

        public List<Case_Progress> Case_Progresses { get; set; } 
    }
}
