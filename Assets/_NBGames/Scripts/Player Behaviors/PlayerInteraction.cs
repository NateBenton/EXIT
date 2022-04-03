using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.Managers;
using UnityEngine;

namespace _NBGames.Scripts.Player_Behaviors
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Transform _itemHolder;
        [SerializeField] private Camera _camera = null;
        [SerializeField] private LayerMask _layerMaskToHit = new LayerMask();
        [SerializeField] private LayerMask _examineMask = new LayerMask();
        [SerializeField] private float _interactionDistance = 100f;
        [SerializeField] private float _examineInteractionDistance = 35f;

        private Ray _ray;
        private RaycastHit _hit;
        private InteractableBehavior _interactableBehavior;
        private GameObject _objectLookedAt, _lastObjectLookedAt;
        private bool _canInteractWithObject;
        private bool _playerExamining;
        private float _examineLengthModifier;

        private void OnEnable()
        {
            EventManager.onTogglePlayerRaycast += ToggleExamine;
            EventManager.onItemPickedUp += ReparentItem;
        }
        
        private void OnDisable()
        {
            EventManager.onTogglePlayerRaycast -= ToggleExamine;
            EventManager.onItemPickedUp -= ReparentItem;
        }

        private void ReparentItem(AddToInventoryBehavior callingObject)
        {
            callingObject.transform.parent = _itemHolder;
            ChangeViewItemPosition(callingObject.transform, callingObject.PositionInView, callingObject.RotationInView);
        }

        private static void ChangeViewItemPosition(Transform itemTransform, Vector3 newPosition, Quaternion newRotation)
        {
            itemTransform.localPosition = newPosition;
            itemTransform.localRotation = newRotation;
        }
        
        private void ToggleExamine(float examineLengthModifier)
        {
            _playerExamining = !_playerExamining;
            _lastObjectLookedAt = null;
            _interactableBehavior = null;
            _objectLookedAt = null;
            _examineLengthModifier = examineLengthModifier;
        }

        // Update is called once per frame
        void Update()
        {
            FindInteractableObjects();
            InteractWithObject();
        }

        private void FindInteractableObjects()
        {
            if (!_playerExamining)
            {
                _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

                if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, _interactionDistance, _layerMaskToHit))
                {
                    _objectLookedAt = _hit.collider.gameObject;

                    if (_objectLookedAt != _lastObjectLookedAt)
                    {
                        _lastObjectLookedAt = _objectLookedAt;
                        _interactableBehavior = _objectLookedAt.GetComponent<InteractableBehavior>();
                    }

                    if (_interactableBehavior)
                    {
                        _canInteractWithObject = true;
                        EventManager.ChangeCrossHair(_interactableBehavior);
                    }
                
                }
                else
                {
                    _canInteractWithObject = false;
                    EventManager.ResetCrosshair();
                }
            
                Debug.DrawRay(_ray.origin, _ray.direction * _interactionDistance, Color.yellow);
            }
            else
            {
                _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

                if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, _examineInteractionDistance - _examineLengthModifier, _examineMask))
                {
                    _objectLookedAt = _hit.collider.gameObject;

                    if (_objectLookedAt != _lastObjectLookedAt)
                    {
                        _lastObjectLookedAt = _objectLookedAt;
                        _interactableBehavior = _objectLookedAt.GetComponent<InteractableBehavior>();
                    }

                    if (_interactableBehavior)
                    {
                        _canInteractWithObject = true;
                    }
                }
                else
                {
                    _canInteractWithObject = false;
                }
            
                Debug.DrawRay(_ray.origin, _ray.direction * (_examineInteractionDistance - _examineLengthModifier), Color.yellow);
            }
        }

        private void InteractWithObject()
        {
            if (!_canInteractWithObject) return;

            if (ControlManager.instance.Player.GetButtonDown("Action Button"))
            {
                if (_interactableBehavior)
                {
                    _interactableBehavior.Interact();
                }
            }
        }
    }
}
