using UnityEngine;
using System; 

[System.Serializable]
public class zombie_balao : zombie
{
    [Header("Atributos do Bal�o")]
    public float altitudeDeVoo = 1.0f; // Qu�o mais alto ele voa
    public GameObject balaoVisual;     // Arraste o Sprite do bal�o aqui
    public Sprite spriteCaido;         // A imagem dele sem o bal�o

    // Nossos interruptores de estado
    private bool estaVoando = true;
    private SpriteRenderer meuSpriteRenderer;

    //Configurando o zumbi 
    void Start()
    {
        meuSpriteRenderer = GetComponent<SpriteRenderer>();

       
        // Move o zumbi para cima na altura de voo
        transform.position += new Vector3(0, altitudeDeVoo, 0);

    }

    // Substitui a l�gica do Pai
    protected override void Update()
    {
        if (estaVoando)
        {
            // ...ele ignora as plantas e apenas se move para a esquerda.
            Mover(); // A fun��o Mover() do pai ainda funciona!

            // Checa se morreu (caso algo mate ele no ar)
            if (saude <= 0)
            {
                Morrer(); // Morrer voando
            }
        }
        
        else
        {
            // (que inclui o Raycast para detectar plantas e atacar)
            base.Update();
        }
    }

    //  O gatilho para estourar
    public override void tomarDano(int danoRecebido)
    {
     
        if (estaVoando)
        {
            // O primeiro tiro ESTOURA o bal�o em vez de dar dano.
            Debug.Log("BAL�O ESTOUROU!");
            EstourarBalao();

            // N�o chamamos base.tomarDano() para que o "1 tiro"
            // apenas estoure o bal�o e n�o tire vida.
        }
       
        else
        {

            base.tomarDano(danoRecebido);
        }
    }

    // A fun��o de transi��o
    private void EstourarBalao()
    {
        estaVoando = false; // Muda o estado

        //  Remove o visual do bal�o
        if (balaoVisual != null)
        {
            Destroy(balaoVisual);
        }

        // Troca o sprite principal para "ca�do"
        if (meuSpriteRenderer != null && spriteCaido != null)
        {
            meuSpriteRenderer.sprite = spriteCaido;
        }

        //  Devolve o zumbi para o ch�o
        transform.position -= new Vector3(0, altitudeDeVoo, 0);

        
    }
    public bool IsFlying()
    {
        return estaVoando;
    }
}