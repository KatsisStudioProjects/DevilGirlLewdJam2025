using UnityEngine;
using UnityEngine.InputSystem;

namespace LewdJam2025.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        [SerializeField] GameObject ObjectToMove;
        [SerializeField] Rigidbody rb;
        [SerializeField] Animator _animator;
        [SerializeField] Transform _modelTransform;

        private Vector3 _moveDirection;
        private float _jumpForce;
        [SerializeField] private bool _isCrouching, _isAttacking, _onGround, _canDoubleJump;

        [SerializeField] float horMoveSpeed;
        [SerializeField] float vertMoveSpeed;

        [SerializeField] Transform _fireballSpawnLocation;
        [SerializeField] GameObject _fireballRef;

        [SerializeField] LayerMask _groundMask, _wallMask;

        public InputActionReference move;
        public InputActionReference attack;
        public InputActionReference jump;

        private CapsuleCollider _playerCollider;

        private bool _onConsole;

        void Awake()
        {
            Instance = this;

            if (ObjectToMove == null)
                ObjectToMove = gameObject;
        }

        private void Start()
        {
            _isCrouching = false;
            _fireballRef.SetActive(false);
            _playerCollider = GetComponent<CapsuleCollider>();
            _onConsole = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(!_onConsole)
            {
                _moveDirection = move.action.ReadValue<Vector2>();

                if (jump.action.WasPressedThisFrame() && CanStandCheck())
                {
                    Jump();
                }

                if (_isCrouching)
                {
                    _playerCollider.height = 1f;
                    _playerCollider.center = new Vector3(0f, 0.5f, 0f);
                }
                else
                {
                    _playerCollider.height = 2f;
                    _playerCollider.center = new Vector3(0f, 1f, 0f);
                }

                _animator.SetBool("IsAttacking", _isAttacking = attack.action.ReadValue<float>() > 0);
            }
            else
            {
                //On a console, change up code.
            }
            

        }

        private void FixedUpdate()
        {
            if (_onConsole) return;

            HandleMovement();
            _animator.SetBool("OnGround", _onGround = GroundCheck());
        }

        void HandleMovement()
        {
            _isCrouching = _moveDirection.y < -0.1f || !CanStandCheck();
            _animator.SetBool("IsCrouching", _isCrouching);

            float hSpeed = horMoveSpeed;

            hSpeed = _isCrouching ? horMoveSpeed / 1.75f : horMoveSpeed;

            if (_isAttacking)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, rb.linearVelocity.z);
                return;
            }

            if (rb.linearVelocity.x > 0.1 || rb.linearVelocity.x < -0.1) _animator.SetBool("IsRunning", true);
            else _animator.SetBool("IsRunning", false);

            if (rb.linearVelocity.x > 0.1) _modelTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            else if (rb.linearVelocity.x < -0.1) _modelTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            float hRamp = Mathf.Lerp(rb.linearVelocity.x, _moveDirection.x * hSpeed, Time.deltaTime);

            rb.linearVelocity = new Vector3(hRamp, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        void Jump()
        {
            if (!_onGround && !_canDoubleJump) return;

            if (!_onGround && _canDoubleJump)
                _canDoubleJump = false;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * vertMoveSpeed);

        }

        public void ShootAFireball()
        {
            if (!_onGround) return;

            GameObject fireball = Instantiate(_fireballRef, _fireballSpawnLocation.position, Quaternion.identity);

            fireball.GetComponent<ProjectileController>().SetupProjectile(_modelTransform.localRotation.y == 0 ? 1 : -1, true);

            fireball.SetActive(true);
        }


        bool CanStandCheck()
        {
            RaycastHit hitAny;

            Vector3 castOrigin = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

            if (Physics.Raycast(castOrigin, transform.TransformDirection(Vector3.up), out hitAny, 1.5f, _wallMask))
            {
                //Debug.DrawRay(castOrigin, transform.TransformDirection(Vector3.up) * hitAny.distance, Color.yellow);
                //Debug.Log("Cant Stand");
                return false;
            }
            else
            {
                //Debug.DrawRay(castOrigin, transform.TransformDirection(Vector3.up) * hitAny.distance, Color.blue);
                //Debug.Log("Can Stand");
                return true;
            }
        }


        bool GroundCheck()
        {
            RaycastHit hitGround;

            Vector3 castOrigin = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

            if (Physics.Raycast(castOrigin, transform.TransformDirection(Vector3.down), out hitGround, 1.5f, _groundMask))
            {
                //Debug.DrawRay(castOrigin, transform.TransformDirection(Vector3.down) * hitGround.distance, Color.green);
                //Debug.Log("On Ground");
                _canDoubleJump = true;
                return true;
            }
            else
            {
                //Debug.DrawRay(castOrigin, transform.TransformDirection(Vector3.down) * hitGround.distance, Color.red);
                //Debug.Log("Not On Ground");
                return false;
            }
                
        }
    }
}