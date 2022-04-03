using System.Linq;
using _NBGames.Scripts.Inventory.ScriptableObjects;
using _NBGames.Scripts.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;

namespace _NBGames.Scripts.Inventory.Classes
{
    [System.Serializable]
    public class WeaponsToReload
    {
        #if UNITY_EDITOR
        public ItemData[] IsWeapon => AssetDatabase.FindAssets("t:ItemData", new []{"Assets/_CTGames/ItemData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>).Where(x => x.itemType == ItemType.Weapon)
            .ToArray();
        
        
        private ValueDropdownList<ItemData> GetItems()
        {
            var dropdown = new ValueDropdownList<ItemData>();

            IsWeapon.ForEach(item => dropdown.Add($"{item.itemName} ({item.name})", item));
            return dropdown;
        }
        #endif

        [ValueDropdown("GetItems")] 
        public ItemData weapon;
    }
}
