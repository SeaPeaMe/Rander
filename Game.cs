using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rander._2D;
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
        static int FixedUpdateTime = (int)((float)1 / GameSettings.TargetFPS * 1000);

        public static bool PauseGame = false;

        internal static List<Component> BaseScripts = new List<Component>();

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
            PauseGame = true;
            Screen.ApplyChanges();

            // Load Base Scripts
            BaseScripts.Add(new MouseInput());
            BaseScripts.Add(new Rand());
            BaseScripts.Add(new Input());
            BaseScripts.Add(new Time());
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
            MyGame.Main.Initialize();
            MyGame.Main.Start();

            Debug.LogSuccess("Finished!");
            FixedUpdate();
            PauseGame = false;
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

                MyGame.Main.Update();

                Level.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Gametime = gameTime;

            if (IsActive && !PauseGame)
            {
                graphics.GraphicsDevice.Clear(Screen.BackgroundColor);

                // Draw Objects
                Drawing.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, Screen.Filter, null, transformMatrix: Level.Active2DCamera != null ? Level.Active2DCamera.Matrix : Matrix.Identity);

                // Updates Base Scripts
                foreach (Component Com in BaseScripts.ToList())
                {
                    Com.Draw();
                }

                MyGame.Main.Draw();

                Level.Draw();

                Drawing.End();
            }

            base.Draw(gameTime);
        }

        internal static void FixedUpdate()
        {
            if (!PauseGame)
            {
                MyGame.Main.FixedUpdate();
                Level.FixedUpdate();
            }

            Time.Wait(FixedUpdateTime, () => FixedUpdate());
        }

        public static void Close(bool CloseConsole = false)
        {
            PauseGame = true;
            Level.ClearLevel();
            PauseGame = true;

            Drawing.Dispose();

            Debug.LogWarning("Closing Window...");

            gameWindow.Dispose();

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