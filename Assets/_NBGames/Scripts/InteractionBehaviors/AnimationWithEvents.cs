using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _NBGames.Scripts.InteractionBehaviors
{
    public class AnimationWithEvents : MonoBehaviour
    {
        [SerializeField] private UnityEvent _eventsAfterAnimation;
        [SerializeField] private string _triggerName;
        private int _triggerHash;

        [Tooltip("Time to wait before firing off events.")]
        [SerializeField] private float _timeBeforeEvents = 1.5f;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (!_animator)
            {
                Debug.LogWarning($"Animator is null on {gameObject.name}");
            }

            if (_triggerName == null)
            {
                Debug.LogWarning($"No trigger name set on {gameObject.name}");
            }
            else
            {
                _triggerHash = Animator.StringToHash(_triggerName);
            }
        }

        public void TriggerAnimation()
        {
            StartCoroutine(AnimationRoutine());
        }

        private IEnumerator AnimationRoutine()
        {
            if (!_animator) yield return null;
            _animator.SetTrigger(_triggerHash);

            yield return new WaitForSeconds(_timeBeforeEvents);
            _eventsAfterAnimation?.Invoke();
        }
    }
}
