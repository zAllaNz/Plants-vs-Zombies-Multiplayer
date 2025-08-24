using UnityEngine;
using UnityEngine.Rendering;

public class brainScript : MonoBehaviour
{
    public int BrainValor = 50;

    private void OnMouseDown()
    {
        GameManager gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.Addbrains(BrainValor);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
