﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Rander._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
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
        static bool FullScreen = true;

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
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Sets up the window to device's resolution and in full screen
            TargetElapsedTime = TimeSpan.FromSeconds((float)1 / TargetFPS);
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = VSync;

            if (Resolution == Vector2.Zero) Resolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            graphics.PreferredBackBufferWidth = Resolution.ToPoint().X;
            graphics.PreferredBackBufferHeight = Resolution.ToPoint().Y;
            graphics.IsFullScreen = FullScreen;
            graphics.ApplyChanges();

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
                graphics.GraphicsDevice.Clear(BackgroundColor);

                Drawing.Begin(SpriteSortMode.FrontToBack, samplerState: Filter);
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