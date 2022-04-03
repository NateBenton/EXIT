namespace _NBGames.Scripts.Utilities
{
	// Crosshair Enum
    public enum CrosshairType
    {
        Main,
        Interactable,
        Examine,
        Take,
        Locked,
        Light
    }

	// ItemType
    public enum ItemType
    {
        Key,
        Consumable,
        Weapon
    }

    public enum ItemUsageSetting
    {
        CannotBeUsed,
        CraftingOnly,
        CanBeUsedAlone
    }

    public enum ItemBehavior
    {
        Other,
        Ammo
    }
    
    public enum BindingType
    {
        Normal,
        Inventory,
        Examine,
        Padlock,
        Disabled
    }
}
