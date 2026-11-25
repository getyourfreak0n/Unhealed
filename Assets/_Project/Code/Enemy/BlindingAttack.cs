using System.Collections;
using _Project.Code.Gameplay;
using _Project.Code.Player;
using _Project.Code.ScriptableObjects.EnemySO.Attacks;
using UnityEngine;

namespace _Project.Code.Enemy
{
    public class BlindingAttack : MonoBehaviour
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
            StartCoroutine(Attack());
            Destroy(gameObject, 5f);
        } 

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController player))
            {
                if (_hasEffect)
                {
                    EventManager.Instance.TriggerBlinded();
                }   
                player.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }

        void Initializer()
        {
            _hasEffect = so.hasEffect;
            _damage = so.Damage;
        }

        IEnumerator Attack()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            WaitForSeconds wait = new WaitForSeconds(0.8f);

            yield return wait;
            
            _rb.AddForce(transform.forward * 7.5f, ForceMode.Impulse);
        }
        
    }
}
