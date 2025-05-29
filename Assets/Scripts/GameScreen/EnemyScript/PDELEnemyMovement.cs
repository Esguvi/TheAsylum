using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public int routine;
    public float cronometer;
    public Animator animator;
    public Quaternion angle;
    public float grade;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EnemyBehaviour()
    {
        cronometer += 1 * Time.deltaTime;
        if (cronometer >= 4)
        {
            routine = Random.Range(0, 2);
            cronometer = 0;
        }

        switch (routine)
        {
            case 0:
                animator.SetBool("isWalking", false);
                break;
            case 1:
                grade = Random.Range(0, 360);
                angle = Quaternion.Euler(0, grade, 0);
                routine++;
                break;
            case 2:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                transform.GetComponent<CharacterController>().Move(Vector3.forward * 100 * Time.deltaTime);
                animator.SetBool("isWalking", true);
                break;
        }
    }

    private void Update()
    {
        EnemyBehaviour();
    }
}
