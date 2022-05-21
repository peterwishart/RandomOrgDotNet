using System.Diagnostics;

namespace RandomOrgDotNet
{
    interface IAdvisoryWaitContext
    {
        Task AcquireAndWaitAsync();
        void SetWaitTime(int delayMs);
    }

    internal class AdvisoryWait
    {
        internal Stopwatch lastCallStopwatch = Stopwatch.StartNew();
        internal long lastCallAdvisoryDelayTicks = 0;
        private readonly SemaphoreSlim callLock = new SemaphoreSlim(1);

        internal AdvisoryWaitContext NewContext()
        {
            return new AdvisoryWaitContext(this);
        }

        internal class AdvisoryWaitContext : IAdvisoryWaitContext, IDisposable
        {
            private readonly AdvisoryWait parent;

            public AdvisoryWaitContext(AdvisoryWait parent)
            {
                this.parent = parent;
            }

            public async Task AcquireAndWaitAsync()
            {
                await parent.callLock.WaitAsync();
                var limit = parent.lastCallAdvisoryDelayTicks;
                var time = parent.lastCallStopwatch.ElapsedTicks;

                if (time < limit)
                {
                    var delayMs = (int)((limit - time) / TimeSpan.TicksPerMillisecond);
                    if (delayMs > 0)
                    {
                        await Task.Delay(delayMs);
                    }
                }
            }

            public void Dispose()
            {
                parent.callLock.Release();
            }

            public void SetWaitTime(int delayMs)
            {
                parent.lastCallAdvisoryDelayTicks = delayMs * TimeSpan.TicksPerMillisecond;
                parent.lastCallStopwatch.Restart();
            }
        }
    }
}
