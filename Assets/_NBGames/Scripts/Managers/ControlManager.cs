using System;
using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.Utilities;
using Rewired;
using UnityEngine;

namespace _NBGames.Scripts.Managers
{
    public class ControlManager : MonoBehaviour
    {
        public static ControlManager instance { get; private set; }
        private int _playerID = 0;

        public Player player { get; private set; }


        private BindingType _bindingType;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogError("ControlManager already exists! Destroying!");
                Destroy(this.gameObject);
            }

            player = ReInput.players.GetPlayer(_playerID);
        }

        private void OnEnable()
        {
            EventManager.onItemPickedUp += EnableItemControls;
            EventManager.onItemConfirmed += EnableDefaultControls;
        }
        
        private void OnDisable()
        {
            EventManager.onItemPickedUp -= EnableItemControls;
            EventManager.onItemConfirmed -= EnableDefaultControls;
        }

        public void ChangeControlType(BindingType binding)
        {
            _bindingType = binding;

            switch (_bindingType)
            {
                case BindingType.Normal:
                    player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    player.controllers.maps.SetMapsEnabled(false, "Examine");
                    player.controllers.maps.SetMapsEnabled(true, "Default");
                    player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    player.controllers.maps.SetMapsEnabled(false, "Disabled");
                    break;
                case BindingType.Inventory:
                    player.controllers.maps.SetMapsEnabled(true, "Inventory");
                    player.controllers.maps.SetMapsEnabled(false, "Examine");
                    player.controllers.maps.SetMapsEnabled(false, "Default");
                    player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    break;
                case BindingType.Examine:
                    player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    player.controllers.maps.SetMapsEnabled(true, "Examine");
                    player.controllers.maps.SetMapsEnabled(false, "Default");
                    player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    break;
                case BindingType.Padlock:
                    player.controllers.maps.SetMapsEnabled(true, "Padlock");
                    player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    player.controllers.maps.SetMapsEnabled(false, "Default");
                    break;
                case BindingType.Disabled:
                    player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    player.controllers.maps.SetMapsEnabled(false, "Examine");
                    player.controllers.maps.SetMapsEnabled(false, "Default");
                    player.controllers.maps.SetMapsEnabled(true, "Disabled");
                    break;
            }
        }
        
        private void EnableDefaultControls()
        {
            ChangeControlType(BindingType.Normal);
        }
        
        private void EnableItemControls(AddToInventoryBehavior obj)
        {
            ChangeControlType(BindingType.Disabled);
        }

        public void EnablePadlockControls()
        {
            ChangeControlType(BindingType.Padlock);
        }

        public void DisablePadlockControls()
        {
            ChangeControlType(BindingType.Normal);
        }

        public void DisableControls()
        {
            ChangeControlType(BindingType.Disabled);
        }

        public void EnableControls()
        {
            ChangeControlType(BindingType.Normal);
        }

        public float GetAxisRawRelative(string axis, float multiplier)
        {
            var value = player.GetAxisRaw(axis);
            if (player.GetAxisCoordinateMode(axis) == AxisCoordinateMode.Absolute)
            {
                value *= Time.unscaledDeltaTime * multiplier;
            }

            return value;
        }

        public float GetAxisRelative(string axis, float multiplier)
        {
            var value = player.GetAxis(axis);
            if (player.GetAxisCoordinateMode(axis) == AxisCoordinateMode.Absolute)
            {
                value *= Time.unscaledDeltaTime * multiplier;
            }

            return value;
        }
    }
}
