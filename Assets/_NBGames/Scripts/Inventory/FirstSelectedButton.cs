using _NBGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _NBGames.Scripts.Inventory
{
    public class FirstSelectedButton : MonoBehaviour
    {
        [SerializeField] private Button _firstSelectedButton;
        
        // Start is called before the first frame update
        void Start()
        {
            _firstSelectedButton.Select();
            UIManager.instance.Initialize();

            Destroy(this.gameObject);
        }
    }
}
