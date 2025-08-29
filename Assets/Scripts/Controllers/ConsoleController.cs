using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class ConsoleController : MonoBehaviour
    {
        [SerializeField] Material[] Faces;
        [SerializeField] Renderer faceRenderer;

        [SerializeField] Transform _playerAnchor;
        [SerializeField] Transform _cameraAnchor;

        [SerializeField] TMPro.TextMeshPro _hackText;
        [SerializeField] TMPro.TextMeshPro _timerText;

        [SerializeField] float _detectionRadius;
        [SerializeField] LayerMask _playerMask;
        [SerializeField] MinigameController _minigameController;

        public bool InZone => _minigameController.InZone;

        int _faceIndex = 0;
        float _timerMax = 10.9f;
        float _timer;
        bool _onConsole = false;

        private void Update()
        {
            if(!_onConsole)
                CheckPlayerInRange();
            else
            {
                _timer -= Time.deltaTime;
                _timerText.text = ((int)_timer).ToString();
            }

            //if (_timeDelay <= 0)
            //{
            //    _timeDelay = 3f;
            //    SetFaceMaterial(_faceIndex);
            //    _faceIndex++;
            //    if (_faceIndex >= Faces.Length)
            //        _faceIndex = 0;
            //}

            //_timeDelay -= Time.deltaTime;
        }

        void SetFaceMaterial(int matIndex)
        {
            faceRenderer.material = Faces[matIndex];
        }

        void CheckPlayerInRange()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _playerMask);

            if (hits.Length > 0)
            {
                //Player in range, display hack text
                GameManager.Instance.AssignInRangeConsole(this, true);
                _hackText.gameObject.SetActive(true);
            }
            else
            {
                GameManager.Instance.AssignInRangeConsole(this, false);
                _hackText.gameObject.SetActive(false);
                _onConsole = false;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }

        public void UsePanel(Transform player, Transform camera)
        {
            _onConsole = true;
            player.position = _playerAnchor.position;

            _faceIndex++;
            if (_faceIndex >= Faces.Length)
                _faceIndex = 0;

            SetFaceMaterial(_faceIndex);

            _timer = _timerMax;
            _timerText.text = ((int)_timer).ToString();
            _minigameController.gameObject.SetActive(true);

            _hackText.gameObject.SetActive(false);
        }

        public Vector3 GetAnchorPos()
        {
            return _cameraAnchor.position;
        }
    }
}