using _NBGames.Scripts.Inventory.ScriptableObjects;

namespace _NBGames.Scripts.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public bool slotEnabled = true;
    }
}
