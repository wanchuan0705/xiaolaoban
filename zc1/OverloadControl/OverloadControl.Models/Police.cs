using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 执法人员信息表
    /// </summary>
   public class Police
    {
        public int Id { get; set; }
        public string ?Account { get; set; }
        public string ?Password { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Sex { get; set; }

        public List<Police_Category>? police_Categories { get; set; }
        public List<Police_Case>? Police_Cases { get; set; }
        public List<Police_Role>? Police_Roles { get; set; }

        
    }
}
