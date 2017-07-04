using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AnomalyDetection.Core.Models;
using System.Linq;
using AnomalyDetection.Core.Helpers;

namespace AnomalyDetection.Core
{
    public class NetworkBuilder
    {
        public event SimpleNotificationEventHandler<FlaggedPurchase> AnomalousPurchaseOccured;

        private readonly Network _network;
        private readonly NetworkTraverser _networkTraverser;
        private readonly NetworkConfiguration _config;
        private readonly PurchaseMerger _purchaseMerger;

        private long _purchaseSerialNumber = 0;

        public NetworkBuilder(NetworkConfiguration config)
        {
            if (object.ReferenceEquals(config, null)) throw new ArgumentNullException(nameof(config));
            _config = config;

            _network = new Network();
            _networkTraverser = new NetworkTraverser(_network, config);
            _purchaseMerger = new PurchaseMerger(config);
        }

        public Network Get() => _network;

        public void AddFriendship(int firstPersonId, int secondPersonId, DateTime dateAndTime)
        {
            //get first person
            Person first = _network.Persons[firstPersonId];
            //if person not found, add it
            if (first == null)
            {
                first = new Person(firstPersonId, _config.MaxPurchases);
                _network.Persons.Add(first);
            }

            //get second person
            Person second = _network.Persons[secondPersonId];
            //if person not found, add it
            if (second == null)
            {
                second = new Person(secondPersonId, _config.MaxPurchases);
                _network.Persons.Add(second);
            }

            first.Friends.Add(second);
            second.Friends.Add(first);
        }

        public void RemoveFriendship(int firstPersonId, int secondPersonId)
        {
            //get first person
            Person first = _network.Persons[firstPersonId];
            Person second = _network.Persons[secondPersonId];

            if (first != null && second != null)
            {
                if (first.Friends.Contains(second)) first.Friends.Remove(second);
                if (second.Friends.Contains(first)) second.Friends.Remove(first);
            }
        }
        
        public void AddPurchase(decimal amount, int personId, DateTime dateAndTime, bool isLearningMode)
        {
            Purchase purchase = new Purchase() { Amount = amount, PersonId = personId, DateAndTime = dateAndTime, SerialNumber = ++_purchaseSerialNumber};

            //find corresponding person by id
            Person person = _network.Persons[personId];

            //if person doesn't exist in the collection then add it first
            if (person == null)
            {
                person = new Person(personId, _config.MaxPurchases);
                _network.Persons.Add(person);
            }

            //add purchase
            person.Purchases.Add(purchase);

            if (!isLearningMode)
            {
                //find social group
                IList<Person> socialGroup = _networkTraverser.GetSocialGroup(person);

                //merge social group purchases
                IList<Purchase> socialGroupPurchases = _purchaseMerger.Merge(socialGroup.Select(_ => _.Purchases)).ToList();

                //calculate sample statistics
                int count = socialGroupPurchases.Count;
                double mean = socialGroupPurchases.Average(_ => (double)_.Amount);
                double standardDeviation = socialGroupPurchases.StandardDeviation(_ => (double)_.Amount);

                //check for anomaly
                if (count >= _config.MinPurchases && (double)amount > mean + _config.DeltaSigma * standardDeviation)
                {
                    //signal anomaly to file exporter
                    this.OnAnomalousPurchaseOccured(this, new SimpleNotificationEventArgs<FlaggedPurchase>(new FlaggedPurchase(purchase, mean, standardDeviation)));
                }
            }
        }
        

        protected virtual void OnAnomalousPurchaseOccured(object sender, SimpleNotificationEventArgs<FlaggedPurchase> e)
        {
            AnomalousPurchaseOccured?.Invoke(sender, e);
        }
    }



}
