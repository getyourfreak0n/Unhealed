using System;
using _Project.Code.Core.Singleton;
using UnityEngine;

namespace _Project.Code.Gameplay
{
    public class EventManager : SingletonBase<EventManager>
    {
        [SerializeField] GameObject[] levelOnes;
        [SerializeField] GameObject[] levelTwos;

        public event Action LevelTwo;
        public event Action LevelThree;
        public event Action Blinded;
        
        private bool levelTwoTriggered;
        private bool levelThreeTriggered;

        void Update()
        {
            if (!levelTwoTriggered && AreAllEnemiesDestroyed(levelOnes))
            {
                levelTwoTriggered = true; 
                TriggerLevelTwo();
                
            }

            if (!levelThreeTriggered && AreAllEnemiesDestroyed(levelTwos))
            {
                levelThreeTriggered = true;
                TriggerLevelThree();
               
            }
        }

        public void TriggerBlinded()
        {
            Blinded?.Invoke();
        }
        
        bool AreAllEnemiesDestroyed(GameObject[] enemies)
        {
            if (enemies == null || enemies.Length == 0) return false;

            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    // This will print the name of the exact enemy blocking the level transition
                    return false;
                }
            }

            return true;
        }

        void TriggerLevelTwo()
        {
            LevelTwo?.Invoke();
        }

        void TriggerLevelThree()
        {
            LevelThree?.Invoke();
        }
    }
}