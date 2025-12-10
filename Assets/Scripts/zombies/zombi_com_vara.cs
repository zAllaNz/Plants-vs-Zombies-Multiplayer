using UnityEngine;
using System.Collections;

public class ZumbiSaltoComVara : zombie
{
    [Header("Atributos do Salto com Vara")]
    public float velocidadeComVara = 2.5f;
    public float alturaSalto = 1.0f;
    public float duracaoSalto = 1.0f; // Ajuste para bater com o tempo da animação!

    // Variáveis de controle
    private bool temVara = true;
    private bool estaSaltando = false;

    // O Start é importante para configurar o estado inicial
    protected override void Start()
    {
        base.Start(); // Pega o Animator do pai
        // O zumbi já nasce no estado "Idle" (que na sua imagem é ele correndo com vara)
    }

    protected override void Update()
    {
        // Se estiver pulando, não faz nada 
        if (estaSaltando) return;

        // Se morreu
        if (saude <= 0)
        {
            Morrer();
            return;
        }

        // Detecta plantas
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, alcanceAtaque, camadaPlanta);
        Debug.DrawRay(transform.position, Vector2.left * alcanceAtaque, Color.cyan);

        if (hit.collider != null)
        {
            if (temVara)
            {
                // SE TEM VARA: PULA
                StartCoroutine(SaltarSobrePlanta());
            }
            else
            {
                // SE NÃO TEM VARA: ATACA (Como zumbi normal)
                estaAtacando = true;

                // Avisa o Animator para ir de "Walk" -> "Attack"
                if (anim != null) anim.SetBool("IsAttacking", true);

                Atacar(hit.collider.gameObject);
            }
        }
        else
        {
            // SE NÃO TEM PLANTA NA FRENTE
            estaAtacando = false;

            // Avisa o Animator para ir de "Attack" -> "Walk"
            if (anim != null) anim.SetBool("IsAttacking", false);

            Mover();
        }
    }

    protected override void Mover()
    {
        // Se tem vara, corre rápido. Se não, usa o movimento normal do pai (lento)
        if (temVara)
        {
            transform.position += Vector3.left * velocidadeComVara * Time.deltaTime;
        }
        else
        {
            base.Mover();
        }
    }

    private IEnumerator SaltarSobrePlanta()
    {
        estaSaltando = true;

        
        // Dispara a transição Idle -> Jump 
        if (anim != null) anim.SetTrigger("Jump");

        // Proteção matemática
        if (duracaoSalto <= 0.1f) duracaoSalto = 1.0f;

        Vector3 posicaoInicial = transform.position;
        Vector3 posicaoFinal = new Vector3(posicaoInicial.x - 1.5f, posicaoInicial.y, posicaoInicial.z);

        float tempoDecorrido = 0;

        while (tempoDecorrido < duracaoSalto)
        {
            tempoDecorrido += Time.deltaTime;
            float progresso = tempoDecorrido / duracaoSalto;

            // Arco do pulo
            float x = Mathf.Lerp(posicaoInicial.x, posicaoFinal.x, progresso);
            float y = posicaoInicial.y + Mathf.Sin(progresso * Mathf.PI) * alturaSalto;

            transform.position = new Vector3(x, y, posicaoInicial.z);

            yield return null;
        }

        // Aterrissagem
        transform.position = new Vector3(posicaoFinal.x, posicaoInicial.y, posicaoInicial.z);

        estaSaltando = false;
        temVara = false; // Perde a vara

      
    }
}