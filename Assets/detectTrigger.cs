using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectTrigger : MonoBehaviour
{
    public Collider2D plat;
    private void Update()
    {
        /*if (plat.bounds.Intersects(GetComponent<Collider2D>().bounds))
        {
            Debug.Log("dentro");
        }
        else
        {
            Debug.Log("fuera");
        }*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("se ha entrado");
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("se queda");
    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("se sale");
    }
}
