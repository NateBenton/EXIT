using System;
using System.Linq;
using _NBGames.Scripts.Inventory.ScriptableObjects;
using _NBGames.Scripts.Managers;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace _NBGames.Scripts.Inventory.Classes
{
    public class AddToInventoryBehavior : InteractableBehavior
    {
        #region VARIABLES

        [SerializeField] private Vector3 _positionInView;
        [SerializeField] private Quaternion _rotationInView;

        private bool _isPickedUp, _isColliderNull;
        private Collider _collider;
        public Vector3 PositionInView => _positionInView;
        public Quaternion RotationInView => _rotationInView;

        #if UNITY_EDITOR
        private ItemData[] itemsArray => AssetDatabase.FindAssets("t:ItemData", new []{"Assets/ItemData"})
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>)
            .ToArray();
        

        private ValueDropdownList<ItemData> GetItems()
        {
            var dropdown = new ValueDropdownList<ItemData>();

            itemsArray.ForEach(item => dropdown.Add($"{item.itemName} ({item.name})", item));
            return dropdown;
        }
        #endif
        
        [Required("Item Data is missing!", InfoMessageType.Error)]
        [ValueDropdown("GetItems")]
        [SerializeField] private ItemData _itemData = null;

        #endregion
        
        #region PROPERTIES

        public ItemData itemData => _itemData;

        #endregion

        #region METHODS

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _isColliderNull = _collider == null;
        }

        public override void Interact()
        {
            AddItemToInventory();
        }

        private void AddItemToInventory()
        {
            DialogueManager.instance.ShowAlert($"You got the {itemData.itemName}", Mathf.Infinity);
            DisableCollider();
            EventManager.AddItemToInventory(this);
            _isPickedUp = true;
            SoundManager.Instance.PlaySound(8);
        }

        private void Update()
        {
            if (!_isPickedUp) return;

            if (!ControlManager.instance.Player.GetButtonDown("ConfirmItem")) return;
            EventManager.ItemConfirmed();
            DialogueManager.instance.HideAlert();
            Destroy(this.gameObject);
        }

        public void DisableCollider()
        {
            if (_isColliderNull) return;
            _collider.enabled = false;
        }

        public void EnableCollider()
        {
            if (_isColliderNull) return;
            _collider.enabled = true;
        }

        #endregion
    }
}
