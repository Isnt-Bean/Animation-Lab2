using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> prefabPoints;
    [SerializeField] public PathManager pm;
    public List<Waypoints> thePath;
    public Waypoints target;

    public float MoveSpeed;
    public float RotateSpeed;
    
    public Animator animator;
    private bool isWalking;

    void Start()
    {
        isWalking = true;
        animator.SetBool("isWalking", isWalking);
        thePath = pm.getPath();
        if (thePath != null && thePath.Count > 0)
        {
            target = thePath[0];
        }
        prefabPoints = new List<GameObject>();
    }

    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void moveForward()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget<stepSize)
        {
            return;
        }

        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir*stepSize);
    }

    void Update()
    {
        rotateTowardsTarget();
        moveForward();
    }

    private void OnTriggerEnter(Collider other)
    {
        target = pm.GetNextTarget();
    }
    
}
