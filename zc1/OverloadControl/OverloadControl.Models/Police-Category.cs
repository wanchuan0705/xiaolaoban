using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models
{
    public class Police_Category
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
        public int PoliceId { get; set; }
        public Police police { get; set; }
    }
}
