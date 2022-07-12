using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnMouseClick;

    [Header("Mudar posição ao ultrapassar o limite da câmera")]
    public bool invertPosition;

    [Space]
    public InputData inputData;
    public LayerMask layerToCollideWith;
    public float moveSpeed;

    private Vector3 clickedPosition;
    private Vector3 releasePosition;
    private Vector3 direction;

    private Rigidbody2D rigidbody2D;
    private Camera camera;
    private PlayerVisualEffects playerVisualEffects;
    private Collider2D collider2D;

    private bool hitBlock;

    private LevelManager checkCanNextLevel;
    private int increaseAmountCollidedBlocks;

    private AudioSource startPlaySFX;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerVisualEffects = GetComponent<PlayerVisualEffects>();
        collider2D = GetComponent<Collider2D>();

        camera = FindObjectOfType<Camera>();
        checkCanNextLevel = FindObjectOfType<LevelManager>();

        startPlaySFX = GetComponent<AudioSource>();
        Invoke("StartPlayFSX", 0.5f);
    }

    private void Update()
    {
        HandleMovement();
        StopPlayerPosition();
    }

    private void FixedUpdate()
    {
        ChangeTheTrajectoryOfTheBall(invertPosition);
    }

    private void ChangeTheTrajectoryOfTheBall(bool canInvertPosition)
    {
        if (canInvertPosition)
        {
            Vector3 porcentagem = Camera.main.WorldToViewportPoint(transform.position);
            if (porcentagem.x > 1 || porcentagem.x < 0 || porcentagem.y > 1 || porcentagem.y < 0)
            {
                transform.position = -transform.position;
            }
        }
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

            SFXManager.instance.PlaySFX();
        }

        if (collision.gameObject.CompareTag("BlockWall"))
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            direction = Vector2.Reflect(rigidbody2D.velocity, wallNormal).normalized;

            rigidbody2D.velocity = direction * moveSpeed;

            SFXManager.instance.PlaySFX();
        }
    }

    void HandleMovement()
    {
        if (checkCanNextLevel.isNextLevel)
        {
            if (inputData.isPressed)
            {
                collider2D.enabled = false;

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
                playerVisualEffects.StopCoroutine("WaitForDrawTrail");

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
                Invoke("ActivePlayerCollider2D", 0.1F);

                if (hitBlock)
                {
                    return;
                }

                releasePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                releasePosition = new Vector3(releasePosition.x, releasePosition.y, 0f);

                playerVisualEffects.ChangeDotActiveState(false);
                playerVisualEffects.ResetBallSize();
                playerVisualEffects.StartCoroutine("WaitForDrawTrail");

                CalculateDirection();
                MovePlayerInDirection();
            }
        }
    }

    void ActivePlayerCollider2D()
    {
        collider2D.enabled = true;
    }

    private void StartPlayFSX()
    {
        startPlaySFX.Play();
    }

    void StopPlayerPosition()
    {
        if (transform.position.x > 50 && transform.position.x < 55 ||
            transform.position.x < -50 && transform.position.x > -55 ||
            transform.position.y > 35 && transform.position.y < 45 ||
            transform.position.y < -35 && transform.position.y > -45)
        {
            rigidbody2D.velocity = Vector2.zero;
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

    private bool CheckIfHitBlock()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, layerToCollideWith);

        return hitBlock;
    }
}
