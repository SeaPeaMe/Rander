/////////////////////////////////////
///          Base Script          ///
///         Use: Game Time        ///
///         Attatch: Base         ///
/////////////////////////////////////

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
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

        public static System.Timers.Timer Wait(int waitTime, Action call)
        {
            System.Timers.Timer tim = new System.Timers.Timer(waitTime);
            tim.Elapsed += (source, exceptions) => {
                lock (Game.Timers) // Locks timer list so the new thread can edit it without breaking
                {
                    lock (Game.ThreadSync)
                    {
                        Game.Timers.Remove(tim);
                        Game.ThreadSync.Add(call);
                        tim.Dispose();
                    }
                }
            };
            tim.AutoReset = false;

            Game.Timers.Add(tim);
            tim.Start();

            return tim;
        }

        public static System.Timers.Timer WaitUntil(Func<bool> condition, Action call)
        {
            System.Timers.Timer tim = new System.Timers.Timer(10);
            tim.Elapsed += (source, exceptions) => {
                if (condition()) {
                    lock (Game.Timers)
                    {
                        lock (Game.ThreadSync) {
                            Game.Timers.Remove(tim);
                            tim.Stop();
                            Game.ThreadSync.Add(call);
                            tim.Dispose();
                        }
                    }
                }
            };
            tim.AutoReset = true;

            Game.Timers.Add(tim);
            tim.Start();

            return tim;
        }

        public static void CancelWait(System.Timers.Timer wait)
        {
            if (wait != null) {
                Game.Timers.Remove(wait);
                wait.Stop();
                wait.Dispose();
            }
        }
    }
}
