

using System.Runtime.CompilerServices;

namespace UtilityBot.Services
{
    public class NumbersHandler
    {
        private int[] _numbers;

        public NumbersHandler(int[] numbers)
        {
            _numbers = numbers;
        }

        public int SumNumbers()
        {
            return _numbers.Sum();
        }


    }
}
