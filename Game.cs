using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Rander._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Rander
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        static int TargetFPS = 144;
        static bool VSync = true;
        static SamplerState Filter = SamplerState.PointClamp; // Makes textures nice and crisp when doing pixel art
        public static Color BackgroundColor = Color.CornflowerBlue;
        static Vector2 Resolution = Vector2.Zero; // Leave as Vector2.Zero for automatic resolution

        public static GraphicsDeviceManager graphics;
        public static Draw2D Drawing;
        public static Microsoft.Xna.Framework.Game gameWindow;
        public static GameTime Gametime = new GameTime();

        public static List<Timer> Timers = new List<Timer>();
        public static List<SoundEffectInstance> Sounds = new List<SoundEffectInstance>();

        static List<Component> BaseScripts = new List<Component>();

        public static Dictionary<string, Object2D> Objects2D = new Dictionary<string, Object2D>();

        public Game()
        {
            Debug.LogWarning("Initializing...");
            graphics = new GraphicsDeviceManager(this);

            // Sets up the window to device's resolution and in full screen
            TargetElapsedTime = TimeSpan.FromSeconds((float)1 / TargetFPS);
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = VSync;

            if (Resolution == Vector2.Zero) Resolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            graphics.PreferredBackBufferWidth = Resolution.ToPoint().X;
            graphics.PreferredBackBufferHeight = Resolution.ToPoint().Y;
            graphics.IsFullScreen = true;

            gameWindow = this;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Load Default Values
            Screen.Width = graphics.PreferredBackBufferWidth;
            Screen.Height = graphics.PreferredBackBufferHeight;

            // Load Base Scripts
            BaseScripts.Add(new MouseInput());
            BaseScripts.Add(new Rand());
            BaseScripts.Add(new Time());
            BaseScripts.Add(new Input());

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
            DefaultValues.DefaultFont = ContentLoader.LoadFont("Defaults/Arial");
            DefaultValues.PixelTexture = ContentLoader.LoadTexture("Defaults/Pixel.png");

            if (MyGame.Main.OnGameLoad())
            {
                Debug.LogSuccess("Finished!");
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Close(true);

                // Update important scripts first
                foreach (Component Scr in BaseScripts.ToList())
                {
                    Scr.Update();
                }

                MyGame.Main.OnUpdate();

                // Update 2D
                foreach (Object2D Obj in Objects2D.Values.ToList())
                {
                    Obj.Update();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Gametime = gameTime;

            if (IsActive)
            {
                graphics.GraphicsDevice.Clear(BackgroundColor);

                Drawing.Begin(SpriteSortMode.FrontToBack, samplerState: Filter);
                // Updates Base Scripts
                foreach (Component Com in BaseScripts.ToList())
                {
                    Com.Draw();
                }

                MyGame.Main.OnDraw();

                // Draws 2D
                foreach (Object2D Obj in Objects2D.Values.ToList())
                {
                    Obj.Draw();
                }
                Drawing.End();
            }

            base.Draw(gameTime);
        }

        public static void Close(bool CloseConsole = false)
        {
            Debug.LogWarning("Disposing Objects & Instances...");
            Debug.Log("     2D Objects...");
            Objects2D.Clear();
            Debug.Log("     Graphic Window...");
            gameWindow.Dispose();
            Debug.Log("     Sound Instances...");
            for (int i = 0; i < Sounds.Count;)
            {
                Sounds[0].Stop();
                Sounds[0].Dispose();
                Sounds.RemoveAt(0);
            }

            Debug.LogWarning("Disposing Timers...");
            for (int i = 0; i < Timers.Count;)
            {
                Timers[0].Stop();
                Timers[0].Dispose();
                Timers.RemoveAt(0);
            }

            Debug.LogWarning("Disposing Resources...");
            ContentLoader.Dispose();

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
    }

    public class Screen
    {
        public static int Width;
        public static int Height;
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
            Draw(DefaultValues.PixelTexture, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}