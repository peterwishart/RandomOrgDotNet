// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// apikey
// You need to create an account (free) here: https://accounts.random.org/create
// Then create an API key via https://accounts.random.org/ => API Services
string apiKey = string.Empty;

var client = new RandomOrgDotNet.RandomClient(apiKey);

(bool running, int requestsUsedPercent, int bitsUsedPercent) = await client.GetUsageAsync();
Console.WriteLine($"apiKey status: running {running} requestsUsed: {requestsUsedPercent}% bitsUsed: {bitsUsedPercent}%");

Console.WriteLine("Rolling 10 dice:");
var a1 = await client.GenerateIntegersAsync(10, 1, 6);
Console.WriteLine(string.Join(",", a1));

Console.WriteLine("More random things:");
Parallel.For(1, 6, (iter) =>
{
    Task.Run(async () =>
    {
        switch (iter)
        {
            case 1:
                Console.WriteLine(string.Join(",", await client.GenerateGuidsAsync(2)));
                break;
            case 2:
                Console.WriteLine(string.Join(",", await client.GenerateStringsAsync(10, 7, "acgt")));
                break;
            case 3:
                Console.WriteLine(string.Join(",", await client.GenerateDecimalsAsync(10, 4)));
                break;
            case 4:
                Console.WriteLine(string.Join(",", await client.GenerateGaussiansAsync(10, 0.0, 1.0, 8)));
                break;
            case 5:
                Console.WriteLine(string.Join(",", (await client.GenerateByteArraysAsync(4, 16)).Select(arr => Convert.ToBase64String(arr))));
                break;

        }
    }).Wait();
});

(running, requestsUsedPercent, bitsUsedPercent) = await client.GetUsageAsync();
Console.WriteLine($"apiKey status: running {running} requestsUsed: {requestsUsedPercent}% bitsUsed: {bitsUsedPercent}%");

Console.WriteLine("Finished, press a key to quit");
Console.ReadKey();
