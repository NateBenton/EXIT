using System;
using _NBGames.Scripts.Inventory.Classes;
using UnityEngine;

namespace _NBGames.Scripts.InteractionBehaviors
{
    public class DoorOpen : InteractableBehavior
    {
        [SerializeField] private AddToInventoryBehavior _itemInside;
        private Animator _animator;
        private AudioSource _audioSource;

        private bool _isOpen;
        private bool _canInteract = true;
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Close = Animator.StringToHash("Close");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();

            if (!_animator)
            {
                Debug.LogWarning($"Animator is null on {gameObject.name}");
            }

            if (!_audioSource)
            {
                Debug.LogWarning($"AudioSource is null on {gameObject.name}");
            }
        }
        
        public override void Interact()
        {
            if (!_animator) return;
            if (!_canInteract) return;

            if (_audioSource)
            {
                _audioSource.Play();
            }
            
            if (_isOpen)
            {
                ToggleInteraction();
                _animator.SetTrigger(Close);
                _isOpen = false;
            }
            else
            {
                ToggleInteraction();
                _animator.SetTrigger(Open);
                _isOpen = true;
            }
        }

        private void ToggleInteraction()
        {
            _canInteract = !_canInteract;
        }

        public void EnableHiddenItem()
        {
            ToggleInteraction();
            if (_itemInside == null) return;
            _itemInside.EnableCollider();
        }

        public void DisableHiddenItem()
        {
            ToggleInteraction();
            if (_itemInside == null) return;
            _itemInside.DisableCollider();
        }
    }
}
