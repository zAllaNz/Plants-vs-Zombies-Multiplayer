using System.Collections;
using UnityEngine;

public class RepeaterController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject peaPrefab; // Use o mesmo prefab da Ervilha aqui
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Atributos")]
    public float fireRate = 1.5f;
    public float delayBetweenPeas = 0.15f; // Tempo entre a 1ª e a 2ª ervilha
    public float detectionRange = 10f;

    private bool isShooting = false; // Para não interromper a rajada

    void Update()
    {
        // Só inicia uma nova rajada se detectar zumbi E não estiver atirando já
        if (ZombieInLane() && !isShooting)
        {
            StartCoroutine(ShootBurst());
        }
    }

    bool ZombieInLane()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    IEnumerator ShootBurst()
    {
        isShooting = true;

        // Tiro 1
        Instantiate(peaPrefab, shootingPoint.position, shootingPoint.rotation);
        
        // Espera um pouquinho
        yield return new WaitForSeconds(delayBetweenPeas);

        // Tiro 2
        Instantiate(peaPrefab, shootingPoint.position, shootingPoint.rotation);

        // Espera o resto do tempo de recarga (fireRate menos o tempo que já gastou)
        yield return new WaitForSeconds(fireRate - delayBetweenPeas);

        isShooting = false;
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.cyan; // Cor ciano para diferenciar
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
        }
    }
}