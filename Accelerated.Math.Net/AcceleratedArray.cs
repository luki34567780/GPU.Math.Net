namespace Accelerated.Math.Net
{
    public class AcceleratedArray<T> where T : unmanaged
    {
        private IMem<T> _mem;

        public T this[int index]
        {
            get => _mem[index];
            set => _mem[index] = value;
        }


    }
}