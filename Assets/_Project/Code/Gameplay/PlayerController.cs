using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class PlayerController : MonoBehaviour
    
    
    {
        #region References

        CharacterController _characterController;
        //CinemachineCamera _cineCamera;
        
        #endregion
        
        #region SerializedFields

        [Header("Movement Settings")]
        [SerializeField] float maxSpeed = 10f;
        [SerializeField] float acceleration = 5f, _deceleration = 5f;
        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 5f;
        [SerializeField] float groundCheckDistance = 0.4f;
        [SerializeField] float gravity = 9.4f;
        [SerializeField] float gravityMultiplier = 2f;
        [Header("Camera Settings")]
        [SerializeField] float camRotationSpeed = 50f;
            #endregion

        #region PrivateFields
        
        //movement
        Vector3 _moveInput;
        Vector3 _currentVector;
        Vector3 _moveVector;
        float _verticalVelocity;

        //JUMP
        bool _isGrounded;
        LayerMask _groundMask;
        float _offset = 0.5f;
        #endregion
        
        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            //_cineCamera = FindFirstObjectByType<CinemachineCamera>();
            _groundMask = LayerMask.GetMask("Ground");
            
            Cursor.lockState = CursorLockMode.Locked;
        }


        void Update()
        { 
            GroundCheck();
            PlayerMovement();
        }

        
        //get move Vector2 input
        public void MoveInput(Vector2 moveInput)
        {
            _moveInput = Vector3.ClampMagnitude(new Vector3(moveInput.x, 0, moveInput.y), 1f);
        }

        public void OnJump()
        {
            if (_isGrounded)
            {
                _verticalVelocity = jumpForce;
            }
        }


        void GroundCheck()
        {
            Ray ray = new Ray(transform.position + Vector3.down * _offset , Vector3.down);

            _isGrounded = Physics.Raycast(ray, groundCheckDistance, _groundMask);
        }

        void PlayerMovement()
        { 
            Vector3 moveInput = new Vector3(_moveInput.x, 0, _moveInput.z);
            Vector3 horizontalVector =  moveInput * maxSpeed;
           
           // rate changes depends on if there is input or not. And lerp movement based on that
           float rate = horizontalVector.sqrMagnitude > 0.01f ? acceleration : _deceleration;
           _currentVector = Vector3.Lerp(_currentVector, horizontalVector, rate * Time.deltaTime);
           
           //apply downforce to y 
           if (_isGrounded && _verticalVelocity < 0)
           {
               _verticalVelocity = -2f;
           }
           else
           {
               _verticalVelocity -= gravity * gravityMultiplier * Time.deltaTime;
           }
           
           //put everything together
           _moveVector = new Vector3(_currentVector.x,_verticalVelocity, _currentVector.z);
           _characterController.Move(_moveVector * Time.deltaTime);
        }
    }
}
