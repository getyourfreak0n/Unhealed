using System;
using _Project.Code.Enemy;
using UnityEngine;

namespace _Project.Code.Gameplay.Combat
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] Transform crosshair;
        [SerializeField] float fireRadius = 0.4f;

        LayerMask _enemyLayer;

        void Awake()
        {
            _enemyLayer = LayerMask.GetMask("Enemy");
        }

        public void Shoot()
        {
            Ray ray = new Ray(crosshair.position, crosshair.forward);
            
            if (Physics.SphereCast(ray, fireRadius, out RaycastHit hit, 100f, _enemyLayer))
            {
                if (hit.collider.TryGetComponent<IEnemy>(out var enemy))
                {
                    enemy.TakeDamage();
                }
            }
        }
    }
}
