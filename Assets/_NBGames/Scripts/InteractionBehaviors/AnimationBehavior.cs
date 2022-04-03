using System;
using UnityEngine;

namespace _NBGames.Scripts.InteractionBehaviors
{
    public class AnimationBehavior : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animationTrigger;
        
        private int _animationHash;
        
        private void Awake()
        {
            if (_animationTrigger == null) return;
            _animationHash = Animator.StringToHash(_animationTrigger);
        }

        public void PlayAnimation()
        {
            if (!_animator) return;
            _animator.SetTrigger(_animationHash);
        }
    }
}
