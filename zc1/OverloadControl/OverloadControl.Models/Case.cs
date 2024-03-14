using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 案件表
    /// </summary>
   public class Case
    {
        public string CaseNo { get; set; }
        public int Id 
        { get; set; }
        public string Address 
        { get; set; }
        public string Types 
        { get; set; }
        public string Details 
        { get; set; }
        public string Platenumber
        { get; set; }
        public DateTime Date
        { get; set; }
        public string Phone
        { get; set; }
        public string Image
        { get; set; }
        public string PolicerName
        { get; set; }
        public string State
        { get; set; }
        public string ViolatorsName
        { get; set; }
        public string ApplicationTime
        { get; set; }
        public string Description
        { get; set; }
        public string Model
        { get; set; }
        public string Judgment
        { get; set; }
        public string M_Content
        { get; set; }
        public string Content
        { get; set; }
        public string PolicerName1
        { get; set; }
        public string ViolatorsPhone
        { get; set; }
        public string OrderTake { get; set; }
        public string LegalArticles{ get; set; }

        public List<Case_Progress> Case_Progresses { get; set; } 
        public List<Law_Case> law_Cases { get; set; } 
        public List<Police_Case> Police_Cases { get; set; } 
        //public int VId { get; set; }
        //public Violators Violators { get; set; }
    }
}
