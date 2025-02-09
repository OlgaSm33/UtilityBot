
namespace UtilityBot.Extensions
{
    public static class StringExtension
    {
        public static int[] StringToInts(this string str)
        {
            var strNumbers = str.Split(' ');
            int[] ints = new int[strNumbers.Length];
            for (int i = 0; i < strNumbers.Length; i++)
            {
                ints[i] = Convert.ToInt32(strNumbers[i]);
            }
            return ints;
        }
    }
}
