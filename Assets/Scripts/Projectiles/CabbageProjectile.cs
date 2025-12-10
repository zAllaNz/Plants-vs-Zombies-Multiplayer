using UnityEngine;

public class CabbageProjectile : MonoBehaviour
{
    public float travelDuration = 1.0f;
    public float arcHeight = 2.5f;
    public int damage = 20;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float startTime;
    private GameObject targetZombie; 

    public void Initialize(Vector3 startPos, GameObject target)
    {
        startPosition = startPos;
        targetZombie = target;
        transform.position = startPosition;
        startTime = Time.time;
    }

    void Update()
    {
        if (targetZombie == null)
        {
            Destroy(gameObject); 
            return;
        }

        targetPosition = targetZombie.transform.position;
        float t = (Time.time - startTime) / travelDuration;
        Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
        float arc = arcHeight * Mathf.Sin(t * Mathf.PI);
        currentPos.y += arc;
        transform.position = currentPos;

        if (t >= 1f)
        {
            HitZombie();
        }
    }

    void HitZombie()
    {
        // Se o alvo ainda existe
        if (targetZombie != null)
        {

            // Tenta pegar o script especifico do balão
            zombie_balao scriptBalao = targetZombie.GetComponent<zombie_balao>();

            // Se for um balão E estiver voando
            if (scriptBalao != null && scriptBalao.EstaVoando())
            {
                return; // Sai da função imediatamente
            }
            // Busca o script no alvo
            zombie scriptZumbi = targetZombie.GetComponent<zombie>();
            
            if (scriptZumbi != null)
            {
                scriptZumbi.tomarDano(damage);
            }
        }
        Destroy(gameObject);
    }
}