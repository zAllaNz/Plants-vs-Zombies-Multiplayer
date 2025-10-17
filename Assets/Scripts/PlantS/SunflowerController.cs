using System.Collections;
using UnityEngine;

public class SunflowerController : MonoBehaviour
{
    [Header("Setup")]
    public GameObject sunPrefab;
    public Transform spawnPoint;

    [Header("Stats")]
    public float productionTime = 15f;

    void Start()
    {
        StartCoroutine(ProduceSun());
    }

    IEnumerator ProduceSun()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionTime);

            Instantiate(sunPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}