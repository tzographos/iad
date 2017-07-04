using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AnomalyDetection.Core;
using AnomalyDetection.Core.Models;

namespace AnomalyDetection.Core
{
    public class NetworkTraverser
    {
        private readonly Network _network;
        private readonly NetworkConfiguration _config;

        public NetworkTraverser(Network network, NetworkConfiguration config)
        {
            if (object.ReferenceEquals(network, null)) throw new ArgumentNullException(nameof(network));
            _network = network;

            if (object.ReferenceEquals(config, null)) throw new ArgumentNullException(nameof(config));
            _config = config;
        }
        

        public IList<Person> GetSocialGroup(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));

            IList<Person> result = new List<Person>();
            ICollection<int> visitedPersonIds = new List<int>(new int[] { person.Id });

            int currentLevel = 0;
            Queue<Person> q = new Queue<Person>(new Person[] { person }); //person queue
            Queue<int> lq = new Queue<int>(new int[] { currentLevel });   //level queue   --for each person added to the persons' queue add it's corresponding level to the level queue

            while (q.Count > 0)
            {
                Person p = q.Dequeue();
                currentLevel = lq.Dequeue();

                if (currentLevel < _config.RelationshipDepth)
                {
                    foreach (Person f in p.Friends)
                    {
                        if (!visitedPersonIds.Contains(f.Id))
                        {
                            q.Enqueue(f);
                            lq.Enqueue(currentLevel + 1);
                            visitedPersonIds.Add(f.Id);
                            result.Add(f);
                        }
                    }
                }
            }

            return result;
        }


    }
}
