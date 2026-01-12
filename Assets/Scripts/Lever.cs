using UnityEngine;

public class Lever : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;
    private bool flipped = false;
    public AudioClip leverSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !flipped)
        {
            playerInRange = true;
            InteractionPrompt.Instance?.Show("Press E to interact");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionPrompt.Instance?.Hide();
        }
    }

    private void Update()
    {
        if (!playerInRange || flipped) return;

        if (Input.GetKeyDown(interactKey))
        {
            FlipLever();
        }
    }

    void FlipLever()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && leverSound != null)
        {
            audio.PlayOneShot(leverSound);
        }

        flipped = true;

        ObjectiveManager.Instance?.RegisterLeverFlipped();

        var r = GetComponentInChildren<Renderer>();
        if (r != null) r.material.color = Color.gray;

        InteractionPrompt.Instance?.Hide();
        GetComponent<Collider>().enabled = false;
    }
}
