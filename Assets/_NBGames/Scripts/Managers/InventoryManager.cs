using System.Collections.Generic;
using System.Linq;
using _NBGames.Scripts.Inventory.ScriptableObjects;
using _NBGames.Scripts.ItemUseBehaviors;
using _NBGames.Scripts.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _NBGames.Scripts.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        #region VARIABLES
        
        [SerializeField] private _NBGames.Scripts.Inventory.Inventory _activeInventory = null;
        [SerializeField] private GameObject _inventoryController = null;

        [SerializeField] [AssetList(AutoPopulate = true, Path = "CraftData")] private CraftData[] _craftData;
        private List<ItemData> _itemsToCombine = new List<ItemData>();

        private bool _isInventoryOpen;
        private GameObject _cachedSubmenuItem, _previousSelectedObject;
        private PromptItemUseBehavior _itemUsePrompt;
        private int _selectedSlotID;
        private ItemData _itemToCraft, _secondItemToCombine;

        private readonly List<int> _combinedItemSlotIDs = new List<int>();

        public enum InventoryState
        {
            Normal,
            Submenu,
            MenuExamine,
            MenuUseItem,
            Combine
        }
        
        public InventoryState inventoryState { get; private set; }

        #endregion

        #region PROPERTIES

        public static InventoryManager instance { get; private set; }

        public List<ItemData> itemsToBeUsed { get; set; } = new List<ItemData>();

        public bool isSelectionMenuOpen { get; set; }

        public ItemData itemInSlot { get; private set; }

        private _NBGames.Scripts.Inventory.Inventory ActiveInventory => _activeInventory;

        public GameObject SelectedInventorySlot { get; private set; }

        #endregion

        #region METHODS
        
        private void Awake()
        {
            inventoryState = InventoryState.Normal;

            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogWarning("InventoryManager already exists. Destroying!");
                Destroy(this.gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.onToggleInventory += ToggleInventoryController;
            EventManager.onChangeDefaultInventoryButton += ChangeSelectedInventorySlot;
            EventManager.onCloseSelectionMenu += CloseSelectionMenu;
            EventManager.onInventoryExamineCancel += CancelExamine;
            EventManager.onOpenInventoryFromItemInteraction += ToggleInventoryControllerUseItem;
        }

        private void OnDisable()
        {
            EventManager.onToggleInventory -= ToggleInventoryController;
            EventManager.onChangeDefaultInventoryButton -= ChangeSelectedInventorySlot;
            EventManager.onCloseSelectionMenu -= CloseSelectionMenu;
            EventManager.onInventoryExamineCancel -= CancelExamine;
            EventManager.onOpenInventoryFromItemInteraction -= ToggleInventoryControllerUseItem;
        }

        private void ToggleInventoryController()
        {
            _isInventoryOpen = !_isInventoryOpen;
            _inventoryController.SetActive(_isInventoryOpen);

            if (_isInventoryOpen)
            {
                itemInSlot = _activeInventory.inventorySlots[_selectedSlotID].item;
                UIManager.Instance.UpdateItemTextInfo();
            }

            if (inventoryState != InventoryState.MenuUseItem) return;
            inventoryState = InventoryState.Normal;
            _itemUsePrompt.DisplayMessageCloseMenu();
        }

        private void ToggleInventoryControllerUseItem(PromptItemUseBehavior itemUsePrompt)
        {
            _itemUsePrompt = itemUsePrompt;
            ToggleInventoryController();
            inventoryState = InventoryState.MenuUseItem;
        }

        public void SelectItemButton()
        {
            if (!itemInSlot) return;
            switch (inventoryState)
            {
                case InventoryState.Normal:
                    inventoryState = InventoryState.Submenu;
                    isSelectionMenuOpen = true;

                    UIManager.Instance.OpenSelectionMenu();
                    EventSystem.current.SetSelectedGameObject(UIManager.Instance.FirstSelectedSubmenuItem);
                    break;
                
                case InventoryState.Combine:
                    CheckRecipe();
                    break;
                
                case InventoryState.MenuUseItem:
                    if ( _itemUsePrompt.itemsRequired.Contains(itemInSlot))
                    {
                        _itemUsePrompt.RemoveItemFromItemsRequired(itemInSlot);
                        _itemUsePrompt.OnUse();

                        for (var i = 0; i < _activeInventory.inventorySlots.Count; i++)
                        {
                            if (ActiveInventory.inventorySlots[i].item != itemInSlot) continue;
                            _activeInventory.inventorySlots[i].item = null;
                            UIManager.Instance.ClearItemSlot(i);
                            break;
                        }

                        inventoryState = InventoryState.Normal;
                        EventManager.ToggleInventory();
                    }
                    else
                    {
                        EventManager.ToggleInventory();
                        _itemUsePrompt.DisplayMessageIncorrectItem();
                    }
                    break;
            }
        }

        private void ChangeSelectedInventorySlot(GameObject slot, int slotID)
        {
            SelectedInventorySlot = slot;
            _selectedSlotID = slotID;
            itemInSlot = _activeInventory.inventorySlots[slotID].item;
            EventSystem.current.firstSelectedGameObject = slot;
        }

        private void CloseSelectionMenu()
        {
            isSelectionMenuOpen = false;
            inventoryState = InventoryState.Normal;
        }

        public void UseItem()
        {
            switch (itemInSlot.itemUsageSetting)
            {
                case ItemUsageSetting.CanBeUsedAlone:
                    break;
                
                case ItemUsageSetting.CannotBeUsed:
                    break;
            }
        }

        public void SelectFirstItemToCombine()
        {
            //if (!itemInSlot.participatesInCrafting) gray out the button in CORE
            inventoryState = InventoryState.Combine;
            isSelectionMenuOpen = false;
            _previousSelectedObject = SelectedInventorySlot;
            
            _itemsToCombine.Add(itemInSlot);
            
            _combinedItemSlotIDs.Add(_selectedSlotID);
            UIManager.Instance.ToggleCombineBackgroundOnFirstSlot(_selectedSlotID);
            EventManager.CloseSubMenuForCombine();
            UIManager.Instance.CloseSelectionMenu();
            isSelectionMenuOpen = false;
            
            EventSystem.current.SetSelectedGameObject(SelectedInventorySlot);
        }

        private void CheckRecipe()
        {
            if (SelectedInventorySlot == _previousSelectedObject)
            {
                //Debug.Log("same item");
            }
            else
            {
                _secondItemToCombine = itemInSlot;
                _itemsToCombine.Add(_secondItemToCombine);
                _combinedItemSlotIDs.Add(_selectedSlotID);
                _itemsToCombine = _itemsToCombine.OrderBy(x => x.itemName).ToList();

                foreach (var recipe in _craftData)
                {
                    if (recipe.ingredients.Count != _itemsToCombine.Count) continue;

                    if (!_itemsToCombine.SequenceEqual(recipe.ingredients)) continue;
                    _itemToCraft = recipe.itemCrafted;
                    RemoveItemsBeingCombined();
                    break;
                }

                if (_itemToCraft != null) return;
                _itemsToCombine.Remove(_secondItemToCombine);
                _combinedItemSlotIDs.Remove(_selectedSlotID);
            }
        }

        private void RemoveItemsBeingCombined()
        {
            foreach (var slotID in _combinedItemSlotIDs)
            {
                _activeInventory.inventorySlots[slotID].item = null;
                UIManager.Instance.ClearItemSlot(slotID);
            }
            
            AddCraftedItemToInventory();
        }

        private void AddCraftedItemToInventory()
        {
            _activeInventory.inventorySlots[_selectedSlotID].item = _itemToCraft;
            itemInSlot = _activeInventory.inventorySlots[_selectedSlotID].item;
            UIManager.Instance.RedrawAfterItemCombo(_combinedItemSlotIDs[0]);
            inventoryState = InventoryState.Normal;
            
            _combinedItemSlotIDs.Clear();
            _itemsToCombine.Clear();
            _itemToCraft = null;

        }

        public void CancelCombine()
        {
            EventSystem.current.SetSelectedGameObject(_previousSelectedObject);
            UIManager.Instance.ToggleCombineBackgroundOnFirstSlot(_combinedItemSlotIDs[0]);
            _combinedItemSlotIDs.Clear();
            _itemsToCombine.Clear();
            inventoryState = InventoryState.Normal;
            itemInSlot = _activeInventory.inventorySlots[_selectedSlotID].item;
        }

        public void ExamineItem()
        {
            _cachedSubmenuItem = EventSystem.current.currentSelectedGameObject;
            inventoryState = InventoryState.MenuExamine;
            EventManager.ExamineObjectSelectedFromMenu(itemInSlot.examinableObject, itemInSlot.examineLengthModifier);
            UIManager.Instance.ExamineObjectChosen();
        }

        private void CancelExamine()
        {
            inventoryState = InventoryState.Submenu;
            EventSystem.current.SetSelectedGameObject(_cachedSubmenuItem);
        }

        #endregion
        
        
    }
}
