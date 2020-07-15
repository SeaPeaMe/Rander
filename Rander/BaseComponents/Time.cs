/////////////////////////////////////
///          Base Script          ///
///         Use: Game Time        ///
///         Attatch: Base         ///
/////////////////////////////////////

using System;
using System.Linq.Expressions;
using System.Timers;

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
            Timer tim = new Timer(waitTime);
            tim.Elapsed += (source, exceptions) => {
                lock (Game.Timers) // Locks timer list so the new thread can edit it without breaking
                {
                    Game.Timers.Remove(tim);
                    call();
                    tim.Dispose();
                }
            };
            tim.AutoReset = false;

            Game.Timers.Add(tim);
            tim.Start();
        }

        public static void WaitUntil(Func<bool> condition, Action call)
        {
            Timer tim = new Timer(10);
            tim.Elapsed += (source, exceptions) => {
                if (condition()) {
                    lock (Game.Timers)
                    {
                        Game.Timers.Remove(tim);
                        tim.Stop();
                        call();
                        tim.Dispose();
                    }
                }
            };
            tim.AutoReset = true;

            Game.Timers.Add(tim);
            tim.Start();
        }
    }
}
