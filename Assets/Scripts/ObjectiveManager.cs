using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [Header("Goal")]
    public int leversRequired = 3;

    [Header("UI")]
    public TMP_Text leverCounterText;

    public int LeversFlipped { get; private set; } = 0;
    public bool HatchUnlocked => LeversFlipped >= leversRequired;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void RegisterLeverFlipped()
    {
        LeversFlipped++;
        if (LeversFlipped > leversRequired) LeversFlipped = leversRequired;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (leverCounterText != null)
        {
            leverCounterText.text = $"Levers: {LeversFlipped}/{leversRequired}";
        }
    }
}
