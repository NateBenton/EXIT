using System.Linq;
using _NBGames.Scripts.Inventory.ScriptableObjects;
using _NBGames.Scripts.Utilities;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace _NBGames.Scripts.Editor
{
    public class CraftingEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/CTGames/Crafting Editor")]
        private static void OpenWindow()
        {
            GetWindow<CraftingEditor>().Show();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            GUIEvents.onRefreshCraftingMenu += RefreshMenuTree;
        }

        private static CreateNewCraftData _createNewCraftData;
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GUIEvents.onRefreshCraftingMenu -= RefreshMenuTree;

            if (_createNewCraftData != null)
            {
                DestroyImmediate(_createNewCraftData.craftData);
            }
        }
        
        private CraftData[] craftingRecipes => AssetDatabase.FindAssets("t:CraftData", new []{"Assets/CraftData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<CraftData>)
            .ToArray();

        private static OdinMenuTree _tree;
        
        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree();

            _createNewCraftData = new CreateNewCraftData();

            foreach (var item in craftingRecipes)
            {
                _tree.Add($"Crafting Recipes/{item.recipeName} ({item.name})", item);
            }

            _tree.Add("Create New", _createNewCraftData);
            _tree.SortMenuItemsByName(false);

            return _tree;
        }
        
        private static void RefreshMenuTree()
        {
            if (_tree.Selection.SelectedValue != _createNewCraftData)
            {
                var selection = _tree.Selection.SelectedValue;
                var window = GetWindow<CraftingEditor>();
                window.ForceMenuTreeRebuild();
                window.TrySelectMenuItemWithObject(selection);
            }
        }
        
        protected override void OnBeginDrawEditors()
        {
            if (!(_tree?.Selection.SelectedValue is CraftData asset)) return;
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("Delete"))
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
        

        public class CreateNewCraftData
        {
            public CreateNewCraftData()
            {
                craftData = CreateInstance<CraftData>();
                craftData.recipeName = "New Recipe";
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
            public CraftData craftData;
            [Button("Add New Item")]
            private void CreateNewData()
            {
                AssetDatabase.CreateAsset(craftData, "Assets/CraftData/" + GenerateRandomNumbers() + ".asset");
                AssetDatabase.SaveAssets();
                
                // Create new instance of the SO
                craftData = CreateInstance<CraftData>();
                craftData.recipeName = "New Recipe";
            }
        }
    }
}
