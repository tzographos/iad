using System;
using System.Collections.Generic;
using System.Text;

namespace AnomalyDetection.Core.Models
{
    public class FlaggedPurchase : Purchase
    {
        public double Mean { get; set; }
        public double StandardDeviation { get; set; }

        public FlaggedPurchase()
        {
        }

        public FlaggedPurchase(Purchase purchase) : this()
        {
            this.SerialNumber = purchase.SerialNumber;
            this.Amount = purchase.Amount;
            this.PersonId = purchase.PersonId;
            this.DateAndTime = purchase.DateAndTime;
        }

        public FlaggedPurchase(Purchase purchase, double mean, double standardDeviation) : this(purchase)
        {
            this.Mean = mean;
            this.StandardDeviation = standardDeviation;
        }
    }
}
