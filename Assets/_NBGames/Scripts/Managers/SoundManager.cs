using UnityEngine;

namespace _NBGames.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;
        private AudioSource _audioSource;
    
        public static SoundManager instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogError("SoundManager already exists. Destroying!");
                Destroy(this.gameObject);
            }

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError($"AudioSource is null on {gameObject.name}");
            }
        }

        public void PlaySound(int index)
        {
            if (_audioSource == null) return;
            _audioSource.PlayOneShot(_audioClips[index]);
        }
    }
}
