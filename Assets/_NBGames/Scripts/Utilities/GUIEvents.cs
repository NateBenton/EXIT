using System;

namespace _NBGames.Scripts.Utilities
{
    public class GUIEvents
    {
        public static event Action onRefreshMenu;

        public static void RefreshMenuTree()
        {
            onRefreshMenu?.Invoke();
        }

        public static event Action onRefreshCraftingMenu;
        public static void RefreshCraftingMenuTree()
        {
            onRefreshCraftingMenu?.Invoke();
        }
    }
}
