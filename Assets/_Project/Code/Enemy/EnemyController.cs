using _Project.Code.Enemy;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    public void TakeDamage(float damage)
    {
            Debug.Log($"Ouch! Fuck off! Took {damage} damage!");
    }
}
