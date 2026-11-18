using _Project.Code.Player;
using _Project.Code.ScriptableObjects.EnemySO.Attacks;
using UnityEngine;

namespace _Project.Code.Enemy
{
    public class NormalAttack : MonoBehaviour
    {
        [SerializeField] EnemyAttackSO so;
        
        Rigidbody _rb;
        int _damage;
        bool _hasEffect;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();    
        }

        void Start()
        {
            Initializer();
            Destroy(gameObject, 5f);
            
            _rb.AddForce(transform.forward * Random.Range(1f,2.5f), ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController player))
            {
                player.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }

        void Initializer()
        {
            _hasEffect = so.hasEffect;
            _damage = so.Damage;
        }
    }
}
