using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour
{
    public GameObject dotPrefab;
    public int dotAmount;

    [Space]
    [Header("Line Variables")]
    public AnimationCurve followCurve;
    public float followSpeed;

    [Space]
    [Header("Pulse Variables")]
    public AnimationCurve expandCurve;
    public float expandAmount;
    public float expandSpeed;

    private Vector3 startSize;
    private Vector3 targetSize;
    private float scrollAmount;

    private float dotGap;

    private GameObject[] dotArray;

    private TrailRenderer trailRenderer;

    void Start()
    {
        dotGap = 1f / dotAmount;

        trailRenderer = GetComponentInChildren<TrailRenderer>();

        InitPulseEffectVariables();
        SpawnDots();
    }

    private void InitPulseEffectVariables()
    {
        startSize = transform.localScale;
        targetSize = startSize * expandAmount;
    }

    private void SpawnDots()
    {
        dotArray = new GameObject[dotAmount];

        for (int i = 0; i < dotAmount; i++)
        {
            GameObject dot = Instantiate(dotPrefab);
            dot.SetActive(false);
            dotArray[i] = dot;
        }
    }

    public void SetDotPos(Vector3 startPosition, Vector3 endPosition)
    {
        for (int i = 0; i < dotAmount; i++)
        {
            Vector3 dotPosition = dotArray[i].transform.position;
            Vector3 targetPosition = Vector2.Lerp(startPosition, endPosition, i * dotGap);

            float smoothSpeed = (1f - followCurve.Evaluate(i * dotGap)) * followSpeed;

            //dotArray[i].transform.position = targetPosition;

            dotArray[i].transform.position = Vector2.Lerp(dotPosition, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }

    public void ChangeDotActiveState(bool state)
    {
        for (int i = 0; i < dotAmount; i++)
        {
            dotArray[i].SetActive(state);
        }
    }

    public void SetDotStartPosition(Vector3 position)
    {
        for (int i = 0; i < dotAmount; i++)
        {
            dotArray[i].transform.position = position;
        }
    }

    public void MakeBallPulse()
    {
        scrollAmount += Time.deltaTime * expandSpeed;

        float percent = expandCurve.Evaluate(scrollAmount);

        transform.localScale = Vector2.Lerp(startSize, targetSize, percent);
    }

    public void ResetBallSize()
    {
        transform.localScale = startSize;
        scrollAmount = 0;
    }

    public void ChangeTrailState(bool emitting, float time)
    {
        trailRenderer.emitting = emitting;
        trailRenderer.time = time;
    }
}
