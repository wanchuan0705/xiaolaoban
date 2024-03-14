using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 角色表
    /// </summary>
   public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public List<Police_Role> Police_Roles { get; set; } 
      
        
    }
}
