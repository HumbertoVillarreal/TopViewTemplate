using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{

    public Transform waypointFather;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currWaypointIndex;
    private bool isWaiting;
    private bool blocked = false;
    private Animator animator;

    private float lastInputX;
    private float lastInputY;

    private void Start()
    {
        waypoints = new Transform[waypointFather.childCount];
        animator = GetComponent<Animator>();

        for(int i = 0; i < waypointFather.childCount; i++)
        {
            waypoints[i] = waypointFather.GetChild(i);
        }
    }


    private void Update()
    {
        if(PauseController.IsGamePaused || isWaiting || blocked)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastInputX);
            animator.SetFloat("LastInputY", lastInputY);
            return;
        }

        //Move to waypoint
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currWaypointIndex];

        Vector2 dir = (target.position - transform.position).normalized;

        if (dir.magnitude > 0) { 
            lastInputX = dir.x;
            lastInputY = dir.y;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("InputX", dir.x);
        animator.SetFloat("InputY", dir.y);
        animator.SetBool("isWalking", dir.magnitude > 0f);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            //WaitWaipoint
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        animator.SetBool("isWalking", false);

        animator.SetFloat("LastInputX", lastInputX);
        animator.SetFloat("LastInputY", lastInputY);

        yield return new WaitForSeconds(waitTime);

        currWaypointIndex = loopWaypoints ? (currWaypointIndex + 1) % waypoints.Length : Mathf.Min(currWaypointIndex + 1, waypoints.Length - 1);

        isWaiting = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "InteractionDetector") { blocked = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "InteractionDetector") { blocked = false; }
    }

}
