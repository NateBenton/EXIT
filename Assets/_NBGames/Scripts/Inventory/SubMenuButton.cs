using System;
using _NBGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _NBGames.Scripts.Inventory
{
    public class SubMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler,
        ISubmitHandler, IPointerClickHandler
    {
        [SerializeField] private Button _button = null;
        [SerializeField] private GameObject _cursorObject = null;
        [SerializeField] private string _actionName;

        private enum ActionType
        {
            Use,
            Equip,
            Examine,
            Combine,
            Discard
        }

        [SerializeField] private ActionType _actionType;

        private void OnEnable()
        {
            EventManager.onCloseSelectionMenu += UnhighlightButton;
            EventManager.onCloseSubMenuForCombine += UnhighlightButton;
        }

        private void OnDisable()
        {
            EventManager.onCloseSelectionMenu -= UnhighlightButton;
            EventManager.onCloseSubMenuForCombine -= UnhighlightButton;
        }

        public void OnSelect(BaseEventData eventData)
        {
            SelectButton();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            UnhighlightButton();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SelectButton();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            ButtonAction();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ButtonAction();
        }

        private void SelectButton()
        {
            _button.Select();
            UIManager.instance.UpdateActionDescriptionText(_actionName);
            _cursorObject.SetActive(true);
        }

        private void UnhighlightButton()
        {
            _cursorObject.SetActive(false);
        }

        private void ButtonAction()
        {
            switch (_actionType)
            {
                case ActionType.Use:
                    InventoryManager.instance.UseItem();
                    break;
                case ActionType.Combine:
                    InventoryManager.instance.SelectFirstItemToCombine();
                    break;
                case ActionType.Examine:
                    InventoryManager.instance.ExamineItem();
                    break;
                case ActionType.Equip:
                    break;
                case ActionType.Discard:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
