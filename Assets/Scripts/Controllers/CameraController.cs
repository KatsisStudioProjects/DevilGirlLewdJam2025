using Unity.VisualScripting;
using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private float yOffset;

        // Update is called once per frame
        void Update()
        {
            //transform.LookAt(target);
            //transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y + 2f, -10f), Time.deltaTime * 5f);

            transform.position = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        }
    }
}