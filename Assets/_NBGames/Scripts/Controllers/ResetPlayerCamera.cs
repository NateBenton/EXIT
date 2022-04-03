using UnityEngine;

namespace _NBGames.Scripts.Controllers
{
    public class ResetPlayerCamera : MonoBehaviour
    {
        [SerializeField] private Vector3[] _newPositions;
        [SerializeField] private Quaternion[] _newRotations;
        
        public void ResetCamera(int index)
        {
            transform.localPosition = _newPositions[index];
            transform.localRotation = _newRotations[index];
        }
    }
}