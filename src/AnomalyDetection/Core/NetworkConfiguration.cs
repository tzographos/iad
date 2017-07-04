using System;
using System.Collections.Generic;
using System.Text;

namespace AnomalyDetection.Core
{
    public class NetworkConfiguration
    {
        public int RelationshipDepth { get; set; }  // D
        public int MaxPurchases { get; set; }       // T
        public int MinPurchases { get; set; }       // Tmin (e.g. 2)
        public double DeltaSigma { get; set; }      // How many standard deviations above mean (e.g. 3)
    }
}
