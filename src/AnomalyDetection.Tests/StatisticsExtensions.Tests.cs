using System;
using System.Collections.Generic;
using Xunit;
using AnomalyDetection.Core.Helpers;


namespace AnomalyDetection.Tests
{
    [Trait("StatisticsExtensions", "Default")]
    public class StatisticsExtensions_Tests
    {
        private class SimpleObj
        {
            public double Value { get; set; }

            public SimpleObj(double value) => this.Value = value;
        }


        [Fact]
        public void StandardDeviation_SimpleExample_Succeeds()
        {
            //arrange
            IList<SimpleObj> list = new List<SimpleObj>() { new SimpleObj(600), new SimpleObj(470), new SimpleObj(170), new SimpleObj(430), new SimpleObj(300) };
            
            //act
            double sigma = list.StandardDeviation(o => o.Value);

            //assert
            Assert.InRange(sigma, 147.30, 147.35);
        }

        [Fact]
        public void StandardDeviation_NullValue_Throws()
        {
            //arrange
            IList<SimpleObj> list = null;

            //act
            Action action = () => list.StandardDeviation(o => o.Value);

            //assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void StandardDeviation_OnlyOneValue_Succeeds()
        {
            //arrange
            IList<SimpleObj> list = new List<SimpleObj>() { new SimpleObj(600) };

            //act
            double sigma = list.StandardDeviation(o => o.Value);

            //assert
            Assert.Equal(0d, sigma);  //for 1 element: stdev of POPULATION is zero; stdev of SAMPLE is infinite
        }


    }
}
