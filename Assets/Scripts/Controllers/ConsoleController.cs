using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        [SerializeField] List<DoorController> _doorControllers;

        public bool InZone => _minigameController.InZone;

        int _faceIndex = 0;
        float _timerMax = 10.9f;
        float _timer;
        bool _onConsole = false;
        bool _consoleOpen = false;

        public bool ConsoleOpen => _consoleOpen;

        private void Update()
        {
            if (_consoleOpen) return;

            if (!_onConsole)
                CheckPlayerInRange();
            else
            {
                _timer -= Time.deltaTime;
                _timerText.text = ((int)_timer).ToString();
            }
        }

        void SetFaceMaterial(int matIndex)
        {
            faceRenderer.material = Faces[matIndex];
        }

        public void AddToTimer()
        {
            _timer += 1.25f;
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

            if (_doorControllers != null)
            {
                Gizmos.color = Color.blue;
                foreach (DoorController con in _doorControllers)
                {
                    Gizmos.DrawLine(transform.position, con.transform.position);
                }
            }
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

        public IEnumerator EndConsoleMinigame()
        {
            if (_consoleOpen) yield return null;
            else
            {
                _faceIndex++;
                _consoleOpen = true;
                SetFaceMaterial(_faceIndex);
                _minigameController.gameObject.SetActive(false);

                yield return new WaitForSeconds(2f);

                _faceIndex++;
                SetFaceMaterial(_faceIndex);

                foreach (DoorController con in _doorControllers)
                {
                    con.Open();
                }
            }
        }

        public void AddDoorToList(DoorController door)
        {
            _doorControllers.Add(door);
        }

        public Vector3 GetAnchorPos()
        {
            return _cameraAnchor.position;
        }
    }
}