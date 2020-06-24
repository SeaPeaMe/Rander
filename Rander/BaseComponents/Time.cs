/////////////////////////////////////
///          Base Script          ///
///         Use: Game Time        ///
///         Attatch: Base         ///
/////////////////////////////////////

using System;
using System.Threading;
namespace Rander
{
    public class Time : Component
    {

        public static float FrameTime = 0;
        public static float TimeSinceStart = 0;

        public override void Update()
        {
            TimeSinceStart = (float)Game.Gametime.TotalGameTime.TotalSeconds;
        }

        public override void Draw()
        {
            FrameTime = (float)Game.Gametime.ElapsedGameTime.TotalSeconds;
        }

        public static void Wait(int waitTime, Action call)
        {
            Thread thr = new Thread(new ThreadStart(() => {
                while (!Game.KillThreads)
                {
                    Thread.Sleep(waitTime);
                    call();
                }
            }));

            thr.Start();
        }
    }
}
