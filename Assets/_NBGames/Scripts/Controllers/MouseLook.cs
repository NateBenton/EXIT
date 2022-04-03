using _NBGames.Scripts.Managers;
using ECM.Components;
using Rewired;
using UnityEngine;

namespace _NBGames.Scripts.Controllers
{
    public class MouseLook : ECM.Components.MouseLook
    {
        [SerializeField] private float _absoluteMultiplier = 200f;
        [SerializeField] private float _mouseSensitivityX = 1f;
        [SerializeField] private float _mouseSensitivityY = 1f;

        public override void LookRotation(CharacterMovement movement, Transform cameraTransform)
        {
            var yaw = ControlManager.instance.GetAxisRelative("Look Horizontal", _absoluteMultiplier) *
                      lateralSensitivity;
            
            var pitch = ControlManager.instance.GetAxisRelative("Look Vertical", _absoluteMultiplier) *
                        verticalSensitivity;

            if (ReInput.controllers.GetLastActiveControllerType() == ControllerType.Mouse)
            {
                yaw *= _mouseSensitivityX;
                pitch *= _mouseSensitivityY;
            }

            var yawRotation = Quaternion.Euler(0.0f, yaw, 0.0f);
            var pitchRotation = Quaternion.Euler(-pitch, 0.0f, 0.0f);

            characterTargetRotation *= yawRotation;
            cameraTargetRotation *= pitchRotation;

            if (clampPitch)
                cameraTargetRotation = ClampPitch(cameraTargetRotation);

            if (smooth)
            {
                // On a rotating platform, append platform rotation to target rotation

                if (movement.platformUpdatesRotation && movement.isOnPlatform && movement.platformAngularVelocity != Vector3.zero)
                {
                    characterTargetRotation *=
                        Quaternion.Euler(movement.platformAngularVelocity * Mathf.Rad2Deg * Time.deltaTime);
                }

                movement.rotation = Quaternion.Slerp(movement.rotation, characterTargetRotation,
                    smoothTime * Time.deltaTime);

                cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, cameraTargetRotation,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                movement.rotation *= yawRotation;
                cameraTransform.localRotation *= pitchRotation;

                if (clampPitch)
                    cameraTransform.localRotation = ClampPitch(cameraTransform.localRotation);
            }

            UpdateCursorLock();
        }
    }
}
