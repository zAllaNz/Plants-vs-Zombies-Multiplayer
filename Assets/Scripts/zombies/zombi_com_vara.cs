
using UnityEngine;
using System.Collections; 

public class ZumbiSaltoComVara : zombie 
{
    [Header("Atributos do Salto com Vara")]
    public float velocidadeComVara = 2.5f; // Zumbi com vara é mais rápido
    public float alturaSalto = 1.0f;
    public float duracaoSalto = 0.8f;

    // Variáveis de controle internas
    private bool temVara = true;
    private bool estaSaltando = false;

    protected override void Update()
    {
        // Se o zumbi estiver no meio de um salto, não faça mais nada
        if (estaSaltando)
        {
            return;
        }

        // Se a saúde acabar, ele morre, não importa o que esteja fazendo
        if (saude <= 0)
        {
            Morrer();
            return; // Sai do método
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, alcanceAtaque, camadaPlanta);
        Debug.DrawRay(transform.position, Vector2.left * alcanceAtaque, Color.cyan);

        if (hit.collider != null)
        {
            if (temVara)
            {
                
                StartCoroutine(SaltarSobrePlanta());
            }
            else // Se não tem mais a vara, ele ataca normalmente
            {
                estaAtacando = true;
                Atacar(hit.collider.gameObject);
            }
        }
        else
        {
            // Se não encontrou nada, ele anda
            estaAtacando = false;
            Mover();
        }
    }

    // Também vamos sobrescrever o método Mover
    protected override void Mover()
    {
        // Se ele ainda tem a vara, usa a velocidade rápida
        if (temVara)
        {
            transform.position += Vector3.left * velocidadeComVara * Time.deltaTime;
        }
        else 
        {
            
            base.Mover();
        }
    }

    // funão do salto
    private IEnumerator SaltarSobrePlanta()
    {
        estaSaltando = true;

        Vector3 posicaoInicial = transform.position;
        // O zumbi vai pular um pouco para frente da sua posição atual
        Vector3 posicaoFinal = new Vector3(posicaoInicial.x - 1.5f, posicaoInicial.y, posicaoInicial.z);

        float tempoDecorrido = 0;

        // Loop que executa a cada frame durante a duração do salto
        while (tempoDecorrido < duracaoSalto)
        {
            tempoDecorrido += Time.deltaTime;
            float progresso = tempoDecorrido / duracaoSalto;

            float x = Mathf.Lerp(posicaoInicial.x, posicaoFinal.x, progresso);
            float y = posicaoInicial.y + Mathf.Sin(progresso * Mathf.PI) * alturaSalto;

            transform.position = new Vector3(x, y, posicaoInicial.z);

            yield return null; 
        }

        // Garante que o zumbi termine na posição exata
        transform.position = posicaoFinal;

        // Ao final do salto
        estaSaltando = false;
        temVara = false; // O zumbi perde a vara

    }
}