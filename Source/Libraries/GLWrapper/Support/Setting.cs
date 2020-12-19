using System;

namespace GLWrapper.Support
{
    public class Setting
    {
        public (int Width, int Height) ScreenSize;
        public static Setting CreateSettings(Func<Setting,Setting> config)
        {
            return config(new Setting());
        }
        protected Setting()
        {
            
        }
    }
}
