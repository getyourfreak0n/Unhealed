using System.Collections;
using UnityEngine;

namespace _Project.Code.Enemy
{
    public class EnemyController : MonoBehaviour, IEnemy
    {
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float travelSpeed = 5f;
        
        private int waypointIndex = 0;
        
        void Update()
        {
            if (Vector3.Distance(waypoints[waypointIndex].position, transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards
                    (transform.position, waypoints[waypointIndex].position, travelSpeed * Time.deltaTime);
            }
            else
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
            }
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
