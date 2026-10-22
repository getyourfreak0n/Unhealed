namespace _Project.Code.Gameplay.StateMachine
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}