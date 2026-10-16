using _Project.Code.Enemy;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    public void TakeDamage()
    {
            Debug.Log("Ouch! Fuck off!");
    }
}
