using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;

namespace Rander.BaseComponents
{
    public class Audio : Component
    {
        static List<SoundEffect> NoOverlap = new List<SoundEffect>();

        public static SoundEffectInstance PlaySound(SoundEffect sound, float pitch = 0, float volume = 1, bool loop = false, bool allowOverlap = false)
        {
            // Makes sure the sounds can't overlap
            if (!NoOverlap.Contains(sound))
            {
                try
                {
                    SoundEffectInstance Snd = sound.CreateInstance();
                    Snd.IsLooped = loop;
                    Snd.Pitch = pitch;
                    Snd.Volume = volume;
                    Snd.Play();

                    // If the sound is to not loop, dispose the sound instance once it's done
                    if (!loop)
                    {
                        Time.Wait((int)sound.Duration.TotalMilliseconds, () => { Level.Sounds.Remove(Snd); Snd.Stop(); Snd.Dispose(); });
                    }

                    if (!allowOverlap)
                    {
                        NoOverlap.Add(sound);
                        Time.Wait((int)sound.Duration.TotalMilliseconds, () => { NoOverlap.Remove(sound); });
                    }

                    Level.Sounds.Add(Snd, sound);

                    return Snd;
                } catch
                {
                    if (volume > 1)
                    {
                        Debug.LogError("Volume of a sound can not be above 1!", true);
                    }
                    else if (volume < 0)
                    {
                        Debug.LogError("Volume of a sound can not be below 0!", true);
                    }

                    if (pitch > 1)
                    {
                        Debug.LogError("Pitch of a sound can not be above 1!", true);
                    }
                    else if (pitch < -1)
                    {
                        Debug.LogError("Pitch of a sound can not be below -1!", true);
                    }

                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void StopSound(SoundEffectInstance sound)
        {
            if (Level.Sounds.ContainsKey(sound))
            {
                // If the no overlap list has the sound that is being stopped, remove it so the
                // user doesn't have to wait to play the sound again
                SoundEffect snd;
                if (Level.Sounds.TryGetValue(sound, out snd))
                {
                    if (NoOverlap.Contains(snd))
                    {
                        NoOverlap.Remove(snd);
                    }
                }

                Level.Sounds.Remove(sound);
                sound.Stop();
                sound.Dispose();
            }
        }

        public static void StopAllAudio()
        {
            foreach (SoundEffectInstance Sound in Level.Sounds.Keys.ToArray())
            {
                Sound.Stop();
                Sound.Dispose();
            }
            Level.Sounds.Clear();
        }
    }
}
