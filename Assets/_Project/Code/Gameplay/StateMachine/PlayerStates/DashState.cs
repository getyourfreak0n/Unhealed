using _Project.Code.Player;
using UnityEngine;

namespace _Project.Code.Gameplay.StateMachine.PlayerStates
{
    public class DashState : IState
    {
        
        readonly PlayerController _player;

        public DashState(PlayerController player)
        {
            _player = player;
        }
        
        public void Enter()
        {
            _player.StartTime = Time.time;

            _player.StartCoroutine(_player.Dash());
        }

        public void FrameUpdate()
        {
            PlayerRotation();
        }

        public void Exit()
        {
            _player.StopCoroutine(_player.Dash());
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
