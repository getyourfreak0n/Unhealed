using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Project.Code
{
    public class VolumeEffects : MonoBehaviour
    {
        Volume _volume;
        WhiteBalance _wb;
        ChannelMixer _mixer;

        void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.profile.TryGet(out _wb);
            _volume.profile.TryGet(out _mixer);
        }

        void Update()
        {
            // _wb.temperature.Override(Mathf.Lerp
            //     (_wb.temperature.min, _wb.temperature.max, Mathf.PingPong(Time.time * 2, 1))); 
            // _wb.tint.Override(Mathf.Lerp
            //     (_wb.tint.max, _wb.tint.min, Mathf.PingPong(Time.time * 2, 1)));
            
            
            _mixer.redOutRedIn.Override(Mathf.Lerp(_mixer.redOutRedIn.min, _mixer.redOutRedIn.max, Mathf.PingPong(Time.time * 2, 1)));
            _mixer.redOutBlueIn.Override(Mathf.Lerp(_mixer.redOutBlueIn.min, _mixer.redOutBlueIn.max, Mathf.PingPong(Time.time * 2, 1)));
            _mixer.redOutGreenIn.Override(Mathf.Lerp(_mixer.redOutGreenIn.min, _mixer.redOutGreenIn.max, Mathf.PingPong(Time.time * 2, 1)));
        }
    }
}
