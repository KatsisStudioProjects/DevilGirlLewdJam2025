using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class ConsoleController : MonoBehaviour, IInteractable
    {
        [SerializeField] Material[] Faces;
        [SerializeField] Renderer faceRenderer;

        [SerializeField] Transform PlayerAnchor;

        [SerializeField] TMPro.TextMeshPro textMeshPro;

        [SerializeField] float _detectionRadius;
        [SerializeField] LayerMask _playerMask;

        int faceIndex = 0;
        float timeDelay = 3f;

        private void Update()
        {
            CheckPlayerInRange();

            if(timeDelay <= 0)
            {
                timeDelay = 3f;
                SetFaceMaterial(faceIndex);
                faceIndex++;
                if (faceIndex >= Faces.Length)
                    faceIndex = 0;
            }

            timeDelay -= Time.deltaTime;
        }

        void SetFaceMaterial(int matIndex)
        {
            faceRenderer.material = Faces[matIndex];
        }

        void CheckPlayerInRange()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, _playerMask);

            if(hits.Length > 0)
            {
                //Player in range, display hack text
                GameManager.Instance.AssignInRangeConsole(this, true);
                textMeshPro.gameObject.SetActive(true);
            }
            else
            {
                GameManager.Instance.AssignInRangeConsole(this, false);
                textMeshPro.gameObject.SetActive(false);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }

        void UsePanel()
        {
            //PlayerController.Instance.transform.position = PlayerAnchor.position;

            //textMeshPro.gameObject.SetActive(!textMeshPro.gameObject.activeSelf);
        }

        public void Interact(GameObject interacter)
        {
            if (interacter.GetComponent<PlayerController>() == null) return;
        }
    }
}