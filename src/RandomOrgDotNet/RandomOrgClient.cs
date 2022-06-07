using RandomOrgDotNet.JsonRpc;
using StreamJsonRpc;

namespace RandomOrgDotNet
{
    public sealed class RandomClient : IDisposable
    {
        private readonly string apiKey;
        private const string ApiUri = "https://api.random.org/json-rpc/4/invoke";
        private readonly IBasicRandomApiV4 apiStub;

        private readonly PregeneratedRandomization? seed;

        private readonly HttpClient httpClient;
        private readonly AdvisoryWait advisoryWait = new AdvisoryWait();

        private static string ToCamelCase(string input)
        {
            if (!char.IsLower(input.First()))
            {
                var asArray = input.ToCharArray();
                asArray[0] = char.ToLowerInvariant(input[0]);
                return new string(asArray);
            }
            else
            {
                return input;
            }
        }

        public RandomClient(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));

            this.apiKey = apiKey;
            var options = new JsonRpcProxyOptions()
            {
                MethodNameTransform = ToCamelCase
            };

            var requestUri = new Uri(ApiUri);
            httpClient = new HttpClient();
            var handler = new HttpClientMessageHandler(httpClient, requestUri);
            apiStub  = StreamJsonRpc.JsonRpc.Attach<IBasicRandomApiV4>(handler, options);
        }

        public RandomClient(string apiKey, string seedIdentifier) : this(apiKey)
        {
            seed = PregeneratedRandomization.ForIdentifier(seedIdentifier);
        }

        public RandomClient(string apiKey, DateTime seedDateTime) : this(apiKey)
        {
            seed = PregeneratedRandomization.ForDate(seedDateTime);
        }

        public async Task<IEnumerable<int>> GenerateIntegersAsync(int count, int minimum, int maximum)
        {
            using (var waitLock = advisoryWait.NewContext())
            {
                await waitLock.AcquireAndWaitAsync();
                var rawResult = await apiStub.GenerateIntegers(apiKey, count, minimum, maximum, true, 10, seed);
                waitLock.SetWaitTime(rawResult.AdvisoryDelay);
                return rawResult.Random.Data;
            }
        }

        public async Task<IEnumerable<Guid>> GenerateGuidsAsync(int count)
        {
            using (var waitLock = advisoryWait.NewContext())
            {
                await waitLock.AcquireAndWaitAsync();
                var rawResult = await apiStub.GenerateUUIDs(apiKey, count, seed);
                waitLock.SetWaitTime(rawResult.AdvisoryDelay);
                return rawResult.Random.Data.Select(str => Guid.Parse(str));
            }
        }

        public async Task<IEnumerable<string>> GenerateStringsAsync(int count, int stringLength, string charsToUse)
        {
            using (var waitLock = advisoryWait.NewContext())
            {
                await waitLock.AcquireAndWaitAsync();
                var rawResult = await apiStub.GenerateStrings(apiKey, count, stringLength, charsToUse, true, seed);
                waitLock.SetWaitTime(rawResult.AdvisoryDelay);
                return rawResult.Random.Data;
            }
        }

        public async Task<IEnumerable<decimal>> GenerateDecimalsAsync(int count, int decimalPlaces)
        {
            using (var waitLock = advisoryWait.NewContext())
            {
                await waitLock.AcquireAndWaitAsync();
                var rawResult = await apiStub.GenerateDecimalFractions(apiKey, count, decimalPlaces, true, seed);
                waitLock.SetWaitTime(rawResult.AdvisoryDelay);
                return rawResult.Random.Data;
            }
        }

        public async Task<IEnumerable<double>> GenerateGaussiansAsync(int count, double mean, double standardDeviation, int significantDigits)
        {
            using (var waitLock = advisoryWait.NewContext())
            {
                await waitLock.AcquireAndWaitAsync();
                var rawResult = await apiStub.GenerateGaussians(apiKey, count, mean, standardDeviation, significantDigits, seed);
                waitLock.SetWaitTime(rawResult.AdvisoryDelay);
                return rawResult.Random.Data;
            }
        }

        public async Task<IEnumerable<byte[]>> GenerateByteArraysAsync(int count, int length)
        {
            using (var waitLock = advisoryWait.NewContext())
            {
                await waitLock.AcquireAndWaitAsync();
                var rawResult = await apiStub.GenerateBlobs(apiKey, count, length * 8, BlobOutputFormat.hex, seed);
                waitLock.SetWaitTime(rawResult.AdvisoryDelay);
                return rawResult.Random.Data.Select(str => Convert.FromHexString(str));
            }
        }

        public async Task<(bool running, int requestsLeft, int bitsLeft)> GetUsageAsync()
        {
            var rawResult = await apiStub.GetUsage(apiKey);
            var running = rawResult.Status == GetUsageResponseV1.StatusType.running;
            return (running, rawResult.RequestsLeft, rawResult.BitsLeft);
        }

        public void Dispose()
        {
            ((IDisposable)httpClient).Dispose();
        }
    }
}