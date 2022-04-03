using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NBGames.Scripts.Inventory.ScriptableObjects
{
    public class ItemData : ScriptableObject
    {
        #region General

        public override string ToString()
        {
            return itemName;
        }

        // Menu Sprite
        [BoxGroup("Item Settings")]
        [FoldoutGroup("Item Settings/General Settings")]
        [HorizontalGroup("Item Settings/General Settings/Horizontal", Width = 80)]
        [VerticalGroup("Item Settings/General Settings/Horizontal/Previews")]
        [HideLabel, PreviewField(75, ObjectFieldAlignment.Left)]
        public Sprite menuSprite;

        // Item Name
        [HorizontalGroup("Item Settings/General Settings/Horizontal")]
        [VerticalGroup("Item Settings/General Settings/Horizontal/Vertical")]
        [DelayedProperty]
        [OnValueChanged("UpdateMenuTree")]
        [LabelWidth(75)]
        public string itemName;

        private void UpdateMenuTree()
        {
                GUIEvents.RefreshMenuTree();
        }
        
        
        // Require Two Slots?
        [HorizontalGroup("Item Settings/General Settings/Horizontal")] 
        [Indent(2)]
        [PropertySpace(SpaceBefore = 25)]
        [LabelWidth(145)]
        public bool requiresTwoSlots;
        
        // Description
        [HorizontalGroup("Item Settings/General Settings/Horizontal")]
        [VerticalGroup("Item Settings/General Settings/Horizontal/Vertical")]
        [LabelWidth(75)]
        public string description;
        
        // Examine Length Modifier
        [VerticalGroup("Item Settings/General Settings/Horizontal/Vertical")] 
        [LabelWidth(75)]
        [LabelText("Ray Modifier")]
        public float examineLengthModifier;
        
        // Model
        [HorizontalGroup("Item Settings/General Settings/Horizontal")]
        [VerticalGroup("Item Settings/General Settings/Horizontal/Vertical")]
        [LabelWidth(75)]
        [LabelText("Model")]
        public GameObject examinableObject;
        
        // Item Type
        [HorizontalGroup("Item Settings/General Settings/Horizontal")]
        [VerticalGroup("Item Settings/General Settings/Horizontal/Vertical")]
        [LabelWidth(75)]
        [DelayedProperty]
        [OnValueChanged("UpdateMenuTree")]
        public ItemType itemType = ItemType.Consumable;

        #endregion
        
        #region Inventory Settings

        // Can Be Stored
        [FoldoutGroup("Item Settings/Inventory Settings")]
        [VerticalGroup("Item Settings/Inventory Settings/Vertical")]
        [LabelText("Can Be Stored")]
        public bool canBeStored = true;
        
        // Can Be Discarded
        [VerticalGroup("Item Settings/Inventory Settings/Vertical")]
        public bool canBeDiscarded;
        
        // Discarded to Storage Chest
        [VerticalGroup("Item Settings/Inventory Settings/Vertical")]
        [EnableIf("canBeDiscarded")]
        [Indent]
        [LabelText("Discarded to Storage Chest")]
        public bool discardToChest;

        // Can Stack
        [VerticalGroup("Item Settings/Inventory Settings/Vertical")]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon")]
        [LabelText("Can Stack")]
        public bool canStack = true;
        
        // Max Quantity
        [VerticalGroup("Item Settings/Inventory Settings/Vertical")]
        [HorizontalGroup("Item Settings/Inventory Settings/Vertical/QuantityCutoff", Width = 150)]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon")]
        [EnableIf("canStack")]
        [Indent]
        [LabelText("Max Quantity Per Slot")]
        [MinValue(1)]
        public int maxQuantity = 1;
        
        // Usage Count
        [VerticalGroup("Item Settings/Inventory Settings/Vertical")]
        [HorizontalGroup("Item Settings/Inventory Settings/Vertical/UsageCutoff", Width = 150)]
        [HideIf("itemType", ItemType.Weapon)]
        [LabelText("Usage Count")]
        [MinValue(1)]
        public int usageCount = 1;

        #endregion

        #region Weapon Settings

        // Ammo Capacity
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Consumable")]
        [FoldoutGroup("Item Settings/Weapon Settings")]
        
        [HorizontalGroup("Item Settings/Weapon Settings/Split", LabelWidth = 130)]
        [HorizontalGroup("Item Settings/Weapon Settings/Split/Left")]
        [VerticalGroup("Item Settings/Weapon Settings/Split/Left/Vertical")]
        [ProgressBar(1, 100, Height = 18)]
        public int ammoCapacity = 10;
        
        // Base Damage
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Consumable")]
        [HorizontalGroup("Item Settings/Weapon Settings/Split/Right")]
        [VerticalGroup("Item Settings/Weapon Settings/Split/Right/Vertical")]
        [ProgressBar(1, 100, Height = 18)]
        public int baseDamage = 20;
        
        // Reload Speed
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Consumable")]
        [HorizontalGroup("Item Settings/Weapon Settings/Split/Left")]
        [VerticalGroup("Item Settings/Weapon Settings/Split/Left/Vertical")]
        [ProgressBar(1, 4, Height = 18)]
        public float reloadSpeed = 1;
        
        // Critical Hit Multiplier
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Consumable")]
        [HorizontalGroup("Item Settings/Weapon Settings/Split/Right")]
        [VerticalGroup("Item Settings/Weapon Settings/Split/Right/Vertical")]
        [LabelText("Critical Hit Multiplier")]
        [ProgressBar(1, 4, Height = 18)]
        public float criticalMultiplier = 2f;

        #endregion

        #region Usage Settings

        // Usage Settings
        [HideIf("itemType", ItemType.Weapon)]
        [FoldoutGroup("Item Settings/Usage Settings")]
        [VerticalGroup("Item Settings/Usage Settings/Vertical")]
        [HorizontalGroup("Item Settings/Usage Settings/Vertical/UsageTypeCondenser", Width = 300)]
        [LabelWidth(130)]
        [LabelText("Usage Type")]
        public ItemUsageSetting itemUsageSetting = ItemUsageSetting.CanBeUsedAlone;
        
        #endregion

        #region Behavior Settings
        
        // Item Behavior
        [FoldoutGroup("Item Settings/Behavior Settings")]
        [VerticalGroup("Item Settings/Behavior Settings/Main")]
        [HorizontalGroup("Item Settings/Behavior Settings/Main/BehaviorCutoff", Width = 300)]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone")]
        [LabelWidth(130)]
        //[PropertySpace(SpaceAfter = 15)]
        public ItemBehavior itemBehavior;
        
        [TitleGroup("Item Settings/Behavior Settings/Main/Behaviors")]

        [HorizontalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain")]
        // Heals Player?
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [VerticalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/PlayerHeal")]
        [LabelWidth(160)]
        public bool healsPlayer = false;
        
        // Heal Amount
        [HorizontalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/PlayerHeal/Cutoff", Width = 200)]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [EnableIf("healsPlayer")]
        [LabelWidth(160)]
        [Indent]
        public float healAmount;
        
        // Increases Max Health?
        [VerticalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/MaxHealth")]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [LabelWidth(160)]
        public bool increasesMaxHealth;
        
        // Increase Health Amount
        [HorizontalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/MaxHealth/Cutoff", Width = 200)]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [EnableIf("increasesMaxHealth")]
        [LabelWidth(160)]
        [LabelText("Increase Amount")]
        [Indent]
        public float healthIncreaseAmount;
        
        // Increases Max Inventory Slots?
        [VerticalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/MaxInventory")]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [LabelWidth(160)]
        public bool increasesMaxInventory;
        
        // Slots to Add
        [HorizontalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/MaxInventory/Cutoff", Width = 200)]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [EnableIf("increasesMaxInventory")]
        [LabelWidth(160)]
        [MinValue(1)]
        [MaxValue(4)]
        [Indent]
        public int slotsToAdd = 1;
        
        // Increases Reload Speed Multiplier?
        [VerticalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/ReloadSpeed")]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [LabelWidth(160)]
        public bool increasesReloadSpeed;
        
        // Reload Speed Multiplier 
        [HorizontalGroup("Item Settings/Behavior Settings/Main/Behaviors/HorizontalMain/ReloadSpeed/Cutoff", Width = 200)]
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Other")]
        [EnableIf("increasesReloadSpeed")]
        [LabelWidth(160)]
        [LabelText("Increase Multiplier By")]
        [MinValue(0f)]
        [MaxValue(1f)]
        [Indent]
        public float reloadSpeedIncreaseAmount;

        // Weapons to Reload
        [HideIf("@itemType == ItemType.Key || itemType == ItemType.Weapon || " +
                "itemUsageSetting != ItemUsageSetting.CanBeUsedAlone || itemBehavior != ItemBehavior.Ammo")]
        [LabelWidth(115)]
        [PropertySpace(SpaceBefore = 5)]
        [VerticalGroup("Item Settings/Behavior Settings/Main")]
        [HorizontalGroup("Item Settings/Behavior Settings/Main/WeaponCutoff", Width = 350)]
        public WeaponsToReload[] weaponsToReload;

        #endregion

        #region Crafting Settings

        [FoldoutGroup("Item Settings/Crafting Settings")] 
        [VerticalGroup("Item Settings/Crafting Settings/Vertical")]
        public bool participatesInCrafting;

        #endregion

    }
}
