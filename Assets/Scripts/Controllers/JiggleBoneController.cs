using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class JiggleBoneController : MonoBehaviour
    {
        [SerializeField] private Transform _jiggleParent;

        [SerializeField] private Transform[] _jiggleBone;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            foreach (Transform bone in _jiggleBone) { bone.SetParent(_jiggleParent); }
        }
    }
}