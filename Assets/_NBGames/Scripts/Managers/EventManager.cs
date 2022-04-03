using System;
using _NBGames.Scripts.InteractionBehaviors;
using _NBGames.Scripts.Interactions;
using _NBGames.Scripts.Inventory.Classes;
using _NBGames.Scripts.ItemUseBehaviors;
using UnityEngine;

namespace _NBGames.Scripts.Managers
{
    public class EventManager : MonoBehaviour
    {
        #region VARIABLES

        private static EventManager _instance;
        private static InteractableBehavior _interactableBehavior;

        #endregion
        
        
        #region METHODS

        #region PLAYER INTERACTION

        public static event Action<int> onChangeCrosshair;
        public static void ChangeCrossHair(InteractableBehavior objectLookedAt)
        {
            onChangeCrosshair?.Invoke((int) objectLookedAt.CrosshairType);
        }

        public static event Action onResetCrosshair;
        public static void ResetCrosshair()
        {
            onResetCrosshair?.Invoke();
        }

        public static event Action onToggleMouseLock;
        public static void ToggleMouseLock()
        {
            onToggleMouseLock?.Invoke();
        }

        public static event Action<PromptItemUseBehavior> onOpenInventoryFromItemInteraction;
        public static void OpenInventoryFromItemInteraction(PromptItemUseBehavior itemInteractedWith)
        {
            onOpenInventoryFromItemInteraction?.Invoke(itemInteractedWith);
        }

        public static event Action<EnvironmentInteraction, GameObject, float, Vector3, Quaternion> onExamineItemInEnvironment;
        public static void ExamineItemInEnvironment(EnvironmentInteraction interactionComponent, 
            GameObject itemToPickup, float examineLengthModifier, Vector3 cameraPosition, Quaternion cameraRotation)
        {
            onExamineItemInEnvironment?.Invoke(interactionComponent, itemToPickup, examineLengthModifier, 
                cameraPosition, cameraRotation);
        }

        public static event Action<float> onTogglePlayerRaycast;
        public static void TogglePlayerRaycast(float examineLengthModifier)
        {
            onTogglePlayerRaycast?.Invoke(examineLengthModifier);
        }

        #endregion
        
        #region INVENTORY

        public static event Action onToggleInventory;
        public static void ToggleInventory()
        {
            onToggleInventory?.Invoke();
        }

        public static event Action onCloseSubMenuForCombine;
        public static void CloseSubMenuForCombine()
        {
            onCloseSubMenuForCombine?.Invoke();
        }

        public static event Action<AddToInventoryBehavior> onItemPickedUp;
        public static void AddItemToInventory(AddToInventoryBehavior callingObject)
        {
            onItemPickedUp?.Invoke(callingObject);
        }
        

        public static event Action<GameObject, int> onChangeDefaultInventoryButton;
        public static void ChangeDefaultInventoryButton(GameObject button, int slotID)
        {
            onChangeDefaultInventoryButton?.Invoke(button, slotID);
        }

        public static event Action onCloseSelectionMenu;
        public static void CloseSelectionMenu()
        {
            onCloseSelectionMenu?.Invoke();
        }

        public static event Action<GameObject, float> onExamineObjectSelectedFromMenu;
        public static void ExamineObjectSelectedFromMenu(GameObject objectExamined, float examineLengthModifier)
        {
            onExamineObjectSelectedFromMenu?.Invoke(objectExamined, examineLengthModifier);
        }

        public static event Action onInventoryExamineCancel;
        public static void CancelExamineAndReturnToInventory()
        {
            onInventoryExamineCancel?.Invoke();
        }

        public static event Action onItemConfirmed;
        public static void ItemConfirmed()
        {
            onItemConfirmed?.Invoke();
        }

        #endregion


        #endregion


    }
}
