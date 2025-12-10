using UnityEngine;
using System;

[System.Serializable]
public class zombie_balao : zombie
{
    [Header("Atributos do Balão")]
    public float altitudeDeVoo = 1.0f;

    private bool estaVoando = true;
    private SpriteRenderer meuSpriteRenderer;

    
    //  variável 'anim' (do pai) vai ser preenchida
    protected override void Start()
    {
        base.Start();

        meuSpriteRenderer = GetComponent<SpriteRenderer>();

        // Move o zumbi para cima na altura de voo
        transform.position += new Vector3(0, altitudeDeVoo, 0);
    }

    protected override void Update()
    {
        if (estaVoando)
        {
            // Enquanto voa, ele só se move 
            Mover();

            if (saude <= 0) Morrer();
        }
        else
        {
            // Quando cai, vira um zumbi normal (detecta plantas, come, etc)
            base.Update();
        }
    }

    public bool EstaVoando()
    {
        return estaVoando;
    }

    public override void tomarDano(int danoRecebido)
    {
        if (estaVoando)
        {
            Debug.Log("BALÃO ESTOUROU!");
            EstourarBalao();
            // Não chama base.tomarDano para o primeiro tiro não tirar vida
        }
        else
        {
            base.tomarDano(danoRecebido);
        }
    }

    private void EstourarBalao()
    {
        estaVoando = false;

        // : Avisando o Animator 
        if (anim != null)
        {
            anim.SetTrigger("estaVoando"); // Ativa a transição Flying -> Dropping
        }

    

        // Devolve o zumbi para o chão fisicamente
        transform.position -= new Vector3(0, altitudeDeVoo, 0);
    }
}