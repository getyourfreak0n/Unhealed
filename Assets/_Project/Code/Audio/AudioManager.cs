using _Project.Code.Core.Singleton;
using UnityEngine;

public class AudioManager : SingletonBase<AudioManager>
{
   private AudioSource _audioSource;

   protected override void Awake()
   {
      base.Awake();
      _audioSource = GetComponent<AudioSource>();
   }

   public void PlaySound(AudioClip clip)
   {
      _audioSource.clip = clip;
      _audioSource.Play();
   }
}
