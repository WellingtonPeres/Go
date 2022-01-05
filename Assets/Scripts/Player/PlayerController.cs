using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnMouseClick;

    public InputData inputData;
    public LayerMask layerToCollideWith;
    public float moveSpeed;

    private Vector3 clickedPosition;
    private Vector3 releasePosition;
    private Vector3 direction;

    private Rigidbody2D rigidbody2D;
    private Camera camera;
    private PlayerVisualEffects playerVisualEffects;

    private bool hitBlock;

    private LevelManager checkCanNextLevel;
    private int increaseAmountCollidedBlocks;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerVisualEffects = GetComponent<PlayerVisualEffects>();

        camera = FindObjectOfType<Camera>();
        checkCanNextLevel = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (checkCanNextLevel.isNextLevel)
        {
            if (inputData.isPressed)
            {
                increaseAmountCollidedBlocks = 0;

                hitBlock = CheckIfHitBlock();

                if (hitBlock)
                {
                    return;
                }

                clickedPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                clickedPosition = new Vector3(clickedPosition.x, clickedPosition.y, 0f);

                ResetPlayerPos();

                playerVisualEffects.SetDotStartPosition(clickedPosition);
                playerVisualEffects.ChangeDotActiveState(true);
                playerVisualEffects.ChangeTrailState(false, 0f);

                if (OnMouseClick != null)
                {
                    OnMouseClick();
                }
            }

            if (inputData.isHeld)
            {
                if (hitBlock)
                {
                    return;
                }

                playerVisualEffects.SetDotPos(clickedPosition, camera.ScreenToWorldPoint(Input.mousePosition));
                playerVisualEffects.MakeBallPulse();
            }

            if (inputData.isReleased)
            {
                if (hitBlock)
                {
                    return;
                }

                releasePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                releasePosition = new Vector3(releasePosition.x, releasePosition.y, 0f);

                playerVisualEffects.ChangeDotActiveState(false);
                playerVisualEffects.ResetBallSize();
                playerVisualEffects.ChangeTrailState(true, 0.75f);

                CalculateDirection();
                MovePlayerInDirection();
            }
        }
    }

    private void CalculateDirection()
    {
        direction = (releasePosition - clickedPosition).normalized;
    }

    private void MovePlayerInDirection()
    {
        rigidbody2D.velocity = direction * moveSpeed;
    }

    void ResetPlayerPos()
    {
        transform.position = clickedPosition;
        rigidbody2D.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            direction = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = direction * moveSpeed;

            increaseAmountCollidedBlocks++;
            checkCanNextLevel.NextLevel(checkCanNextLevel.blockCountInScene, increaseAmountCollidedBlocks);
        }

        if (collision.gameObject.CompareTag("BlockWall"))
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            direction = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = direction * moveSpeed;
        }
    }

    private bool CheckIfHitBlock()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, layerToCollideWith);

        return hitBlock;
    }
}
