using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class MinigameController : MonoBehaviour
    {
        [SerializeField] Transform marker;
        [SerializeField] AnimationCurve speedCurve;
        [Range(0.25f, 1.5f)]
        [SerializeField] float markerSpeed;

        float _updateMarkerPos = 0f;
        float _currentTime = 0f;

        public bool InZone => _updateMarkerPos <= 30f || _updateMarkerPos >= 330f;

        private void Update()
        {
            _updateMarkerPos = speedCurve.Evaluate(_currentTime += (Time.deltaTime*markerSpeed)) * 360;
            marker.eulerAngles = new Vector3(0f, 180f, _updateMarkerPos);
        }
    }
}