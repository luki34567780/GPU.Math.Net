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

        public static Platform[] GetPlatforms() => Cl.GetPlatformIDs(out var error) ?? throw new Cl.Exception(error);

        public static Device[] GetDevices(Platform platform) => GetDevices(platform, DeviceType.All);

        public static Device[] GetDevices(Platform platform, DeviceType type) => Cl.GetDeviceIDs(platform, type, out var error) ?? throw new Cl.Exception(error);

        public static Device GetSomeDevice() => GetDevices(GetPlatforms().First()).First();

        public GPU(Device device, CommandQueueProperties commandQueueProperties)
        {
            Device = device;
            Context = Cl.CreateContext(null, 1, new[] { device }, null, IntPtr.Zero, out var _);
            Queue = Cl.CreateCommandQueue(Context, device, commandQueueProperties, out var _);
        }

        public GPU(Device device) : this(device, CommandQueueProperties.None) { }

        public IMem<T> CreateBuffer<T>(MemFlags flags, T[] data) where T : struct => Cl.CreateBuffer<T>(Context, flags, data, out var error);
        public IMem<T> CreateBuffer<T>(MemFlags flags, int size) where T : struct => Cl.CreateBuffer<T>(Context, flags, size, out var error);
        public IMem CreateBuffer(MemFlags flags, int size) => Cl.CreateBuffer(Context, flags, size, out var error);

        public void CopyToHost<T>(IMem<T> source, T[] destination, int count, int offset) where T : struct => Cl.EnqueueReadBuffer(Queue, source, Bool.False, offset, count, destination, 0, null, out var _);

        public void CopyToHost(IMem source, byte* destination, int count, int startOffset) => Cl.EnqueueReadBuffer(Queue, source, Bool.False, (nint)startOffset, (nint)count, (nint)destination, 0, null, out var _);

        public void CopyToDevice<T>(T[] source, IMem<T> destination, int count, int offset) where T : struct => Cl.EnqueueWriteBuffer(Queue, destination, Bool.False, offset, count, source, 0, null, out var _);

        public void CopyToDevice(byte* source, IMem destination, int count, int offset) => Cl.EnqueueWriteBuffer(Queue, destination, Bool.False, (nint)offset, (nint)count, (nint)source, 0, null, out var _);

        public void CopyOnDevice<T>(IMem<T> source, IMem<T> destination, int sourceOffset, int destinationOffset, int count) where T : struct => Cl.EnqueueCopyBuffer(Queue, source, destination, (nint)sourceOffset, (nint)destinationOffset, (nint)count, 0, null, out var _);

        public void ExecuteKernel(Kernel kernel, int globalWorkSize)
        {
            var status = Cl.EnqueueNDRangeKernel(Queue, kernel, 1, null, new IntPtr[] { (nint)globalWorkSize }, null, 0, null, out var _);

            if (status != ErrorCode.Success)
            {
                throw new Cl.Exception(status);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
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
