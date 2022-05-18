using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBossController : MonoBehaviour
{
    public Vector3 destino;
    public float speed;
    Vector3 dir;

    private void Start()
    {
        dir = (destino - gameObject.transform.position).normalized;
    }
    void Update()
    {
        transform.position += dir * Time.deltaTime * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("player"))
        {
            if (collision.transform.GetComponent<HeroController>().vulnerable)
            {
                collision.transform.GetComponent<HeroController>().hit();
            }
            Destroy(gameObject);
        }
    }
}
