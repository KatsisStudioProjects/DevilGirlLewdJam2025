using UnityEngine;

namespace LewdJam2025.Controllers
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] float lifespan;

        Rigidbody rb;

        float _dir;
        bool _friendly;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void SetupProjectile(float dir, bool isPlayerProjectile)
        {
            _dir = dir;
            _friendly = isPlayerProjectile;
        }

        private void Update()
        {
            lifespan -= Time.deltaTime;

            if (lifespan < 0) Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            rb.linearVelocity += new Vector3(_dir, 0f, 0f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Player" && !_friendly)
            {
                Destroy(gameObject);
            }

            if (collision.collider.tag == "Enemy" && _friendly)
            {
                Destroy(gameObject);
            }
        }
    }
}