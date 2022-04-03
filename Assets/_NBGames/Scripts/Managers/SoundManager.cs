using UnityEngine;

namespace _NBGames.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;
        private AudioSource _audioSource;
    
        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogWarning("SoundManager already exists. Destroying!");
                Destroy(this.gameObject);
            }

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogWarning($"AudioSource is null on {gameObject.name}");
            }
        }

        public void PlaySound(int index)
        {
            if (!_audioSource) return;
            _audioSource.PlayOneShot(_audioClips[index]);
        }
    }
}
