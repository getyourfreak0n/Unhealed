using UnityEngine;

namespace _Project.Code.Gameplay.Combat
{
    public interface IDamageable
    {
        public void HitVFX(Vector3 hitPoint, Vector3 hitDirection);
    }
}