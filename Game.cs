using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rander
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static Draw2D Drawing;
        public static Microsoft.Xna.Framework.Game gameWindow;
        public static GameTime Gametime = new GameTime();

        public static List<System.Timers.Timer> Timers = new List<System.Timers.Timer>();
        public static List<Action> ThreadSync = new List<Action>();
        public static bool PauseGame = false;

        static List<Component> BaseScripts = new List<Component>();

        public Game()
        {
            Debug.LogWarning("Initializing...");

            // Sets window and graphics
            gameWindow = this;
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };

            // Deletes Temp
            if (Directory.Exists(DefaultValues.ExecutableTempFolderPath + "/Content"))
            {
                Directory.Delete(DefaultValues.ExecutableTempFolderPath + "/Content", true);
            }

            Content.RootDirectory = DefaultValues.ExecutableTempFolderPath + "/Content/";
            DefaultValues.ContentPath = DefaultValues.ExecutableTempFolderPath + "/Content/";

        // Decompresses and/or creates Content file
        DecompressContent:
            if (Directory.Exists(DefaultValues.ExecutableFolderPath + "/Content"))
            {
                Debug.LogWarning("    Rebuilding Content.dat...");
                FolderCompressor.Compress(DefaultValues.ExecutableFolderPath + "/Content", DefaultValues.ExecutableFolderPath + "/Content.dat", System.IO.Compression.CompressionLevel.Fastest, true);
                Debug.LogWarning("    Overwriting Settings.dat...");
                if (File.Exists(DefaultValues.ExecutableFolderPath + "/Settings.dat")) File.Delete(DefaultValues.ExecutableFolderPath + "/Settings.dat");
                goto DecompressContent;
            }
            else
            {
                if (File.Exists(DefaultValues.ExecutableFolderPath + "/Content.dat"))
                {
                    Debug.Log("    Decompressing Content...");
                    FolderCompressor.Decompress(DefaultValues.ExecutableFolderPath + "/Content.dat", DefaultValues.ExecutableTempFolderPath + "/Content", false);
                }
                else
                {
                    Debug.LogError("There's no content file/folder!", true);
                }
            }

            if (Directory.Exists(DefaultValues.ExecutableFolderPath + "/Content")) Directory.Delete(DefaultValues.ExecutableFolderPath + "/Content", true);

            GameSettings.LoadSettings();
        }

        protected override void Initialize()
        {
            // Sets up the window to device's resolution and in full screen
            IsFixedTimeStep = false;
            Screen.ApplyChanges();

            // Load Base Scripts
            BaseScripts.Add(new MouseInput());
            BaseScripts.Add(new Rand());
            BaseScripts.Add(new Time());
            BaseScripts.Add(new Input());
            BaseScripts.Add(new DebugMenu());

            foreach (Component Com in BaseScripts.ToList())
            {
                Com.Start();
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            Drawing = new Draw2D(GraphicsDevice);

            Debug.LogSuccess("Initialization Complete!");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Debug.LogWarning("Loading Content...");

            // Sets default graphic stuff
            DefaultValues.DefaultFont = ContentLoader.LoadFont("Defaults/UASQUARE.TTF");
            DefaultValues.PixelTexture = ContentLoader.LoadTexture("Defaults/Pixel.png");

            Debug.LogWarning("Initializing Game...");
            if (MyGame.Main.OnGameLoad())
            {
                Debug.LogSuccess("Finished!");
            }
        }

        protected override void UnloadContent()
        {
            // Disposes everything if X button pressed in top right
            Close(true);
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive && !PauseGame)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Close(true);

                // Update important scripts first
                foreach (Component Scr in BaseScripts.ToList())
                {
                    Scr.Update();
                }

                // Sync all threads
                foreach (Action call in ThreadSync.ToArray())
                {
                    call();
                }
                ThreadSync.Clear();

                MyGame.Main.OnUpdate();

                Level.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Gametime = gameTime;

            if (IsActive && !PauseGame)
            {
                graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil | ClearOptions.DepthBuffer, Screen.BackgroundColor, 0, 0);

                DepthStencilState MaskStencil = new DepthStencilState
                {
                    StencilEnable = true,
                    StencilFunction = CompareFunction.Always,
                    StencilPass = StencilOperation.Replace,
                    ReferenceStencil = 1,
                    DepthBufferEnable = true,
                };

                DepthStencilState ImageStencil = new DepthStencilState
                {
                    StencilEnable = true,
                    StencilFunction = CompareFunction.LessEqual,
                    StencilPass = StencilOperation.Keep,
                    ReferenceStencil = 1,
                    DepthBufferEnable = true,
                };

                BlendState Transparency = new BlendState
                {
                    ColorSourceBlend = Blend.SourceColor, // multiplier of the source color
                    ColorBlendFunction = BlendFunction.Max, // function to combine colors
                    ColorDestinationBlend = Blend.DestinationColor, // multiplier of the destination color
                    AlphaSourceBlend = Blend.SourceAlpha, // multiplier of the source alpha
                    AlphaBlendFunction = BlendFunction.Subtract, // function to combine alpha
                    AlphaDestinationBlend = Blend.DestinationAlpha, // multiplier of the destination alpha
                };

                // Draw Masks
                Drawing.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, Screen.Filter, null);
                Drawing.Draw(ContentLoader.LoadTexture("Editor/TestImages/Mask.png"), new Rectangle(Input.Mouse.Position.X, Input.Mouse.Position.Y, 100, 100), null, Color.White, 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
                Drawing.End();

                // Draw Objects
                Drawing.Begin(SpriteSortMode.BackToFront, Transparency, Screen.Filter, null);

                // Updates Base Scripts
                foreach (Component Com in BaseScripts.ToList())
                {
                    Com.Draw();
                }

                MyGame.Main.OnDraw();

                Level.Draw();

                Drawing.End();
            }

            base.Draw(gameTime);
        }

        public static void Close(bool CloseConsole = false)
        {
            PauseGame = true;
            Level.ClearLevel();
            PauseGame = true;

            Drawing.Dispose();

            Debug.LogWarning("Disposing Resources...");

            if (CloseConsole)
            {
                Environment.Exit(0);
            }
        }
    }

    public class DefaultValues
    {
        public static SpriteFont DefaultFont;
        public static Texture2D PixelTexture;
        public static string ExecutableFolderPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ExecutableTempFolderPath = Path.GetTempPath() + "/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static string ContentPath;
    }

    public class Draw2D : SpriteBatch
    {
        public GraphicsDevice graphicsDevice;

        public Draw2D(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void DrawLine(Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            Draw(DefaultValues.PixelTexture, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}