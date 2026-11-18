using _Project.Code.Core.Singleton;
using UnityEngine;

namespace _Project.Code.Audio
{
   public class AudioManager : SingletonBase<AudioManager>
   {
      public AudioSource AudioSource { get; private set; }

      protected override void Awake()
      {
         base.Awake();
         AudioSource = GetComponent<AudioSource>();
      }

      public void PlaySound(AudioClip clip,  float volume, float lowPitch, float highPitch)
      {
         AudioSource.clip = clip;
         AudioSource.pitch = Random.Range(lowPitch, highPitch);
         AudioSource.PlayOneShot(AudioSource.clip, volume);
      }
   }
}
