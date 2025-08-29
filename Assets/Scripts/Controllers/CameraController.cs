using Unity.VisualScripting;
using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform lookatTarget;

        [SerializeField] private float yOffset;

        bool _sideScrollFollow = true;
        Vector3 _sideScrollPosition;
        Vector3 _currentTrackPosition;

        private void Start()
        {
            _sideScrollPosition = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        }

        // Update is called once per frame
        void Update()
        {
            //transform.LookAt(lookatTarget);
            //transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y + 2f, -10f), Time.deltaTime * 5f);


            //Update sidescroll pos to follow player at all times
            _sideScrollPosition = new Vector3(target.position.x, target.position.y + yOffset, -10f);
            transform.position = _sideScrollPosition;

        }

        public void SetNewAnchor(Vector3 newAnchor)
        {
            if (newAnchor == null)
            {
                _sideScrollFollow = true; ;
            }
            else
            {
                _currentTrackPosition = newAnchor;
                _sideScrollFollow = false;
            }
        }
    }
}