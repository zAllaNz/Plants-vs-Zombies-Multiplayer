using UnityEngine;
using System;

[System.Serializable]
public class Cacto : Plant
{
    [Header("Cacto - Visuais")]
    public Sprite spriteEsticado; // Arraste a imagem dele esticado aqui
    private Sprite spriteNormal;  // O script guarda o sprite normal sozinho
    private SpriteRenderer meuSpriteRenderer;

    [Header("Cacto - Detecção")]
    public float alcanceDeteccao = 15f; // Quão longe na fileira ele "vê"
    public LayerMask camadaZumbi;       // Em qual layer os zumbis estão (Layer "Zombie")

    [Header("Cacto - Ataque")]
    public GameObject prefabProjetil;   // O prefab "tiro_cacto"

    [Header("Ajuste de Mira (Altura)")]
    public float alturaNormal = 0f;     // A altura Y da boca quando ele está BAIXO
    public float alturaEsticado = 0.8f; // A altura Y da boca quando ele está ALTO 

    // Mudei o nome para garantir que o Inspector atualize e pare de dar erro
    public Transform LocalDeTiro;

    public float taxaDeDisparo = 1.8f;  // Atira a cada 1.8s
    private float timerDisparo;

    private bool estaEsticado = false;
    private Animator animcacto; // Controla o estado (normal/esticado)

    private float alturaAlvo;


    protected override void Start()
    {
        // 1. CHAMA A LÓGICA DO PAI (Plant.cs) para registrar no GameManager
        base.Start();

        animcacto = GetComponent<Animator>();
        meuSpriteRenderer = GetComponent<SpriteRenderer>();
        spriteNormal = meuSpriteRenderer.sprite; // Salva o sprite "normal" atual
        timerDisparo = taxaDeDisparo; // Prepara o primeiro disparo
    }

    // UPDATE É CHAMADO A CADA FRAME
    void Update()
    {
        // A. Roda a lógica de detecção (procura balões)
        VerificarZumbisBalao();

        // B. Roda a lógica de ataque (tiro normal)
        timerDisparo -= Time.deltaTime;
        if (timerDisparo <= 0)
        {
            Atirar();
        }
    }

 

    void VerificarZumbisBalao()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, alcanceDeteccao, camadaZumbi);
        bool balaoEncontrado = false;

        foreach (RaycastHit2D hit in hits)
        {
            // Pega o script do zumbi
            zombie_balao scriptBalao = hit.collider.GetComponent<zombie_balao>();

            // Verifica se o script existe E SE ELE ESTÁ VOANDO
            if (scriptBalao != null && scriptBalao.EstaVoando())
            {
                balaoEncontrado = true;
                break;
            }
        }

        // Gerencia o visual (Esticar ou Encolher)
        if (balaoEncontrado && !estaEsticado)
        {
            Esticar();
        }
        else if (!balaoEncontrado && estaEsticado)
        {
            Encolher();
        }
    }

    void Esticar()
    {
        estaEsticado = true;

        // Avisa o Animator (Visual)
        if (animcacto != null) animcacto.SetBool("TaEsticado", true);

        // Sobe a Mira (Físico)
        if (LocalDeTiro != null)
        {
            // Mantém o X e Z, muda só o Y para cima
            LocalDeTiro.localPosition = new Vector3(LocalDeTiro.localPosition.x, alturaEsticado, LocalDeTiro.localPosition.z);
        }

    }

    void Encolher()
    {
        // Debug para ver se ele entra aqui
        Debug.Log("ENCOLHENDO! Movendo mira para: " + alturaAlvo);

        estaEsticado = false;

        // 1. Avisa o Animator (Visual)
        if (animcacto != null) animcacto.SetBool("TaEsticado", false);

        // 2. Desce a Mira (Físico)
        if (LocalDeTiro != null)
        {
            // Volta o Y para a altura normal
            LocalDeTiro.localPosition = new Vector3(LocalDeTiro.localPosition.x, alturaNormal, LocalDeTiro.localPosition.z);
   
        }
    }

    void Atirar()
    {
        timerDisparo = taxaDeDisparo;

        // Desenha a linha para você ver se a mira está na altura certa
        if (LocalDeTiro != null)
        {
            Debug.DrawRay(LocalDeTiro.position, Vector2.right * alcanceDeteccao, Color.red, 0.1f);
        }

        // Usa o pontoDeDisparo atual (que já foi movido para cima ou para baixo)
        RaycastHit2D hitZumbi = Physics2D.Raycast(LocalDeTiro.position, Vector2.right, alcanceDeteccao, camadaZumbi);

        if (hitZumbi.collider != null)
        {
            // O mesmo gatilho serve para os dois ataques!
            if (animcacto != null) animcacto.SetTrigger("atirar");

            // Cria o tiro
            if (prefabProjetil != null && LocalDeTiro != null)
            {
                Instantiate(prefabProjetil, LocalDeTiro.position, Quaternion.identity);
            }
        }
    }
}
