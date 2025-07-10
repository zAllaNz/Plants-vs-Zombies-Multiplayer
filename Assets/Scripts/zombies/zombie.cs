using UnityEngine;

public class zombie : MonoBehaviour
{
    public float velocidade;
    public int saude;

    private void FixedUpdate()
    {
        
        transform.position -= new Vector3(velocidade * Time.fixedDeltaTime, 0, 0);
    }
}