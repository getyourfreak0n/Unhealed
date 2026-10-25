using _Project.Code.Gameplay.StateMachine;
using _Project.Code.Gameplay.StateMachine.PlayerStates;
using UnityEngine;

namespace _Project.Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region References

        public CharacterController CharacterController {get; private set;}
        
        // State Machine 
        public StateMachine StateMachine {get; private set;}
        public DefaultState DefaultState {get; private set;}
        public DashState DashState {get; private set;}
        //public JumpState JumpState {get; private set;}
        
        #endregion
        
        #region SerializedFields

        [field:SerializeField] public float maxSpeed { get; private set; } = 10f;
        [field:SerializeField] public float acceleration { get; private set; } = 5f; 
        [field:SerializeField] public float deceleration { get; private set; } = 5f;
        [field:SerializeField] public float gravity { get; private set; }= 9.4f;
        [field:SerializeField] public float gravityMultiplier { get; private set; } = 2f;
        //[field:SerializeField] public float jumpForce { get; private set; }= 5f;
        //[field:SerializeField] public float groundCheckDelay { get; private set; }= 0.4f;
        //[SerializeField, Range(0f,0.5f)] float coyoteTime = 0.2f;
        
        
        [field:SerializeField,Range(0f,1f)] public float mouseSensitivity { get; private set; }= 0.2f;
        [field:SerializeField,Range(0f,0.05f)] public float mouseRigidity { get; private set; }= 0.03f;
        
        #endregion

        #region Properties
        
        //movement
        public Vector3 MoveInput { get; private set; }
        public Vector3 CurrentVector {get; set;}
        public Vector3 MoveVector {get; set;}
        public float VerticalVelocity {get; set;}
        
        //JUMP
        // public float _lastJumpTime {get; private set;}
        // public bool IsJumping {get; set;}
        // bool _hasJumped;

        //Look
        public float LookInputX {get; private set;}
        public Vector3 RotateVector {get; set;}
        
        #endregion

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            //machine
            StateMachine = new StateMachine();
            
            //states
            DefaultState = new DefaultState(this);
            DashState = new DashState(this);
            
            //JumpState = new JumpState(this);
            
            CharacterController = GetComponent<CharacterController>();
        }

        void Start()
        {
            StateMachine.InitializeState(DefaultState);
        }

        void Update()
        { 
            StateMachine._currentState?.FrameUpdate();
        }
        
        public void GetMoveInput(Vector2 moveInput)
        {
            MoveInput = Vector3.ClampMagnitude(new Vector3(moveInput.x, 0, moveInput.y), 1f);
        }

        public void  GetLookInput(Vector2 lookInput)
        {
            LookInputX = lookInput.x;
        }

        public void OnDash(bool isDashing)
        {
            if (isDashing)
            {
                StateMachine.ChangeState(DashState);
            }
        }

        /*public void OnJump(bool isJumping)
        {
            if (isJumping && CharacterController.isGrounded)
            {
                _lastJumpTime = Time.time;
                IsJumping = isJumping;
                StateMachine.ChangeState(JumpState);
            }
        }*/ 
    }
}
