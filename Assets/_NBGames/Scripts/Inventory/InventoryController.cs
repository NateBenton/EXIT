using _NBGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _NBGames.Scripts.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            switch (InventoryManager.instance.inventoryState)
            {
                case InventoryManager.InventoryState.Normal:
                    NormalStateControls();
                    break;
                
                case InventoryManager.InventoryState.Combine:
                    CombineStateControls();
                    break;
                
                case InventoryManager.InventoryState.Submenu:
                    SubmenuStateControls();
                    break;
                
                case InventoryManager.InventoryState.MenuUseItem:
                    NormalStateControls();
                    break;
            }
        }

        private void NormalStateControls()
        {
            if (ControlManager.instance.Player.GetButtonDown("Inventory Close") ||
                ControlManager.instance.Player.GetButtonDown("Inventory Cancel"))
            {
                EventManager.ToggleInventory();
            }
        }

        private void CombineStateControls()
        {
            if (ControlManager.instance.Player.GetButtonDown("Inventory Close") ||
                ControlManager.instance.Player.GetButtonDown("Inventory Cancel"))
            {
                InventoryManager.instance.CancelCombine();
            }
        }

        private void SubmenuStateControls()
        {
            if (ControlManager.instance.Player.GetButtonDown("Inventory Cancel"))
            {
                EventManager.CloseSelectionMenu();
                EventSystem.current.SetSelectedGameObject(InventoryManager.instance.SelectedInventorySlot);
            }
        }
    }
}
