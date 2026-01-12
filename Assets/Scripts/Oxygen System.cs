using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen Settings (READ NOTE IN SCRIPT)")]    // When changing the max oxygen, change "max value" in the slider UI. also change the low oxygen section 

    public float maxOxygen = 100f;
    public float baseDrainPerSecond = 1f;
    public float sprintMultiplier = 2f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Other")]
    public int index = 4;
    public Slider oxygenSlider;

    [Header("Low Oxygen Warning Sound")]
    public float lowOxygenThreshold = 25f;   
    public AudioClip sound;
    public float volume = 1f;
    private float currentOxygen;
    private bool isDead = false;
    private AudioSource audioSource;
    private bool lowOxygenPlaying = false;

    void Start()
    {
        currentOxygen = maxOxygen;

        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = maxOxygen;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; 
    }

    void Update()
    {
        if (isDead) return;

        float drainRate = baseDrainPerSecond;
        if (Input.GetKey(sprintKey))
        {
            drainRate *= sprintMultiplier;
        }

        currentOxygen -= drainRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxygen;
        }

        HandleLowOxygenSound();

        if (currentOxygen <= 0f)
        {
            HandleDeath();
        }
    }

    public void AddOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);
        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxygen;
        }
        HandleLowOxygenSound();
    }

    private void HandleLowOxygenSound()
    {
        if (audioSource == null || sound == null) return;

        if (currentOxygen <= lowOxygenThreshold)
        {
            if (!lowOxygenPlaying)
            {
                audioSource.clip = sound;
                audioSource.loop = true;
                audioSource.volume = volume;
                audioSource.Play();
                lowOxygenPlaying = true;
            }
        }
        else
        {
            if (lowOxygenPlaying)
            {
                audioSource.Stop();
                lowOxygenPlaying = false;
            }
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        if (audioSource != null) audioSource.Stop();
        SceneManager.LoadScene(index);
    }
}
