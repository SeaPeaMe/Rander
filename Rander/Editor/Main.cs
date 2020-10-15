using Microsoft.Xna.Framework;
using Rander._2D;
using System.IO;

namespace Rander.Editor
{
    class Main
    {
        public static void Init()
        {
            Debug.LogSuccess("----- BEGIN EDITOR -----");
            Screen.BackgroundColor = new Color(40, 40, 40);

            Debug.LogWarning("Init...");
            SetPresets();
            UI.Init();
        }

        public static void Update()
        {
            if (Input.Mouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (UI.AddWindow != null) UI.AddWindow.Dispose(true);

                UI.RClick.Position = Input.Mouse.Position.ToVector2();
                UI.RClick.Enabled = true;
            }
        }

        static void SetPresets()
        {
            if (!Directory.Exists(ContentLoader.ContentPath + "/Editor/Presets/"))
            {
                // Adds 2D Presets
                Debug.LogWarning("Creating Presets...");
                Presets.Save(new Object2D("Editor_Preset2DObject", Vector2.Zero, new Vector2(50, 50), 0, null, Alignment.Center, 0, null, null), ContentLoader.ContentPath + "/Editor/Presets/2D/Empty.pre", true);
                Presets.Save(new Object2D("Editor_Preset2DObject_Image", Vector2.Zero, new Vector2(50, 50), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, Color.White) }, Alignment.Center, 0, null, null), ContentLoader.ContentPath + "/Editor/Presets/2D/Image.pre", true);
                Presets.Save(new Object2D("Editor_Preset2DObject_Text", Vector2.Zero, new Vector2(135, 30), 0, new Component2D[] { new Text2DComponent("Hello World!", DefaultValues.DefaultFont, Color.White, 0.18f, Alignment.TopLeft, true) }, Alignment.Center, 0, null, null), ContentLoader.ContentPath + "/Editor/Presets/2D/Text.pre", true);
            }
        }
    }

    class UI
    {
        const float EditorUILayer = 1f;
        static Object2D Properties;
        static Object2D LevelContent;
        static Object2D TopBar;
        static Object2D FileButton;
        public static Object2D RClick;
        public static Object2D AddWindow;

        public static void Init()
        {
            Debug.Log("    UI");

            // TopBar
            TopBar = new Object2D("Editor_TopBar", Vector2.Zero, new Vector2(Screen.Resolution.X, 25), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.ToolBar),
                new Spacer2DComponent(SpacerOption.HorizontalSpacer, new Vector2(5, 0), Alignment.MiddleLeft, new Vector2(5, 0))
            }, Alignment.TopLeft, EditorUILayer - 0.0001f);

            FileButton = new Object2D("Editor_FileButton", Vector2.Zero, new Vector2(100, 25), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.ToolBar),
                new Text2DComponent("File", DefaultValues.DefaultFont, EditorTheme.Window, 0, 0.2f, Alignment.Center, false),
                new Button2DComponent(
                    onHover: () => FileButton.GetComponent<Image2DComponent>().Color = new Color(EditorTheme.ToolBar.ToVector3() + new Vector3(0.1f)),
                    onExit: () => FileButton.GetComponent<Image2DComponent>().Color = EditorTheme.ToolBar
                    ),
                new Dropdown2DComponent(new Object2D[] { new Object2D("Editor_SaveButton", Vector2.Zero, new Vector2(240, 25), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.ToolBar), new Text2DComponent("Save Level", DefaultValues.DefaultFont, EditorTheme.Window, 0, 0.2f, Alignment.Center) , new Button2DComponent(onClick: () => Level.SaveLevel(FileExplorer.SaveFile("Save"))) }) }, new Vector2(250, 500), new Vector2(5, 5), EditorTheme.Window, true)
            }, Alignment.TopLeft, EditorUILayer - 0.0005f);

            TopBar.AddChild(FileButton);

            // Properties Bar
            Properties = new Object2D("Editor_PropertiesContainer", new Vector2(Screen.Resolution.X, Screen.Resolution.Y), new Vector2(300, Screen.Resolution.Y - 75), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.Window),
                new Spacer2DComponent(SpacerOption.VerticalSpacer, new Vector2(0, 10), Alignment.TopCenter, new Vector2(0, 10))
            }, Alignment.BottomRight, EditorUILayer - 0.0005f);
            Properties.Parent = new Object2D("Editor_PropertiesContainerTitle", Properties.Position + new Vector2(0, -Properties.Size.Y), new Vector2(300, 50), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.TitleBar),
                new Text2DComponent("Properties", DefaultValues.DefaultFont, EditorTheme.Window, 0, 0.2f, Alignment.Center, false, 1)
            }, Alignment.BottomRight, EditorUILayer - 0.0005f);

            // Level Contents
            LevelContent = new Object2D("Editor_LevelContents", new Vector2(0, Screen.Resolution.Y), new Vector2(300, Screen.Resolution.Y - 75), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.Window),
                new Spacer2DComponent(SpacerOption.VerticalSpacer, new Vector2(0, 10), Alignment.TopCenter, new Vector2(0, 10))
            }, Alignment.BottomLeft, EditorUILayer - 0.0005f);

            LevelContent.Parent = new Object2D("Editor_LevelContentTitle", LevelContent.Position + new Vector2(0, -LevelContent.Size.Y), new Vector2(300, 50), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.TitleBar),
                new Text2DComponent("Level Content", DefaultValues.DefaultFont, EditorTheme.Window, 0, 0.2f, Alignment.Center, false, 1)
            }, Alignment.BottomLeft, EditorUILayer - 0.0005f);

            // RightClickMenu
            RClick = new Object2D("Editor_RClickMenu", Vector2.Zero, new Vector2(250, 500), 0, new Component2D[] {
                new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.Window),
                new Spacer2DComponent(SpacerOption.VerticalSpacer, new Vector2(0, 5), Alignment.TopCenter, new Vector2(0, 5))
            }, Alignment.TopLeft, EditorUILayer - 0.0004f, null, new Object2D[] { 
                // Menu Items
                new Object2D("Editor_RClickMenu_NewObject2D", Vector2.Zero, new Vector2(240, 25), 0, new Component2D[] {
                    new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.ToolBar),
                    new Text2DComponent("Create 2D Object", DefaultValues.DefaultFont, EditorTheme.Window, 0.1f, 0.2f, Alignment.Center, false),
                    new Button2DComponent(
                    onHover: () => RClick.Children.Find((x) => x.ObjectName == "Editor_RClickMenu_NewObject2D").GetComponent<Image2DComponent>().Color = new Color(EditorTheme.ToolBar.ToVector3() + new Vector3(0.1f)),
                    onExit: () => RClick.Children.Find((x) => x.ObjectName == "Editor_RClickMenu_NewObject2D").GetComponent<Image2DComponent>().Color = EditorTheme.ToolBar,
                    onClick: Add2DObject
                    )
                })
            });
            RClick.Enabled = false;
        }

        static void Add2DObject()
        {
            RClick.Enabled = false;
            AddWindow = new Object2D("Editor_SelectWindow2D", Screen.Resolution / 2, new Vector2(250, 500), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.Window), new Spacer2DComponent(SpacerOption.VerticalSpacer, new Vector2(5, 5), Alignment.TopCenter, new Vector2(0, 5)) }, Alignment.Center, EditorUILayer - 0.0005f);

            int i = 0;
            foreach (string Preset in Directory.GetFiles(ContentLoader.ContentPath + "/Editor/Presets/2D/"))
            {
                Object2D Btn = new Object2D(Preset, Vector2.Zero, new Vector2(240, 25), 0, new Component2D[] { new Image2DComponent(DefaultValues.PixelTexture, EditorTheme.ToolBar), new Text2DComponent(Path.GetFileNameWithoutExtension(Preset), DefaultValues.DefaultFont, EditorTheme.Window, 0, 0.2f, Alignment.Center), new InstantiateObject() }, Alignment.TopCenter, EditorUILayer - 0.0005f, AddWindow);
                Btn.AddComponent(new Button2DComponent(
                    onHover: () => Btn.GetComponent<Image2DComponent>().Color = new Color(EditorTheme.ToolBar.ToVector3() + new Vector3(0.1f)),
                    onExit: () => Btn.GetComponent<Image2DComponent>().Color = EditorTheme.ToolBar,
                    onClick: () => Btn.GetComponent<InstantiateObject>().Ins()
                    ));
                i++;
            }
        }
    }

    class EditorTheme
    {
        public static Color Window = new Color(60, 60, 60);
        public static Color ToolBar = new Color(120, 120, 120);
        public static Color TitleBar = new Color(100, 100, 100);
    }
}
