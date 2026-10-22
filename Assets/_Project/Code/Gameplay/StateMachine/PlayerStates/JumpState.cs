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
            Debug.Log("Entered jump state");
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}
