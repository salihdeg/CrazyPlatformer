using Managers;
using UnityEngine;

namespace Enemies
{
    public class AutoMove : MonoBehaviour
    {
        [SerializeField] private Vector3[] _positions;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private bool _lookMoveSide = true;
        [SerializeField] private bool _moveWithPlayer = false;
        private Vector3 _currentDest;
        private int _currentDestIndex;

        private void Start()
        {
            if (_positions.Length < 2)
            {
                this.enabled = false;
            }
            else
            {
                transform.position = _positions[0];
                _currentDest = _positions[1];
                _currentDestIndex = 1;
            }
        }

        private void Update()
        {
            if (!GameManager.isStart) return; //If game is not started don't do anything

            if (Vector3.Distance(transform.position, _currentDest) == 0)
            {
                Vector3 lastPos = _currentDest;
                if (_currentDestIndex + 1 < _positions.Length)
                    _currentDestIndex++;
                else
                    _currentDestIndex = 0;

                _currentDest = _positions[_currentDestIndex];

                if (_lookMoveSide)
                {
                    LookMoveSide(lastPos);
                }
            }

            MoveTo(_currentDest);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_moveWithPlayer) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.SetParent(transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!_moveWithPlayer) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.SetParent(null);
            }
        }

        private void LookMoveSide(Vector3 lastPos)
        {
            if (lastPos.x < _currentDest.x)
            {
                Vector3 newScale = new Vector3(-1f, 1f, 1f);
                transform.localScale = newScale;
            }
            else if (lastPos.x > _currentDest.x)
            {
                Vector3 newScale = new Vector3(1f, 1f, 1f);
                transform.localScale = newScale;
            }
        }

        private void MoveTo(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, _speed * Time.deltaTime);
        }
    }
}