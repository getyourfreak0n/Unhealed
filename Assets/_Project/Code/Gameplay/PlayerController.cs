using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        #region References

        CharacterController _characterController;
        CinemachineCamera _cineCamera;
        
        #endregion
        
        #region SerializeFields

        [Header("Movement Settings")]
        [SerializeField] float _targetSpeed = 10f;
        [SerializeField] float _acceleration = 5f, _deceleration = 5f;
        [SerializeField] float _gravity = 9.4f;
        [Header("Jump Settings")]
        [SerializeField] float _groundCheckDistance = 0.4f; 
        
            #endregion

        #region PrivateFields
        
        //movement
        Vector3 _moveInput;
        Vector3 _currentVector;
        Vector3 _moveVector;

        //JUMP
        bool _isGrounded;
        LayerMask _groundMask;
        float offset = 0.3f;
        #endregion
        
        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _cineCamera = FindFirstObjectByType<CinemachineCamera>();
            _groundMask = LayerMask.GetMask("Ground");
            
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
            Ray ray = new Ray(transform.position * offset , Vector3.down);

            _isGrounded = Physics.Raycast(ray, _groundCheckDistance, _groundMask);
            
            Debug.Log("isGrounded: " + _isGrounded);
        }
        

        void PlayerMovement()
        {
           Vector3 targetVector =  _targetSpeed * _moveInput;
           
           float rate = targetVector.sqrMagnitude > 0.01f ? _acceleration : _deceleration;
           
           _currentVector = Vector3.Lerp(_currentVector, targetVector, rate * Time.deltaTime);

           _moveVector = _currentVector * Time.deltaTime;
           
           _characterController.Move(_moveVector);
        }
    }
}
