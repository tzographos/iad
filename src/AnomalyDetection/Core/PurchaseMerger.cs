using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnomalyDetection.Core.Helpers;
using AnomalyDetection.Core.Models;

namespace AnomalyDetection.Core
{
    public class PurchaseMerger
    {
        private readonly NetworkConfiguration _config;

        public PurchaseMerger(NetworkConfiguration config)
        {
            if (object.ReferenceEquals(config, null)) throw new ArgumentNullException(nameof(config));
            _config = config;
        }

        /// <summary>
        /// Gets the last M purchases of the social group of N people. Each person has it's own list of M purchases.
        /// </summary>
        /// <param name="socialGroupPurchases"></param>
        /// <returns></returns>
        public IEnumerable<Purchase> Merge(IEnumerable<RollingList<Purchase>> socialGroupPurchases)
        {
            if (socialGroupPurchases == null) throw new ArgumentNullException(nameof(socialGroupPurchases));

            //rolling list uses an underlying linked list to add the purchases; newer purchases are added to the end of the linked list.
            //keep a list of pointers initially at the end of each RollingList to find the latest purchases (highest SerialNumber).
            List<LinkedListNode<Purchase>> cursors = socialGroupPurchases.Select(p => p.LastNode).ToList();
            int remainingItems = _config.MaxPurchases;

            while (remainingItems > 0 && cursors.Any(c => c != null))
            {
                //find max cursor index by SerialNumber
                int index = 0;
                long maxSerialNumber = long.MinValue;
                int maxIndex = int.MinValue;

                foreach (var c in cursors)
                {
                    if ((cursors[index]?.Value.SerialNumber ?? long.MinValue) > maxSerialNumber)
                    {
                        maxSerialNumber = cursors[index].Value.SerialNumber;
                        maxIndex = index;
                    }
                    index++;
                }

                yield return cursors[maxIndex].Value;

                remainingItems--;
                cursors[maxIndex] = cursors[maxIndex].Previous;
            }
        }

    }
}
