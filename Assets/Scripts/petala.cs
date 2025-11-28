using UnityEngine;

public class Petala : MonoBehaviour
{
    private bool cair = false;
    public float velocidadeQueda = 100f; // Aumentei um pouco para ser visível na UI
    public float velocidadeRotacao = 100f;
    
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    void Start()
    {
        // Salva a posição e rotação originais assim que o jogo começa
        posicaoInicial = transform.localPosition; // Usamos localPosition pois é filha do girassol
        rotacaoInicial = transform.localRotation;
    }

    void Update()
    {
        if (cair)
        {
            // Movimento de queda (em relação à tela/UI)
            transform.Translate(Vector3.down * velocidadeQueda * Time.deltaTime, Space.World);

            // Rotação durante a queda
            transform.Rotate(Vector3.forward * velocidadeRotacao * Time.deltaTime);
        }
    }

    public void IniciarQueda()
    {
        cair = true;
        // Removemos o Destroy aqui. Quem vai esconder é o outro script.
    }

    public void ResetarPosicao()
    {
        cair = false;
        transform.localPosition = posicaoInicial;
        transform.localRotation = rotacaoInicial;
    }
}