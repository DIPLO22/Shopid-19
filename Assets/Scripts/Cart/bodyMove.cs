using System.Collections.Generic;
using UnityEngine;

public class bodyMove : MonoBehaviour
{
    public int counter;
    public float followDistance = 1.5f;
    public float bufferDist;
    public GameObject body;
    public Transform next;
    public int reqTrailLen;
    public bool isHead, isTail, canSpawn = false, spawnTrigger = false;
    public GameObject sparks;
    [HideInInspector]
    public List<Vector3> trail;
    private Vector3 lastSpark;
    private wheel[] wheels;
    private bool isBroke;

    void Awake()
    {
        isBroke = false;
        trail.Add(transform.position);
        // its always 2 frames ahead to get the required one
        reqTrailLen = Mathf.FloorToInt((followDistance / bufferDist) + 2);
        wheels = GetComponentsInChildren<wheel>();
    }

    void FixedUpdate()
    {
        if(!isBroke)
        {
            // make trail from the head
            trailMaker();

            moveBody();
        }
    }

    void moveBody()
    {
        if(trail.Count > reqTrailLen)
        {
            if(!isHead)
            {
                transform.LookAt(next);
                transform.rotation *= Quaternion.Euler(0,-90,0);
            }


            if(!isTail)
            {
                Vector3 moveDir = body.transform.position - trail[trail.Count - reqTrailLen - 1];
                body.transform.position = trail[trail.Count - reqTrailLen - 1];
            }

            trail.RemoveRange(0, trail.Count - reqTrailLen);
        }
    }

    void trailMaker()
    {
        // store position if object moving
        float travelDone = Vector3.Distance(transform.position, trail[trail.Count-1]);
        if(travelDone >= bufferDist)
        {
            int x = Mathf.FloorToInt(travelDone/bufferDist);
            for(int i = 0; i < x; i++)
            {
                trail.Add(resizeDirVector(transform.position, trail[trail.Count-1], bufferDist));
                if(spawnTrigger)
                    counter+=1;
            }
        }
    }

    //resizing a vector in the same direction from same start point
    Vector3 resizeDirVector(Vector3 end, Vector3 start, float size)
    {
        return start + ((end-start).normalized * size);
    }

    //trigger for removal of carts
    public void trailBreaker(float explForce)
    {
        if(!isTail)
        {
            body.GetComponent<bodyMove>().trailBreaker(explForce - Random.Range(50,70));
            breaker(explForce);
            return;
        }
        if(isTail)
        {
            breaker(explForce);
            return;
        }
    }

    //explosion and removal of carts
    void breaker(float explForce)
    {
        isBroke = true;
        GetComponent<Rigidbody>().isKinematic = false;
        Vector3 randomExplPos = Random.insideUnitSphere * 3;
        randomExplPos.y = 0;
        GetComponent<Rigidbody>().AddExplosionForce(explForce,transform.position + randomExplPos * 5, 0, Random.Range(1,2));
        next.GetComponent<bodyMove>().isTail = true;
        gameManager.gManager.tail = next.gameObject;
        Invoke("disableBoxColl",15);
        Destroy(gameObject,20);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        gameManager.gManager.changeScore(-1);
    }

    void disableBoxColl()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        GameObject spark = Instantiate(sparks, pos, Quaternion.identity) as GameObject;
        spark.GetComponent<ParticleSystem>().Emit(Mathf.Clamp((int)collision.impulse.magnitude/5,10,50));
        Destroy(spark,3);
        lastSpark = contact.point;
    }

    void OnCollision(Collision collision)
    {
        if(lastSpark!=Vector3.zero)
        {
            Vector3 pos = collision.contacts[0].point;
            if(Vector3.Distance(lastSpark,pos) > 0.01f)
            {
                GameObject spark = Instantiate(sparks,pos,Quaternion.identity) as GameObject;
                spark.GetComponent<ParticleSystem>().Emit(Mathf.Clamp((int)collision.impulse.magnitude/5,10,50));
                Destroy(spark,3);
                lastSpark = pos;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        lastSpark = Vector3.zero;
    }
}
