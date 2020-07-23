using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class npcAI : MonoBehaviour
{
    public float maxDistance = 200;
    public bool isGrouned = false;
    NavMeshAgent npcNav;
    BoxCollider npcBox;
    Rigidbody rb;

    void Start()
    {
        npcNav = GetComponent<NavMeshAgent>();
        npcBox = GetComponent<BoxCollider>();
        npcBox.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(npcNav.enabled)
            if(npcNav.remainingDistance==0)
                setNewDestination();
    }

    void OnCollisionEnter(Collision coll)
    {
        npcNav.enabled = false;
        isGrouned = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            rb.isKinematic = false;
            npcBox.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 9)
            if(npcNav.enabled)
            {
                rb.isKinematic = true;
                npcBox.enabled = false;
            }
    }

    void setNewDestination()
    {
        float walkDistance = Random.value * maxDistance;
        Vector3 randomDirection = Random.insideUnitSphere * walkDistance;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkDistance, 1);
        Vector3 finalPosition = hit.position;
        npcNav.SetDestination(finalPosition);
    }
}
