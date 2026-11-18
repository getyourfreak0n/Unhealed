using UnityEngine;

namespace _Project.Code.Gameplay.Combat
{
    public class BubbleLogic : MonoBehaviour
    {
        private static MaterialPropertyBlock _block;
        private MeshRenderer _renderer;

        void Awake()
        {
            _block ??= new MaterialPropertyBlock();
            _renderer = GetComponent<MeshRenderer>();
        
            _block.SetColor("_BaseColor", Random.ColorHSV());
            _renderer.SetPropertyBlock(_block);
        }
    }
}
