using System.Collections.Generic;
using System.Linq;
using _NBGames.Scripts.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace _NBGames.Scripts.Inventory.ScriptableObjects
{
    public class CraftData : ScriptableObject
    {
        public override string ToString()
        {
            return recipeName;
        }

        #if UNITY_EDITOR
        private ItemData[] participatesInCraftingItems => AssetDatabase.FindAssets("t:ItemData", new []{"Assets/ItemData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>).Where(x => x.participatesInCrafting)
            .ToArray();
        
        
        
        private ValueDropdownList<ItemData> GetItems()
        {
            var dropdown = new ValueDropdownList<ItemData>();

            participatesInCraftingItems.ForEach(item => dropdown.Add($"{item.itemName} ({item.name})", item));
            return dropdown;
        }
        #endif


        // Recipe Name
        [BoxGroup("Recipe Settings")]
        [FoldoutGroup("Recipe Settings/Items")]
        [VerticalGroup("Recipe Settings/Items/Vertical")]
        [HorizontalGroup("Recipe Settings/Items/Vertical/NameFiller", Width = 0.5f)]
        [DelayedProperty]
        [OnValueChanged("UpdateMenuTree")]
        [LabelWidth(90)]
        public string recipeName;
        
        private void UpdateMenuTree()
        {
            GUIEvents.RefreshCraftingMenuTree();
        }

        // Item Crafted
        [VerticalGroup("Recipe Settings/Items/Vertical")]
        [HorizontalGroup("Recipe Settings/Items/Vertical/DropdownFiller", Width = 0.5f)]
        [ValueDropdown("GetItems")]
        [LabelWidth(90)]
        public ItemData itemCrafted;

        // Craft Combos
        [VerticalGroup("Recipe Settings/Items/Vertical")]
        [LabelText("Items Combined to Create")]
        [EnableIf("@itemCrafted != null")]
        [Indent]
        [DelayedProperty]
        [OnValueChanged("SortList")]
        [ValueDropdown("GetItems")]
        public List<ItemData> ingredients;

        private void SortList()
        {
            ingredients = ingredients.OrderBy(x => x.itemName).ToList();
        }

        // Ingredient Order Matters
        [FoldoutGroup("Recipe Settings/Additional Settings")]
        [VerticalGroup("Recipe Settings/Additional Settings/Vertical")]
        [LabelWidth(150)]
        public bool ingredientOrderMatters;

        // Requires Work Bench
        [VerticalGroup("Recipe Settings/Additional Settings/Vertical")] 
        [LabelWidth(150)]
        public bool requiresWorkBench;

    }
}
