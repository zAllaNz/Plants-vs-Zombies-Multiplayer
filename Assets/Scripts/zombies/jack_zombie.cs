using UnityEngine;
using System;

public class jack_zombie : zombie
{
    [Header("Configuração do Zombie")]
    public float tempoParaExplodir = 30.0f;
    public float raioDaExplosao = 2.0f;
    public int danoDaExplosao = 5000;

    [Header("Visuais")]
    public GameObject efeitoExplosaoPrefab; 
    public LayerMask camadaPlantas;

    // Controladores internos 
    private float timer;
    private bool jaExplodiu = false;

    
    protected override void Start()
    {
        base.Start(); // Inicializa o Animator do pai
        timer = tempoParaExplodir; //  Configura o timer
    }

    protected override void Update()
    {
        // Mantém o comportamento de atacar e de andar 
        base.Update();

        if (!jaExplodiu)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Explodir();
            }
        }
    }

    void Explodir()
    {
        jaExplodiu = true;

        // efeito do prefab
        if (efeitoExplosaoPrefab != null)
        {
            Instantiate(efeitoExplosaoPrefab, transform.position, Quaternion.identity);
        }

        // CAUSA O DANO NA ÁREA 
        Collider2D[] plantasAtingidas = Physics2D.OverlapCircleAll(transform.position, raioDaExplosao, camadaPlantas);

        foreach (Collider2D colisor in plantasAtingidas)
        {
            Plant planta = colisor.GetComponent<Plant>();
            if (planta != null)
            {
                planta.TakeDamage(danoDaExplosao);
            }
        }

        // DESTRÓI O ZUMBI (O Fim)
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioDaExplosao);
    }
}