using System;
using System.Collections.Generic;
using System.Text;
using AnomalyDetection.Core;
using AnomalyDetection.Core.Helpers;
using AnomalyDetection.Core.Models;
using Xunit;

namespace AnomalyDetection.Tests
{
    [Trait("NetworkTraverser", "Default")]
    public class NetworkTraverser_Tests
    {

        public NetworkTraverser_Tests()
        {

        }

        [Fact]
        public void GetSocialGroup_NoFriends_Succeeds()
        {
            //arrange
            NetworkConfiguration config = new NetworkConfiguration() { RelationshipDepth = 5, MaxPurchases = 50 };
            Network network = new Network();
            NetworkTraverser networkTraverser = new NetworkTraverser(network, config);
            
            Person p0 = new Person(1, config.MaxPurchases);
            network.Persons.Add(p0);
            
            //act
            IList<Person> result = networkTraverser.GetSocialGroup(p0);

            //assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetSocialGroup_FirstLevelFriendsOnly_Succeeds(int depth)
        {
            //arrange
            NetworkConfiguration config = new NetworkConfiguration() { RelationshipDepth = depth, MaxPurchases = 50 };
            Network network = new Network();
            NetworkTraverser networkTraverser = new NetworkTraverser(network, config);

            Person p0 = new Person(1, config.MaxPurchases);
            Person p1a = new Person(2, config.MaxPurchases);
            Person p1b = new Person(3, config.MaxPurchases);
            Person p1c = new Person(4, config.MaxPurchases);
            p0.Friends.AddRange(new[] { p1a, p1b, p1c });
            p1a.Friends.Add(p1b);
            network.Persons.AddRange(new[] { p0, p1a, p1b, p1c });

            //act
            IList<Person> result = networkTraverser.GetSocialGroup(p0);

            //assert
            IDictionary<int, int> expectedResults = new Dictionary<int, int>() { { 1, 3 }, { 2, 3 } };

            Assert.NotNull(result);
            Assert.Equal(expectedResults[depth], result.Count);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void GetSocialGroup_TwoLevelFriends_Succeeds(int depth)
        {
            //arrange
            NetworkConfiguration config = new NetworkConfiguration() { RelationshipDepth = depth, MaxPurchases = 50 };
            Network network = new Network();
            NetworkTraverser networkTraverser = new NetworkTraverser(network, config);

            Person p0 = new Person(1, config.MaxPurchases);
            Person p1a = new Person(2, config.MaxPurchases);
            Person p1b = new Person(3, config.MaxPurchases);
            Person p1c = new Person(4, config.MaxPurchases);
            Person p2a = new Person(5, config.MaxPurchases);
            Person p2b = new Person(6, config.MaxPurchases);
            Person p2c = new Person(7, config.MaxPurchases);

            p0.Friends.AddRange(new[] { p1a, p1b, p1c });
            p1a.Friends.Add(p1b);
            p1b.Friends.Add(p2b);
            p1c.Friends.AddRange(new[] { p2a, p2b, p2c });
            p2a.Friends.Add(p1a);
            network.Persons.AddRange(new[] { p0, p1a, p1b, p1c, p2a, p2b, p2c });

            //act
            IList<Person> result = networkTraverser.GetSocialGroup(p0);

            //assert
            IDictionary<int, int> expectedResults = new Dictionary<int, int>() { { 1, 3 }, { 2, 6 }, { 3, 6 }, { 10, 6 } };

            Assert.NotNull(result);
            Assert.Equal(expectedResults[depth], result.Count);
        }


    }
}
