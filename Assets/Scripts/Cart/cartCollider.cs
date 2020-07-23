using EZCameraShake;
using UnityEngine;

public class cartCollider : MonoBehaviour
{
    public GameObject sparks;
    private Vector3 lastSpark;
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        GameObject spark = Instantiate(sparks, pos, Quaternion.identity) as GameObject;
        spark.GetComponent<ParticleSystem>().Emit(Mathf.Clamp((int)collision.impulse.magnitude/5,10,50));
        Destroy(spark,3);
        lastSpark = contact.point;

        if(collision.collider.gameObject.layer==10)
        {
            if(collision.impulse.magnitude>100)
            {
                collision.collider.GetComponent<bodyMove>().trailBreaker(UnityEngine.Random.Range(1400,1500));
                CameraShaker.Instance.ShakeOnce(4,4,0.1f,1f);
            }
        }
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
