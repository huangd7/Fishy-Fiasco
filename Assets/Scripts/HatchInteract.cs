using UnityEngine;
using UnityEngine.SceneManagement;

public class HatchInteract : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public int winSceneBuildIndex = 4;

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (ObjectiveManager.Instance.HatchUnlocked)
            InteractionPrompt.Instance?.Show("Press E to escape");
        else
            InteractionPrompt.Instance?.Show("Hatch locked (find 3 levers)");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        InteractionPrompt.Instance?.Hide();
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(interactKey) && ObjectiveManager.Instance.HatchUnlocked)
        {
            SceneManager.LoadScene(winSceneBuildIndex);
        }
    }
}
