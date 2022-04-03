using System.Linq;
using _NBGames.Scripts.Inventory.ScriptableObjects;
using _NBGames.Scripts.Utilities;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _NBGames.Scripts.Editor
{
    public class ItemEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/CTGames/Item Editor")]
        private static void OpenWindow()
        {
            GetWindow<ItemEditor>().Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GUIEvents.onRefreshMenu += RefreshMenuTree;
        }
        
        private static CreateNewItemData _createNewItemData;
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GUIEvents.onRefreshMenu -= RefreshMenuTree;
            
            if (_createNewItemData != null)
            {
                DestroyImmediate(_createNewItemData.itemData);
            }
        }
        
        private ItemData[] consumableItems => AssetDatabase.FindAssets("t:ItemData", new []{"Assets/ItemData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>).Where(x => x.itemType == ItemType.Consumable)
            .ToArray();

        private ItemData[] keyItems => AssetDatabase.FindAssets("t:ItemData", new []{"Assets/ItemData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>).Where(x => x.itemType == ItemType.Key)
            .ToArray();

        private ItemData[] weaponItems => AssetDatabase.FindAssets("t:ItemData", new []{"Assets/ItemData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>).Where(x => x.itemType == ItemType.Weapon)
            .ToArray();

        private static OdinMenuTree _tree;
        
        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree();

            _createNewItemData = new CreateNewItemData();

            consumableItems.ForEach(item => _tree.Add($"Consumables/{item.itemName} ({item.name})", item));
            keyItems.ForEach(item => _tree.Add($"Key Items/{item.itemName} ({item.name})", item));
            weaponItems.ForEach(item => _tree.Add($"Weapons/{item.itemName} ({item.name})", item));
            
            _tree.SortMenuItemsByName(false);
            _tree.Add("Create New", _createNewItemData);

            _tree.EnumerateTree().AddIcons<ItemData>(x => x.menuSprite);

            return _tree;
        }

        protected override void OnBeginDrawEditors()
        {
            if (!(_tree?.Selection.SelectedValue is ItemData asset)) return;
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("Delete This"))
                {
                    var path = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();

                    var attemptedSelection = _tree.MenuItems[0].ChildMenuItems[0].Value;
                    TrySelectMenuItemWithObject(attemptedSelection);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private static void RefreshMenuTree()
        {
            if (_tree.Selection.SelectedValue == _createNewItemData) return;
            var selection = _tree.Selection.SelectedValue;
            var window = GetWindow<ItemEditor>();
            window.ForceMenuTreeRebuild();
            window.TrySelectMenuItemWithObject(selection);
        }
        
        public class CreateNewItemData
        {
            public CreateNewItemData()
            {
                itemData = CreateInstance<ItemData>();
                itemData.itemName = "New Item";
            }

            private string GenerateRandomNumbers()
            {
                const string glyph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var randomString = "";

                for (var i = 0; i < 4; i++)
                {
                    randomString += glyph[Random.Range(0, glyph.Length)];
                }

                return randomString;
            }
            
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public ItemData itemData;
            [Button("Add New Item")]
            private void CreateNewData()
            {
                AssetDatabase.CreateAsset(itemData, "Assets/ItemData/" + GenerateRandomNumbers() + ".asset");
                AssetDatabase.SaveAssets();
                
                // Create new instance of the SO
                itemData = CreateInstance<ItemData>();
                itemData.itemName = "New Item";
            }
        }
    }
}
