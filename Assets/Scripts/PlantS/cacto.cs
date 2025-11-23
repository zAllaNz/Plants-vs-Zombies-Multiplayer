

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
    public LayerMask camadaZumbi;      // Em qual layer os zumbis estão

    [Header("Cacto - Ataque")]
    public GameObject prefabProjetil;    // O "espinho" que ele atira
    public Transform pontoDeDisparo;   // O local de onde o espinho sai
    public float taxaDeDisparo = 1.8f; // Atira a cada 1.8s
    private float timerDisparo;

    private bool estaEsticado = false; // Controla o estado (normal/esticado)

  
    protected override void Start()
    {
        // CHAMA A LÓGICA DO PAI (Plant.cs)
        base.Start();

        //  LÓGICA PRÓPRIA DO CACTO
        meuSpriteRenderer = GetComponent<SpriteRenderer>();
        spriteNormal = meuSpriteRenderer.sprite; // Salva o sprite "normal"
        timerDisparo = taxaDeDisparo; // Prepara o primeiro disparo
    }

    //  UPDATE É CHAMADO A CADA FRAME
    void Update()
    {
        // Roda a lógica de detecção
        VerificarZumbisBalao();

        // Roda a lógica de ataque
        timerDisparo -= Time.deltaTime;
        if (timerDisparo <= 0)
        {
            Atirar();
        }
    }

    void VerificarZumbisBalao()
    {
        // Solta um raio que detecta TODOS os zumbis na fileira
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, alcanceDeteccao, camadaZumbi);

        bool balaoEncontrado = false;

        // 5. Procura por um zumbi de balão
        foreach (RaycastHit2D hit in hits)
        {
            // Se o zumbi atingido tiver o script "zombi_balao", encontramos.
   
            if (hit.collider.GetComponent<zombie_balao>() != null)
            {
                balaoEncontrado = true;
                break; // Achamos um, não precisa mais procurar
            }
        }

        // 6. Gerencia o estado (Esticar ou Encolher)
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
        Debug.Log("FUNÇÃO ESTICAR FOI CHAMADA!");
        estaEsticado = true;
        meuSpriteRenderer.sprite = spriteEsticado;
    
    }

    void Encolher()
    {
        estaEsticado = false;
        meuSpriteRenderer.sprite = spriteNormal;
    }

    void Atirar()
    {
        timerDisparo = taxaDeDisparo; // Reseta o timer

        // Só atira se tiver um zumbi (qualquer) na frente
        RaycastHit2D hitZumbi = Physics2D.Raycast(pontoDeDisparo.position, Vector2.right, alcanceDeteccao, camadaZumbi);

        if (hitZumbi.collider != null)
        {
            Instantiate(prefabProjetil, pontoDeDisparo.position, Quaternion.identity);
        }
    }
}