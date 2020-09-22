using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rander
{
    public class ContentLoader
    {
        public static string ContentPath = DefaultValues.ContentPath;
        public static Dictionary<string, SoundEffect> LoadedSounds = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, SpriteFont> LoadedFonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, Texture2D> Loaded2DTextures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Model> Loaded3DModels = new Dictionary<string, Model>();

        public static Texture2D LoadTexture(string image)
        {
            Texture2D Tex = null;
            // If texture is already in memory, reference that instead of having to load the texture again
            if (Loaded2DTextures.ContainsKey(image))
            {
                Loaded2DTextures.TryGetValue(image, out Tex);
            }
            else
            {
                if (File.Exists(Game.gameWindow.Content.RootDirectory + "/" + image))
                {
                    // I hate the XNA content system, so I'll use streams whenever possible
                    FileStream ImageStream = File.OpenRead(Game.gameWindow.Content.RootDirectory + "/" + image);
                    Tex = Texture2D.FromStream(Game.graphics.GraphicsDevice, ImageStream);
                    ImageStream.Dispose();

                    Loaded2DTextures.Add(image, Tex);
                }
                else
                {
                    Debug.LogError("The Image \"" + image + "\" does not exist!", true, 2);
                }
            }

            return Tex;
        }

        public static SpriteFont LoadFont(string font)
        {
            SpriteFont outFont = null;
            // If font is already in memory, reference that instead of having to load the texture again
            if (LoadedFonts.ContainsKey(font))
            {
                LoadedFonts.TryGetValue(font, out outFont);
            }
            else
            {
                if (File.Exists(Game.gameWindow.Content.RootDirectory + "/" + font))
                {
                    outFont = TtfFontBaker.Bake(File.ReadAllBytes(ContentPath + "/" + font), 100, 1024, 1024, new[] { CharacterRange.BasicLatin, CharacterRange.Latin1Supplement, CharacterRange.LatinExtendedA, CharacterRange.Cyrillic }).CreateSpriteFont(Game.graphics.GraphicsDevice);
                    LoadedFonts.Add(font, outFont);
                }
                else
                {
                    Debug.LogError("The Font \"" + font + "\" does not exist!", true, 2);
                }
            }

            return outFont;
        }

        public static SoundEffect LoadSound(string sound)
        {
            SoundEffect outSound = null;
            // If sound is already in memory, reference that instead of having to load it again
            if (LoadedSounds.ContainsKey(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + sound)))
            {
                LoadedSounds.TryGetValue(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + sound), out outSound);
            }
            else
            {
                // Sound must be a WAV file or else it has a spack
                if (File.Exists(Game.gameWindow.Content.RootDirectory + "/" + sound + ".wav"))
                {
                    Stream SndStream = File.OpenRead(Game.gameWindow.Content.RootDirectory + "/" + sound + ".wav");
                    outSound = SoundEffect.FromStream(SndStream);
                    SndStream.Dispose();

                    LoadedSounds.Add(Path.GetFileNameWithoutExtension(Game.gameWindow.Content.RootDirectory + "/" + sound), outSound);
                }
                else
                {
                    Debug.LogError("The Sound \"" + Game.gameWindow.Content.RootDirectory + "/" + sound + ".wav\" does not exist!", true, 2);
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
