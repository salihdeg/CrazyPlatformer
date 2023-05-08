using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _foot;
        [SerializeField] private Transform _wallCheck;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private LayerMask _touchableLayers;

        [Header("Attribures")]
        [SerializeField] private float _jumpForce = 7.0f;
        [SerializeField] private float _jumpTime = 0.3f;
        [SerializeField] private bool _isJumping;
        [SerializeField] private bool _isGrounded = true;

        private float _jumpTimeCounter;
        private float _coyoteTime = 0.2f;
        private float _coyoteTimeCounter;
        private float _jumpBufferTime = 0.2f;
        private float _jumpBufferCounter;

        //WallSlideAndJump
        private bool _isWallSliding;
        private float _wallSlidingSpeed = 2f;

        public bool isWallJumping;
        private float _wallJumpingDirection;
        private float _wallJumpingTime = 0.2f;
        private float _wallJumpingCounter;
        private float _wallJumpingDuration = 0.4f;
        [SerializeField] private Vector2 _wallJumpingPower = new Vector2(7f, 13f);

        [Header("")]
        [SerializeField] private float _rayDistance = 0.12f;
        private float _xInput;
        private PlayerController _playerController;

        //Anim Constants
        private readonly string JUMP_BOOL_ANIM = "IsJumping";
        private readonly string GROUNDED_BOOL_ANIM = "IsGrounded";
        private readonly string WALLSLIDE_BOOL_ANIM = "IsWallSliding";

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            GroundCheck();
            if (!GameManager.isStart) return;

            _xInput = Input.GetAxis("Horizontal");
            JumpController();
            WallSlide();
            WallJump();
        }

        #region Jump
        private void JumpController()
        {
            Coyote();
            JumpBuffer();

            if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f)
            {
                _jumpTimeCounter = _jumpTime;
                Jump();
                _isJumping = true;
            }

            if (Input.GetKey(KeyCode.Space) && _isJumping)
            {
                if (_jumpTimeCounter > 0f)
                {
                    Jump();
                    _coyoteTimeCounter = 0f;
                    _jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    _isJumping = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isJumping = false;
                _coyoteTimeCounter = 0f;
            }

            JumpAnimation();
        }

        private void JumpBuffer()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpBufferCounter = _jumpBufferTime;
            else
                _jumpBufferCounter -= Time.deltaTime;
        }

        private void Coyote()
        {
            if (_isGrounded)
                _coyoteTimeCounter = _coyoteTime;
            else
                _coyoteTimeCounter -= Time.deltaTime;
        }

        private void GroundCheck()
        {
            RaycastHit2D hit = Physics2D.Raycast(_foot.transform.position, Vector2.down, _rayDistance, _touchableLayers);
            Debug.DrawRay(_foot.transform.position, Vector3.up * -_rayDistance, Color.red);

            if (hit.collider != null)
                _isGrounded = true;
            else
                _isGrounded = false;

            AnimationIsGroundBool();
        }

        public void Jump()
        {
            if (_rb != null)
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
        #endregion

        #region WallSlideAndWallJump

        private bool IsWalled()
        {
            return Physics2D.OverlapCircle(_wallCheck.position, 0.2f, _wallLayer);
        }

        private void WallSlide()
        {
            if (IsWalled() && !_isGrounded && _xInput != 0)
            {
                _isWallSliding = true;
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_wallSlidingSpeed, float.MaxValue));
                CancelInvoke(nameof(StopWallJumping));
            }
            else
            {
                _isWallSliding = false;
            }

            WallSlideAnimation();
        }

        private void WallJump()
        {
            if (_isWallSliding)
            {
                isWallJumping = false;
                _wallJumpingDirection = -transform.localScale.x;
                _wallJumpingCounter = _wallJumpingTime;
            }
            else
            {
                _wallJumpingCounter -= Time.deltaTime;
            }

            WallSlideAnimation();

            if (Input.GetKeyDown(KeyCode.Space) && _wallJumpingCounter > 0f)
            {
                isWallJumping = true;
                _rb.velocity = new Vector2(_wallJumpingDirection * _wallJumpingPower.x, _wallJumpingPower.y);
                _wallJumpingCounter = 0f;

                if (transform.localScale.x != _wallJumpingDirection)
                {
                    _playerController.playerTurnScale = _playerController.playerTurnScale == 1 ? -1 : 1;
                    Vector3 localScale = transform.localScale;
                    localScale.x *= -1f;
                    transform.localScale = localScale;
                }

                Invoke(nameof(StopWallJumping), _wallJumpingDuration);
            }
        }

        private void StopWallJumping()
        {
            isWallJumping = false;
            WallSlideAnimation();
        }
        #endregion

        #region Animations
        private void WallSlideAnimation()
        {
            _animator.SetBool(WALLSLIDE_BOOL_ANIM, _isWallSliding);
        }

        private void JumpAnimation()
        {
            _animator.SetBool(JUMP_BOOL_ANIM, _isJumping);
        }

        private void AnimationIsGroundBool()
        {
            _animator.SetBool(GROUNDED_BOOL_ANIM, _isGrounded);
        }
        #endregion
    }
}

