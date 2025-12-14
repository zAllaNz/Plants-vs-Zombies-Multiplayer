using UnityEngine;

public class CabbageProjectile : MonoBehaviour
{
    public float travelDuration = 1.0f; // Tempo de voo
    public float arcHeight = 2.0f;      // Altura da curva
    public int damage = 20;             

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float startTime;
    private GameObject targetZombie; 

    // Inicializa o voo (Chamado pela planta)
    public void Initialize(Vector3 startPos, GameObject target)
    {
        startPosition = startPos;
        targetZombie = target;
        transform.position = startPosition;
        startTime = Time.time;
    }

    void Update()
    {
        // Se o zumbi morreu antes do repolho chegar, destrói o repolho
        if (targetZombie == null)
        {
            Destroy(gameObject); 
            return;
        }

        // Atualiza a posição do alvo (caso ele ande)
        targetPosition = targetZombie.transform.position;

        // Calcula a Parábola (Arco)
        float t = (Time.time - startTime) / travelDuration;
        Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
        
        // Adiciona a altura no eixo Y usando Seno
        float arc = arcHeight * Mathf.Sin(t * Mathf.PI);
        currentPos.y += arc;

        transform.position = currentPos;

        // Se chegou ao fim do tempo de viagem (t >= 1), causa dano
        if (t >= 1f)
        {
            HitZombie();
        }
    }

    void HitZombie()
    {
        if (targetZombie != null)
        {
            // LÓGICA UNIVERSAL: Busca o script no pai ou no próprio objeto
            // Garante que acerte Zumbi Jornal, Balão, etc.
            zombie scriptZumbi = targetZombie.GetComponentInParent<zombie>();
            
            if (scriptZumbi != null)
            {
                scriptZumbi.tomarDano(damage);
            }
        }
        Destroy(gameObject);
    }
}