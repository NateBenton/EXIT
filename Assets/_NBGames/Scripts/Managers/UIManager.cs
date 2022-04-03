using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.ItemUseBehaviors;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _NBGames.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] private _NBGames.Scripts.Inventory.Inventory _inventory = null;
        [SerializeField] private GameObject _inventoryObject = null;
        [SerializeField] private Image _crosshairImage = null;
        [SerializeField] private InventorySlotUI[] _inventoryUISlots = null;
        [SerializeField] private GameObject _selectionMenu = null;
        [SerializeField] private GameObject _firstSelectedSubmenuItem;
        [SerializeField] private CanvasGroup _inventorySlotCanvasGroup;
        [SerializeField] private float _subMenuXOffset = 120f;
        [SerializeField] private float _subMenuYOffset = 25f;
        [SerializeField] private TextMeshProUGUI _actionDescriptionText;
        [SerializeField] private TextMeshProUGUI _itemTitle;
        [SerializeField] private TextMeshProUGUI _itemDescription;
        [SerializeField] private GameObject _actionInstructions;

        private InventorySlotUI _slotSelected;
        private bool _inventoryIsOpen;

        #endregion
        
        #region PROPERTIES

        public static UIManager Instance { get; private set; }
        public GameObject FirstSelectedSubmenuItem => _firstSelectedSubmenuItem;

        #endregion

        #region METHODS

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);

                DialogueManager.instance.interruptActiveConversations = true;
            }
            else
            {
                Debug.LogWarning("UIManager already exists. Destroying!");
                Destroy(this.gameObject);
            }
        }

        public void Initialize()
        {
            _inventoryObject.SetActive(false);
        }

        private void OnEnable()
        {
            EventManager.onToggleInventory += ToggleInventory;
            EventManager.onCloseSelectionMenu += CloseSelectionMenu;
            EventManager.onInventoryExamineCancel += CancelExamine;
            EventManager.onOpenInventoryFromItemInteraction += ToggleInventoryUseItem;
            EventManager.onItemPickedUp += HideCrosshair;
            EventManager.onItemConfirmed += EnableCrosshair;
        }

        private void OnDisable()
        {
            EventManager.onToggleInventory -= ToggleInventory;
            EventManager.onCloseSelectionMenu -= CloseSelectionMenu;
            EventManager.onInventoryExamineCancel -= CancelExamine;
            EventManager.onOpenInventoryFromItemInteraction -= ToggleInventoryUseItem;
            EventManager.onItemPickedUp -= HideCrosshair;
            EventManager.onItemConfirmed -= EnableCrosshair;
        }
        
        private void EnableCrosshair()
        {
            _crosshairImage.gameObject.SetActive(true);
        }
        
        private void HideCrosshair(AddToInventoryBehavior obj)
        {
            _crosshairImage.gameObject.SetActive(false);
        }

        private void ToggleInventory()
        {
            _inventoryIsOpen = !_inventoryIsOpen;
            
            _inventoryObject.SetActive(_inventoryIsOpen);
            _actionInstructions.SetActive(_inventoryIsOpen);
            _crosshairImage.enabled = !_inventoryIsOpen;

            if (_inventoryIsOpen)
            {
                ToggleSlots();
                DrawSlotData();
            }
        }

        private void ToggleInventoryUseItem(PromptItemUseBehavior itemUsePrompt)
        {
            ToggleInventory();
        }

        private void ToggleSlots()
        {
            for (var i = 0; i < _inventoryUISlots.Length; i++)
            {
                if (_inventory.inventorySlots[i].slotEnabled)
                {
                    _inventoryUISlots[i].parentObject.SetActive(true);
                }
                else
                {
                    _inventoryUISlots[i].parentObject.SetActive(false);
                    _inventoryUISlots[i].icon.SetActive(false);
                }
            }
        }

        private void DrawSlotData()
        {
            for (var i = 0; i < _inventory.inventorySlots.Count; i++)
            {
                if (!_inventory.inventorySlots[i].slotEnabled || !_inventory.inventorySlots[i].item) continue;
                _inventoryUISlots[i].iconSprite.sprite = _inventory.inventorySlots[i].item.menuSprite;
                _inventoryUISlots[i].icon.SetActive(true);
            }
        }

        public void ClearItemSlot(int i)
        {
            _inventoryUISlots[i].iconSprite.sprite = null;
            _inventoryUISlots[i].icon.SetActive(false);
        }
        
        public void OpenSelectionMenu()
        {
            MoveSelectionMenu();
            
            _selectionMenu.SetActive(true);
            _inventorySlotCanvasGroup.blocksRaycasts = false;
            _inventorySlotCanvasGroup.interactable = false;
        }

        public void CloseSelectionMenu()
        {
            _actionDescriptionText.text = "";
            _selectionMenu.SetActive(false);
            _inventorySlotCanvasGroup.blocksRaycasts = true;
            _inventorySlotCanvasGroup.interactable = true;
        }

        private void MoveSelectionMenu()
        {
            var selectedButton = InventoryManager.instance.SelectedInventorySlot.transform.position;
            selectedButton.x += _subMenuXOffset;
            selectedButton.y -= _subMenuYOffset;

            _selectionMenu.transform.position = selectedButton;
        }

        public void UpdateActionDescriptionText(string text)
        {
            _actionDescriptionText.text = text;
        }

        public void UpdateItemTextInfo()
        {
            var item = InventoryManager.instance.itemInSlot;

            if (item)
            {
                _itemTitle.text = item.itemName;
                _itemDescription.text = item.description;
            }
            else
            {
                _itemTitle.text = "";
                _itemDescription.text = "";
            }
        }

        public void ExamineObjectChosen()
        {
            _selectionMenu.SetActive(false);
            _inventoryObject.SetActive(false);
            _actionDescriptionText.text = "rotate mouse = rotate model!! xD";
        }

        private void CancelExamine()
        {
            _selectionMenu.SetActive(true);
            _inventoryObject.SetActive(true);
        }

        public void ToggleCrosshair(bool value)
        {
            _crosshairImage.enabled = value;
        }

        public void ToggleCombineBackgroundOnFirstSlot(int slotID)
        {
            var activeState = _inventoryUISlots[slotID].combineBackground.activeInHierarchy;
            _inventoryUISlots[slotID].combineBackground.SetActive(!activeState);
        }

        public void RedrawAfterItemCombo(int id)
        {
            ToggleCombineBackgroundOnFirstSlot(id);
            DrawSlotData();
            UpdateItemTextInfo();
        }

        #endregion
    
    }
}
