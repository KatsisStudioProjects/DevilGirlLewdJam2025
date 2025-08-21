using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class ConsoleController : MonoBehaviour, IInteractable
    {
        [SerializeField] Material[] Faces;
        [SerializeField] Renderer faceRenderer;

        [SerializeField] Transform PlayerAnchor;

        [SerializeField] TMPro.TextMeshPro textMeshPro;

        int faceIndex = 0;
        float timeDelay = 3f;

        private void Update()
        {
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