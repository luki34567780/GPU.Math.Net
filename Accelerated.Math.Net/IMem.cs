using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accelerated.Math.Net
{
    public interface IMem<T> : IDisposable
    {
        public bool Disposed { get; }
        public T this[int index] { get; set; }

        public void Add(IMem<T> other);
        public void Add(IMem<T> other, int count);
        public IMem<T> AddToNewArray(IMem<T> other);
        public IMem<T> AddToNewArray(IMem<T> other, int count);
        public void Subtract(IMem<T> other);
        public void Subtract(IMem<T> other, int count);
        public IMem<T> SubtractToNewArray(IMem<T> other);
        public IMem<T> SubtractToNewArray(IMem<T> other, int count);
        public void Multiply(IMem<T> other);
        public void Multiply(IMem<T> other, int count);
        public IMem<T> MultiplyToNewArray(IMem<T> other);
        public IMem<T> MultiplyToNewArray(IMem<T> other, int count);
        public void Divide(IMem<T> other);
        public void Divide(IMem<T> other, int count);
        public IMem<T> DivideToNewArray(IMem<T> other);
        public IMem<T> DivideToNewArray(IMem<T> other, int count);
        public void Sqrt();
        public void Sqrt(int count);
        public IMem<T> SqrtToNewArray();
        public IMem<T> SqrtToNewArray(int count);
        public void RSqrt();
        public void RSqrt(int count);
        public IMem<T> RSqrtToNewArray();
        public IMem<T> RSqrtToNewArray(int count);
        public void Modulo(T value);
        public void Modulo(T value, int count);
        public IMem<T> ModuloToNewArray(T value);
        public IMem<T> ModuloToNewArray(T value, int count);
        public void Modulo(IMem<T> values);
        public void Modulo(IMem<T> values, int count);
        public IMem<T> ModuloToNewArray(IMem<T> values);
        public IMem<T> ModuloToNewArray(IMem<T> values, int count);
        public void Pow(T value);
        public void Pow(T value, int count);
        public IMem<T> PowToNewArray(T value);
        public IMem<T> PowToNewArray(T value, int count);
        public void Pow(IMem<T> values);
        public void Pow(IMem<T> values, int count);
        public IMem<T> PowToNewArray(IMem<T> values);
        public IMem<T> PowToNewArray(IMem<T> values, int count);
        public void Sin();
        public void Sin(int count);
        public IMem<T> SinToNewArray();
        public IMem<T> SinToNewArray(int count);
        public void Cos();
        public void Cos(int count);
        public IMem<T> CosToNewArray();
        public IMem<T> CosToNewArray(int count);
        public void Tan();
        public void Tan(int count);
        public IMem<T> TanToNewArray();
        public IMem<T> TanToNewArray(int count);
        public void Log();
        public void Log(int count);
        public IMem<T> LogToNewArray();
        public IMem<T> LogToNewArray(int count);
        public void Log2();
        public void Log2(int count);
        public IMem<T> Log2ToNewArray();
        public IMem<T> Log2ToNewArray(int count);
        public void Log10();
        public void Log10(int count);
        public IMem<T> Log10ToNewArray();
        public IMem<T> Log10ToNewArray(int count);
        public void Abs();
        public void Abs(int count);
        public IMem<T> AbsToNewArray();
        public IMem<T> AbsToNewArray(int count);
    }
}
