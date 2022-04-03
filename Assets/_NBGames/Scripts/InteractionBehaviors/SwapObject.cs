using UnityEngine;

namespace _NBGames.Scripts.InteractionBehaviors
{
    public class SwapObject : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToEnable;

        public void SwapObjects()
        {
            _objectToEnable.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
