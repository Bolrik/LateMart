namespace System
{
    public static partial class Extension
    {
        public static int Pow(this int value, int power)
        {
            // Should be sufficent
            return (int)Math.Pow(value, power);
        }



        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
                value = min;

            if (value > max)
                value = max;

            return value;
        }

        public static int ClampMin(this int value, int min)
        {
            return value.Max(value, min);
        }

        public static int ClampMax(this int value, int max)
        {
            return value.Min(value, max);
        }

        public static int Min(this int value, params int[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Min(value, other[i]);
            }

            return value;
        }

        public static int Max(this int value, params int[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Max(value, other[i]);
            }

            return value;
        }

        public static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        public static int Sign(this int value)
        {
            return Math.Sign(value);
        }
    }
}
