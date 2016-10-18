using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Autoplay
{
    class RyzeMenu
    {
        public static Menu Menu;
        public void Init()
        {
            Menu = MainMenu.AddMenu("Ryze Reworked", "whatever", "RyzeRework");
            Menu.AddGroupLabel("Loaded");
        }
    }
}