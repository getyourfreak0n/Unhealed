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
        private float _currentSpeed;
        
        //private fields        
        Vector3 _moveInput;
        Vector3 _lastMoveInput;
        
        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _cineCamera = FindFirstObjectByType<CinemachineCamera>();
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        { 
            AccelerationDeceleration(); 
            PlayerMovement();
        }

        
        //get move Vector2 input
        public void MoveInput(Vector2 moveInput)
        {
            _moveInput = Vector3.ClampMagnitude(new Vector3(moveInput.x, 0, moveInput.y), 1f);

            if (_moveInput.sqrMagnitude > 0.01f)
            {
                    _lastMoveInput = _moveInput;
            }
        }
        

        void PlayerMovement()
        {
            Vector3 moveDirection = _currentSpeed * Time.deltaTime * _lastMoveInput;

            if (!_characterController.isGrounded)
            {
                    moveDirection.y -= _gravity * Time.deltaTime;
            }
            _characterController.Move(moveDirection);
        }
        
        void AccelerationDeceleration()
        {
            bool isMoving = _moveInput.sqrMagnitude > 0.01f;

           _currentSpeed = isMoving ? 
               Mathf.Lerp(_currentSpeed,_targetSpeed,_acceleration * Time.deltaTime) :
               Mathf.Lerp(_currentSpeed, 0f, _deceleration * Time.deltaTime);
        }
    }
}
