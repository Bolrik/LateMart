namespace System
{
    public static partial class Extension
    {
        public static float Abs(this float value) => Math.Abs(value);

        public static float Sign(this float value) => Math.Sign(value);

        public static int SignInt(this float value) => Math.Sign(value);

        public static float Clamp(this float value, float min, float max)
        {
            if (value < min)
                value = min;

            if (value > max)
                value = max;

            return value;
        }

        public static float ClampMin(this float value, float min)
        {
            return value.Max(value, min);
        }

        public static float ClampMax(this float value, float max)
        {
            return value.Min(value, max);
        }

        public static float Loop(this float value, float length)
        {
            return Clamp(value - (float)Math.Floor(value / length) * length, 0.0f, length);
        }

        public static float PingPong(this float value, float length)
        {
            value = value.Loop(length * 2F);
            float toReturn = length - Math.Abs(value - length);

            return toReturn;
        }

        public static float Lerp(this float from, float to, float time)
        {
            return from + (to - from) * time.Clamp(0, 1);
        }

        // Unclamped Lerp
        public static float LerpU(this float from, float to, float time)
        {
            return from + (to - from) * time;
        }

        public static float SmoothStep(this float value)
        {
            return value * value * (3 - 2 * value);
        }

        public static float Pow(this float value, float power) => (float)Math.Pow(value, power);

        public static float Map(this float value, float from1, float to1, float from2, float to2)
        {
            float vFrom = value - from1,
                toFrom1 = to1 - from1,
                toFrom2 = to2 - from2;

            return vFrom / toFrom1 * toFrom2 + from2;
        }

        public static float Min(this float value, params float[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Min(value, other[i]);
            }

            return value;
        }

        public static float Max(this float value, params float[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                value = Math.Max(value, other[i]);
            }

            return value;
        }
    }
}
