﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoutineRange : MonoBehaviour
{
    public float from;
    public float to;
    public float step;
    public bool useDeltaTime;
    [SerializeField] private UnityEventFloat onValue;
    [SerializeField] private UnityEvent onEnd;

    private bool isRunning;
    private Coroutine coroutine;

    private void OnValidate()
    {
        if (from > to)
            from = to;
    }

    public void Play(bool forceRestart)
    {
        if (isRunning)
        {
            if (forceRestart)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
            }
            else
                return;
        }

        if (from < to)
            coroutine = StartCoroutine(Routine(from, to));
        else
            coroutine = StartCoroutine(RoutineReversed(from, to));
    }

    public void PlayReversed(bool forceRestart)
    {
        if (isRunning)
        {
            if (forceRestart)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
            }
            else
                return;
        }

        if (from > to)
            coroutine = StartCoroutine(Routine(to, from));
        else
            coroutine = StartCoroutine(RoutineReversed(to, from));
    }

    private IEnumerator Routine(float min, float max)
    {
        isRunning = true;
        for (float i = min; i < max; i += step)
        {
            onValue.Invoke(i);
            yield return 0;
        }
        onValue.Invoke(to);
        onEnd.Invoke();
        isRunning = false;
    }

    private IEnumerator RoutineReversed(float max, float min)
    {
        isRunning = true;
        for (float i = max; i > min; i += step)
        {
            onValue.Invoke(i);
            yield return 0;
        }
        onValue.Invoke(to);
        onEnd.Invoke();
        isRunning = false;
    }
}
