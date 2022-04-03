using System;
using _NBGames.Scripts.Inventory.Classes;
using UnityEngine;

namespace _NBGames.Scripts.InteractionBehaviors
{
    public class DrawerOpen : InteractableBehavior
    {
        [SerializeField] protected Vector3 _openPosition;
        [SerializeField] protected Vector3 _closedPosition;
        [SerializeField] protected float _openSpeed = 1f;
        [SerializeField] protected AddToInventoryBehavior _itemWithin;
        
        protected float _step;
        protected bool _isOperating, _isOpen;

        private AudioSource _audioSource;
        private bool _isAudioSourceNull;
        
        public Vector3 OpenPosition
        {
            get => _openPosition;
            set => _openPosition = value;
        }

        public Vector3 ClosedPosition
        {
            get => _closedPosition;
            set => _closedPosition = value;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _isAudioSourceNull = _audioSource == null;

            if (_isAudioSourceNull)
            {
                Debug.LogError($"AudioSource is null on {gameObject.name}");
            }
        }

        public override void Interact()
        {
            if (_isOperating) return;
            _isOperating = true;

            if (_isAudioSourceNull) return;
            _audioSource.Play();
        }

        protected virtual void Update()
        {
            if (!_isOperating) return;
            _step = _openSpeed * Time.deltaTime;

            if (_isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        protected virtual void Open()
        {
            transform.position = Vector3.MoveTowards(transform.position, _openPosition, _step);
            if (!(Vector3.Distance(transform.position, _openPosition) < _openSpeed * Time.deltaTime)) return;
            _isOpen = true;
            _isOperating = false;

            if (_itemWithin == null) return;
            _itemWithin.EnableCollider();
        }

        protected virtual void Close()
        {
            transform.position = Vector3.MoveTowards(transform.position, _closedPosition, _step);

            if (!(Vector3.Distance(transform.position, _closedPosition) < _openSpeed * Time.deltaTime)) return;
            _isOperating = false;
            _isOpen = false;

            if (_itemWithin == null) return;
            _itemWithin.DisableCollider();
        }
    }
}
