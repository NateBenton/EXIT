using UnityEngine;
using UnityEngine.Events;

namespace _NBGames.Scripts.Inventory.Classes
{
    public class InteractableEvent : InteractableBehavior
    {
        [SerializeField] private UnityEvent _eventsOnInteraction;

        [Tooltip("Destroys this component after interaction is triggered.")]
        [SerializeField] private bool _destroyAfterInteraction;

        public override void Interact()
        {
            _eventsOnInteraction?.Invoke();

            if (!_destroyAfterInteraction) return;
            Destroy(this);
        }
    }
}
