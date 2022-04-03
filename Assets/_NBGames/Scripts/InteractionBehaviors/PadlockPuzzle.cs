using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace _NBGames.Scripts.InteractionBehaviors
{
    public class PadlockPuzzle : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private PadlockWheel[] _padlockWheels;
        [SerializeField] private Light[] _wheelLights;
        [SerializeField] private int[] _combination;
        
        [Header("Misc")]
        [SerializeField] private GameObject _unlockedDoor;
        [SerializeField] private float _wheelRotationPerMovement = 36f;
        [SerializeField] private Animator _animator;

        [Header("Events")]
        [SerializeField] private UnityEvent _eventsOnCompletion;
        
        

        public PadlockWheel[] PadlockWheels => _padlockWheels;
        public int[] Combination => _combination;
        private List<int> _enteredCombo = new List<int>();
        public Light[] WheelLights => _wheelLights;
        
        private int _currentWheelIndex = 0;
        private int _maxWheelIndex;
        private GameObject _currentWheel;
        private float _targetX;

        private bool _isAnimatorNull, _isUnlocked;
        private static readonly int Fail = Animator.StringToHash("Fail");
        private static readonly int Unlock = Animator.StringToHash("Unlock");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _isAnimatorNull = _animator == null;

            if (_isAnimatorNull)
            {
                Debug.LogWarning($"Animator is null on {gameObject.name}");
            }

            if (_combination.Length != 0)
            {
                foreach (var wheel in _padlockWheels)
                {
                    _enteredCombo.Add(wheel.StartingNumber);
                }
            }
            else
            {
                Debug.LogWarning($"Combination was not set on {gameObject.name}");
            }
        }

        private void Start()
        {
            _maxWheelIndex = _wheelLights.Length;
        }

        private void Update()
        {
            ProcessInput();
        }
        
        private void ProcessInput()
        {
            if (ControlManager.instance.player.GetButtonDown("Padlock Right"))
            {
                DisableCurrentWheelLight();
                IncreaseWheelIndex();
                EnableCurrentWheelLight();
            }

            if (ControlManager.instance.player.GetButtonDown("Padlock Left"))
            {
                DisableCurrentWheelLight();
                DecreaseWheelIndex();
                EnableCurrentWheelLight();
            }

            if (ControlManager.instance.player.GetButtonDown("Padlock Up"))
            {
                ChangeWheelRotation(true);
            }
            
            if (ControlManager.instance.player.GetButtonDown("Padlock Down"))
            {
                ChangeWheelRotation(false);
            }

            if (ControlManager.instance.player.GetButtonDown("Padlock Confirm"))
            {
                if (_isUnlocked) return;
                if (_combination.SequenceEqual(_enteredCombo))
                {
                    StartCoroutine(UnlockPad());
                    SoundManager.instance.PlaySound(5);
                    _isUnlocked = true;
                }
                else
                {
                    _animator.SetTrigger(Fail);
                    SoundManager.instance.PlaySound(4);
                }
            }
        }

        private IEnumerator UnlockPad()
        {
            DisableCurrentWheelLight();
            _animator.SetTrigger(Unlock);
            yield return new WaitForSeconds(1.5f);
            _unlockedDoor.SetActive(true);
            _eventsOnCompletion?.Invoke();
        }

        private void ChangeWheelRotation(bool isTurningUp)
        {
            _currentWheel = _padlockWheels[_currentWheelIndex].gameObject;
            SoundManager.instance.PlaySound(6);
            
            if (isTurningUp)
            {
                _targetX = (_currentWheel.transform.rotation.x - _wheelRotationPerMovement);

                if (_enteredCombo[_currentWheelIndex] == 9)
                {
                    _enteredCombo[_currentWheelIndex] = 0;
                }
                else
                {
                    _enteredCombo[_currentWheelIndex]++;
                }
            }
            else
            {
                _targetX = (_currentWheel.transform.rotation.x + _wheelRotationPerMovement);
                
                if (_enteredCombo[_currentWheelIndex] == 0)
                {
                    _enteredCombo[_currentWheelIndex] = 9;
                }
                else
                {
                    _enteredCombo[_currentWheelIndex]--;
                }
            }

            _currentWheel.transform.Rotate(_targetX, 0f, 0f);
        }

        private void IncreaseWheelIndex()
        {
            if ((_currentWheelIndex + 1) >= _maxWheelIndex)
            {
                ResetWheelIndex();
            }
            else
            {
                _currentWheelIndex++;
            }
        }

        private void DecreaseWheelIndex()
        {
            if ((_currentWheelIndex - 1) < 0)
            {
                _currentWheelIndex = (_maxWheelIndex - 1);
            }
            else
            {
                _currentWheelIndex--;
            }
        }

        public void EnableCurrentWheelLight()
        {
            _wheelLights[_currentWheelIndex].enabled = true;
            ControlManager.instance.ChangeControlType(BindingType.Padlock);
        }

        public void DisableCurrentWheelLight()
        {
            _wheelLights[_currentWheelIndex].enabled = false;
        }

        public void ResetWheelIndex()
        {
            // Reset the wheel index
            _currentWheelIndex = 0;
        }
    }
}
