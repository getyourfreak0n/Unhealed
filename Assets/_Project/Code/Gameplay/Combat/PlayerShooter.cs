using _Project.Code.Enemy;
using UnityEngine;

namespace _Project.Code.Gameplay.Combat
{
    public class PlayerShooter : MonoBehaviour
    {
        //serialized fields
        [SerializeField] Transform crosshair;
        [SerializeField] float fireRadius = 0.4f;
        [SerializeField] float nextFireTime = 0.4f;

        //private fields
        LayerMask _enemyLayer;
        float _lastFireTime;
        
        void Awake()
        {
            _enemyLayer = LayerMask.GetMask("Enemy");
        }

        public void Shoot()
        {
            if (RateOfFire()) return;

            Ray ray = new Ray(crosshair.position, crosshair.forward);
            
            if (Physics.SphereCast(ray, fireRadius, out RaycastHit hit, 100f, _enemyLayer))
            { 
                if (hit.collider.TryGetComponent<IEnemy>(out var enemy))
                {
                        enemy.TakeDamage();
                }
            }
        }

        bool RateOfFire()
        {
            if (Time.time - _lastFireTime <= nextFireTime)
            {
                return true;
            }

            _lastFireTime = Time.time;
            return false;
        }
    }
}
