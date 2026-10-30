using _Project.Code.Player;
using _Project.Code.Utilities;
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
                DictionaryManager.Instance.compD.TryGetValue("Player" , out var playerC);
                PlayerController player =  playerC as PlayerController;

                if (player != null) player.TakeDamage(1);
            }
            
            Destroy(gameObject);
        }
    }
}
