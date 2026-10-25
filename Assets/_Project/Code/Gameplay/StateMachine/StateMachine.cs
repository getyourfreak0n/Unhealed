namespace _Project.Code.Gameplay.StateMachine
{
    public class StateMachine
    {
        public IState _currentState;
   
        public void InitializeState(IState startingState)
        {
            _currentState = startingState;
            _currentState.Enter();
        }
   
        public void ChangeState(IState nextState)
        {
            if (_currentState == nextState) return;
       
            _currentState?.Exit();
            _currentState = nextState;
            _currentState.Enter();
        }

        public void UpdateState()
        {
            _currentState?.FrameUpdate();
        }
    }
}
