using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        CharacterController _characterController;
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }
    }
}
