using Accelerated.Math.Net;
using Accelerated.Math.Net.Backends;

namespace Gpu.Math.Net.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new Avx128<int>(1000, new int[1000].Select(x => Random.Shared.Next(1000)).ToArray());
            var b = new Avx128<int>(1000, new int[1000].Select(x => Random.Shared.Next(1000)).ToArray());

            a.Add(b);
        }
    }
}