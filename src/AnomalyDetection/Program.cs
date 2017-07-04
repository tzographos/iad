using System;
using System.Text;
using AnomalyDetection.Core;
using AnomalyDetection.Core.Models;
using Newtonsoft.Json.Linq;
using AnomalyDetection.Core.Helpers;
using System.Linq;

namespace AnomalyDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\r\nAnomaly Detection, created by Theodore Zographos, July 2017, for the Insight Coding Challenge.\r\n");

            if (args.Length < 3)
            {
                Console.WriteLine(@"
Usage: AnomalyDetection [mandatory_parameters] [optional parameters]

Mandatory parameters:
1) file path of batch_log.json
2) file path of stream_log.json
3) file path of flagged_purchases.json

Optional parameters:
--verbose : Detailed output

");
#if DEBUG
                Console.WriteLine("Press enter key to exit...");
                Console.ReadLine();
#endif
                return;
            }
#if DEBUG
            int argCount = 0;
            foreach (string arg in args)
            {
                Console.WriteLine($"Argument #{++argCount} = {arg}");
            }
#endif


            string[] extraParameters = args.Skip(3).ToArray();
            bool isVerboseEnabled = extraParameters.Contains("--verbose");
            
            RunApplication(args, isVerboseEnabled);
        }


        static void RunApplication(string[] args, bool isVerboseEnabled = false)
        {
            Console.WriteLine("Reading log files...");
            JsonFileContentManager contentManager = new JsonFileContentManager(args[0], args[1], args[2], Encoding.UTF8);

            Console.WriteLine("Resolving parameters...");
            contentManager.ReadParameters(out int degrees, out int consecutivePurchases);
            Console.WriteLine($"\t> Parameters: Degrees = {degrees}, ConsecutivePurchases = {consecutivePurchases}.");

            NetworkConfiguration configuration = new NetworkConfiguration() { RelationshipDepth = degrees, MaxPurchases = consecutivePurchases, MinPurchases = 2, DeltaSigma = 3 };

            NetworkBuilder networkBuilder = new NetworkBuilder(configuration);
            networkBuilder.AnomalousPurchaseOccured += (sender, e) => {
                if (isVerboseEnabled)
                {
                    Console.WriteLine($"\t> Anomalous purchase amount of {e.Value.Amount:0.00}, from person {e.Value.PersonId}. Mean = {e.Value.Mean:0.00}, sigma = {e.Value.StandardDeviation:0.00}.");
                }
                contentManager.AddFlaggedPurchase(e.Value.ToJObject());
            };

            Console.WriteLine("Creating initial social network state...");
            foreach (JObject log in contentManager.ReadBatchLog())
            {
                AddLogItemToNetworkBuilderHelper(networkBuilder, log, true);
            }
            Console.WriteLine($"Social network has {networkBuilder.Get().Persons.Count} persons...");

            Console.WriteLine("Initiating stream...");
            foreach (JObject log in contentManager.ReadStreamLog())
            {
                AddLogItemToNetworkBuilderHelper(networkBuilder, log, false);
            }

            Console.WriteLine("Saving flagged purchases to log file...");
            int count = contentManager.WriteFlaggedPurchases();
            Console.WriteLine($"Saved {count} flagged purchases to log file.");

        }

        private static void AddLogItemToNetworkBuilderHelper(NetworkBuilder networkBuilder, JObject logItem, bool isLearningMode)
        {
            string eventType = logItem["event_type"].Value<string>() ?? string.Empty;
            DateTime dateAndTime = logItem["timestamp"].Value<DateTime>();

            switch (eventType.ToLower())
            {
                case "purchase":
                {
                    decimal amount = logItem["amount"].Value<decimal>();
                    int personId = logItem["id"].Value<int>();
                    networkBuilder.AddPurchase(amount, personId, dateAndTime, isLearningMode);

                    break;
                }
                case "befriend":
                {
                    int id1 = logItem["id1"].Value<int>();
                    int id2 = logItem["id2"].Value<int>();
                    networkBuilder.AddFriendship(id1, id2, dateAndTime);

                    break;
                }
                case "unfriend":
                {
                    int id1 = logItem["id1"].Value<int>();
                    int id2 = logItem["id2"].Value<int>();
                    networkBuilder.AddFriendship(id1, id2, dateAndTime);

                    break;
                }
                default:
                {
                    throw new NotImplementedException($"Cannot handle event type: {eventType}.");
                }
            }

        }

    }

}
