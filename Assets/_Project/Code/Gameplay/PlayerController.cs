using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        //references
        CharacterController _characterController;
        CinemachineCamera _cineCamera;
        
        [Header("Movement Settings")]
        [SerializeField] float _targetSpeed = 10f;
        [SerializeField] float _acceleration = 5f, _deceleration = 5f;
        [SerializeField] float _gravity = 9.4f;
        
        //private fields        
        Vector3 _moveInput;
        Vector3 currentVector;
        
        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _cineCamera = FindFirstObjectByType<CinemachineCamera>();
            
            Cursor.lockState = CursorLockMode.Locked;
        }


        void Update()
        { 
            PlayerMovement();
        }

        
        //get move Vector2 input
        public void MoveInput(Vector2 moveInput)
        {
            _moveInput = Vector3.ClampMagnitude(new Vector3(moveInput.x, 0, moveInput.y), 1f);
        }

        public void OnJump()
        {
            
        }
        

        void PlayerMovement()
        {
           Vector3 targetVector =  _targetSpeed * _moveInput;
           
           float rate = targetVector.sqrMagnitude > 0.01f ? _acceleration : _deceleration;
           
           currentVector = Vector3.Lerp(currentVector, targetVector, rate * Time.deltaTime);

           Vector3 moveVector = currentVector * Time.deltaTime;
            
            if (!_characterController.isGrounded)
            {
                currentVector.y -= _gravity * Time.deltaTime;
            }
            _characterController.Move(moveVector);
        }
    }
}
