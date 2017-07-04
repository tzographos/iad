using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnomalyDetection.Core.Helpers;

namespace AnomalyDetection.Core.Models
{
    public class Person
    {
        public int Id { get; set; }
        public RollingList<Purchase> Purchases { get; set; }
        public ICollection<Person> Friends { get; set; }


        public Person(int id, int maximumPurchases)
        {
            if (maximumPurchases < 1) throw new ArgumentException($"Invalid value for maximum purchases.", nameof(maximumPurchases));

            this.Id = id;
            this.Purchases = new RollingList<Purchase>(maximumPurchases);
            this.Friends = new List<Person>();
        }
    }

  


}
