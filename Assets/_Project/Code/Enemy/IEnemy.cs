using UnityEngine;

namespace _Project.Code.Enemy
{
    public interface IEnemy
    {
         void TakeDamage(float damage);
         void HitVFX(Vector3 hitPoint, Vector3 hitDirection);
    }
}
