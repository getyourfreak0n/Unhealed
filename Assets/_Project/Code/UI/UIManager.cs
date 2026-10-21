using System.Collections;
using _Project.Code.Core.Singleton;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Code.UI
{
    public class UIManager : SingletonBase<UIManager>
    {
        
        [SerializeField] CinemachineCamera cam;
        
        private float _targetValue;

        private IEnumerator EffectFOV()
        {
            _targetValue = cam.Lens.FieldOfView + 1f;
            cam.Lens.FieldOfView = _targetValue;
                
            yield return null;
        }
        
        public void TriggerCoroutine()
        {
            StartCoroutine(EffectFOV());
        }
    }
}
