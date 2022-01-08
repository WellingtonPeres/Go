using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAnimation : MonoBehaviour
{
    [Header("Array PingPong points")]
    public bool isPingPong;
    public Transform[] pingPongPoints;
    public float moveSpeed;
    [SerializeField] private int currentPoint = 0;

    [Header("Rotation Self")]
    public bool isRotation;
    public float rotationSpeed;

    [Header("Rotation Object Children")]
    public GameObject children;
    public bool isChildrenRotation;
    public float rotationChildrenSpeed;

    void Start()
    {
        children.transform.position = pingPongPoints[currentPoint].position;
    }

    void Update()
    {
        if (isPingPong)
        {
            if (children.transform.position == pingPongPoints[0].position)
            {
                currentPoint = 1;
            }
            else if (children.transform.position == pingPongPoints[1].position)
            {
                currentPoint = 0;
            }

            children.transform.position = Vector3.MoveTowards(children.transform.position,
                                        pingPongPoints[currentPoint].position,
                                        moveSpeed * Time.deltaTime);
        }

        if (isRotation)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
        }

        if (isChildrenRotation)
        {
            children.transform.Rotate(0, 0, rotationChildrenSpeed * Time.deltaTime, Space.World);
        }
    }
}
