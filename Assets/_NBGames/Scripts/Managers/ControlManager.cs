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
        private const int _playerID = 0;

        public Player Player { get; private set; }


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
                Debug.LogWarning("ControlManager already exists! Destroying!");
                Destroy(this.gameObject);
            }

            Player = ReInput.players.GetPlayer(_playerID);
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
                    Player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    Player.controllers.maps.SetMapsEnabled(false, "Examine");
                    Player.controllers.maps.SetMapsEnabled(true, "Default");
                    Player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    Player.controllers.maps.SetMapsEnabled(false, "Disabled");
                    break;
                case BindingType.Inventory:
                    Player.controllers.maps.SetMapsEnabled(true, "Inventory");
                    Player.controllers.maps.SetMapsEnabled(false, "Examine");
                    Player.controllers.maps.SetMapsEnabled(false, "Default");
                    Player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    break;
                case BindingType.Examine:
                    Player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    Player.controllers.maps.SetMapsEnabled(true, "Examine");
                    Player.controllers.maps.SetMapsEnabled(false, "Default");
                    Player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    break;
                case BindingType.Padlock:
                    Player.controllers.maps.SetMapsEnabled(true, "Padlock");
                    Player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    Player.controllers.maps.SetMapsEnabled(false, "Default");
                    break;
                case BindingType.Disabled:
                    Player.controllers.maps.SetMapsEnabled(false, "Padlock");
                    Player.controllers.maps.SetMapsEnabled(false, "Inventory");
                    Player.controllers.maps.SetMapsEnabled(false, "Examine");
                    Player.controllers.maps.SetMapsEnabled(false, "Default");
                    Player.controllers.maps.SetMapsEnabled(true, "Disabled");
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
            var value = Player.GetAxisRaw(axis);
            if (Player.GetAxisCoordinateMode(axis) == AxisCoordinateMode.Absolute)
            {
                value *= Time.unscaledDeltaTime * multiplier;
            }

            return value;
        }

        public float GetAxisRelative(string axis, float multiplier)
        {
            var value = Player.GetAxis(axis);
            if (Player.GetAxisCoordinateMode(axis) == AxisCoordinateMode.Absolute)
            {
                value *= Time.unscaledDeltaTime * multiplier;
            }

            return value;
        }
    }
}
