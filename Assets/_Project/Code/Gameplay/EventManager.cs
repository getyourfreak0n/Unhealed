using System;
using _Project.Code.Core.Singleton;

namespace _Project.Code.Gameplay
{
    public class EventManager : SingletonBase<EventManager>
    {
        public event Action LevelOne;
        public event Action Blinded;

        public void TriggerLevelOne()
        {
            LevelOne?.Invoke();
        }

        public void TriggerBlinded()
        {
            Blinded?.Invoke();
        }
    }
}
