using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NavScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> points;
    public Transform player;

    private int randomInt;
    private float delayTime;
    private bool persigiendo = false;
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

        if (persigiendo)
        {
            if (agent.remainingDistance > 0 && agent.remainingDistance < 150)
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isCatch", true);
                if (agent.remainingDistance > 0 && agent.remainingDistance < 50)
                {
                    SceneManager.LoadScene("GameOverScreen", LoadSceneMode.Single);
                }
            }
           }

        if (!Physics.Linecast((transform.position + offset), (player.position + offset)))
        {
            anim.SetBool("isRunning", true);
            agent.SetDestination(player.position);
            persigiendo = true;
        }
        else
        {
            persigiendo = false;
            if (delayTime < Time.time)
            {
                agent.SetDestination(destine.position);
                anim.SetBool("isRunning", true);

                if (agent.remainingDistance > 0 && agent.remainingDistance < 1)
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
            else
            {
                Debug.Log("Esperando " + (Time.time - delayTime));
            }
        }
    }
}
