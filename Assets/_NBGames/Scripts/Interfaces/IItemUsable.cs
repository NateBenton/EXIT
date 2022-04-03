namespace _NBGames.Scripts.Interfaces
{
    public interface IItemUsable
    {
        void OnUse();
        void DisplayMessageCloseMenu();
        void DisplayMessageIncorrectItem();
        void OpenInventory();
        void DisplayStillIncompleteMessage();
        void SendItemsRequiredToInventoryManager();
        void OnItemRequirementsMet();
        void DisplayPostInteractionMessage();
    }
}
