using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerFire : MonoBehaviour
    {
        [SerializeField] private GameObject _firePrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _reloadTime = 2f;
        private float _waitTime;
        private bool _canFire = true;

        private void Update()
        {
            if (!GameManager.isStart)
                return;

            if (Input.GetKeyDown(KeyCode.K) && _canFire)
            {
                Instantiate(_firePrefab, _spawnPoint.position, Quaternion.identity);
                AudioManager.Instance.Play("FireBall");
                _canFire = false;
            }

            if (!_canFire)
            {
                _waitTime += Time.deltaTime;
                if (_waitTime >= _reloadTime)
                {
                    _canFire = true;
                    _waitTime = 0;
                }
            }
        }
    }
}