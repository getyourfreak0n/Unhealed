using _Project.Code.Gameplay.Combat;
using UnityEngine;

namespace _Project.Code.Gameplay.Environment
{
    public class TerrainController : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject vfxPrefab;
    
        public void HitVFX(Vector3 hitPoint, Vector3 hitDirection)
        {
            GameObject instantiate =Instantiate(vfxPrefab, hitPoint, Quaternion.LookRotation(hitDirection));
            Destroy(instantiate, 1.5f);
        }
    }
}
