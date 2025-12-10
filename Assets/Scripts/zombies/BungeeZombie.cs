
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
    public float VelocidadeDeSubida = 15f;

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
        //  DESCIDA RÁPIDA 
        while (Vector3.Distance(transform.position, posicaoAlvoVetor + ajusteDeAltura) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
             transform.position,
             posicaoAlvoVetor + ajusteDeAltura,
             VelocidadeDeDescida * Time.deltaTime);

            yield return null;
        }

        // ESPERA DRAMÁTICA
        Debug.Log("BungeeZombie esperando...");
        yield return new WaitForSeconds(tempoDeEspera);

        //  DESCIDA FINAL
        while (Vector3.Distance(transform.position, posicaoAlvoVetor) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
             transform.position,
             posicaoAlvoVetor,
             VelocidadeDeDescida * Time.deltaTime);

            yield return null;
        }

        //  PEGAR A PLANTA 
        if (plantaAlvo != null)
        {
            // Transforma a planta em filha do zumbi para ela subir junto
            plantaAlvo.transform.SetParent(this.transform);

            // Desliga o script da planta para ela parar de atirar enquanto sobe
            Plant scriptPlanta = plantaAlvo.GetComponent<Plant>();
            if (scriptPlanta != null) scriptPlanta.enabled = false;
        }

        // SUBIDA PARA O ESPAÇO 

        // Define um ponto bem alto acima da posição atual (ex: +20 unidades no Y)
        // Isso garante que ele saia da tela independente de onde a planta estava
        Vector3 pontoDeFuga = transform.position + (Vector3.up * 20.0f);

        // Enquanto ele não chegar lá no alto...
        while (Vector3.Distance(transform.position, pontoDeFuga) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                pontoDeFuga, // Agora o alvo é o nosso ponto de fuga
                VelocidadeDeSubida * Time.deltaTime
            );

            yield return null;
        }

        // Tchau!
        Destroy(gameObject);

        // Se a planta ainda existir (estiver presa nele), destrói ela também
        if (plantaAlvo != null) Destroy(plantaAlvo);
    }
}