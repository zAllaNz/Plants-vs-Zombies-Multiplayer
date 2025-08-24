// zombie.cs 
using UnityEngine;

public class zombie : MonoBehaviour
{
    [Header("Atributos do Zumbi")]
    public float velocidade = 1.0f;
    public int saude = 100;

    [Header("Atributos de Ataque")]
    public int dano = 10;
    public float taxaAtaque = 1.0f; // Ataca a cada x segundos
    public float alcanceAtaque = 0.1f; // O quão perto a planta precisa estar
    public LayerMask camadaPlanta; // A Layer em que as plantas estão

    // Variáveis internas
    private float proximoAtaque;
    private bool estaAtacando = false;

    // O método Update é chamado a cada frame
    void Update()
    {
        // 1. Solta um raio invisível para frente para detectar plantas
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, alcanceAtaque, camadaPlanta);
        Debug.DrawRay(transform.position, Vector2.left * alcanceAtaque, Color.red);

        // 2. Verifica se o raio atingiu uma planta
        if (hit.collider != null)
        {
            // Se sim, o zumbi para de andar e começa a atacar
            estaAtacando = true;
            Atacar(hit.collider.gameObject);
        }
        else
        {
            // Se não, o zumbi volta a andar
            estaAtacando = false;
            Mover();
        }

        // 3. Verifica se a saúde chegou a zero
        if (saude <= 0)
        {
            Morrer();
        }
    }

    void Mover()
    {
    
        transform.position += Vector3.left * velocidade * Time.deltaTime;
    }

    void Atacar(GameObject planta)
    {
        // Controla a velocidade de ataque usando um temporizador
        if (Time.time >= proximoAtaque)
        {
            // Pega o script de vida da planta
            Plant vidaPlanta = planta.GetComponent<Plant>();

            // Se o script existir, causa dano
            if (vidaPlanta != null)
            {
                vidaPlanta.TakeDamage(dano);
            }

            // Define o tempo do próximo ataque
            proximoAtaque = Time.time + taxaAtaque;
        }
    }

    void Morrer()
    {
        // Adicionar uma animação de morte, som, etc. futuramente 
        Destroy(gameObject);
    }
}