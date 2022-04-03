using System;
using UnityEngine;

namespace _NBGames.Scripts.Utilities
{
    public class AudioFade : MonoBehaviour
    {
        [SerializeField] private float _fadeSpeed = 1f;
        private AudioSource _audioSource;
        private bool _isAudioSourceNull;
        
        private bool _fadeOut;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _isAudioSourceNull = _audioSource == null;

            if (_isAudioSourceNull)
            {
                Debug.LogError($"AudioSource is null on {gameObject.name}");
            }
        }

        private void Update()
        {
            if (!_fadeOut) return;
            if (_isAudioSourceNull) return;
            _audioSource.volume -= _fadeSpeed * Time.deltaTime;
        }

        public void BeginFade()
        {
            _fadeOut = true;
        }
    }
}