using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public static InteractionPrompt Instance;

    public TMP_Text promptText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Show(string message = "Press E to interact")
    {
        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        promptText.gameObject.SetActive(false);
    }
}
