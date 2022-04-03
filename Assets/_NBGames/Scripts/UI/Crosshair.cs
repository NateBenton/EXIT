using _NBGames.Scripts.Managers;
using _NBGames.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _NBGames.Scripts.UI
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private Image _crosshairImage = null;
        [SerializeField] private Sprite[] _crosshairSprites = null;

        [SerializeField] private CrosshairType _crosshairType;

        private void OnEnable()
        {
            EventManager.onChangeCrosshair += UpdateCrossHair;
            EventManager.onResetCrosshair += ResetCrosshair;
        }

        private void OnDisable()
        {
            EventManager.onChangeCrosshair -= UpdateCrossHair;
            EventManager.onResetCrosshair -= ResetCrosshair;
        }

        private void UpdateCrossHair(int crosshairToChangeTo)
        {
            _crosshairImage.sprite = _crosshairSprites[crosshairToChangeTo];
            _crosshairType = (CrosshairType) crosshairToChangeTo;
        }

        private void ResetCrosshair()
        {
            _crosshairImage.sprite = _crosshairSprites[0];
            _crosshairType = CrosshairType.Main;
        }
    }
}