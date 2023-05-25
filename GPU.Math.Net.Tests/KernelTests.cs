using System.Numerics;
using System.Reflection;

namespace GPU.Math.Net.Tests
{
    public class KernelTests
    {
        public GPU Gpu;

        public KernelTests()
        {
            Gpu = new GPU(GPU.GetSomeDevice());
        }


        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(char))]
        [InlineData(typeof(Half))]
        public void KernelsDoCompileCorrectly(Type type)
        {
            // dynamicly find all functions
            var items = Directory.EnumerateFiles("kernels/generics", "*.cl").Select(x => (x.EndsWith(".cl") ? x.TrimEnd('l').TrimEnd('c').TrimEnd('.') : x).Split('\\').Last());

            foreach (var item in items)
                OperationKernelManager.GetKernel(Gpu, type, item);

            OperationKernelManager.ClearCacheAndDisposeKernels();
        }
    }
}
