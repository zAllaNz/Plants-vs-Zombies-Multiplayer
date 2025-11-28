using UnityEngine;

public class AutoDestruir : MonoBehaviour
{
    public float tempoDeVida = 1.0f; // A imagem some após 1 segundo

    void Start()
    {
        // O objeto destrói a si mesmo depois do tempo definido
        Destroy(gameObject, tempoDeVida);
    }
}