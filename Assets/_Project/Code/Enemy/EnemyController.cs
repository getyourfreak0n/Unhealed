using UnityEngine;
using UnityEngine.AI;

namespace _Project.Code.Enemy
{
    public class EnemyController : MonoBehaviour, IEnemy
    {
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] GameObject player;

        NavMeshAgent _agent;
        
        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            _agent.SetDestination(player.transform.position);
        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"Ouch! Fuck off! Took {damage} damage!");
        }
    
        public void HitVFX(Vector3 hitPoint, Vector3 hitDirection)
        {
            GameObject instantiate =Instantiate(vfxPrefab, hitPoint, Quaternion.LookRotation(hitDirection));
            Destroy(instantiate, 1f);
        }
    }
}
