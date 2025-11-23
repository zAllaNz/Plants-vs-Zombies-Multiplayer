
using System.Collections;
using UnityEngine;

public class BungeeZombie : MonoBehaviour
      
{
    [Header("Alvo")]
    // Esta variável será definida pelo hab_bungee.cs no momento da invocação
    public GameObject plantaAlvo;


    [Header("controle d etempo de velocidade")]
    // Tempo que o zumbi espera antes de "cair" e sequestrar
    public float tempoDeEspera = 6.0f;
    public float VelocidadeDeDescida = 20f;
    public float  VelocidadeDeSubida = 15f;

    [Header("posições")]
    public Vector3 alturaSpawn = new Vector3(0, 3, 0); // Quão alto ele aparece
    private Vector3 posicaoInicial; // Posição exata no céu
    private Vector3 posicaoAlvoVetor; // Posição exata da planta
    private Vector3 ajusteDeAltura = new Vector3(0, 1, 0);


    void Start()
    {
        // Garante que temos um alvo
        if (plantaAlvo == null)
        {
            Debug.LogError("BungeeZombie foi invocado sem uma plantaAlvo!");
            Destroy(gameObject); // Se não tem alvo, se auto-destrói
            return;
            }

            posicaoAlvoVetor = plantaAlvo.transform.position;

            // Calcula e salva a posição inicial 
            posicaoInicial = posicaoAlvoVetor + alturaSpawn;

            // Define a posição inicial do zumbi
            transform.position = posicaoInicial;

            StartCoroutine(SequestrarPlantaRoutine());
    }

    IEnumerator SequestrarPlantaRoutine()
    {

        // É AQUI QUE VOCÊ COLOCA A ANIMAÇÃO DELE DESCENDO ALLAN ---
        // (Por enquanto, vamos fazer sem animação)

        // 1 descida 
        while (Vector3.Distance(transform.position, posicaoAlvoVetor+ajusteDeAltura) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
             transform.position,      // Posição atual
             posicaoAlvoVetor+ajusteDeAltura,        // Posição do alvo
             VelocidadeDeDescida * Time.deltaTime);

            yield return null;

        }
        ;

        // fica parado 
        // O jogador vê o zumbi lá em cima (ou a sombra dele)
        Debug.Log("BungeeZombie mirando em " + plantaAlvo.name + ". Esperando " + tempoDeEspera + "s...");
        yield return new WaitForSeconds(tempoDeEspera);
        Debug.Log("BungeeZombie SEQUESTRANDO " + plantaAlvo.name);

       

        // 2 descida 

        while (Vector3.Distance(transform.position, posicaoAlvoVetor) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
             transform.position,      // Posição atual
             posicaoAlvoVetor,        // Posição do alvo
             VelocidadeDeDescida * Time.deltaTime);

            yield return null;


        }

        // Verifica se a planta ainda existe (ela pode ter sido destruída)
        if (plantaAlvo != null)
        {
            // Pega o script da planta e chama o método Die()
            Plant scriptPlanta = plantaAlvo.GetComponent<Plant>();
            if (scriptPlanta != null)
            {
                plantaAlvo.transform.SetParent(this.transform);
            }
            else
            {
                // Se não tiver o script, apenas destrói o objeto
                Destroy(plantaAlvo);
            }
        }


        // --- É AQUI QUE VOCÊ COLOCA A ANIMAÇÃO DELE SUBINDO ALLAN ---
        //subindo 
        while (Vector3.Distance(transform.position, posicaoInicial) > 0.01f)
        {
            // Move o zumbi um pouco em direção à posição inicial
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicaoInicial,
                VelocidadeDeSubida * Time.deltaTime
            );
           

            // Espera até o próximo frame
            yield return null;
        }

        Destroy(gameObject);
        Destroy(plantaAlvo);
       
    }
}