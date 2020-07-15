using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rander
{
    public class ContentLoader
    {
        static Dictionary<string, SoundEffect> LoadedSounds = new Dictionary<string, SoundEffect>();
        static Dictionary<string, SpriteFont> LoadedFonts = new Dictionary<string, SpriteFont>();
        static Dictionary<string, Texture2D> Loaded2DTextures = new Dictionary<string, Texture2D>();

        public static Texture2D LoadTexture(string Image)
        {
            Texture2D Tex = null;
            // If texture is already in memory, reference that instead of having to load the texture again
            if (Loaded2DTextures.ContainsKey(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Image))) {
                Loaded2DTextures.TryGetValue(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Image), out Tex);
            } else
            {
                if (File.Exists(Game.gameWindow.Content.RootDirectory + "/" + Image)) {
                    // I hate the XNA content system, so I'll use streams whenever possible
                    FileStream ImageStream = File.OpenRead(Game.gameWindow.Content.RootDirectory + "/" + Image);
                    Tex = Texture2D.FromStream(Game.graphics.GraphicsDevice, ImageStream);
                    ImageStream.Dispose();

                    Loaded2DTextures.Add(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Image), Tex);
                } else
                {
                    Debug.LogError("The Image \"" + Game.gameWindow.Content.RootDirectory + "/" + Image + "\" does not exist!", true, 2);
                }
            }

            return Tex;
        }

        public static SpriteFont LoadFont(string Font)
        {
            SpriteFont outFont = null;
            // If font is already in memory, reference that instead of having to load the texture again
            if (LoadedFonts.ContainsKey(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Font)))
            {
                LoadedFonts.TryGetValue(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Font), out outFont);
            }
            else
            {
                if (File.Exists(Game.gameWindow.Content.RootDirectory + "/" + Font + ".xnb"))
                {
                    outFont = Game.gameWindow.Content.Load<SpriteFont>(Font);
                    LoadedFonts.Add(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Font), outFont);
                }
                else
                {
                    Debug.LogError("The Font \"" + Game.gameWindow.Content.RootDirectory + "/" + Font + ".xnb\" does not exist!", true, 2);
                }
            }

            return outFont;
        }

        public static SoundEffect LoadSound(string Sound)
        {
            SoundEffect outSound = null;
            // If sound is already in memory, reference that instead of having to load it again
            if (LoadedSounds.ContainsKey(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Sound)))
            {
                LoadedSounds.TryGetValue(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Sound), out outSound);
            }
            else
            {
                // Sound must be a WAV file or else it has a spack
                if (File.Exists(Game.gameWindow.Content.RootDirectory + "/" + Sound + ".wav"))
                {
                    Stream SndStream = File.OpenRead(Game.gameWindow.Content.RootDirectory + "/" + Sound + ".wav");
                    outSound = SoundEffect.FromStream(SndStream);
                    SndStream.Dispose();

                    LoadedSounds.Add(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + Sound), outSound);
                }
                else
                {
                    Debug.LogError("The Sound \"" + Game.gameWindow.Content.RootDirectory + "/" + Sound + ".wav\" does not exist!", true, 2);
                }
            }

            return outSound;
        }

        public static void Dispose()
        {
            Debug.Log("     Sounds...");
            string[] SoundNames = LoadedSounds.Keys.ToArray();
            for (int i = 0; i < LoadedSounds.Count; i++)
            {
                SoundEffect Snd = null;
                if (LoadedSounds.TryGetValue(SoundNames[i], out Snd))
                {
                    Snd.Dispose();
                    LoadedSounds.Remove(SoundNames[i]);
                }
            }

            Debug.Log("     Textures...");
            string[] TextureNames = Loaded2DTextures.Keys.ToArray();
            for (int i = 0; i < Loaded2DTextures.Count; i++)
            {
                Texture2D Tex = null;
                if (Loaded2DTextures.TryGetValue(TextureNames[i], out Tex))
                {
                    Tex.Dispose();
                    Loaded2DTextures.Remove(TextureNames[i]);
                }
            }

            Debug.Log("     Fonts...");
            LoadedFonts.Clear();
        }
    }
}
