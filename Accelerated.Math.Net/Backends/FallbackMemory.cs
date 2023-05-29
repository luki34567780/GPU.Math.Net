using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Accelerated.Math.Net.Backends
{
    // IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IMultiplyOperators<T, T, T>, IDivisionOperators<T, T, T>, IModulusOperators<T, T, T>, 
    public class FallbackMemory<T> : IMem<T> where T : unmanaged, INumber<T>
    {
        public FallbackMemory(int count, T[]? values = null)
        {
            _values = new T[count];

            values?.CopyTo(_values, 0);
        }

        protected T[] _values;

        protected bool _disposed = false;

        public int Count => _values.Length;

        public T this[int index] { get => _values[index]; set => _values[index] = value; }

        public bool Disposed => _disposed;

        public virtual void Dispose() => _disposed = true;

        public virtual void Add(IMem<T> other) => Add(other, Count);

        public virtual void Add(IMem<T> other, int count)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] += other[i];
            }
        }

        public virtual IMem<T> AddToNewArray(IMem<T> other) => AddToNewArray(other, Count);

        public virtual IMem<T> AddToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] + other[i];

            return res;
        }

        public virtual void Subtract(IMem<T> other) => Subtract(other, Count);

        public virtual void Subtract(IMem<T> other, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] -= other[i];
        }

        public virtual IMem<T> SubtractToNewArray(IMem<T> other) => SubtractToNewArray(other, Count);

        public virtual IMem<T> SubtractToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] - other[i];

            return res;
        }

        public virtual void Multiply(IMem<T> other) => Multiply(other, Count);

        public virtual void Multiply(IMem<T> other, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] *= other[i];
        }

        public virtual IMem<T> MultiplyToNewArray(IMem<T> other) => MultiplyToNewArray(other, Count);

        public virtual IMem<T> MultiplyToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] * other[i];

            return res;
        }

        public virtual void Divide(IMem<T> other) => Divide(other, Count);

        public virtual void Divide(IMem<T> other, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] /= other[i];
        }

        public virtual IMem<T> DivideToNewArray(IMem<T> other) => DivideToNewArray(other, Count);

        public virtual IMem<T> DivideToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] / other[i];

            return res;
        }

        public virtual void Sqrt() => Sqrt(Count);

        public virtual void Sqrt(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> SqrtToNewArray() => SqrtToNewArray(Count);

        public virtual IMem<T> SqrtToNewArray(int count)
        {
            var result = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                result[i] = (T)Convert.ChangeType(System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));

            return result;
        }

        public virtual void RSqrt() => RSqrt(Count);

        public virtual void RSqrt(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(1d / System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> RSqrtToNewArray() => RSqrtToNewArray(Count);

        public virtual IMem<T> RSqrtToNewArray(int count)
        {
            var result = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                result[i] = (T)Convert.ChangeType(1d / System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));

            return result;
        }

        public virtual void Modulo(T value) => Modulo(value, Count);

        public virtual void Modulo(T value, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] %= value;
        }

        public virtual IMem<T> ModuloToNewArray(T value) => ModuloToNewArray(value, Count);

        public virtual IMem<T> ModuloToNewArray(T value, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] % value;

            return res;
        }

        public virtual void Modulo(IMem<T> values) => Modulo(values, Count);

        public virtual void Modulo(IMem<T> values, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] %= values[i];
        }

        public virtual IMem<T> ModuloToNewArray(IMem<T> values) => ModuloToNewArray(values, Count);

        public virtual IMem<T> ModuloToNewArray(IMem<T> values, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] % values[i];

            return res;
        }

        public virtual void Pow(T value) => Pow(value, Count);

        public virtual void Pow(T value, int count)
        {
            var val = Convert.ToDouble(value);
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), val), typeof(T));
        }

        public virtual IMem<T> PowToNewArray(T value) => PowToNewArray(value, Count);

        public virtual IMem<T> PowToNewArray(T value, int count)
        {
            var val = Convert.ToDouble(value);
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), val), typeof(T));

            return res;
        }

        public virtual void Pow(IMem<T> values) => Pow(values, Count);

        public virtual void Pow(IMem<T> values, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), Convert.ToDouble(values[i])), typeof(T));
        }

        public virtual IMem<T> PowToNewArray(IMem<T> values) => PowToNewArray(values, Count);

        public virtual IMem<T> PowToNewArray(IMem<T> values, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), Convert.ToDouble(values[i])), typeof(T));

            return res;
        }

        public virtual void Sin() => Sin(Count);

        public virtual void Sin(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Sin(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> SinToNewArray() => SinToNewArray(Count);

        public virtual IMem<T> SinToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Sin(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public virtual void Cos() => Cos(Count);

        public virtual void Cos(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Cos(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> CosToNewArray() => CosToNewArray(Count);

        public virtual IMem<T> CosToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Cos(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public virtual void Tan() => Tan(Count);

        public virtual void Tan(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Tan(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> TanToNewArray() => TanToNewArray(Count);

        public virtual IMem<T> TanToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Tan(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public virtual void Log() => Log(Count);

        public virtual void Log(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Log(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> LogToNewArray() => LogToNewArray(Count);

        public virtual IMem<T> LogToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Log(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public virtual void Log2() => Log2(Count);

        public virtual void Log2(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Log2(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> Log2ToNewArray() => Log2ToNewArray(Count);

        public virtual IMem<T> Log2ToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Log2(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public virtual void Log10() => Log10(Count);

        public virtual void Log10(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Log10(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> Log10ToNewArray() => Log10ToNewArray(Count);

        public virtual IMem<T> Log10ToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Log10(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public virtual void Abs() => Abs(Count);

        public virtual void Abs(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Abs(Convert.ToDouble(_values[i])), typeof(T));
        }

        public virtual IMem<T> AbsToNewArray() => AbsToNewArray(Count);

        public virtual IMem<T> AbsToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Abs(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public virtual void ToDevice(T[] data)
        {
            var span = data.AsSpan();
            ToDevice(ref span);
        }

        public virtual void ToDeviceGuaranteedCopy(T[] data)
        {
            var span = data.AsSpan();
            ToDeviceGuaranteedCopy(ref span);
        }

        public T[] ToCpu() => _values;

        public T[] ToCpuGuaranteedCopy()
        {
            var mem = new T[Count];
            _values.CopyTo(mem, 0);

            return mem;
        }
        public virtual void ToCopGuaranteedCopy(ref Span<T> result) => _values.CopyTo(result);

        public virtual void ToDevice(ref Span<T> data) => data.CopyTo(_values);

        public virtual void ToDeviceGuaranteedCopy(ref Span<T> data) => data.CopyTo(_values);

        public virtual void ToCpuGuaranteedCopy(ref Span<T> destination) => _values.CopyTo(destination);
    }
}
