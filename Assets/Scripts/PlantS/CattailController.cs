using System.Collections;
using UnityEngine;

public class CattailController : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject spikePrefab;
    public Transform shootingPoint;

    [Header("Shooting Stats")]
    public float fireRate = 1.0f;

    private Transform currentTarget;

    void Start()
    {
        StartCoroutine(TargetAndShoot());
    }

    IEnumerator TargetAndShoot()
    {
        while (true)
        {
            // Procura pelo alvo mais próximo
            FindTarget();

            // Se houver um alvo válido, atira
            if (currentTarget != null)
            {
                Shoot();
            }

            // Espera para o próximo tiro
            yield return new WaitForSeconds(fireRate);
        }
    }

    void FindTarget()
    {
        // Encontra TODOS os game objects com a tag "Zombie" na cena
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        Transform closestZombie = null;
        float minX = float.MaxValue;

        if (zombies.Length == 0)
        {
            currentTarget = null;
            return;
        }

        // Itera por todos os zumbis encontrados para achar o que está mais à esquerda
        foreach (GameObject zombie in zombies)
        {
            if (zombie.transform.position.x < minX)
            {
                minX = zombie.transform.position.x;
                closestZombie = zombie.transform;
            }
        }

        currentTarget = closestZombie;
    }

    void Shoot()
    {

        // Cria o projétil no ponto de tiro
        GameObject spikeGO = Instantiate(spikePrefab, shootingPoint.position, Quaternion.identity);

        // Pega o script do projétil e informa a ele qual é o alvo
        spikeGO.GetComponent<CattailSpikeController>().SetTarget(currentTarget);
    }
}