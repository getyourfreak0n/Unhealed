using _Project.Code.Player;
using UnityEngine;

namespace _Project.Code.Gameplay.StateMachine.PlayerStates
{
    public class JumpState : IState
    {
        PlayerController _player;
        
        public JumpState(PlayerController player)
        {
            this._player = player;
        }

        public void Enter()
        {
            Jump();
        }
        
        public void Update()
        {
            if (_player.IsGrounded)
            {
                _player.StateMachine.ChangeState(_player.DefaultState); 
                
                Debug.Log("Anani sikiyim senin!");
            }
            
            Gravity();
            PlayerMovement();
            PlayerRotation();
        }

        public void Exit()
        {
            _player.IsJumping = false;
            _player.MoveVector = Vector3.zero;
        }
        
        void Jump()
        {
            _player.VerticalVelocity = _player.jumpForce;
        }

        void Gravity()
        {
            _player.VerticalVelocity -= _player.gravity * _player.gravityMultiplier * Time.deltaTime;
        }


        void PlayerMovement()
        {
            Vector3 moveVector = new Vector3(_player.MoveVector.x, _player.VerticalVelocity,_player.MoveVector.z);
            
            _player.CharacterController.Move(moveVector * Time.deltaTime);
        }
        void PlayerRotation()
        {
            if (_player.LookInputX != 0f && Mathf.Abs(_player.LookInputX) > _player.mouseRigidity)
            {
                _player.RotateVector = new Vector3( 0, _player.LookInputX, 0);
            }
            else
            {
                _player.RotateVector = Vector3.zero;
            }
            
            _player.transform.Rotate( _player.mouseSensitivity * _player.RotateVector);
        }
    }
}
