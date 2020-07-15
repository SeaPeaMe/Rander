using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Rander.BaseComponents
{
    public class Audio : Component
    {
        static List<SoundEffect> NoOverlap = new List<SoundEffect>();

        public static SoundEffectInstance PlaySound(SoundEffect sound, bool loop = false, bool allowOverlap = false)
        {
            // Makes sure the sounds can't overlap
            if (!NoOverlap.Contains(sound)) {
                SoundEffectInstance Snd = sound.CreateInstance();
                Snd.IsLooped = loop;
                Snd.Play();

                // If the sound is to not loop, dispose the sound instance once it's done
                if (!loop)
                {
                    Time.Wait((int)sound.Duration.TotalMilliseconds, () => { Game.Sounds.Remove(Snd); Snd.Stop(); Snd.Dispose(); });
                }

                if (!allowOverlap)
                {
                    NoOverlap.Add(sound);
                    Time.Wait((int)sound.Duration.TotalMilliseconds, () => { NoOverlap.Remove(sound); });
                }

                Game.Sounds.Add(Snd);

                return Snd;
            } else
            {
                return null;
            }
        }

        public void StopSound(SoundEffectInstance sound)
        {
            if (Game.Sounds.Contains(sound))
            {
                Game.Sounds.Remove(sound);
                sound.Stop();
                sound.Dispose();

                // TODO: Somehow remove sound from NoOverlap
            }
        }
    }
}
