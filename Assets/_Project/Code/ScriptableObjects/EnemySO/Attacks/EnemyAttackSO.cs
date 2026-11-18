using UnityEngine;

namespace _Project.Code.ScriptableObjects.EnemySO.Attacks
{
    [CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "Scriptable Objects/EnemyAttackSO")]
    public class EnemyAttackSO : ScriptableObject
    {
        public int Damage;
        public bool hasEffect;
    }
}
