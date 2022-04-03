using System.Collections.Generic;
using System.Linq;
using _NBGames.Scripts.Interfaces;
using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.Inventory.ScriptableObjects;
using _NBGames.Scripts.Managers;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _NBGames.Scripts.ItemUseBehaviors
{
    public class PromptItemUseBehavior : InteractableBehavior, IItemUsable
    {
        #if UNITY_EDITOR
        private ItemData[] itemsArray => AssetDatabase.FindAssets("t:ItemData", new []{"Assets/ItemData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>)
            .ToArray();
        #endif

        [ValueDropdown("itemsArray")]
        [SerializeField] private List<ItemData> _itemsRequired;

        [SerializeField] private string _messageCloseMenu;
        [SerializeField] private string _messageWrongItem;
        [SerializeField] private string _stillIncompleteMessage;
        [SerializeField] private float _messageWaitTime = 3f;
        
        [LabelText("Display Interaction After Complete?")]
        [SerializeField] private bool _displayMessageComplete;

        [ShowIf("_displayMessageComplete")]
        [LabelText("Message Post-Complete")]
        [SerializeField] private string _interactionMessagePostComplete;

        [SerializeField] private UnityEvent _eventsOnItemUsed;
        
        [Tooltip("Destroys this component after all item conditions have been met.")]
        [SerializeField] private bool _destroyAfterComplete;

        private ItemData[] _itemsUsed;

        private bool _requirementsMet;

        #region PROPERTIES

        protected float messageWaitTime
        {
            get => _messageWaitTime;
            set => _messageWaitTime = value;
        }

        public List<ItemData> itemsRequired => _itemsRequired;

        #endregion

        protected ItemData itemJustUsed { get; private set; }

        public override void Interact()
        {
            if (_requirementsMet)
            {
                if (!_displayMessageComplete) return;
                DialogueManager.ShowAlert(_interactionMessagePostComplete, _messageWaitTime);
            }
            else
            {
                SendItemsRequiredToInventoryManager();
                OpenInventory();
            }
        }
        
        public virtual void SendItemsRequiredToInventoryManager()
        {
            InventoryManager.instance.itemsToBeUsed.Clear();
            InventoryManager.instance.itemsToBeUsed.AddRange(_itemsRequired);
        }

        public virtual void OpenInventory()
        {
            EventManager.OpenInventoryFromItemInteraction(this);
        }
        
        public virtual void DisplayMessageCloseMenu()
        {
            DialogueManager.ShowAlert(_messageCloseMenu, _messageWaitTime);
        }
        
        public void DisplayMessageIncorrectItem()
        {
            DialogueManager.ShowAlert(_messageWrongItem, _messageWaitTime);
        }
        
        public virtual void DisplayStillIncompleteMessage()
        {
            DialogueManager.ShowAlert(_stillIncompleteMessage, _messageWaitTime);
        }
        
        public void RemoveItemFromItemsRequired(ItemData itemToRemove)
        {
            itemJustUsed = itemToRemove;
            
            for (var i = 0; i < _itemsRequired.Count; i++)
            {
                if (_itemsRequired[i] != itemToRemove) continue;
                _itemsRequired.RemoveAt(i);
                break;
            }
        }
        
        public virtual void OnUse()
        {
            if (_itemsRequired.Count != 0)
            {
                DisplayStillIncompleteMessage();
            }
            else
            {
                OnItemRequirementsMet();
            }
        }

        public virtual void OnItemRequirementsMet()
        {
            _requirementsMet = true;
            _eventsOnItemUsed?.Invoke();

            if (!_destroyAfterComplete) return;
            Destroy(this);
        }

        public virtual void DisplayPostInteractionMessage()
        {
            DialogueManager.ShowAlert(_interactionMessagePostComplete, _messageWaitTime);
        }
    }
}
