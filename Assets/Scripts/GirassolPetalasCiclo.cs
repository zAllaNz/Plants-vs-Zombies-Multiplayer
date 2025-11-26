using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirassolPetalasCiclo : MonoBehaviour
{
    public List<GameObject> petalas;
    private bool sumindo = true;
    public float tempoEntrePetalas = 0.5f; // Tempo entre uma pétala e outra cair

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
                // FASE 1: Pétalas caindo uma por uma
                for (int i = 0; i < petalas.Count; i++)
                {
                    // Pega o script da pétala e manda cair
                    Petala scriptPetala = petalas[i].GetComponent<Petala>();
                    if (scriptPetala != null)
                    {
                        scriptPetala.IniciarQueda();
                    }

                    // Espera um tempinho vendo ela cair antes de esconder
                    yield return new WaitForSeconds(0.2f); 
                    
                    // Esconde a pétala (não destrói)
                    petalas[i].SetActive(false);
                    
                    // Espera para a próxima pétala cair
                    yield return new WaitForSeconds(tempoEntrePetalas);
                }

                sumindo = false;
                // Pausa dramática com o girassol "careca"
                yield return new WaitForSeconds(0.5f); 
            }
            else
            {
                // FASE 2: Pétalas renascendo
                for (int i = 0; i < petalas.Count; i++)
                {
                    Petala scriptPetala = petalas[i].GetComponent<Petala>();
                    if (scriptPetala != null)
                    {
                        // Reseta a posição para o lugar original antes de mostrar
                        scriptPetala.ResetarPosicao();
                    }
                    
                    petalas[i].SetActive(true);
                    yield return new WaitForSeconds(tempoEntrePetalas);
                }

                sumindo = true;
                // Pausa com o girassol completo
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}