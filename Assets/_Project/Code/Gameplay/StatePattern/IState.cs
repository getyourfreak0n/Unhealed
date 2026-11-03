namespace _Project.Code.Gameplay.StatePattern
{
    public interface IState
    {
        void Enter();
        void FrameUpdate();
        void Exit();
    }
}