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

        public IMem<T> PowToNewArray(T value)
        {
            throw new NotImplementedException();
        }

        public IMem<T> PowToNewArray(T value, int count)
        {
            throw new NotImplementedException();
        }

        public void Pow(IMem<T> values)
        {
            throw new NotImplementedException();
        }

        public void Pow(IMem<T> values, int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> PowToNewArray(IMem<T> values)
        {
            throw new NotImplementedException();
        }

        public IMem<T> PowToNewArray(IMem<T> values, int count)
        {
            throw new NotImplementedException();
        }

        public void Sin()
        {
            throw new NotImplementedException();
        }

        public void Sin(int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> SinToNewArray()
        {
            throw new NotImplementedException();
        }

        public IMem<T> SinToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public void Cos()
        {
            throw new NotImplementedException();
        }

        public void Cos(int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> CosToNewArray()
        {
            throw new NotImplementedException();
        }

        public IMem<T> CosToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public void Tan()
        {
            throw new NotImplementedException();
        }

        public void Tan(int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> TanToNewArray()
        {
            throw new NotImplementedException();
        }

        public IMem<T> TanToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public void Log()
        {
            throw new NotImplementedException();
        }

        public void Log(int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> LogToNewArray()
        {
            throw new NotImplementedException();
        }

        public IMem<T> LogToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public void Log2()
        {
            throw new NotImplementedException();
        }

        public void Log2(int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> Log2ToNewArray()
        {
            throw new NotImplementedException();
        }

        public IMem<T> Log2ToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public void Log10()
        {
            throw new NotImplementedException();
        }

        public void Log10(int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> Log10ToNewArray()
        {
            throw new NotImplementedException();
        }

        public IMem<T> Log10ToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public void Abs()
        {
            throw new NotImplementedException();
        }

        public void Abs(int count)
        {
            throw new NotImplementedException();
        }

        public IMem<T> AbsToNewArray()
        {
            throw new NotImplementedException();
        }

        public IMem<T> AbsToNewArray(int count)
        {
            throw new NotImplementedException();
        }
    }
}
