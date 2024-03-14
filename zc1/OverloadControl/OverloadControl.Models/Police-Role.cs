using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 执法人员-权限表
    /// </summary>
   public class Police_Role
    {
        public int Id { get; set; }
        public int PoliceId { get; set; }
    


      
        public Police Police { get; set; }


        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
