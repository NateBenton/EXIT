using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Utilities
{
    public class PlaySound : MonoBehaviour
    {
        public void PlaySoundEffect(int index)
        {
            SoundManager.instance.PlaySound(index);
        }
    }
}
