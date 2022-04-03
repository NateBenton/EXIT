using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _NBGames.Scripts.Managers
{
    public class MessageManager : MonoBehaviour
    {
        [SerializeField] private GameObject _subtitlePanel;
        [SerializeField] private TextMeshProUGUI _subtitleText;
        [SerializeField] private Image _subtitlePanelImage;
        [SerializeField] private float _panelAlpha = 0.69f;
        [SerializeField] private float _textAlpha = 1f;
        [SerializeField] private float _fadeInSpeed = 0.1f;
        [SerializeField] private float _fadeOutSpeed = 0.1f;

        private bool _showingMessage;
        private Coroutine _coroutine;

        private static MessageManager Instance { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogWarning("MessageManager already exists! Destroying!");
                Destroy(this.gameObject);
            }
        }

        public void SetSubtitleText(string text, float timeToWait)
        {
            _subtitlePanel.SetActive(true);
            _subtitleText.text = text;

            if (!_showingMessage)
            {
                _showingMessage = true;
                _coroutine = StartCoroutine(FadeInSubtitles(timeToWait, _subtitleText, _subtitlePanelImage));
            }
            else
            {
                StopCoroutine(_coroutine);

                var textColor = _subtitleText.color;
                textColor.a = 0f;
                _subtitleText.color = textColor;

                var panelColor = _subtitlePanelImage.color;
                panelColor.a = 0f;
                _subtitlePanelImage.color = panelColor;
                
                _coroutine = StartCoroutine(FadeInSubtitles(timeToWait, _subtitleText, _subtitlePanelImage));
            }
        }

        private IEnumerator FadeInSubtitles(float timeToWait, TextMeshProUGUI text, Image image)
        {
            var textColor = text.color;
            var panelColor = image.color;
            
            while (text.color.a < _textAlpha)
            {
                textColor.a += Time.deltaTime * _fadeInSpeed;

                if (panelColor.a < _panelAlpha)
                {
                    panelColor.a += Time.deltaTime * _fadeInSpeed;
                }
                
                text.color = textColor;
                image.color = panelColor;
                yield return null;
            }
            
            _coroutine = StartCoroutine(FadeOutSubtitles(timeToWait, _subtitleText, _subtitlePanelImage));
        }

        private IEnumerator FadeOutSubtitles(float timeToWait, TextMeshProUGUI text, Image image)
        {
            yield return new WaitForSeconds(timeToWait);

            var textColor = text.color;
            var panelColor = image.color;
            
            while (text.color.a > Mathf.Epsilon)
            {
                panelColor.a -= Time.deltaTime * _fadeOutSpeed;
                textColor.a = panelColor.a;
                
                
                text.color = textColor;
                image.color = panelColor;
                yield return null;
            }

            _showingMessage = false;
            DisableSubtitlePanel();
        }

        private void DisableSubtitlePanel()
        {
            _subtitlePanel.SetActive(false);
        }
    }
}
