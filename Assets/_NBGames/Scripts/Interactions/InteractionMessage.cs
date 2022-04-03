using _NBGames.Scripts.Inventory.Classes;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace _NBGames.Scripts.Interactions
{
    public class InteractionMessage : InteractableBehavior
    {
        [SerializeField] private string _message = "";
        [SerializeField] private float _messageWaitTime = 3f;

        public override void Interact()
        {
            base.Interact();
            DialogueManager.ShowAlert(_message, _messageWaitTime);
        }
    }
}
