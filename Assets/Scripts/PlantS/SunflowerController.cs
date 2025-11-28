using System.Collections;
using UnityEngine;

public class SunflowerController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject sunPrefab; // O prefab do Sol
    // Removemos o "spawnPoint" pois agora calcularemos baseado na posição da própria planta

    [Header("Atributos")]
    public float productionTime = 10f; // Tempo para gerar sol
    public float spawnRadius = 1.5f;   // Distância máxima onde o sol pode aparecer

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ProduceSun());
    }

    IEnumerator ProduceSun()
    {
        while (true)
        {
            // Espera o tempo de produção
            yield return new WaitForSeconds(productionTime);

            // Toca a animação (se tiver um trigger chamado "Glow" ou similar)
            if (animator != null) animator.SetTrigger("Glow");

            // Gera o sol
            SpawnSun();
        }
    }

    void SpawnSun()
    {
        // 1. Gera uma posição aleatória (X, Y) dentro de um círculo
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;

        // 2. Soma essa posição aleatória à posição atual da planta
        Vector3 finalPos = transform.position + (Vector3)randomPos;

        // 3. Ajuste de Profundidade (Z)
        // Colocamos Z em -1 para garantir que o sol apareça NA FRENTE da planta e seja clicável
        finalPos.z = -1f; 

        // 4. Cria o Sol
        Instantiate(sunPrefab, finalPos, Quaternion.identity);
    }

    // Desenha um círculo amarelo no editor para você ver a área de geração
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}