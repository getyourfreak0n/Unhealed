using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        
        //references
        CharacterController _characterController;
        
        //private data
        Vector3 _moveDirection;
        
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
                PlayerMovement();
        }

        public void MoveInput(Vector2 input)
        {
            _moveDirection = new Vector3(input.x, 0, input.y).normalized;
        }

        void PlayerMovement()
        {
            _characterController.Move(_moveDirection * _moveSpeed * Time.deltaTime);
        }
        
    }
}
