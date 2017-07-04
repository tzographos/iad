using System;
using System.Collections.Generic;
using System.Text;
using AnomalyDetection.Core.Helpers;
using AnomalyDetection.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;


namespace AnomalyDetection.Tests
{
    [Trait("ModelExtensions", "Default")]
    public class ModelExtensions_Tests
    {

        [Fact]
        public void ToJObject_SimpleFlaggedPurchase_Succeeds()
        {
            //arrange
            FlaggedPurchase fp = new FlaggedPurchase() { Amount = 12.3312M, DateAndTime = new DateTime(2017, 7, 4, 1, 2, 3), PersonId = 69, SerialNumber = 1234567, Mean = 18.81, StandardDeviation = 3.14 };
            JObject json = fp.ToJObject();

            //act
            string s = json.ToString(Formatting.None);

            //assert
            Assert.DoesNotContain("\r\n", s);

        }



    }
}
