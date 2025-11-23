using UnityEngine;
using System;


[System.Serializable]

public class zombi_jornal : zombie
{
    [Header("Atributos do Jornal")]
    public float saudeParaEnfurecer = 50; // Com quanta vida ele perde o jornal
    public GameObject jornalVisual; // Arraste o sprite do jornal aqui ( no momento não vou fazer isso) 

    [Header("Stats Enfurecidas (Sem Jornal)")]
    public float velocidadeEnfurecido = 3.0f;
    public int danoEnfurecido = 20;
    public float taxaAtaqueEnfurecida = 0.5f;

    [Header("Controle Visual")]
    public Sprite spriteEnfurecido; // Arraste a imagem "zombi com raiva" aqui
    private SpriteRenderer meuSpriteRenderer;

    // Variável de controle
    private bool ce_ta_bravo = false;

    void Start()
    {
        // Pega o SpriteRenderer do *próprio* objeto
        meuSpriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Enfurecer()
    {
        

        //Marca como enfurecido (para não rodar isso de novo)
        ce_ta_bravo = true;

        // 2. Remove o visual do jornal
        if (jornalVisual != null)
        {
            Destroy(jornalVisual);
        }

        
        velocidade = velocidadeEnfurecido;
        dano = danoEnfurecido;
        taxaAtaque = taxaAtaqueEnfurecida;

        // Allan, muda a animação aqui 
        // GetComponent<Animator>().SetBool("isEnraged", true);

        if (meuSpriteRenderer != null && spriteEnfurecido != null)
        {
            meuSpriteRenderer.sprite = spriteEnfurecido;
        }
    }

    public override void tomarDano(int danoRecebido)
    {
        // Não faz nada se já estiver enfurecido ou morto
        if (ce_ta_bravo || saude <= 0)
        {
            base.tomarDano(danoRecebido); // Continua tomando dano normal
            return;
        }

        // Chama a função base para reduzir a saúde
        base.tomarDano(danoRecebido);

        // Verifica se é hora de ficar enfurecido
        // Checa se a saúde caiu abaixo do limite E se ele ainda não está enfurecido
        if (!ce_ta_bravo && saude <= saudeParaEnfurecer)
        {
            Enfurecer();
        }
    }

    

}
