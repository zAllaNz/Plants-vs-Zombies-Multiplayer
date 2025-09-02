using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirassolPetalasCiclo : MonoBehaviour
{
    public List<GameObject> petalas;
    private bool sumindo = true;

    void Start()
    {
        StartCoroutine(CicloPetalas());
    }

    IEnumerator CicloPetalas()
    {
        while (true)
        {
            if (sumindo)
            {
                for (int i = 0; i < petalas.Count; i++)
                {
                    petalas[i].SetActive(false);
                    yield return new WaitForSeconds(1f);
                }

                sumindo = false;
            }
            else
            {
                for (int i = 0; i < petalas.Count; i++)
                {
                    petalas[i].SetActive(true);
                    yield return new WaitForSeconds(1f);
                }

                sumindo = true;
            }
        }
    }
}
