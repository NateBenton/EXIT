using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace _NBGames.Scripts.ItemUseBehaviors
{
    public class CornyFlakes : PromptItemUseBehavior
    {
        [SerializeField] private GameObject[] _itemDisplays;
        
        public override void OnUse()
        {
            base.OnUse();
            switch (itemJustUsed.itemName)
            {
                case "Corny Flakes":
                    foreach (var item in _itemDisplays)
                    {
                        if (item.name != "Corny Flakes") continue;
                        if (item.gameObject.activeInHierarchy) continue;
                        item.gameObject.SetActive(true);
                        break;
                    }
                    break;
                
                case "Up-N-Go":
                    foreach (var item in _itemDisplays)
                    {
                        if (item.name != "Up-N-Go") continue;
                        if (item.gameObject.activeInHierarchy) continue;
                        item.gameObject.SetActive(true);
                        break;
                    }
                    break;
            }
        }

        public override void OnItemRequirementsMet()
        {
            base.OnItemRequirementsMet();
            DialogueManager.ShowAlert("I completed puzzle xD", messageWaitTime);
        }
    }
}
