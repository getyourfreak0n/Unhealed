using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Code.Gameplay
{
    public class DeathLogic : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
