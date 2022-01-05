using UnityEngine;
using System;

public class Block : MonoBehaviour
{
    public event Action OnBeingHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnBeingHit != null)
        {
            OnBeingHit();
        }
        // Desabilitar o spriteRenderer do object. Pois se clicar em cima do object ele pode começar sumido.

        //gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
