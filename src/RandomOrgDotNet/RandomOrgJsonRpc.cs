using Newtonsoft.Json.Converters;

namespace RandomOrgDotNet.JsonRpc
{
    public struct IntegerResponseV1
    {
        public int[] Data;
        public DateTime CompletionTime;
    }

    public struct DecimalResponseV1
    {
        public decimal[] Data;
        public DateTime CompletionTime;
    }

    public struct DoubleResponseV1
    {
        public double[] Data;
        public DateTime CompletionTime;
    }

    public struct StringResponseV1
    {
        public string[] Data;
        public DateTime CompletionTime;
    }

    public struct Integer2DResponseV2
    {
        public int[][] Data;
        public DateTime CompletionTime;
    }

    public struct GenerateIntegersResponseV1
    {
        public IntegerResponseV1 Random;
        public int BitsUsed;
        public int BitsLeft;
        public int RequestsLeft;
        public int AdvisoryDelay;
    }

    public struct GenerateDecimalFractionsResponseV1
    {
        public DecimalResponseV1 Random;
        public int BitsUsed;
        public int BitsLeft;
        public int RequestsLeft;
        public int AdvisoryDelay;
    }

    public struct GenerateGaussiansResponseV1
    {
        public DoubleResponseV1 Random;
        public int BitsUsed;
        public int BitsLeft;
        public int RequestsLeft;
        public int AdvisoryDelay;
    }

    public struct StringsResponseV1
    {
        public StringResponseV1 Random;
        public int BitsUsed;
        public int BitsLeft;
        public int RequestsLeft;
        public int AdvisoryDelay;
    }

    public struct GetUsageResponseV1
    {
        public enum StatusType { stopped, running, paused };
        public StatusType Status;
        public DateTime CreationTime;
        public int BitsLeft;
        public int RequestsLeft;
        public int TotalBits;
        public int TotalRequests;
    }

    public struct GenerateIntegerSequencesResponseV2
    {
        public Integer2DResponseV2 Random;
        public int BitsUsed;
        public int BitsLeft;
        public int RequestsLeft;
        public int AdvisoryDelay;
    }

    // StreamJsonRpc library uses newtonsoft json, so use its method of fixing enum serialisation
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum BlobOutputFormat { base64, hex };

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
    interface IBasicRandomApiV1
    {
        Task<GenerateIntegersResponseV1> GenerateIntegers(string apiKey, int n, int min, int max, bool replacement = true, int _base = 10);
        Task<GenerateDecimalFractionsResponseV1> GenerateDecimalFractions(string apiKey, int n, int decimalPlaces, bool replacement = true);
        Task<GenerateGaussiansResponseV1> GenerateGaussians(string apiKey, int n, double mean, double standardDeviation, int significantDigits);
        Task<StringsResponseV1> GenerateStrings(string apiKey, int n, int length, string characters, bool replacement = true);
        Task<StringsResponseV1> GenerateUUIDs(string apiKey, int n);
        Task<StringsResponseV1> GenerateBlobs(string apiKey, int n, int size, BlobOutputFormat format = BlobOutputFormat.base64);
        Task<GetUsageResponseV1> GetUsage(string apiKey);
    }

    interface IBasicRandomApiV2
    {
        Task<GenerateIntegersResponseV1> GenerateIntegers(string apiKey, int n, int min, int max, bool replacement = true, int _base = 10);
        Task<GenerateIntegerSequencesResponseV2> GenerateIntegerSequences(string apiKey, int n, int length, int min, int max, bool replacement = true, int _base = 10);
        Task<GenerateIntegerSequencesResponseV2> GenerateIntegerSequences(string apiKey, int n, int[] length, int[] min, int[] max, bool[] replacement, int[] _base);
        Task<GenerateDecimalFractionsResponseV1> GenerateDecimalFractions(string apiKey, int n, int decimalPlaces, bool replacement = true);
        Task<GenerateGaussiansResponseV1> GenerateGaussians(string apiKey, int n, double mean, double standardDeviation, int significantDigits);
        Task<StringsResponseV1> GenerateStrings(string apiKey, int n, int length, string characters, bool replacement = true);
        Task<StringsResponseV1> GenerateUUIDs(string apiKey, int n);
        Task<StringsResponseV1> GenerateBlobs(string apiKey, int n, int size, BlobOutputFormat format = BlobOutputFormat.base64);
        Task<GetUsageResponseV1> GetUsage(string apiKey);
    }

    interface IBasicRandomApiV4
    {
        Task<GenerateIntegersResponseV1> GenerateIntegers(string apiKey, int n, int min, int max, bool replacement = true, int _base = 10, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<GenerateIntegerSequencesResponseV2> GenerateIntegerSequences(string apiKey, int n, int length, int min, int max, bool replacement = true, int _base = 10, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<GenerateIntegerSequencesResponseV2> GenerateIntegerSequences(string apiKey, int n, int[] length, int[] min, int[] max, bool[] replacement, int[] _base, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<GenerateDecimalFractionsResponseV1> GenerateDecimalFractions(string apiKey, int n, int decimalPlaces, bool replacement = true, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<GenerateGaussiansResponseV1> GenerateGaussians(string apiKey, int n, double mean, double standardDeviation, int significantDigits, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<StringsResponseV1> GenerateStrings(string apiKey, int n, int length, string characters, bool replacement = true, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<StringsResponseV1> GenerateUUIDs(string apiKey, int n, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<StringsResponseV1> GenerateBlobs(string apiKey, int n, int size, BlobOutputFormat format = BlobOutputFormat.base64, PregeneratedRandomization? pregeneratedRandomization = null);
        Task<GetUsageResponseV1> GetUsage(string apiKey);
    }
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
}