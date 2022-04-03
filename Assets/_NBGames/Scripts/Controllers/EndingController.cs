using UnityEngine;

namespace _NBGames.Scripts.Controllers
{
    public class EndingController : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _playerCamera;
        [SerializeField] private GameObject _endingCamera;
        [SerializeField] private Transform _cameraDoorPlacement;
        private Vector3 _newPlayerPosition;

        public void SetPlayerToEndingCamera()
        {
            _newPlayerPosition = new Vector3(_endingCamera.transform.position.x, _player.transform.position.y,
                _endingCamera.transform.position.z);

            _player.transform.position = _newPlayerPosition;
            _player.transform.rotation = _endingCamera.transform.rotation;
            
            _playerCamera.transform.rotation = _endingCamera.transform.rotation;
        }

        public void SetEndingCameraToDoor()
        {
            _endingCamera.transform.position = _cameraDoorPlacement.position;
            _endingCamera.transform.rotation = _cameraDoorPlacement.rotation;
        }
    }
}
