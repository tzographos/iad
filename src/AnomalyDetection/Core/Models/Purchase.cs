using System;
using System.Collections.Generic;
using System.Text;

namespace AnomalyDetection.Core.Models
{
    public class Purchase
    {
        public long SerialNumber { get; set; }
        public decimal Amount { get; set; }
        public int PersonId { get; set; }
        public DateTime DateAndTime { get; set; }
    }

   


}
