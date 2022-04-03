using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace _NBGames.Scripts.Interactions
{
    public class EnvironmentInteraction : InteractableBehavior
    {
        [SerializeField] private GameObject _examineModel;
        [SerializeField] private float _examineLengthModifier = 0f;
        [SerializeField] private Vector3 _cameraPosition;
        [SerializeField] private Quaternion _cameraRotation;
        [SerializeField] private UnityEvent _eventsUponInteraction;
        [SerializeField] private UnityEvent _eventsUponExitInteraction;
        

        public void ResetItemPosition()
        {
            // Invoke events if there are any listeners.
            _eventsUponExitInteraction?.Invoke();
            
            // Functionality removed to support HDRP changes.
            //gameObject.SetActive(true);
        }

        public override void Interact()
        {
            base.Interact();
            
            // Functionality removed to support HDRP changes.
            //gameObject.SetActive(false);
            EventManager.ExamineItemInEnvironment(this, _examineModel, _examineLengthModifier, 
                _cameraPosition, _cameraRotation);
            
            // Invoke events if there are any listeners.
            _eventsUponInteraction?.Invoke();
        }
    }
}
