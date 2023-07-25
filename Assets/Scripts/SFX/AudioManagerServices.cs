using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerServices : MonoBehaviour,IAudioManagerService
{
    public static AudioManagerServices instance;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private readonly struct OneShotCoroutine
    {
        /// <summary>
        /// Audio Clip.
        /// </summary>
        public AudioClip Clip { get; }
        /// <summary>
        /// Audio Settings.
        /// </summary>
        public AudioSettings Settings { get; }
        /// <summary>
        /// Delay.
        /// </summary>
        public float Delay { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public OneShotCoroutine(AudioClip clip, AudioSettings settings, float delay)
        {
            //Clip.
            Clip = clip;
            //Settings
            Settings = settings;
            //Delay.
            Delay = delay;
        }
    }

    private bool IsPlayingSource(AudioSource source)
    {
        //Make sure we still have a source!
        if (source == null)
            return false;

        //Return.
        return source.isPlaying;
    }

    private IEnumerator DestroySourceWhenFinished(AudioSource source)
    {
        //Wait for the audio source to complete playing the clip.
        yield return new WaitWhile(() => IsPlayingSource(source));

        //Destroy the audio game object, since we're not using it anymore.
        //This isn't really too great for performance, but it works, for now.
        if (source != null)
            DestroyImmediate(source.gameObject);
    }
    private IEnumerator PlayOneShotAfterDelay(OneShotCoroutine value)
    {
        //Wait for the delay.
        yield return new WaitForSeconds(value.Delay);
        //Play.
        PlayOneShot_Internal(value.Clip, value.Settings);
    }
    private void PlayOneShot_Internal(AudioClip clip, AudioSettings settings)
    {
        //No need to do absolutely anything if the clip is null.
        if (clip == null)
            return;

        //Spawn a game object for the audio source.
        var newSourceObject = new GameObject($"Audio Source -> {clip.name}");
        //Add an audio source component to that object.
        var newAudioSource = newSourceObject.AddComponent<AudioSource>();

        //Set volume.
        newAudioSource.volume = settings.Volume;
        //Set spatial blend.
        newAudioSource.spatialBlend = settings.SpatialBlend;

        //Play the clip!
        newAudioSource.PlayOneShot(clip);

        //Start a coroutine that will destroy the whole object once it is done!
        if (settings.AutomaticCleanup)
            StartCoroutine(nameof(DestroySourceWhenFinished), newAudioSource);
    }
    public void PlayOneShot(AudioClip clip, AudioSettings settings = default)
    {
        PlayOneShot_Internal(clip, settings);

    }

    public void PlayOneShotDelayed(AudioClip clip, AudioSettings settings = default, float delay = 1)
    {
        StartCoroutine(nameof(PlayOneShotAfterDelay), new OneShotCoroutine(clip, settings, delay));

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
