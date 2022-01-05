using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[] blockArray;

    [SerializeField] int blockCount;

    private void Start()
    {
        blockArray = FindObjectsOfType<Block>();
        blockCount = blockArray.Length;
        SubscribeToEvent();
    }

    private void SubscribeToEvent()
    {
        foreach (Block block in blockArray)
        {
            block.OnBeingHit += DecreaseBlockCount;
        }

        FindObjectOfType<PlayerController>().OnMouseClick += ResetAllBlocks;
    }

    void DecreaseBlockCount()
    {
        blockCount--;
    }

    void ResetAllBlocks()
    {
        foreach (Block block in blockArray)
        {
            if (block.gameObject.GetComponent<SpriteRenderer>().enabled == false)
            {
                block.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                block.gameObject.GetComponent<Collider2D>().isTrigger = false;
            }
        }

        blockCount = blockArray.Length;
    }
}
