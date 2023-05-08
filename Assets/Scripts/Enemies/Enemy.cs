using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public void Kill()
        {
            GetComponent<SpriteRenderer>().flipY = true;
            Collider2D[] cols = GetComponents<Collider2D>();

            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].enabled = false;
            }

            Vector3 movement = new Vector3(Random.Range(4, 7), Random.Range(-4, 4), 0);
            transform.position += movement * Time.deltaTime;

            ScoreManager.AddScore(10);

            Destroy(gameObject, 3f);
        }
    }
}