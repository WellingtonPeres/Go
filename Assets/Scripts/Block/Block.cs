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

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }
}
