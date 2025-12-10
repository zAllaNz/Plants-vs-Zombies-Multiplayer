using UnityEngine;
using System;

[System.Serializable]
public class zombi_jornal : zombie
{
    [Header("Atributos do Jornal")]
    public float saudeParaEnfurecer = 50;
    public GameObject jornalVisual;

    [Header("Stats Enfurecidas (Sem Jornal)")]
    public float velocidadeEnfurecido = 3.0f;
    public int danoEnfurecido = 20;
    public float taxaAtaqueEnfurecida = 0.5f;

    // Variável de controle
    private bool ce_ta_bravo = false;

    // Use override e chame base.Start() para o Animator do pai 
    protected override void Start()
    {
        base.Start();
       
    }

    private void Enfurecer()
    {
        // Marca como enfurecido
        ce_ta_bravo = true;

        //  Remove o objeto do jornal (se houver um objeto separado)
        if (jornalVisual != null)
        {
            Destroy(jornalVisual);
        }

        // Atualiza os status para ficar "monstrão"
        velocidade = velocidadeEnfurecido;
        dano = danoEnfurecido;
        taxaAtaque = taxaAtaqueEnfurecida;

    
        if (anim != null)
        {
            anim.SetBool("IsEnraged", true);
        }
    }

    public override void tomarDano(int danoRecebido)
    {
        // Se já está bravo ou morto, segue a vida normal
        if (ce_ta_bravo || saude <= 0)
        {
            base.tomarDano(danoRecebido);
            return;
        }

        base.tomarDano(danoRecebido);

        // Verifica se é hora de ficar enfurecido
        if (!ce_ta_bravo && saude <= saudeParaEnfurecer)
        {
            Enfurecer();
        }
    }
}