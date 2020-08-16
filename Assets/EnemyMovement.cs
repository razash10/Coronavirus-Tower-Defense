using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 4f)] float speed = 1f;

    float time;

    void Start()
    {
        time = 1f / speed;
        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        Stack<Waypoint> path = pathfinder.GetPath();
        StartCoroutine(FollowPath(path));
    }

    IEnumerator FollowPath(Stack<Waypoint> path)
    {
        foreach (Waypoint waypoint in path)
        {
            StartCoroutine(SmoothLerp(time, waypoint));
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator SmoothLerp(float time, Waypoint target)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = target.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
