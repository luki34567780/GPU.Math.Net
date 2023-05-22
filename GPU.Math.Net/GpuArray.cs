using OpenCL.Net;
using OpenCL.Net.Extensions;
using System.Data.Common;

namespace GPU.Math.Net
{
    public unsafe class GpuArray<T> : IDisposable where T : unmanaged
    {
        private bool _disposedValue;

        public GPU Gpu { get; private set; }
        public IMem<T> GpuMem { get; private set; }

        public Type Type { get; private set; }
        public int Elements { get; }
        public int Size { get; }

        public int Identifier { get; }
        public static int Counter = 0;

        public GpuArray(GPU gpu, T[] data, MemFlags flags)
        {
            Identifier = Counter++;

            Elements = data.Length;
            Size = Elements * sizeof(T);
            Gpu = gpu;
            GpuMem = Gpu.CreateBuffer(MemFlags.CopyHostPtr | flags, data);
            Type = typeof(T);
        }

        public GpuArray(GPU gpu, int elementCount, MemFlags flags)
        {
            Identifier = Counter++;

            Elements = elementCount;
            Size = elementCount * sizeof(T);
            Gpu = gpu;
            GpuMem = Gpu.CreateBuffer<T>(flags, Size);
        }

        public T[] ToCpu(T[] destination, int count, int offset)
        {
            Gpu.CopyToHost(GpuMem, destination, count, offset);
            return destination;
        }

        public T[] ToCpu() => ToCpu(new T[Elements], Elements, 0);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                GpuMem.Dispose();
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Add two values and store result into this
        /// </summary>
        /// <param name="other">list of values for addition</param>
        public void Add(GpuArray<T> other) => Add(other, Elements);

        /// <summary>
        /// Add two values and store result into this
        /// </summary>
        /// <param name="other">list of values for addition</param>
        /// <param name="count">the count of the Elements to process</param>
        public void Add(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Add));

            SetKernelArgsGeneric(ref kernel, other);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> AddToNewArray(GpuArray<T> other) => AddToNewArray(other, Elements);

