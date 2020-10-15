using Microsoft.Xna.Framework;
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
        internal static Dictionary<string, SoundEffect> LoadedSounds = new Dictionary<string, SoundEffect>();
        internal static Dictionary<string, SpriteFont> LoadedFonts = new Dictionary<string, SpriteFont>();
        internal static Dictionary<string, Texture2D> Loaded2DTextures = new Dictionary<string, Texture2D>();
        internal static Dictionary<string, Model> Loaded3DModels = new Dictionary<string, Model>();

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
                if (File.Exists(ContentPath + "/" + image))
                {
                    // I hate the XNA content system, so I'll use streams whenever possible
                    FileStream ImageStream = File.OpenRead(ContentPath + "/" + image);
                    Tex = Texture2D.FromStream(Game.graphics.GraphicsDevice, ImageStream);
                    ImageStream.Dispose();

                    Loaded2DTextures.Add(image, Tex);
                }
                else
                {
                    Debug.LogError("The Image \"" + image + "\" does not exist in content folder!", true, 2);
                }
            }

            return Tex;
        }

        public static void DisposeTexture(Texture2D texture)
        {
            string Key = Loaded2DTextures.First((x) => x.Value == texture).Key;
            Loaded2DTextures.First((x) => x.Key == Key).Value.Dispose();
            Loaded2DTextures.Remove(Key);
        }

        public static void DisposeTexture(string texture)
        {
            Loaded2DTextures.First((x) => x.Key == texture).Value.Dispose();
            Loaded2DTextures.Remove(texture);
        }

        public static void DisposeSpriteSheet(SpriteSheet sheet)
        {
            sheet.Dispose();
        }

        public static SpriteSheet LoadSpriteSheet(string image, Vector2 SectionSize)
        {
            SpriteSheet sht = new SpriteSheet();
            sht.ImageName = image;

            Texture2D Sheet = LoadTexture(image);

            if (Sheet.Height % SectionSize.Y != 0 || Sheet.Width % SectionSize.X != 0)
            {
                Debug.LogError("The sheet specified \"" + image + "\" (" + Sheet.Width + ", " + Sheet.Height + ") can not be divided evenly with given parameters (" + SectionSize.X + "," + SectionSize.Y + ") !", true);
                return null;
            }

            for (int y = 0; y < Sheet.Height; y += (int)SectionSize.Y)
            {
                List<Texture2D> Row = new List<Texture2D>();
                for (int x = 0; x < Sheet.Width; x += (int)SectionSize.X)
                {
                    Color[] col = new Color[(int)SectionSize.X * (int)SectionSize.Y];
                    Sheet.GetData(0, new Rectangle(x, y, (int)SectionSize.X, (int)SectionSize.Y), col, 0, (int)SectionSize.X * (int)SectionSize.Y);

                    Texture2D tex = new Texture2D(Game.graphics.GraphicsDevice, (int)SectionSize.X, (int)SectionSize.Y);
                    tex.SetData(col);
                    Row.Add(tex);
                    Loaded2DTextures.Add(image + "_" + (int)(y / SectionSize.Y) + ":" + (int)(x / SectionSize.X), tex);
                }
                sht.Sheet.Add(Row);
            }

            sht.SheetSize = new Vector2(Sheet.Width / SectionSize.X, Sheet.Height / SectionSize.Y);

            DisposeTexture(Sheet);

            return sht;
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
                if (File.Exists(ContentPath + "/" + font))
                {
                    outFont = TtfFontBaker.Bake(File.ReadAllBytes(ContentPath + "/" + font), 100, 1024, 1024, new[] { CharacterRange.BasicLatin, CharacterRange.Latin1Supplement, CharacterRange.LatinExtendedA, CharacterRange.Cyrillic }).CreateSpriteFont(Game.graphics.GraphicsDevice);
                    LoadedFonts.Add(font, outFont);
                }
                else
                {
                    Debug.LogError("The Font \"" + font + "\" does not exist in conteht folder!", true, 2);
                }
            }

            return outFont;
        }

        public static SoundEffect LoadSound(string sound)
        {
            SoundEffect outSound = null;
            // If sound is already in memory, reference that instead of having to load it again
            if (LoadedSounds.ContainsKey(Path.GetFileNameWithoutExtension(ContentPath + "/" + sound)))
            {
                LoadedSounds.TryGetValue(Path.GetFileNameWithoutExtension(ContentPath + "/" + sound), out outSound);
            }
            else
            {
                // Sound must be a WAV file or else it has a spack
                if (File.Exists(ContentPath + "/" + sound) && Path.GetExtension(ContentPath + "/" + sound) == ".wav")
                {
                    Stream SndStream = File.OpenRead(ContentPath + "/" + sound);
                    outSound = SoundEffect.FromStream(SndStream);
                    SndStream.Dispose();

                    LoadedSounds.Add(Path.GetFileNameWithoutExtension(ContentPath + "/" + sound), outSound);
                }
                else
                {
                    if (!File.Exists(ContentPath + "/" + sound))
                    {
                        Debug.LogError("The Sound \"" + ContentPath + "/" + sound + "\" does not exist in content folder!", true, 2);
                    }
                    else if (Path.GetExtension(ContentPath + "/" + sound) != ".wav")
                    {
                        Debug.LogError("The Sound \"" + ContentPath + "/" + sound + "\" is not a .wav file!", true, 2);
                    }
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
