using System.Collections.Generic;
using _CTGames.Scripts.Inventory;
using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Inventory
{
    public class Inventory : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] private List<InventorySlot> _inventorySlots;

        #endregion

        #region PROPERTIES

        public List<InventorySlot> inventorySlots => _inventorySlots;

        #endregion

        #region METHODS

        private void OnEnable()
        {
            EventManager.onItemPickedUp += AddItemToInventory;
        }

        private void OnDisable()
        {
            EventManager.onItemPickedUp -= AddItemToInventory;
        }

        private void AddItemToInventory(AddToInventoryBehavior callingObject)
        {
            foreach (var slot in _inventorySlots)
            {
                if (slot.item || !slot.slotEnabled) continue;
                slot.item = callingObject.itemData;
                break;
            }
        }

        #endregion
       
    }
}
