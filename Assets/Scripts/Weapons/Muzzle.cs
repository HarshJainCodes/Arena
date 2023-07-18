using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour
{

    [Tooltip("Socket at the tip of the Muzzle. Commonly used as a firing point.")]
    [SerializeField]
    private Transform socket;

    [Tooltip("Sprite. Displayed on the player's interface.")]
    [SerializeField]
    private Sprite sprite;
    
    [SerializeField]
    public AudioClip  audioClipFire;


    [Tooltip("Firing Particles.")]
    [SerializeField]
    private GameObject prefabFlashParticles;

    [Tooltip("Number of particles to emit when firing.")]
    [SerializeField]
    private int flashParticlesCount = 5;

    [Tooltip("Muzzle Flash Prefab. A small light we use when firing.")]
    [SerializeField]
    private GameObject prefabFlashLight;

    [Tooltip("Time that the light flashed stays active. After this time, it is disabled.")]
    [SerializeField]
    private float flashLightDuration;

    [Tooltip("Local offset applied to the light.")]
    [SerializeField]
    private Vector3 flashLightOffset;

    private ParticleSystem particles;
    /// <summary>
    /// Instantiated light.
    /// </summary>
    private Light flashLight;

    private void Awake()
    {
        //Null Check.
        if (prefabFlashParticles != null)
        {
            //Instantiate Particles.
            GameObject spawnedParticlesPrefab = Instantiate(prefabFlashParticles, socket);
            //Reset the position.
            spawnedParticlesPrefab.transform.localPosition = default;
            //Reset the rotation.
            spawnedParticlesPrefab.transform.localEulerAngles = default;

            //Get Reference.
            particles = spawnedParticlesPrefab.GetComponent<ParticleSystem>();
        }

        //Null Check.
        if (prefabFlashLight)
        {
            //Instantiate.
            GameObject spawnedFlashLightPrefab = Instantiate(prefabFlashLight, socket);
            //Reset the position.
            spawnedFlashLightPrefab.transform.localPosition = flashLightOffset;
            //Reset the rotation.
            spawnedFlashLightPrefab.transform.localEulerAngles = default;

            //Get reference.
            flashLight = spawnedFlashLightPrefab.GetComponent<Light>();
            //Disable.
            flashLight.enabled = false;
        }
    }

    public  void Effect()
    {
        //Try to play the fire particles from the muzzle!
        if (particles != null)
            particles.Emit(flashParticlesCount);

        AudioManagerServices.instance.PlayOneShot(audioClipFire,new AudioSettings(1.0f,0.0f,true));
        //Make sure that we have a light to flash!
        if (flashLight != null)
        {
            //Enable the light.
            flashLight.enabled = true;
            //Disable the light after a few seconds.
            StartCoroutine(nameof(DisableLight));
        }
    }

    public  Transform GetSocket() => socket;

    public  Sprite GetSprite() => sprite;
    //public  AudioClip GetAudioClipFire() => audioClipFire;

    public  ParticleSystem GetParticlesFire() => particles;
    public  int GetParticlesFireCount() => flashParticlesCount;

    public  Light GetFlashLight() => flashLight;
    public  float GetFlashLightDuration() => flashLightDuration;
    public AudioClip GetAudioClipFire() => audioClipFire;

    private IEnumerator DisableLight()
    {
        //Wait.
        yield return new WaitForSeconds(flashLightDuration);
        //Disable.
        flashLight.enabled = false;
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
