using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioManagerService 
{
    void PlayOneShot(AudioClip clip, AudioSettings settings = default);

    
    void PlayOneShotDelayed(AudioClip clip, AudioSettings settings = default, float delay = 1.0f);
}
