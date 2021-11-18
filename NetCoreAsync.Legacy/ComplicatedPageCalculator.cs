namespace NetCoreAsync.Legacy
{
    using System.Diagnostics;

    /// <summary>
    /// Mimics legacy, long-running code.
    /// </summary>
    public class ComplicatedPageCalculator
    {
        public int CalculateBookPages()
        {
            var watch = new Stopwatch { };

            watch.Start();

            while (true)
            {
                if (watch.ElapsedMilliseconds > 5000)
                {
                    break;
                }
            }

            return 42;
        }
    }
}
