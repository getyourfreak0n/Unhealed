using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        #region References

        CharacterController _characterController;
        
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
        [SerializeField, Range(0f,0.5f)] float coyoteTime = 0.2f;
        [Header("Camera Settings")]
        [SerializeField,Range(0f,1f)] float mouseSensitivity = 0.2f;
        [SerializeField,Range(0f,0.05f)] float mouseRigidity = 0.03f;
            #endregion

        #region PrivateFields
        
        //movement
        Vector3 _moveInput;
        Vector3 _currentVector;
        Vector3 _moveVector;
        float _verticalVelocity;

        //JUMP
        bool _isGrounded;
        bool _hasJumped;
        LayerMask _groundMask;
        float _offset = 0.5f;
        float _lastGroundedTime;
        
        //Look
        float _lookInputX;
        Vector3 rotateVector;
        #endregion
        
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _characterController = GetComponent<CharacterController>();
            _groundMask = LayerMask.GetMask("Ground");
        }


        void Update()
        { 
            GroundCheck();
            PlayerMovement();
            PlayerRotation();
        }

        
        //get move Vector2 input
        public void MoveInput(Vector2 moveInput)
        {
            _moveInput = Vector3.ClampMagnitude(new Vector3(moveInput.x, 0, moveInput.y), 1f);
        }

        public void LookInput(Vector2 lookInput)
        {
            _lookInputX = lookInput.x;
        }

        public void OnJump()
        {
            bool withinCoyoteTime = Time.time - _lastGroundedTime < coyoteTime;
            
            if (!_hasJumped && (_isGrounded || withinCoyoteTime))
            {
                _hasJumped = true;
                _verticalVelocity = jumpForce;
            }
        }
        
        void GroundCheck()
        {
            Ray ray = new Ray(transform.position + Vector3.down * _offset , Vector3.down);

            _isGrounded = Physics.Raycast(ray, groundCheckDistance, _groundMask);

            if (_isGrounded)
            {
                _hasJumped = false;
                _lastGroundedTime = Time.time;
            }
        }

        void PlayerMovement()
        {
            Vector3 moveInput = ((transform.right * _moveInput.x) + (transform.forward * _moveInput.z));
            Vector3 horizontalVector =  moveInput * maxSpeed;

            float rate = horizontalVector.sqrMagnitude > 0.01f ? acceleration : _deceleration;
           _currentVector = Vector3.Lerp(_currentVector, horizontalVector, rate * Time.deltaTime);
           
           if (_isGrounded && _verticalVelocity < 0)
           {
               _verticalVelocity = -2f;
           }
           else
           {
               _verticalVelocity -= gravity * gravityMultiplier * Time.deltaTime;
           }
           
           _moveVector = new Vector3(_currentVector.x,_verticalVelocity, _currentVector.z);
           _characterController.Move(_moveVector * Time.deltaTime);
        }

        void PlayerRotation()
        {
            if (_lookInputX != 0f && Mathf.Abs(_lookInputX) > mouseRigidity)
            {
                rotateVector = new Vector3( 0, _lookInputX, 0);
            }
            else
            {
                rotateVector = Vector3.zero;
            }
            
            transform.Rotate( mouseSensitivity * rotateVector);
        }
    }
}
