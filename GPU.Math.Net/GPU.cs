using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCL.Net;
using OpenCL.Net.Extensions;

namespace GPU.Math.Net
{
    public unsafe class GPU : IDisposable
    {
        public CommandQueue Queue { get; }
        public Context Context { get; }

        public Device Device { get; }

        private bool disposedValue;

        public static Platform[] GetPlatforms()
        {
            var res = Cl.GetPlatformIDs(out var error);
            ThrowOnError(error);
            return res;
        }

        public static Device[] GetDevices(Platform platform)
        {
            return GetDevices(platform, DeviceType.All);
        }

        public static Device[] GetDevices(Platform platform, DeviceType type)
        {
            var result = Cl.GetDeviceIDs(platform, type, out var error);
            ThrowOnError(error);
            return result;
            
        }

        public static Device GetSomeDevice()
        {
            return GetDevices(GetPlatforms().First()).First();
        }

        private void ApplicationExitDisposer(object? _, EventArgs _2)
        {
            Dispose();
        }

        public GPU(Device device, CommandQueueProperties commandQueueProperties)
        {
            Device = device;
            Context = Cl.CreateContext(null, 1, new[] { device }, null, IntPtr.Zero, out var error);
            ThrowOnError(error);
            Queue = Cl.CreateCommandQueue(Context, device, commandQueueProperties, out error);
            ThrowOnError(error);

            AppDomain.CurrentDomain.ProcessExit += ApplicationExitDisposer;
        }

        private static void ThrowOnError(ErrorCode err) => ClHelpers.ThrowOnError(err);

        public GPU(Device device) : this(device, CommandQueueProperties.None) { }

        public IMem<T> CreateBuffer<T>(MemFlags flags, T[] data) where T : struct
        {
            var res = Cl.CreateBuffer<T>(Context, flags, data, out var error);
            ThrowOnError(error);
            return res;
        }

        public IMem<T> CreateBuffer<T>(MemFlags flags, int size) where T : struct
        {
            var res = Cl.CreateBuffer<T>(Context, flags, size, out var error);
            ThrowOnError(error);
            return res;
        }

        public IMem CreateBuffer(MemFlags flags, int size)
        {
            var res = Cl.CreateBuffer(Context, flags, size, out var error);
            ThrowOnError(error);
            return res;
        }

        public void CopyToHost<T>(IMem<T> source, T[] destination, int count, int offset) where T : struct
        {
            var error = Cl.EnqueueReadBuffer(Queue, source, Bool.False, offset, count, destination, 0, null, out var _);
            ThrowOnError(error);
        }

        public void CopyToHost(IMem source, byte* destination, int count, int startOffset)
        {
            var error = Cl.EnqueueReadBuffer(Queue, source, Bool.False, (nint)startOffset, (nint)count, (nint)destination, 0, null, out var _);
            ThrowOnError(error);
        }

        public void CopyToDevice<T>(T[] source, IMem<T> destination, int count, int offset) where T : struct
        {
            var error = Cl.EnqueueWriteBuffer(Queue, destination, Bool.False, offset, count, source, 0, null, out var _);
            ThrowOnError(error);
        }

        public void CopyToDevice(byte* source, IMem destination, int count, int offset)
        {
            var error = Cl.EnqueueWriteBuffer(Queue, destination, Bool.False, (nint)offset, (nint)count, (nint)source, 0, null, out var _);
            ThrowOnError(error);
        }

        public void CopyOnDevice<T>(IMem<T> source, IMem<T> destination, int sourceOffset, int destinationOffset, int count) where T : struct
        {
            var error = Cl.EnqueueCopyBuffer(Queue, source, destination, (nint)sourceOffset, (nint)destinationOffset, (nint)count, 0, null, out var res);
            ThrowOnError(error);
        }

        public void ExecuteKernel(Kernel kernel, int globalWorkSize)
        {
            var status = Cl.EnqueueNDRangeKernel(Queue, kernel, 1, null, new IntPtr[] { (nint)globalWorkSize }, null, 0, null, out var _);
            ThrowOnError(status);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                AppDomain.CurrentDomain.ProcessExit -= ApplicationExitDisposer;
                if (disposing)
                {
                    // dispose managed state (managed objects)
                }

                Queue.Finish();
                Queue.Dispose();
                Context.Dispose();
                disposedValue = true;
            }
        }

        ~GPU()
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
