using _NBGames.Scripts.Interfaces;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _NBGames.Scripts.Inventory.Classes
{
    public class InteractableBehavior : MonoBehaviour, IInteractable
    {
        [SerializeField] private CrosshairType _crosshairType = CrosshairType.Interactable;

        public CrosshairType CrosshairType
        {
            get => _crosshairType;
            set => _crosshairType = value;
        }


        public virtual void Interact()
        {
            
        }
    }
}
