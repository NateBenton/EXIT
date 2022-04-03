using System;
using _NBGames.Scripts.Inventory.Classes;
using UnityEngine;

namespace _NBGames.Scripts.InteractionBehaviors
{
    public class DoorOpen : InteractableBehavior
    {
        [SerializeField] private AddToInventoryBehavior _itemInside;
        private Animator _animator;
        private bool _isAnimatorNull, _isAudioSourceNull;
        private AudioSource _audioSource;

        private bool _isOpen;
        private bool _canInteract = true;
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Close = Animator.StringToHash("Close");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _isAnimatorNull = _animator == null;

            _audioSource = GetComponent<AudioSource>();
            _isAudioSourceNull = _audioSource == null;

            if (_isAnimatorNull)
            {
                Debug.LogError($"Animator is null on {gameObject.name}");
            }

            if (_isAudioSourceNull)
            {
                Debug.LogError($"AudioSource is null on {gameObject.name}");
            }
        }
        
        public override void Interact()
        {
            if (_isAnimatorNull) return;
            if (!_canInteract) return;

            if (!_isAudioSourceNull)
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
