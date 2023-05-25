using System.Reflection;

namespace GPU.Math.Net.Tests
{
    public class UnitTest1
    {
        public GPU Gpu;

        public UnitTest1()
        {
            Gpu = new GPU(GPU.GetSomeDevice());
        }

        public static dynamic CastTo(object value, Type target)
        {
            MethodInfo castMethod = typeof(UnitTest1)
                .GetMethod("PerformCast", BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(target);

            return castMethod.Invoke(null, new object[] { value });
        }
        private static T PerformCast<T>(object value)
        {
            if (value.GetType() != typeof(T))
                return (T)CastTo(value, value.GetType());

            return (T)value;
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        public void KernelsDoCompileCorrectly(Type type)
        {
            var items = new string[] { "Add", "Subtract", "Multiply", "Divide", "Sqrt", "RSqrt", "ModuloConstantValue", "ModuloDynamicValue", "PowConstantValue", "PowDynamicValue", "Sin", "Cos", "Tan", "Log", "Log2", "Log10", "Abs" };

            foreach (var item in items)
                OperationKernelManager.GetKernel(Gpu, type, item);
        }
    }
}