using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Code.Enemy
{
    public interface IEnemy
    {
         void TakeDamage(float damage);
         void CameraImpulse();
         void FlickerEffect();
    }
}
