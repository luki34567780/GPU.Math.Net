using Accelerated.Math.Net;
using Accelerated.Math.Net.Backends.Fallback;

namespace Gpu.Math.Net.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new FallbackMemory<int>(1000, new int[1000].Select(x => Random.Shared.Next(1000)).ToArray());

            a.Pow(2);
        }
    }
}