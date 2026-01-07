using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   

public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen Settings (READ NOTE IN SCRIPT)")]
    public float maxOxygen = 100f;
    public float baseDrainPerSecond = 1f;
    public float sprintMultiplier = 2f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Other")]
    public int index = 4;
    // When changing the max oxygen, change "max value" in the slider UI
    public Slider oxygenSlider;   
    private float currentOxygen;
    private bool isDead = false;

    void Start()
    {
        currentOxygen = maxOxygen;

        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = maxOxygen;
        }
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

        if (currentOxygen <= 0f)
        {
            HandleDeath();
        }
    }

    // Call this from air pickups
    public void AddOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);

        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxygen;
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        SceneManager.LoadScene(index);
    }
}
