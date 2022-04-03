using _NBGames.Scripts.Interfaces;

namespace _NBGames.Scripts.Inventory.Classes
{
    public class ExaminableBehavior : InteractableBehavior, IExaminable
    {
        public override void Interact()
        {
            Examine();
        }

        public virtual void Examine()
        {
            //Debug.Log(this.gameObject + " was examined.");
        }
    }
}
