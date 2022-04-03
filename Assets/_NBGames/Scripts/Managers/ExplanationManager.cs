using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _NBGames.Scripts.Managers
{
    public class ExplanationManager : MonoBehaviour
    {
        [Header("Images & Text")]
        [SerializeField] private GameObject[] _explanationImages;
        [SerializeField] private GameObject[] _explanationText;
        [SerializeField] private GameObject _instructionsText;

        [Header("Buttons")]
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _exitButton;

        private int _index;


        public void NextButton()
        {
            SoundManager.Instance.PlaySound(7);
            if (_index == _explanationImages.Length - 1)
            {
                EnableReplayExit();
            }
            else
            {
                DisableCurrentExplanation();
                _index++;
                EnableCurrentExplanation();

                if (!_backButton.IsInteractable())
                {
                    _backButton.interactable = true;
                }
            }
        }

        private void EnableReplayExit()
        {
            DisableCurrentExplanation();
            _instructionsText.SetActive(true);
            _nextButton.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _replayButton.gameObject.SetActive(true);
            _exitButton.gameObject.SetActive(true);
        }

        public void BackButton()
        {
            SoundManager.Instance.PlaySound(7);
            DisableCurrentExplanation();
            _index--;
            EnableCurrentExplanation();

            if (_index == 0)
            {
                _backButton.interactable = false;
            }
        }

        public void ReplayButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitButton()
        {
            Application.Quit();
        }

        private void DisableCurrentExplanation()
        {
            _explanationImages[_index].SetActive(false);
            _explanationText[_index].SetActive(false);
        }

        private void EnableCurrentExplanation()
        {
            _explanationImages[_index].SetActive(true);
            _explanationText[_index].SetActive(true);
        }
    }
}
