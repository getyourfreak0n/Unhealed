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
            
        }

        public void FrameUpdate()
        {
            _player.CharacterController.Move(_player.MoveVector * Time.deltaTime);
        }

        public void Exit()
        {
        }
    }
}
