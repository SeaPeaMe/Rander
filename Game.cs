using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rander._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Rander
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        static int TargetFPS = 144;
        static bool VSync = true;

        public static GraphicsDeviceManager graphics;
        public static Draw2D Drawing;
        public static Microsoft.Xna.Framework.Game gameWindow;
        public static GameTime Gametime = new GameTime();

        public static bool KillThreads = false;

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

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;

            gameWindow = this;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Load Default Values
            Screen.Width = graphics.PreferredBackBufferWidth;
            Screen.Height = graphics.PreferredBackBufferHeight;

            GraphicsDevice.SetVertexBuffer(new VertexBuffer(GraphicsDevice, new VertexDeclaration(new VertexElement[] { new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0) }), 8, BufferUsage.None));

            // Load Base Scripts
            BaseScripts.Add(new MouseInput());
            BaseScripts.Add(new Rand());
            BaseScripts.Add(new Time());
            BaseScripts.Add(new Input());

            foreach (Component Com in BaseScripts.ToList())
            {
                Com.Start();
            }

            Debug.LogSuccess("Initialization Complete!");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Debug.LogWarning("Loading Content...");

            // Sets default graphic stuff
            DefaultValues.DefaultFont = ContentLoader.LoadFont("Defaults/Arial");
            DefaultValues.PixelTexture = ContentLoader.LoadTexture("Defaults/Pixel.png");

            // Create a new SpriteBatch, which can be used to draw textures.
            Drawing = new Draw2D(GraphicsDevice);

            if (MyGame.Main.OnGameLoad())
            {
                Debug.LogSuccess("Finished!");
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            KillThreads = true;
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
                graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

                Drawing.Begin(SpriteSortMode.FrontToBack);
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
            // Clears all the lists so it doesn't run them again and then closes the game window
            KillThreads = true;
            Objects2D.Clear();
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