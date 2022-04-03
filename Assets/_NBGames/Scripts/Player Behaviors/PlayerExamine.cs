using _NBGames.Scripts.Interactions;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;

namespace _CTGames.Scripts.Player_Behaviors
{
    public class PlayerExamine : MonoBehaviour
    {
        [SerializeField] private GameObject _examineContainer;
        [SerializeField] private GameObject _examinedObject;
        [SerializeField] private float _rotationSpeed = 60f;
        [SerializeField] private float _absoluteMultiplier = 200f;
        [SerializeField] private GameObject _examineCamera;

        private readonly Quaternion _adjustedRotation = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        private bool _isExamining;
        private float _rotationX, _rotationY;
        private Quaternion _defaultRotation;
        private bool _examinedFromEnvironment;
        private EnvironmentInteraction _environmentComponent;

        private bool _mouseButtonDown;
        private float _examineLengthModifier;


        private void OnEnable()
        {
            //EventManager.onExamineObjectSelectedFromMenu += SpawnExaminedObject;
            EventManager.onExamineItemInEnvironment += SpawnExaminedObject;
        }

        private void OnDisable()
        {
            //EventManager.onExamineObjectSelectedFromMenu -= SpawnExaminedObject;
            EventManager.onExamineItemInEnvironment -= SpawnExaminedObject;
        }
        
        private void SpawnExaminedObject(GameObject examinedObject, float examineLengthModifier, 
            Vector3 cameraPosition, Quaternion cameraRotation)
        {
            _examineCamera.SetActive(true);
            _examineCamera.transform.position = cameraPosition;
            _examineCamera.transform.rotation = cameraRotation;
            
            // Functionality changed to support HDRP (more below)
            
            /*_examineCamera.transform.rotation = _adjustedRotation;
            _examineContainer.transform.rotation = _adjustedRotation;
            _examinedObject = Instantiate(examinedObject, _examineContainer.transform, false);
            _defaultRotation = _examinedObject.transform.rotation;*/
            
            ControlManager.instance.ChangeControlType(BindingType.Examine);
            _isExamining = true;
            UIManager.Instance.ToggleCrosshair(false);
            _examineLengthModifier = examineLengthModifier;

            EventManager.TogglePlayerRaycast(examineLengthModifier);
        }

        private void SpawnExaminedObject(EnvironmentInteraction interactionComponent, GameObject examinedObject,
            float examineLengthModifier, Vector3 cameraPosition, Quaternion cameraRotation)
        {
            _examinedFromEnvironment = true;
            _environmentComponent = interactionComponent;
            EventManager.ToggleMouseLock();

            SpawnExaminedObject(examinedObject, examineLengthModifier, cameraPosition, cameraRotation);

        }

        private void Update()
        {
            if (!_isExamining) return;

            // This code was originally written with Camera Stacking in mind.
            // However, as of time of writing, HDRP has no support for camera stacking
            // As a result, functionality is changed for the HDRP version of this project to use
            // fixed camera angles for item examining.
            
            /*if (ControlManager.instance.player.GetAxis("Examine Mouse Horizontal") != 0.0f && 
                ControlManager.instance.player.GetButton("Examine Mouse Click"))
            {
                RotationX(false);
            }
            
            else if (ControlManager.instance.player.GetAxis("Examine Horizontal") != 0.0f)
            {
                RotationX(true);
            }
            
            if (ControlManager.instance.player.GetAxis("Examine Mouse Vertical") != 0.0f && 
                ControlManager.instance.player.GetButton("Examine Mouse Click"))
            {
                RotationY(false);
            }
            
            else if (ControlManager.instance.player.GetAxis("Examine Vertical") != 0.0f)
            {
                RotationY(true);
            }

            if (ControlManager.instance.player.GetButton("Examine Reset Zoom"))
            {
                _examinedObject.transform.rotation = _defaultRotation;
            }*/

            if (!ControlManager.instance.Player.GetButtonDown("Examine Cancel")) return;
            _examineCamera.SetActive(false);
            UIManager.Instance.ToggleCrosshair(true);
                
            if (_examinedFromEnvironment)
            {
                EventManager.TogglePlayerRaycast(_examineLengthModifier);
                ControlManager.instance.ChangeControlType(BindingType.Normal);
                _environmentComponent.ResetItemPosition();
                    
                EventManager.ToggleMouseLock();
                    
                _examinedFromEnvironment = false;
            }
            else
            {
                ControlManager.instance.ChangeControlType(BindingType.Inventory);
                EventManager.CancelExamineAndReturnToInventory();
                EventManager.TogglePlayerRaycast(_examineLengthModifier);
            }
                
            //Destroy(_examinedObject);
            _isExamining = false;
        }

        public void ExitExamine()
        {
            _examineCamera.SetActive(false);
            UIManager.Instance.ToggleCrosshair(true);
            
            EventManager.TogglePlayerRaycast(_examineLengthModifier);
            ControlManager.instance.ChangeControlType(BindingType.Normal);
            _environmentComponent.ResetItemPosition();
                    
            EventManager.ToggleMouseLock();
                    
            _examinedFromEnvironment = false;
            _isExamining = false;
        }

        private void RotationX(bool isAbsolute)
        {
            if (isAbsolute)
            {
                _rotationX = ControlManager.instance.GetAxisRawRelative("Examine Horizontal", _absoluteMultiplier) *
                             _rotationSpeed * Mathf.Deg2Rad;
            }
            else
            {
                _rotationX = ControlManager.instance.Player.GetAxis("Examine Mouse Horizontal") *
                             _rotationSpeed * Mathf.Deg2Rad;
            }
            
            _examinedObject.transform.RotateAround(_examinedObject.transform.position, Vector3.up, -_rotationX);
        }
        
        private void RotationY(bool isAbsolute)
        {
            if (isAbsolute)
            {
                _rotationY = ControlManager.instance.GetAxisRawRelative("Examine Vertical", _absoluteMultiplier) *
                             _rotationSpeed * Mathf.Deg2Rad;
            }
            else
            {
                _rotationY = ControlManager.instance.Player.GetAxis("Examine Mouse Vertical") *
                             _rotationSpeed * Mathf.Deg2Rad;
            }

            _examinedObject.transform.RotateAround(_examinedObject.transform.position, Vector3.right, -_rotationY);
        }
    }
}
