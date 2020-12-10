/////////////////////////////////////
///          Base Script          ///
///          Use: Random          ///
///         Attatch: Base         ///
/////////////////////////////////////

using System;

namespace Rander
{
    public class Rand : Component
    {

        static Random Random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year);

        public static int RandomInt(int Min, int Max)
        {
            return Random.Next(Min, Max + 1);
        }

        public static float RandomFloat(float Min, float Max)
        {
            return (float)(Random.NextDouble() * (Max - Min) + Min);
        }

        public static double RandomDouble(double Min, double Max)
        {
            return Random.NextDouble() * (Max - Min) + Min;
        }
    }
}
