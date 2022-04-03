using _NBGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _NBGames.Scripts.Inventory
{
    public class InventoryButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler,
        ISubmitHandler, IPointerClickHandler
    {
        [SerializeField] private Button _button = null;
        [SerializeField] private GameObject _slotSelectedBackground = null;
        [SerializeField] private int slotID = 0;

        public void OnSelect(BaseEventData eventData)
        {
            _slotSelectedBackground.SetActive(true);
            EventManager.ChangeDefaultInventoryButton(this.gameObject, slotID);
            
            UIManager.instance.UpdateItemTextInfo();
            
            SoundManager.instance.PlaySound(7);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (InventoryManager.instance.isSelectionMenuOpen) return;
            _slotSelectedBackground.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _button.Select();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            InventoryManager.instance.SelectItemButton();
            SoundManager.instance.PlaySound(7);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                InventoryManager.instance.SelectItemButton();
                SoundManager.instance.PlaySound(7);
            }
        }
    }
}
