using UnityEngine;

namespace _Project.Code.ScriptableObjects.EnemySO.Stats
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        public EnemyType EnemyType;
        public int Health;
        public int MoveSpeed;
        public AudioClip DamageClip;
    }
    
    public enum EnemyType
    {
        Type1,
        Type2,
    }
}
