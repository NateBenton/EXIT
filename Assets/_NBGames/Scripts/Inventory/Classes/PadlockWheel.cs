using UnityEngine;

namespace _NBGames.Scripts.Inventory.Classes
{
    public class PadlockWheel : MonoBehaviour
    {
        [SerializeField] private int _startingNumber = 0;

        public int StartingNumber
        {
            get => _startingNumber;
            set => _startingNumber = value;
        }
    }
}
