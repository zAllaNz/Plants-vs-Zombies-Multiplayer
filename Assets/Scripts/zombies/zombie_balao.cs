using UnityEngine;
using System; 

[System.Serializable]
public class zombie_balao : zombie
{
    [Header("Atributos do Balão")]
    public float altitudeDeVoo = 1.0f; // Quão mais alto ele voa
    public GameObject balaoVisual;     // Arraste o Sprite do balão aqui
    public Sprite spriteCaido;         // A imagem dele sem o balão

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

    // Substitui a lógica do Pai
    protected override void Update()
    {
        if (estaVoando)
        {
            // ...ele ignora as plantas e apenas se move para a esquerda.
            Mover(); // A função Mover() do pai ainda funciona!

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
            // O primeiro tiro ESTOURA o balão em vez de dar dano.
            Debug.Log("BALÃO ESTOUROU!");
            EstourarBalao();

            // Não chamamos base.tomarDano() para que o "1 tiro"
            // apenas estoure o balão e não tire vida.
        }
       
        else
        {

            base.tomarDano(danoRecebido);
        }
    }

    // A função de transição
    private void EstourarBalao()
    {
        estaVoando = false; // Muda o estado

        //  Remove o visual do balão
        if (balaoVisual != null)
        {
            Destroy(balaoVisual);
        }

        // Troca o sprite principal para "caído"
        if (meuSpriteRenderer != null && spriteCaido != null)
        {
            meuSpriteRenderer.sprite = spriteCaido;
        }

        //  Devolve o zumbi para o chão
        transform.position -= new Vector3(0, altitudeDeVoo, 0);

        
    }
}