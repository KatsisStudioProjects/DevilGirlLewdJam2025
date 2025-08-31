using Unity.VisualScripting;
using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _lookatTarget;

        [SerializeField] private float _yOffset;

        [SerializeField] private Transform[] _pivots;

        int _currentPivotIndex;

        bool _sideScrollFollow = true;
        Vector3 _sideScrollPosition;
        Vector3 _currentTrackPosition;

        private void Start()
        {
            _sideScrollPosition = new Vector3(_target.position.x, _target.position.y + _yOffset, -10f);
        }

        // Update is called once per frame

        public void ChangePivot(int pivotIndex)
        {
            transform.position = _pivots[pivotIndex].position;
        }
    }
}