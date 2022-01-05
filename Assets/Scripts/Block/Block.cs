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
        // Colocar uma anima��o de desaparecer
        // Desabilitar o spriteRenderer do object. Pois se clicar em cima do object ele pode come�ar sumido.
        gameObject.SetActive(false);
    }
}
