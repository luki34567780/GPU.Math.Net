using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Accelerated.Math.Net.Backends.Fallback
{
    // IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>, IMultiplyOperators<T, T, T>, IDivisionOperators<T, T, T>, IModulusOperators<T, T, T>, 
    public class FallbackMemory<T> : IMem<T> where T : unmanaged, INumber<T>
    {
        public FallbackMemory(int count, T[]? values = null)
        {
            _values = new T[count];

            values?.CopyTo(_values, 0);
        }

        private T[] _values;

        public int Count => _values.Length;

        public T this[int index] { get => _values[index]; set => _values[index] = value; }

        public bool Disposed => throw new NotImplementedException();

        public void Dispose() { }

        public void Add(IMem<T> other) => Add(other, Count);

        public void Add(IMem<T> other, int count)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] += other[i];
            }
        }

        public IMem<T> AddToNewArray(IMem<T> other) => AddToNewArray(other, Count);

        public IMem<T> AddToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);
            
            for (int i = 0; i < count; i++)
                res[i] = _values[i] + other[i];

            return res;
        }

        public void Subtract(IMem<T> other) => Subtract(other, Count);

        public void Subtract(IMem<T> other, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] -= other[i];
        }

        public IMem<T> SubtractToNewArray(IMem<T> other) => SubtractToNewArray(other, Count);

        public IMem<T> SubtractToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] - other[i];

            return res;
        }

        public void Multiply(IMem<T> other) => Multiply(other, Count);

        public void Multiply(IMem<T> other, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] *= other[i];
        }

        public IMem<T> MultiplyToNewArray(IMem<T> other) => MultiplyToNewArray(other, Count);

        public IMem<T> MultiplyToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] * other[i];

            return res;
        }

        public void Divide(IMem<T> other) => Divide(other, Count);

        public void Divide(IMem<T> other, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] /= other[i];
        }

        public IMem<T> DivideToNewArray(IMem<T> other) => DivideToNewArray(other, Count);

        public IMem<T> DivideToNewArray(IMem<T> other, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] / other[i];

            return res;
        }

        public void Sqrt() => Sqrt(Count);

        public void Sqrt(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> SqrtToNewArray() => SqrtToNewArray(Count);

        public IMem<T> SqrtToNewArray(int count)
        {
            var result = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                result[i] = (T)Convert.ChangeType(System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));

            return result;
        }

        public void RSqrt() => RSqrt(Count);

        public void RSqrt(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(1d / System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> RSqrtToNewArray() => RSqrtToNewArray(Count);

        public IMem<T> RSqrtToNewArray(int count)
        {
            var result = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                result[i] = (T)Convert.ChangeType(1d / System.Math.Sqrt(Convert.ToDouble(_values[i])), typeof(T));

            return result;
        }

        public void Modulo(T value) => Modulo(value, Count);

        public void Modulo(T value, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] %= value;
        }

        public IMem<T> ModuloToNewArray(T value) => ModuloToNewArray(value, Count);

        public IMem<T> ModuloToNewArray(T value, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] % value;

            return res;
        }

        public void Modulo(IMem<T> values) => Modulo(values, Count);

        public void Modulo(IMem<T> values, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] %= values[i];
        }

        public IMem<T> ModuloToNewArray(IMem<T> values) => ModuloToNewArray(values, Count);

        public IMem<T> ModuloToNewArray(IMem<T> values, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = _values[i] % values[i];

            return res;
        }

        public void Pow(T value) => Pow(value, Count);

        public void Pow(T value, int count)
        {
            var val = Convert.ToDouble(value);
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), val), typeof(T));
        }

        public IMem<T> PowToNewArray(T value) => PowToNewArray(value, Count);

        public IMem<T> PowToNewArray(T value, int count)
        {
            var val = Convert.ToDouble(value);
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), val), typeof(T));

            return res;
        }

        public void Pow(IMem<T> values) => Pow(values, Count);

        public void Pow(IMem<T> values, int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), Convert.ToDouble(values[i])), typeof(T));
        }

        public IMem<T> PowToNewArray(IMem<T> values)
        {
            throw new NotImplementedException();
        }

        public IMem<T> PowToNewArray(IMem<T> values, int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Pow(Convert.ToDouble(_values[i]), Convert.ToDouble(values[i])), typeof(T));

            return res;
        }

        public void Sin() => Sin(Count);

        public void Sin(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Sin(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> SinToNewArray() => SinToNewArray(Count);

        public IMem<T> SinToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Sin(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public void Cos() => Cos(Count);

        public void Cos(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Cos(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> CosToNewArray() => CosToNewArray(Count);

        public IMem<T> CosToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Cos(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public void Tan() => Tan(Count);

        public void Tan(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Tan(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> TanToNewArray() => TanToNewArray(Count);

        public IMem<T> TanToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Tan(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public void Log() => Log(Count);

        public void Log(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Log(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> LogToNewArray() => LogToNewArray(Count);

        public IMem<T> LogToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Log(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public void Log2() => Log2(Count);

        public void Log2(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Log2(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> Log2ToNewArray() => Log2ToNewArray(Count);

        public IMem<T> Log2ToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Log2(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public void Log10() => Log10(Count);

        public void Log10(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Log10(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> Log10ToNewArray() => Log10ToNewArray(Count);

        public IMem<T> Log10ToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Log10(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }

        public void Abs() => Abs(Count);

        public void Abs(int count)
        {
            for (int i = 0; i < count; i++)
                _values[i] = (T)Convert.ChangeType(System.Math.Abs(Convert.ToDouble(_values[i])), typeof(T));
        }

        public IMem<T> AbsToNewArray() => AbsToNewArray(Count);

        public IMem<T> AbsToNewArray(int count)
        {
            var res = new FallbackMemory<T>(count);

            for (int i = 0; i < count; i++)
                res[i] = (T)Convert.ChangeType(System.Math.Abs(Convert.ToDouble(_values[i])), typeof(T));

            return res;
        }
    }
}
