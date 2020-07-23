using UnityEngine;

public class wheel : MonoBehaviour
{
    public float speed, steer, maxSteer = 30;
    [Space]
    public float radius = 0.03f;
    private float xRot;
    public bool steerAble, driftAble;
    private Rigidbody r;
    private ParticleSystem.EmissionModule p;
    public TrailRenderer t;

    void Awake()
    {
        r = GameObject.FindGameObjectWithTag("rigidbodyPlayer").GetComponent<Rigidbody>();
        if(driftAble)
            p = GetComponent<ParticleSystem>().emission;
    }
    void FixedUpdate()
    {
        speed = r.velocity.magnitude;
        xRot = -speed/radius;
        transform.Rotate(xRot * Time.deltaTime,0,0,Space.Self);

        if(steerAble)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, steer * maxSteer + 90, transform.localEulerAngles.z);

        if(driftAble)
        {
            if(Mathf.Abs(steer)>0.9 )
            {
                p.enabled = true;
                t.emitting = true;
            }
            else
            {
                p.enabled = false;
                t.emitting = false;
            }
        } 

    }
}
