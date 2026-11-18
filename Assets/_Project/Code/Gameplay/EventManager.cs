using System;
using _Project.Code.Core.Singleton;
using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class EventManager : SingletonBase<EventManager>
    {
        public event Action LevelOne;

        public void TriggerLevelOne()
        {
            LevelOne?.Invoke();
        }
    }
}
