using UnityEngine;
using UnityEngine.InputSystem;

namespace LewdJam2025.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        #region Properties

        public static PlayerController Instance;

        [SerializeField] GameObject ObjectToMove;
        [SerializeField] Rigidbody rb;
        [SerializeField] Animator _animator;
        [SerializeField] Transform _modelTransform;

        private Vector3 _moveDirection;
        private float _jumpForce;
        [SerializeField] private bool _isCrouching, _isAttacking, _onGround, _canDoubleJump;

        [SerializeField] float _horMoveSpeed;
        [SerializeField] float _vertMoveSpeed;

        [SerializeField] Transform _fireballSpawnLocation;
        [SerializeField] GameObject _fireballRef;

        [SerializeField] LayerMask _groundMask, _wallMask;

        public InputActionReference MoveAction;
        public InputActionReference AttackAction;
        public InputActionReference JumpAction;

        private CapsuleCollider _playerCollider;
        //Probably should change to _canMove
        private bool _canMove;

        #endregion
        void Awake()
        {
            // No need to have singleton :) Perhaps using OnTriggerEnter might be better! 
            Instance = this;

            if (ObjectToMove == null)
                ObjectToMove = gameObject;
        }

        private void Start()
        {
            _isCrouching = false;
            _fireballRef.SetActive(false);
            _playerCollider = GetComponent<CapsuleCollider>();
            _canMove = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(!_canMove)
            {
                _moveDirection = MoveAction.action.ReadValue<Vector2>();
 
                if (JumpAction.action.WasPressedThisFrame() && CanStandCheck()) Jump();

                HandleCrouching();

                _animator.SetBool("IsAttacking", _isAttacking = AttackAction.action.ReadValue<float>() > 0);
            }
            else
            {
                //On a console, change up code.
            }
            
        }

        private void FixedUpdate()
        {
            HandleMovement();
            _animator.SetBool("OnGround", _onGround = GroundCheck());
        }

        #region Movement
        void HandleMovement()
        {
            if(!_canMove) return;

            _isCrouching = _moveDirection.y < -0.1f || !CanStandCheck();
            _animator.SetBool("IsCrouching", _isCrouching);

            // If crouching, reduce horizontal speed
            float hSpeed = _isCrouching ? _horMoveSpeed * 0.75f : _horMoveSpeed;

            // Don't move if attacking
            if (_isAttacking)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, rb.linearVelocity.z);
                return;
            }

            // Handle horizontal movement
            if (rb.linearVelocity.x > 0.1 || rb.linearVelocity.x < -0.1) _animator.SetBool("IsRunning", true);
            else _animator.SetBool("IsRunning", false);

            // Rotate the model based on movement direction
            if (rb.linearVelocity.x > 0.1) _modelTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            else if (rb.linearVelocity.x < -0.1) _modelTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            // Ramping horizontal movement
            float hRamp = Mathf.Lerp(rb.linearVelocity.x, _moveDirection.x * hSpeed, Time.deltaTime);
            // Apply horizontal movement
            rb.linearVelocity = new Vector3(hRamp, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        void HandleCrouching()
        {
            if (!_canMove) return;

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
        }
        void Jump()
        {
            if (!_onGround && !_canDoubleJump) return;

            if (!_onGround && _canDoubleJump) _canDoubleJump = false;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * _vertMoveSpeed);
        }

        #endregion

        #region Actions
        public void ShootAFireball()
        {
            if (!_onGround) return;

            GameObject fireball = Instantiate(_fireballRef, _fireballSpawnLocation.position, Quaternion.identity);

            fireball.GetComponent<ProjectileController>().SetupProjectile(_modelTransform.localRotation.y == 0 ? 1 : -1, true);

            fireball.SetActive(true);
        }
        #endregion

        #region Checks
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
        #endregion
    }
}