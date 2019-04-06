using System.Buffers;
using Newtonsoft.Json;

namespace OptWebApi
{
    internal class NewtonPool: IArrayPool<char>
    {
        private readonly ArrayPool<char> _arrayPool;

        public NewtonPool(ArrayPool<char> arrayPool)
        {
            _arrayPool = arrayPool;
        }
            
        public char[] Rent(int minimumLength)
        {
            return _arrayPool.Rent(minimumLength);
        }

        public void Return(char[] array)
        {
            _arrayPool.Return(array);
        }
    }
}