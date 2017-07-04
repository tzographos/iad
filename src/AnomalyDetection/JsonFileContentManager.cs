using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnomalyDetection
{
    public class JsonFileContentManager
    {
        private readonly string _batchLogInputFilePath;
        private readonly string _streamLogInputFilePath;
        private readonly string _flaggedPurchasesOutputFilePath;
        private readonly string[] _batchLogLines;
        private readonly string[] _streamLogLines;
        private readonly IList<JObject> _flaggedPurchases = new List<JObject>();

        public JsonFileContentManager(string batchLogInputFilePath, string streamLogInputFilePath, string flaggedPurchasesOutputFilePath, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(batchLogInputFilePath)) throw new ArgumentNullException(nameof(batchLogInputFilePath));
            if (string.IsNullOrWhiteSpace(streamLogInputFilePath)) throw new ArgumentNullException(nameof(streamLogInputFilePath));
            if (string.IsNullOrWhiteSpace(flaggedPurchasesOutputFilePath)) throw new ArgumentNullException(nameof(flaggedPurchasesOutputFilePath));
            _batchLogInputFilePath = batchLogInputFilePath;
            _streamLogInputFilePath = streamLogInputFilePath;
            _flaggedPurchasesOutputFilePath = flaggedPurchasesOutputFilePath;

            if (File.Exists(batchLogInputFilePath))
            {
                _batchLogLines = File.ReadAllLines(batchLogInputFilePath, encoding ?? Encoding.UTF8).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                if (!_batchLogLines.Any()) throw new FileLoadException("No data found in batch log file!");
            }
            else
            {
                throw new FileNotFoundException($"Cannot find file {batchLogInputFilePath}.");
            }

            if (File.Exists(streamLogInputFilePath))
            {
                _streamLogLines = File.ReadAllLines(streamLogInputFilePath, encoding ?? Encoding.UTF8).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            }
            else
            {
                _streamLogLines = new string[] { };
            }
        }
        
        public void ReadParameters(out int degrees, out int consecutivePurchases)
        {
            JObject parameters = JObject.Parse(_batchLogLines[0]);

            degrees = parameters["D"].Value<int>();
            consecutivePurchases = parameters["T"].Value<int>();
        }


        public IEnumerable<JObject> ReadBatchLog()
        {
            foreach(string line in _batchLogLines.Skip(1))
            {
                yield return JObject.Parse(line);
            }
        }

        public IEnumerable<JObject> ReadStreamLog()
        {
            foreach (string line in _streamLogLines)
            {
                yield return JObject.Parse(line);
            }
        }

        public void AddFlaggedPurchase(JObject flagged)
        {
            _flaggedPurchases.Add(flagged);
        }

        public int WriteFlaggedPurchases()
        {
            File.WriteAllLines(
                _flaggedPurchasesOutputFilePath,
                _flaggedPurchases.Select(f => f.ToString(Formatting.None)),
                Encoding.UTF8
            );
            return _flaggedPurchases.Count;
        }

    }
}
