using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnomalyDetection.Core;
using AnomalyDetection.Core.Helpers;
using AnomalyDetection.Core.Models;
using Xunit;

namespace AnomalyDetection.Tests
{
    [Trait("PurchaseMerger", "Default")]
    public class PurchaseMerger_Tests
    {

        [Fact]
        public void Merge_NullPurchases_Throws()
        {
            //arrange 
            NetworkConfiguration config = new NetworkConfiguration() { MaxPurchases = 5 };
            PurchaseMerger merger = new PurchaseMerger(config);
            IList<RollingList<Purchase>> socialGroupPurchases = null;

            //act
            Action action = () => merger.Merge(socialGroupPurchases).ToArray();

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        public void Merge_ThreePurchaseLists_Succeeds(int maxPurchases)
        {
            //arrange 
            NetworkConfiguration config = new NetworkConfiguration() { MaxPurchases = maxPurchases };
            PurchaseMerger merger = new PurchaseMerger(config);

            RollingList<Purchase> purch1 = new RollingList<Purchase>(config.MaxPurchases);
            RollingList<Purchase> purch2 = new RollingList<Purchase>(config.MaxPurchases);
            RollingList<Purchase> purch3 = new RollingList<Purchase>(config.MaxPurchases);

            purch1.Add(new Purchase() { SerialNumber = 1 });      //person1 purchases:  1, 2, 5, 10
            purch1.Add(new Purchase() { SerialNumber = 2 });      //person2 purchases:  3, 11
            purch2.Add(new Purchase() { SerialNumber = 3 });      //person3 purchases:  4, 6, 7, 8, 9
            purch3.Add(new Purchase() { SerialNumber = 4 });
            purch1.Add(new Purchase() { SerialNumber = 5 });
            purch3.Add(new Purchase() { SerialNumber = 6 });
            purch3.Add(new Purchase() { SerialNumber = 7 });
            purch3.Add(new Purchase() { SerialNumber = 8 });
            purch3.Add(new Purchase() { SerialNumber = 9 });
            purch1.Add(new Purchase() { SerialNumber = 10 });
            purch2.Add(new Purchase() { SerialNumber = 11 });

            IList<RollingList<Purchase>> socialGroupPurchases = new List<RollingList<Purchase>>() { purch1, purch2, purch3 };

            //act 
            IList<Purchase> groupLatestPurchases = merger.Merge(socialGroupPurchases).ToList();

            //assert
            IDictionary<int, int> expectedPurchaseCount = new Dictionary<int, int>() { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 5, 5 }, { 10, 10 }, { 15, 11 } };
            IDictionary<int, long[]> expectedSerialNumbers = new Dictionary<int, long[]>()
            {
                { 1, new long[] {11} },
                { 2, new long[] {11, 10} },
                { 3, new long[] {11, 10, 9} },
                { 5, new long[] {11, 10 , 9, 8, 7} },
                { 10, new long[] {11, 10, 9, 8, 7, 6, 5, 4, 3, 2} },
                { 15, new long[] {11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1} },
            };

            Assert.Equal(expectedPurchaseCount[maxPurchases], groupLatestPurchases.Count);
            Assert.True(groupLatestPurchases.Select(p => p.SerialNumber).SequenceEqual(expectedSerialNumbers[maxPurchases]));
        }
    }
}
