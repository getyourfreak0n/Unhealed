using UnityEditor.Animations;
using UnityEngine;

namespace _Project.Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponsSO", menuName = "Scriptable Objects/WeaponsSO")]
    public class WeaponsSO : ScriptableObject
    {
        public float damage;
        public float nextFireTime;
        public float recoilAmount;
        public float recoilSpeed;
        public float spreadAmount;
        public float fireDistance;
        public AudioClip clip;
        public AnimatorController weaponAnimator;
        public Vector3 spriteScale;
    }
}