        public GpuArray<T> AddToNewArray(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Add));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, other.GpuMem);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        /// <summary>
        /// Subtract all Elements of Array and stores Results in this
        /// </summary>
        /// <param name="other">other array</param>
        public void Subtract(GpuArray<T> other) => Subtract(other, Elements);

        /// <summary>
        /// Subtract count Elements of Array and stores Results in this
        /// </summary>
        /// <param name="other">other array</param>
        /// <param name="count">number of Items</param>
        public void Subtract(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Subtract));

            SetKernelArgsGeneric(ref kernel, other);

            Gpu.ExecuteKernel(kernel, count);
        }

        /// <summary>
        /// Subtracts two Arrays and stores Result into new Array
        /// </summary>
        /// <param name="other">other Array</param>
        /// <returns>Result Array</returns>
        public GpuArray<T> SubtractToNewArray(GpuArray<T> other) => SubtractToNewArray(other, Elements);

        /// <summary>
        /// Subtracts two Arrays and stores Result into new Array
        /// </summary>
        /// <param name="other">other Array</param>
        /// <param name="count">count of Items</param>
        /// <returns>Result Array</returns>
        public GpuArray<T> SubtractToNewArray(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Subtract));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, other.GpuMem);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Multiply(GpuArray<T> other) => Multiply(other, Elements);

        public void Multiply(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Multiply));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, other.GpuMem);
            Cl.SetKernelArg(kernel, 2, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> MultiplyToNewArray(GpuArray<T> other) => MultiplyToNewArray(other, Elements);

        public GpuArray<T> MultiplyToNewArray(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Multiply));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, other.GpuMem);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Divide(GpuArray<T> other) => Divide(other, Elements);

        public void Divide(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Divide));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, other.GpuMem);
            Cl.SetKernelArg(kernel, 2, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> DivideToNewArray(GpuArray<T> other) => DivideToNewArray(other, Elements);

        public GpuArray<T> DivideToNewArray(GpuArray<T> other, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Divide));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, other.GpuMem);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Sqrt() => Sqrt(Elements);

        public void Sqrt(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Sqrt));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> SqrtToNewArray() => SqrtToNewArray(Elements);

        public GpuArray<T> SqrtToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Sqrt));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void RSqrt() => RSqrt(Elements);

        public void RSqrt(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(RSqrt));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> RSqrtToNewArray() => RSqrtToNewArray(Elements);

        public GpuArray<T> RSqrtToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(RSqrt));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Modulo(T value) => Modulo(value, Elements);

        public void Modulo(T value, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "ModuloConstantValue");

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, value);
            Cl.SetKernelArg(kernel, 2, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> ModuloToNewArray(T value) => ModuloToNewArray(value, Elements);

        public GpuArray<T> ModuloToNewArray(T value, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "ModuloConstantValue");
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, value);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Modulo(GpuArray<T> values) => Modulo(values, Elements);

        public void Modulo(GpuArray<T> values, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "ModuloDynamicValue");

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, values.GpuMem);
            Cl.SetKernelArg(kernel, 2, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> ModuloToNewArray(GpuArray<T> values) => ModuloToNewArray(values, Elements);

        public GpuArray<T> ModuloToNewArray(GpuArray<T> values, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "ModuloDynamicValue");
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, values.GpuMem);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Pow(T value) => Pow(value, Elements);

        public void Pow(T value, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "PowConstantValue");

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, value);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> PowToNewArray(T value) => PowToNewArray(value, Elements);

        public GpuArray<T> PowToNewArray(T value, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "PowConstantValue");
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, value);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Pow(GpuArray<T> values) => Pow(values, Elements);

        public void Pow(GpuArray<T> values, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "PowDynamicValue");

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, values.GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> PowToNewArray(GpuArray<T> values) => PowToNewArray(values, Elements);

        public GpuArray<T> PowToNewArray(GpuArray<T> values, int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, "PowDynamicValue");
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, values.GpuMem);
            Cl.SetKernelArg(kernel, 2, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Sin() => Sin(Elements);

        public void Sin(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Sin));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> SinToNewArray() => SinToNewArray(Elements);

        public GpuArray<T> SinToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Sin));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Cos() => Cos(Elements);

        public void Cos(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Cos));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> CosToNewArray() => CosToNewArray(Elements);

        public GpuArray<T> CosToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Cos));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Tan() => Tan(Elements);

        public void Tan(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Tan));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> TanToNewArray() => TanToNewArray(Elements);

        public GpuArray<T> TanToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Tan));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Log() => Log(Elements);

        public void Log(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Log));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> LogToNewArray() => LogToNewArray(Elements);

        public GpuArray<T> LogToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Log));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Log2() => Log2(Elements);

        public void Log2(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Log2));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> Log2ToNewArray() => Log2ToNewArray(Elements);

        public GpuArray<T> Log2ToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Log2));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Log10() => Log10(Elements);

        public void Log10(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Log10));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> Log10ToNewArray() => Log10ToNewArray(Elements);

        public GpuArray<T> Log10ToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Log10));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public void Abs() => Abs(Elements);

        public void Abs(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Abs));

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, GpuMem);

            Gpu.ExecuteKernel(kernel, count);
        }

        public GpuArray<T> AbsToNewArray() => AbsToNewArray(Elements);

        public GpuArray<T> AbsToNewArray(int count)
        {
            var kernel = OperationKernelManager.GetKernel(Gpu, Type, nameof(Log10));
            var result = new GpuArray<T>(Gpu, count, MemFlags.ReadWrite);

            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, result.GpuMem);

            Gpu.ExecuteKernel(kernel, count);

            return result;
        }

        public static GpuArray<T> operator +(GpuArray<T> left, GpuArray<T> right) => left.AddToNewArray(right);
        public static GpuArray<T> operator -(GpuArray<T> left, GpuArray<T> right) => left.SubtractToNewArray(right);
        public static GpuArray<T> operator *(GpuArray<T> left, GpuArray<T> right) => left.MultiplyToNewArray(right);
        public static GpuArray<T> operator /(GpuArray<T> left, GpuArray<T> right) => left.DivideToNewArray(right);
        public static GpuArray<T> operator %(GpuArray<T> left, GpuArray<T> right) => left.ModuloToNewArray(right);
        public static GpuArray<T> operator %(GpuArray<T> left, T right) => left.ModuloToNewArray(right);

        private void SetKernelArgsGeneric(ref Kernel kernel, GpuArray<T> other)
        {
            Cl.SetKernelArg(kernel, 0, GpuMem);
            Cl.SetKernelArg(kernel, 1, other.GpuMem);
            Cl.SetKernelArg(kernel, 2, GpuMem);
        }

        ~GpuArray()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}