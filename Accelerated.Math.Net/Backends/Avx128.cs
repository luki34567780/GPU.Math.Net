using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Accelerated.Math.Net.Backends
{
    internal static class Imports
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern nint memcpy(nint dest, nint src, nint count);
    }

    public unsafe class Vector128ForTypeGenerator<T> where T : unmanaged, INumber<T>
    {
        public int ItemsInVector = Vector128<T>.Count;
        public delegate Vector128<T> LoadVector128Delegate(T* src);
        public delegate void StoreVector128Delegate(T* address, Vector128<T> src);
        public delegate Vector128<T> AddVector128Delegate(Vector128<T> left, Vector128<T> right);
        public delegate Vector128<T> SubtractVector128Delegate(Vector128<T> left, Vector128<T> right);
        public delegate Vector128<T> MultiplyVector128Delegate(Vector128<T> left, Vector128<T> right);
        public delegate Vector128<T> DivideVector128Delegate(Vector128<T> left, Vector128<T> right);
        public delegate Vector128<T> SqrtVector128Delegate(Vector128<T> value);
        public delegate Vector128<T> RSqrtVector128Delegate(Vector128<T> value);
        public delegate Vector128<T> AbsVector128Delegate(Vector128<T> value);
        public LoadVector128Delegate LoadDel;
        public StoreVector128Delegate StoreDel;
        public AddVector128Delegate AddDel;
        public SubtractVector128Delegate SubtractDel;
        public MultiplyVector128Delegate MultiplyDel;
        public DivideVector128Delegate DivideDel;
        public SqrtVector128Delegate SqrtDel;
        public RSqrtVector128Delegate RSqrtDel;
        public AbsVector128Delegate AbsDel;
        public LoadVector128Delegate LoadAllignedDel;
        public StoreVector128Delegate StoreAllignedDel;
        public Func<Vector128<T>, Vector128<T>, Vector128<T>> ModuloDynamicValueFunc;
        public Func<Vector128<T>, T, Vector128<T>> ModuloConstantValueFunc;

        private Vector128<T> ModuloDynamicValueImpl(Vector128<T> values, Vector128<T> modulo)
        {
            // Perform integer division
            Vector128<T> quotient = DivideDel(values, modulo);

            // Multiply the quotient by the divisor
            Vector128<T> scaledValue = MultiplyDel(values, modulo);

            // Subtract the scaled value from the original dividend
            Vector128<T> remainder = SubtractDel(values, scaledValue);

            return remainder;
        }

        private Vector128<T> ModuloConstantValueImpl(Vector128<T> values, T modulo)
        {
            Vector128<T> divisorVector = Vector128.Create(modulo);

            return ModuloDynamicValueImpl(values, divisorVector);
        }

        private static Delegate CreateDelegate(string name, int parameterIndex, Type parameter, Type del, Type baseType)
        {
            var m = baseType.GetMethods();
            var methods = m.Where(x => x.Name.Equals(name)).ToArray();
            var methodsWithSignature = methods.Where(x => x.GetParameters()[parameterIndex].ParameterType == parameter).ToArray();
            var a = methodsWithSignature.First().GetParameters();
            return methodsWithSignature.First().CreateDelegate(del);
        }

        private Vector128<T> MultiplyFallback(Vector128<T> left, Vector128<T> right)
        {
            var result = new T[ItemsInVector];
            for (int i = 0; i < ItemsInVector; i++)
                result[i] = left[i] * right[i];

            return Vector128.Create(result);
        }

        public List<(string delName, string name, int parameterIndex, Type parameter, Type del, Type source, Delegate? backup)> Functions = new()
        {
            (nameof(LoadDel), "LoadVector128", 0, typeof(T*), typeof(LoadVector128Delegate), typeof(Sse2), null),
            (nameof(LoadAllignedDel), "LoadAllignedVector128", 0, typeof(T*), typeof(LoadVector128Delegate), typeof(Sse2), null),
            (nameof(StoreDel), "Store", 0, typeof(T*), typeof(StoreVector128Delegate), typeof(Sse2), null),
            (nameof(StoreAllignedDel), "StoreAligned", 0, typeof(T*), typeof(StoreVector128Delegate), typeof(Sse2), null),
            (nameof(AddDel), "Add", 0, typeof(Vector128<T>), typeof(AddVector128Delegate), typeof(Sse2), delegate (Vector128<T> left, Vector128<T> right)),
            (nameof(SubtractDel), "Subtract", 0, typeof(Vector128<T>), typeof(SubtractVector128Delegate), typeof(Sse2), null),
            (nameof(MultiplyDel), "Multiply", 0, typeof(Vector128<T>), typeof(MultiplyVector128Delegate), typeof(Sse2), null),
            (nameof(DivideDel), "Divide", 0, typeof(Vector128<T>), typeof(DivideVector128Delegate), typeof(Sse2), null),
            (nameof(SqrtDel), "Sqrt", 0, typeof(Vector128<T>), typeof(SqrtVector128Delegate), typeof(Sse2), null),
            (nameof(RSqrtDel), "ReciprocalSqrt", 0, typeof(Vector128<T>), typeof(RSqrtVector128Delegate), typeof(Sse2), null),
            (nameof(AbsDel), "Abs", 0, typeof(Vector128<T>), typeof(AbsVector128Delegate), typeof(Sse2), null),
        };

        public Vector128ForTypeGenerator()
        {
            foreach(var item in Functions)
            {
                Delegate del;
                try
                {
                    del = CreateDelegate(item.name, item.parameterIndex, item.parameter, item.del, item.source);
                }
                catch
                {
                    del = item.backup;
                }

                GetType().GetProperty(item.delName).SetValue(this, del);
            }

            //LoadDel = CreateDelegate("LoadVector128", 0, typeof(T*), typeof(LoadVector128Delegate), typeof(Sse2));
            //LoadAllignedDel = CreateDelegate("LoadAlignedVector128", 0, typeof(T*), typeof(LoadVector128Delegate), typeof(Sse2));
            //StoreDel = CreateDelegate("Store", 0, typeof(T*), typeof(StoreVector128Delegate), typeof(Sse2));
            //StoreAllignedDel = CreateDelegate("StoreAligned", 0, typeof(T*), typeof(StoreVector128Delegate), typeof(Sse2));
            //AddDel = CreateDelegate("Add", 0, typeof(Vector128<T>), typeof(AddVector128Delegate), typeof(Sse2));
            //SubtractDel = CreateDelegate("Subtract", 0, typeof(Vector128<T>), typeof(SubtractVector128Delegate), typeof(Sse2));
            //MultiplyDel = CreateDelegate("Multiply", 0, typeof(Vector128<T>), typeof(MultiplyVector128Delegate), typeof(Avx));
            //DivideDel = CreateDelegate("Divide", 0, typeof(Vector128<T>), typeof(DivideVector128Delegate), typeof(Sse2));
            //SqrtDel = CreateDelegate("Sqrt", 0, typeof(Vector128<T>), typeof(SqrtVector128Delegate), typeof(Sse2));
            //RSqrtDel = CreateDelegate("ReciprocalSqrt", 0, typeof(Vector128<T>), typeof(RSqrtVector128Delegate), typeof(Sse2));
            //AbsDel = CreateDelegate("Abs", 0, typeof(Vector128<T>), typeof(AbsVector128Delegate), typeof(Avx));
            ModuloConstantValueFunc = ModuloConstantValueImpl;
            ModuloDynamicValueFunc = ModuloDynamicValueImpl;
        }
    }
    public unsafe class Avx128<T> : FallbackMemory<T>, IMem<T> where T : unmanaged, INumber<T>
    {
        public const int AllignmentBytes = 16;
        
        public Vector128ForTypeGenerator<T> Methods { get; } = new Vector128ForTypeGenerator<T>();
        public int ItemsInVector { get; } = Vector<T>.Count;
        public int ItemsToAllignment { get; }
        public T* Ptr { get; }
        private GCHandle _handle;

        public Avx128(int count, T[]? values = null) : base(count, values)
        {
            _handle = GCHandle.Alloc(_values, GCHandleType.Pinned);
            Ptr = (T*)_handle.AddrOfPinnedObject();

            ItemsToAllignment = (int)System.Math.Ceiling((double)(((int)Ptr + AllignmentBytes - 1) & -AllignmentBytes - (int)Ptr) / sizeof(T));
        }

        public Vector128<T> Load(T* src) => Methods.LoadDel(src);
        public Vector128<T> LoadAlligned(T* src) => Methods.LoadAllignedDel(src);
        public void Store(T* location, Vector128<T> vector) => Methods.StoreDel(location, vector);
        public void StoreAlligned(T* location, Vector128<T> vector) => Methods.StoreAllignedDel(location, vector);
        public Vector128<T> Add(Vector128<T> left, Vector128<T> right) => Methods.AddDel(left, right);
        public Vector128<T> Subtract(Vector128<T> left, Vector128<T> right) => Methods.SubtractDel(left, right);
        public Vector128<T> Multiply(Vector128<T> left, Vector128<T> right) => Methods.MultiplyDel(left, right);
        public Vector128<T> Divide(Vector128<T> left, Vector128<T> right) => Methods.DivideDel(left, right);
        public Vector128<T> Sqrt(Vector128<T> value) => Methods.SqrtDel(value);
        public Vector128<T> RSqrt(Vector128<T> value) => Methods.RSqrtDel(value);
        public Vector128<T> Abs(Vector128<T> value) => Methods.AbsDel(value);

        private nint memcpy(nint dest, nint src, nint count) => memcpy(dest, src, count);
        public override void Dispose()
        {
            base.Dispose();

            _handle.Free();
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public override void Add(IMem<T> mem) => Add(mem, Count);

        public override void Add(IMem<T> mem, int count)
        {
            var other = (Avx128<T>)mem;
            base.Add(other, ItemsToAllignment);

            int vectorCount = (int)((count - ItemsToAllignment) % ItemsInVector);

            T* ptr = Ptr;
            T* otherPtr = other.Ptr;

            int i = 0;
            for (; i < vectorCount; i++)
            {
                var vec = LoadAlligned(ptr);
                var vec2 = LoadAlligned(otherPtr);
                var result = Add(vec, vec2);
                Store(ptr, result);

                ptr += 16;
                otherPtr += 16;
            }

            while (i < count)
            {
                _values[i] += other[i];
            }
        }

        public override Avx128<T> AddToNewArray(IMem<T> other)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> AddToNewArray(IMem<T> other, int count)
        {
            throw new NotImplementedException();
        }

        public override void Subtract(IMem<T> other)
        {
            throw new NotImplementedException();
        }

        public override void Subtract(IMem<T> other, int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> SubtractToNewArray(IMem<T> other)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> SubtractToNewArray(IMem<T> other, int count)
        {
            throw new NotImplementedException();
        }

        public override void Multiply(IMem<T> other)
        {
            throw new NotImplementedException();
        }

        public override void Multiply(IMem<T> other, int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> MultiplyToNewArray(IMem<T> other)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> MultiplyToNewArray(IMem<T> other, int count)
        {
            throw new NotImplementedException();
        }

        public override void Divide(IMem<T> other)
        {
            throw new NotImplementedException();
        }

        public override void Divide(IMem<T> other, int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> DivideToNewArray(IMem<T> other)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> DivideToNewArray(IMem<T> other, int count)
        {
            throw new NotImplementedException();
        }

        public override void Sqrt()
        {
            throw new NotImplementedException();
        }

        public override void Sqrt(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> SqrtToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> SqrtToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void RSqrt()
        {
            throw new NotImplementedException();
        }

        public override void RSqrt(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> RSqrtToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> RSqrtToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void Modulo(T value)
        {
            throw new NotImplementedException();
        }

        public override void Modulo(T value, int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> ModuloToNewArray(T value)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> ModuloToNewArray(T value, int count)
        {
            throw new NotImplementedException();
        }

        public override void Modulo(IMem<T> values)
        {
            throw new NotImplementedException();
        }

        public override void Modulo(IMem<T> values, int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> ModuloToNewArray(IMem<T> values)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> ModuloToNewArray(IMem<T> values, int count)
        {
            throw new NotImplementedException();
        }

        public override void Pow(T value)
        {
            throw new NotImplementedException();
        }

        public override void Pow(T value, int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> PowToNewArray(T value)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> PowToNewArray(T value, int count)
        {
            throw new NotImplementedException();
        }

        public override void Pow(IMem<T> values)
        {
            throw new NotImplementedException();
        }

        public override void Pow(IMem<T> values, int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> PowToNewArray(IMem<T> values)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> PowToNewArray(IMem<T> values, int count)
        {
            throw new NotImplementedException();
        }

        public override void Sin()
        {
            throw new NotImplementedException();
        }

        public override void Sin(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> SinToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> SinToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void Cos()
        {
            throw new NotImplementedException();
        }

        public override void Cos(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> CosToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> CosToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void Tan()
        {
            throw new NotImplementedException();
        }

        public override void Tan(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> TanToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> TanToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void Log()
        {
            throw new NotImplementedException();
        }

        public override void Log(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> LogToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> LogToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void Log2()
        {
            throw new NotImplementedException();
        }

        public override void Log2(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> Log2ToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> Log2ToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void Log10()
        {
            throw new NotImplementedException();
        }

        public override void Log10(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> Log10ToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> Log10ToNewArray(int count)
        {
            throw new NotImplementedException();
        }

        public override void Abs()
        {
            throw new NotImplementedException();
        }

        public override void Abs(int count)
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> AbsToNewArray()
        {
            throw new NotImplementedException();
        }

        public override Avx128<T> AbsToNewArray(int count)
        {
            throw new NotImplementedException();
        }
    }
}
