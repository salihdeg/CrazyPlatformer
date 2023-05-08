using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class FireMove : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _destroyEffect;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.Rotate(0, (FindObjectOfType<PlayerController>().playerTurnScale == 1 ? 0 : 180), 0);
        _rb.velocity = _speed * transform.right;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //GetComponent<Animator>().Play("Fire_Bomb_End");
            GetComponent<Animator>().SetTrigger("Explode");
            Destroy(_rb);
            Instantiate(_destroyEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            //GetComponent<Animator>().Play("Fire_Bomb_End");
            GetComponent<Animator>().SetTrigger("Explode");
            Destroy(_rb);
        }
    }
}
