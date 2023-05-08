using Enemies;
using Managers;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _deathPoint;

        [Header("Attributes")]
        [SerializeField] private float _moveSpeed = 10.0f;
        [SerializeField] public static int maxHealth = 3;
        [SerializeField] public static int currentHealth;

        [Header("UI")]
        [SerializeField] private GameObject[] _hearts;

        [Header("")]
        public float playerTurnScale = 1f;

        private float _xInput;
        private GameManager _gameManager;
        private PlayerJump _playerJump;

        //TAG CONSTS
        private readonly string COLLECTABLE_TAG = "Collectable";
        private readonly string END_TAG = "End";
        private readonly string ENEMY_TAG = "Enemy";
        private readonly string TRAP_TAG = "Trap";

        //ANIM CONSTS
        private readonly string RUN_BOOL_ANIM = "IsRunning";
        private readonly string ISDEAD_BOOL_ANIM = "IsDead";

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _playerJump = GetComponent<PlayerJump>();
        }

        private void Start()
        {
            if (!GameManager.isRestart)
            {
                currentHealth = maxHealth;
            }
            SetHearts();
            _animator.SetBool(ISDEAD_BOOL_ANIM, false);
        }

        private void Update()
        {
            if (!GameManager.isStart) return;

            RunAnimation(_xInput);

            if (!_playerJump.isWallJumping)
            {
                TurnPlayer(_xInput);
            }

            if (transform.position.y < _deathPoint.position.y && _rb != null)
            {
                Die();
            }
        }

        private void FixedUpdate()
        {
            _xInput = Input.GetAxis("Horizontal");

            if (!GameManager.isStart) return;

            if (!_playerJump.isWallJumping)
                Move(_xInput);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IfEnemyThenKill(collision.gameObject);
            Collect(collision.gameObject);
            EndFlag(collision.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IfEnemyThenDie(collision.gameObject);
        }

        //Custom Methods

        #region MoveAndTurn
        private void Move(float moveInput)
        {
            if (_rb != null)
                _rb.velocity = new Vector2(moveInput * _moveSpeed, _rb.velocity.y);
        }

        private void TurnPlayer(float moveInput)
        {
            if (moveInput > 0)
            {
                playerTurnScale = 1f;
            }
            else if (moveInput < 0)
            {
                playerTurnScale = -1f;
            }

            transform.localScale = new Vector3(playerTurnScale, 1f, 1f);
        }

        #endregion

        #region Animations

        private void RunAnimation(float h)
        {
            if (h != 0)
                _animator.SetBool(RUN_BOOL_ANIM, true);
            else
                _animator.SetBool(RUN_BOOL_ANIM, false);
        }
        #endregion

        #region Collect

        private void Collect(GameObject col)
        {
            if (col.CompareTag(COLLECTABLE_TAG))
            {
                ScoreManager.AddScoreDefault();
                AudioManager.Instance.Play("Eat");
                Destroy(col);
            }
        }

        #endregion

        #region End
        private void EndFlag(GameObject col)
        {
            if (col.CompareTag(END_TAG))
            {
                _gameManager.GoNextSceneIfExist();
            }
        }
        #endregion

        #region Die
        private void IfEnemyThenDie(GameObject col)
        {
            if (col.CompareTag(ENEMY_TAG) || col.CompareTag(TRAP_TAG))
            {
                _playerJump.Jump();
                Destroy(_rb);
                Die();
            }
        }

        private void Die()
        {
            currentHealth--;
            SetHearts();

            if (currentHealth <= 0)
            {
                AudioManager.Instance.Play("Die");
                _animator.SetBool(ISDEAD_BOOL_ANIM, true);
                StartCoroutine(DieCompletely());
            }
            else
            {
                GameManager.isStart = false;
                GameManager.isRestart = true;
                AudioManager.Instance.Play("Die");
                _animator.SetBool(ISDEAD_BOOL_ANIM, true);
                StartCoroutine(ReloadCurrentScene());
            }
        }

        private IEnumerator ReloadCurrentScene()
        {
            yield return new WaitForSeconds(0.5f);
            _gameManager.ReloadCurrentScene();
        }

        private IEnumerator DieCompletely()
        {
            yield return new WaitForSeconds(0.7f);
            _gameManager.restartPanel.SetActive(true);
            _gameManager.SetScoreOnPanels();
            GameManager.isStart = false;

            if (ScoreManager.GetScore() > PlayerPrefsManager.GetHighScore())
                PlayerPrefsManager.SetHighScore(ScoreManager.GetScore());
            gameObject.SetActive(false);
        }
        #endregion

        private void IfEnemyThenKill(GameObject col)
        {
            if (col.CompareTag(ENEMY_TAG))
            {
                col.GetComponent<Enemy>().Kill();
                _playerJump.Jump();
            }
        }

        private void SetHearts()
        {
            for (int i = 0; i < _hearts.Length; i++)
            {
                _hearts[i].SetActive(false);
            }

            for (int i = 0; i < currentHealth; i++)
            {
                _hearts[i].SetActive(true);
            }
        }
    }
}