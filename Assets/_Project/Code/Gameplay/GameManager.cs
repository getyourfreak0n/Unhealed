using _Project.Code.Core.Singleton;
using UnityEngine.SceneManagement;

namespace _Project.Code.Gameplay
{
    public class GameManager : SingletonBase<GameManager>
    {
        public int CurrentLevel {get; private set;}

        protected override void Awake()
        {
            base.Awake();
            CurrentLevel = SceneManager.GetActiveScene().buildIndex;
        }
    }
}
