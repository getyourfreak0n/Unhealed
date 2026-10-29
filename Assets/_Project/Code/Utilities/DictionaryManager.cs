using System.Collections.Generic;
using _Project.Code.Core.Singleton;
using _Project.Code.Player;
using UnityEngine;

namespace _Project.Code.Utilities
{
    public class DictionaryManager : SingletonBase<DictionaryManager>
    {
        public readonly Dictionary<string, Component> compD = new Dictionary<string, Component>();

        PlayerController _player;
        protected override void Awake()
        {
            base.Awake();
            _player = FindAnyObjectByType<PlayerController>();
        }

        void Start()
        {
            compD.Add("Player", _player);
        }

        public void Add(Component component)
        {
            compD.Add(component.name, component);
        }

        public void Remove(Component component)
        {
            compD.Remove(component.name);
        }
    }
}
