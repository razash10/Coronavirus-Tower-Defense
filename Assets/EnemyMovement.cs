using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 4f)] float speed = 1f;
    [SerializeField] List<Waypoint> path = null;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 1f / speed;
        StartCoroutine(FollowPath());
        print("Hey i'm back at start");
    }

    IEnumerator FollowPath()
    {
        print("Starting patrol...");
        foreach (Waypoint waypoint in path)
        {
            print("Visiting block: " + waypoint.name);
            StartCoroutine(SmoothLerp(time, waypoint));
            yield return new WaitForSeconds(time);
        }
        print("Ending patrol");
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
