using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NavScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> points;
    public Transform player;

    private int randomInt;
    private float delayTime;
    private void Start()
    {
        randomInt = Random.Range(0, points.Count);
        delayTime = Time.time + 10;
    }

    void Update()
    {
        Debug.DrawLine(transform.position, (player.position + new Vector3(0, 120f, 0)));
        if (!Physics.Linecast(transform.position, (player.position + new Vector3(0, 120f, 0))))
        {
            Debug.Log("Persiguiendo");
            agent.SetDestination(player.position);
        }
        else
        {
            Debug.Log("Patrullando");
            try
            {
                agent.SetDestination(points[randomInt].position);
            }
            catch (Exception e) 
            {
                randomInt = Random.Range(0, points.Count);
            }
            if (agent.remainingDistance > 0 && agent.remainingDistance < 2 && delayTime < Time.time)
            {
                delayTime = Time.time + 2;
                randomInt = Random.Range(0, points.Count);
                randomInt++;
                Debug.Log(points[randomInt].name);
            }
        }
    }
}
