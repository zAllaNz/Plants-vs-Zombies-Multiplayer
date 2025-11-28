using UnityEngine;
using System;

public class jack_zombie : zombie
{
    [Header("configuração do zombie")]
    public float tempoParaExplodir = 30.0f; // Tempo até o BOOM
    public float raioDaExplosao = 2.0f;     // Tamanho da área 
    public int danoDaExplosao = 5000;

    [Header("Visuais")]
    public GameObject efeitoExplosaoPrefab; // prefab de efeito de explosão
    public LayerMask camadaPlantas;

    // controladores internos 
    private float timer;
    private bool jaExplodiu = false;

    private void Start()
    {
        timer = tempoParaExplodir;

    }

    protected override void Update()
    {
        // mantem o comportamentp de atacar e de andar 
        base.Update();

        if (!jaExplodiu)
        {
            timer -= Time.deltaTime;
            // colocar uma musica aqui 

            if (timer <= 0)
            {
                Explodir();
            }
        }


    }
    void Explodir()
    {
        jaExplodiu = true;

        // Cria o efeito visual 
        if (efeitoExplosaoPrefab != null)
        {
            Instantiate(efeitoExplosaoPrefab, transform.position, Quaternion.identity);
        }

     
        // OverlapCircleAll cria um círculo e retorna tudo que ele tocou na layer especificada
        Collider2D[] plantasAtingidas = Physics2D.OverlapCircleAll(transform.position, raioDaExplosao, camadaPlantas);

        // Destrói as plantas
        foreach (Collider2D colisor in plantasAtingidas)
        {
            Plant planta = colisor.GetComponent<Plant>();
            if (planta != null)
            {
                planta.TakeDamage(danoDaExplosao);
            }
        }

        // O Zumbi morre na explosão
        Morrer(); // Chama a função do pai para destruir o zumbi
    }

    // desenha o raio da explosão 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDaExplosao);
    }

}
