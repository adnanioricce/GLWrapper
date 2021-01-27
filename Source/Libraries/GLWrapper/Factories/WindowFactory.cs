using OpenTK.Windowing.Desktop;

namespace GLWrapper.Factories
{
    public static class WindowFactory
    {
        public static GameWindow CreateDefaultWindow(int width,int height,string title = "Game Window")
        {
            var nativeSettings = new NativeWindowSettings{
                Size = new OpenTK.Mathematics.Vector2i(width,height),
                Title = title
            };
            var defaultSettings = GameWindowSettings.Default;
            return new GameWindow(defaultSettings,nativeSettings);
        }
    }
}