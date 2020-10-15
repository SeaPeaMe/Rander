/////////////////////////////////////
///          Base Script          ///
///         Use: Game Time        ///
///         Attatch: Base         ///
/////////////////////////////////////

using System;
using System.Collections.Generic;

namespace Rander
{
    public class Time : Component
    {
        internal static List<WaitTimer> Timers = new List<WaitTimer>();
        public static float FrameTime = 0;
        public static float TimeSinceStart = 0;

        public override void Update()
        {
            TimeSinceStart = (float)Game.Gametime.TotalGameTime.TotalSeconds;
            FrameTime = (float)Game.Gametime.ElapsedGameTime.TotalSeconds;

            foreach (WaitTimer tim in Timers.ToArray())
            {
                tim.RunTimer();
            }
        }

        public static WaitTimer Wait(int waitTime, Action call)
        {
            if (waitTime <= 0)
            {
                Debug.LogError("Wait time can not be <= 0!", true);
                return null;
            }

            WaitTimer tim = new WaitTimer(waitTime, call);
            Timers.Add(tim);
            return tim;
        }

        public static WaitTimer WaitUntil(Func<bool> condition, Action call)
        {
            WaitTimer tim = new WaitTimer(condition, call);
            Timers.Add(tim);
            return tim;
        }

        public static void CancelWait(WaitTimer wait)
        {
            if (wait != null) wait.Dispose();
        }

        public class WaitTimer
        {
            public Action Call;
            public int WaitTime;
            public bool Repeat;
            Func<bool> Condition;
            int TimeOnCreation;

            public WaitTimer(int waitTime, Action call, bool repeat = false)
            {
                Call = call;
                WaitTime = waitTime;
                Condition = () => 1 == 1;
                Repeat = repeat;
                TimeOnCreation = (int)(TimeSinceStart * 1000);
            }

            public WaitTimer(Func<bool> condition, Action call)
            {
                Call = call;
                WaitTime = 10;
                Condition = condition;
                Repeat = true;
                TimeOnCreation = (int)(TimeSinceStart * 1000);
            }

            public void RunTimer()
            {
                if ((int)(TimeSinceStart * 1000) >= TimeOnCreation + WaitTime)
                {
                    if (Condition())
                    {
                        Call();
                        Dispose();
                    }
                    if (!Repeat) Dispose();
                }
            }

            public void Dispose()
            {
                Timers.Remove(this);
            }
        }
    }
}
