using _Project.Code.Player;
using UnityEngine;

namespace _Project.Code.Gameplay.StateMachine.PlayerStates
{
    public class DefaultState : IState
    {
        PlayerController _player { get; }
        
        public DefaultState(PlayerController player)
        {
            this._player = player;
        }
        
        public void Enter()
        {
        }

        public void Update()
        {
            PlayerMovement();
            PlayerRotation();
        }

        public void Exit()
        {
        }
        
        void PlayerMovement()
        {
            _player.MoveVector = ((_player.transform.right * _player.MoveInput.x) +
                          (_player.transform.forward * _player.MoveInput.z));

            Vector3 horizontalVector =  _player.MoveVector * _player.maxSpeed;

            float rate = horizontalVector.sqrMagnitude > 0.01f ? _player.acceleration : _player.deceleration;
            _player.CurrentVector = Vector3.Lerp(_player.CurrentVector, horizontalVector, rate * Time.deltaTime);
            
            _player.VerticalVelocity = -2f;
            
            _player.MoveVector = new Vector3(_player.CurrentVector.x,_player.VerticalVelocity, _player.CurrentVector.z);
            _player.CharacterController.Move(_player.MoveVector * Time.deltaTime);
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
