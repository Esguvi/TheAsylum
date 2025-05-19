using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NavScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> points;
    public Transform downPoint;
    public Transform upPoint;
    public Transform player;

    private int randomInt;
    private float delayTime;
    private bool down = true;
    private bool changeHeigh;
    private Transform destine;
    private Animator anim;
    private void Start()
    {
        randomInt = Random.Range(0, (points.Count - 1));
        delayTime = Time.time + 2;
        destine = points[randomInt].transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 offset = new Vector3(0, 105f, 0);
        Debug.DrawLine((transform.position+offset), (player.position + offset));
        if (!Physics.Linecast((transform.position+offset), (player.position + offset)))
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if (delayTime < Time.time)
            {

                if ((down && points[randomInt].position.y > 100) || (!down && points[randomInt].position.y < 100))
                {
                    if (down)
                    {
                        agent.SetDestination(downPoint.position);
                    }
                    else
                    {
                        agent.SetDestination(upPoint.position);
                    }
                    changeHeigh = true;
                }
                else
                {
                    agent.SetDestination(destine.position);
                    anim.SetBool("isRunning", true);
                }


                if (agent.remainingDistance > 0 && agent.remainingDistance < 2)
                {
                    anim.SetBool("isRunning", false);
                    delayTime = Time.time + 2;

                    if (destine.childCount > 0)
                    {
                        destine = destine.GetChild(0);
                    }
                    else if (destine.childCount == 0)
                    {
                        randomInt = Random.Range(0, (points.Count - 1));
                        randomInt++;
                        destine = points[randomInt].transform;
                    }
                }
            }
           
        }
    }
}
