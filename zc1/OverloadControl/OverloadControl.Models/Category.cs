using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    /// <summary>
    /// 执法人员类别表
    /// </summary>
   public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Police_Category> police_Categories { get; set; }
    }
}
