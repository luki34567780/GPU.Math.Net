using GPU.Math.Net;
using OpenCL.Net;
using System.Diagnostics;

namespace GpuArrayDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var platforms = GPU.Math.Net.GPU.GetPlatforms();
            var devices = GPU.Math.Net.GPU.GetDevices(platforms[0]);
            using var gpu = new GPU.Math.Net.GPU(devices[0]);

            var rnd = Random.Shared;

            const int items = 100000;
            const int iterations = 1;

            var src = new int[items].Select(x => (int)rnd.Next(0, 100)).ToArray();

            var aCpu = new int[items];

            src.CopyTo(aCpu, 0);

            using var a = new GpuArray<int>(gpu, aCpu, MemFlags.CopyHostPtr | MemFlags.ReadWrite);


            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                a.Modulo(1);
                //a.Multiply(a);
            }

            Cl.Finish(gpu.Queue);

            sw.Stop();
            Console.WriteLine($"Processing on GPU took {sw.Elapsed.Minutes}m {sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                Parallel.For(0, items, (j) =>
                {
                    aCpu[j] = aCpu[j] % 1;
                    //aCpu[j] = aCpu[j] * aCpu[j];

                });
            }

            sw.Stop();
            Console.WriteLine($"Processing on CPU took {sw.Elapsed.Minutes}m {sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms");

            var aGpu = a.ToCpu();

            Console.WriteLine(aGpu.SequenceEqual(aCpu));
            Console.WriteLine($"GPU: {aGpu.Sum(x => x)}, CPU: {aCpu.Sum(x => x)}");
            Console.WriteLine(src.Sum(x => x));
        }
    }
}