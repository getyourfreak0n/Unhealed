using System.Collections;
using _Project.Code.Audio;
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
        
        #endregion
        
        #region SerializedFields

        [field:SerializeField] public float maxSpeed { get; private set; } = 10f;
        [field:SerializeField] public float acceleration { get; private set; } = 5f; 
        [field:SerializeField] public float deceleration { get; private set; } = 5f;
        [field:SerializeField] public float gravity { get; private set; } = -9.4f;
        [SerializeField] private float dashTime = 1f;
        [SerializeField] private float dashSpeed = 5f;
        [SerializeField] private AudioClip dashAudio;
        [field:SerializeField,Range(0f,1f)] public float mouseSensitivity { get; private set; }= 0.2f;
        [field:SerializeField,Range(0f,0.05f)] public float mouseRigidity { get; private set; }= 0.03f;
        
        #endregion

        #region Properties
        
        //movement
        public Vector3 MoveInput { get; private set; }
        public Vector3 CurrentVector {get; set;}
        public Vector3 MoveVector {get; set;}
        public float VerticalVelocity {get; set;}
        public Vector3 LastMoveVector {get; set;}
        
        //JUMP
        // public float _lastJumpTime {get; private set;}
        // public bool IsJumping {get; set;}
        // bool _hasJumped;

        //Look
        public float LookInputX {get; private set;}
        public Vector3 RotateVector {get; set;}
        
        //Dash
        public float StartTime {get; set;}
        #endregion

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            //machine
            StateMachine = new StateMachine();
            DefaultState = new DefaultState(this);
            DashState = new DashState(this);
            
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
        
        public IEnumerator Dash()
        {
            AudioManager.Instance.PlaySound(dashAudio);

            while (Time.time < dashTime + StartTime)
            {
                CharacterController.Move( dashSpeed * Time.deltaTime * LastMoveVector);

                yield return null;
            } 
            
            StateMachine.ChangeState(DefaultState);
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
