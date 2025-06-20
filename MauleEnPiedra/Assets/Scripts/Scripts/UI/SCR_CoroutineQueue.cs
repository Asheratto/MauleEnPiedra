using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CoroutineQueue : MonoBehaviour
{
    [SerializeField] private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    [SerializeField] private bool isRunning = false;

    [SerializeField] private Queue<IEnumerator> textQueue = new Queue<IEnumerator>();
    [SerializeField] private bool isRunningtext = false;

    public void Enqueue(IEnumerator coroutine)
    {
        coroutineQueue.Enqueue(coroutine);
        if (!isRunning)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isRunning = true;

        while (coroutineQueue.Count > 0)
        {
            yield return StartCoroutine(coroutineQueue.Dequeue());
        }

        isRunning = false;
    }

    public void EnqueueText(IEnumerator coroutine)
    {
        textQueue.Enqueue(coroutine);
        if (!isRunningtext)
        {
            StartCoroutine(ProcessTextQueue());
        }
    }

    private IEnumerator ProcessTextQueue()
    {
        isRunningtext = true;

        while (textQueue.Count > 0)
        {
            yield return StartCoroutine(textQueue.Dequeue());
        }

        isRunningtext = false;
    }
}