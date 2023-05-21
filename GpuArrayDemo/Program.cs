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
            var devices = GPU.Math.Net.GPU.GetDevices(platforms[1]);
            using var gpu = new GPU.Math.Net.GPU(devices[0]);

            var rnd = Random.Shared;

            const int items = 100000000;
            const int iterations = 10000;

            var aCpu = new ushort[items].Select(x => (ushort)rnd.Next(0, ushort.MaxValue)).ToArray();
            using var a = new GpuArray<ushort>(gpu, aCpu, MemFlags.CopyHostPtr | MemFlags.ReadWrite);


            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                a.Add(a);
                a.Multiply(a);
            }

            Cl.Finish(gpu.Queue);

            sw.Stop();
            Console.WriteLine($"Processing on GPU took {sw.Elapsed.Minutes}m {sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms");

            sw.Restart();
            for (int i = 0; i < iterations; i++)
            {
                Parallel.For(0, items, (j) =>
                {
                    aCpu[j] = (ushort)(aCpu[j] + aCpu[j]);
                    aCpu[j] = (ushort)(aCpu[j] * aCpu[j]);

                });
            }

            sw.Stop();
            Console.WriteLine($"Processing on CPU took {sw.Elapsed.Minutes}m {sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms");

            var aGpu = a.ToCpu();

            Console.WriteLine(aGpu.SequenceEqual(aCpu));
            Console.WriteLine($"GPU: {aGpu.Sum(x => x)}, CPU: {aCpu.Sum(x => x)}");
        }
    }
}