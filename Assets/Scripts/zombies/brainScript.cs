using UnityEngine;

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
}
