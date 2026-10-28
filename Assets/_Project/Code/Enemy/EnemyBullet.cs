using _Project.Code.Player;
using UnityEngine;

namespace _Project.Code.Enemy
{
    public class EnemyBullet : MonoBehaviour
    {
        Rigidbody _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();    
        }

        void Start()
        {
            Destroy(gameObject, 5f);
            
            _rb.AddForce(transform.forward * Random.Range(1f,5.5f), ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
                other.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
