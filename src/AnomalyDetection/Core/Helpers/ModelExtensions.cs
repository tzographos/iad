using System;
using System.Collections.Generic;
using System.Text;
using AnomalyDetection.Core.Models;
using Newtonsoft.Json.Linq;

namespace AnomalyDetection.Core.Helpers
{
    public static class ModelExtensions
    {
        public static JObject ToJObject(this FlaggedPurchase purchase)
        {
            if (purchase == null) return null;

            JObject result = new JObject();
            result["event_type"] = "purchase";
            result["timestamp"] = $"{purchase.DateAndTime:yyyy-MM-dd HH:mm:ss}";
            result["id"] = purchase.PersonId.ToString();
            result["amount"] = FormattableString.Invariant($"{purchase.Amount:0.00}");
            result["mean"] = FormattableString.Invariant($"{purchase.Mean:0.00}");
            result["sd"] = FormattableString.Invariant($"{purchase.StandardDeviation:0.00}");

            return result;
        }
    }
}
