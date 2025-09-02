using UnityEngine;

public class Petala : MonoBehaviour
{
    private bool cair = false;
    public float velocidadeQueda = 2f;
    public float velocidadeRotacao = 100f;

    void Update()
    {
        if (cair)
        {
            // Movimento de queda
            transform.Translate(Vector3.down * velocidadeQueda * Time.deltaTime, Space.World);

            // Rotação durante a queda
            transform.Rotate(Vector3.forward * velocidadeRotacao * Time.deltaTime);
        }
    }

    public void Cair()
    {
        cair = true;
        // Destroi a pétala após 2 segundos de queda
        Destroy(gameObject, 2f);
    }
}
