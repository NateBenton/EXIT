using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.ItemUseBehaviors;
using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using ECM.Controllers;
using GamingIsLove.Footsteps;
using UnityEngine;

namespace _NBGames.Scripts.Controllers
{
    public class FirstPersonController : BaseFirstPersonController
    {
        [Header("Custom Values")] 
        [SerializeField] private float _crouchAnimationSpeed = 0.5f;
        [SerializeField] private float _crouchSpeedModifier = 2f;
        [SerializeField] private float _crouchTimeout = 1f;

        [Header("Footstepper Settings")] 
        [SerializeField] private GameObject _footStepperObject = null;
        private Footstepper _footStepper;
        private float _walkTimeout;
        
        [Header("MouseLooker Settings")]
        [SerializeField] private MouseLook _mouseLook = null;
        
        private bool _isInventoryOpen;

        public override void Awake()
        {
            base.Awake();

            if (_footStepperObject)
            {
                _footStepper = _footStepperObject.GetComponent<Footstepper>();
                _walkTimeout = _footStepper.walkTimeout;
            }
            else
            {
                Debug.LogWarning("Foot Stepper Object not found!");
            }
        }

        private void OnEnable()
        {
            EventManager.onToggleInventory += ToggleInventoryControls;
            EventManager.onOpenInventoryFromItemInteraction += ToggleInventoryControlsUseItem;
            EventManager.onToggleMouseLock += ToggleMouseLock;
            EventManager.onItemPickedUp += DisableCrouch;
        }

        private void OnDisable()
        {
            EventManager.onToggleInventory -= ToggleInventoryControls;
            EventManager.onOpenInventoryFromItemInteraction -= ToggleInventoryControlsUseItem;
            EventManager.onToggleMouseLock -= ToggleMouseLock;
            EventManager.onItemPickedUp -= DisableCrouch;
        }
        
        private void DisableCrouch(AddToInventoryBehavior obj)
        {
            crouch = false;
        }

        protected override void AnimateView()
        {
            // Scale camera pivot to simulate crouching

            var yScale = isCrouching ? Mathf.Clamp01(crouchingHeight / standingHeight) : 1.0f;

            cameraPivotTransform.localScale = Vector3.MoveTowards(cameraPivotTransform.localScale,
                new Vector3(1.0f, yScale, 1.0f), 5.0f * Time.deltaTime * _crouchAnimationSpeed);
        }
        
        protected override float GetTargetSpeed()
        {
            // Defaults to forward speed

            var targetSpeed = forwardSpeed;

            // Strafe

            if (moveDirection.x > 0.0f || moveDirection.x < 0.0f)
                targetSpeed = strafeSpeed;

            // Backwards

            if (moveDirection.z < 0.0f)
                targetSpeed = backwardSpeed;

            // Forward handled last as if strafing and moving forward at the same time,
            // forward speed should take precedence

            if (moveDirection.z > 0.0f)
                targetSpeed = forwardSpeed;
            
            // Crouch speed
            if (isCrouching)
            {
                targetSpeed = (forwardSpeed / _crouchSpeedModifier);
            }

            // Handle run speed modifier

            if (run && !isCrouching)
            {
                return targetSpeed * runSpeedMultiplier;
            }

            return targetSpeed;
        }
        
        protected override void HandleInput()
        {
            // Toggle pause / resume.
            // By default, will restore character's velocity on resume (eg: restoreVelocityOnResume = true)
            
            // Player input

            moveDirection = new Vector3
            {
                x = ControlManager.instance.Player.GetAxis("Move Horizontal"),
                y = 0.0f,
                z = ControlManager.instance.Player.GetAxis("Move Vertical")
            };

            if (ControlManager.instance.Player.GetButtonDown("Run Button"))
            {
                run = !run;
            }

            if (ControlManager.instance.Player.GetButtonDown("Crouch Button"))
            {
                crouch = !crouch;
                _footStepper.walkTimeout = crouch ? _crouchTimeout : _walkTimeout;
            }
            
            if (ControlManager.instance.Player.GetButtonDown("Menu Button"))
            {
                EventManager.ToggleInventory();
            }
        }

        private void ToggleInventoryControls()
        {
            _mouseLook.SetCursorLock(!_mouseLook.lockCursor);
            _isInventoryOpen = !_isInventoryOpen;

            ControlManager.instance.ChangeControlType(_isInventoryOpen ? BindingType.Inventory : BindingType.Normal);
        }

        private void ToggleInventoryControlsUseItem(PromptItemUseBehavior itemUsePrompt)
        {
            ToggleInventoryControls();
        }

        private void ToggleMouseLock()
        {
            _mouseLook.SetCursorLock(!_mouseLook.lockCursor);
        }
    }
}
