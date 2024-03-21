using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadControl.Models.Dto
{
    public class CaseData
    {
        public int PoliceId { get; set; }
        public string PoliceName { get; set; }
        public int CaseNo { get; set; }
        public string ViolatorsName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public string PlateNumber { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Content { get; set; }
        public string ViolatorsPhone { get; set; }
    }
}