using System;
using EZCameraShake;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public float senstivity = .1f, accn = 2.5f, maxVelocity = 5;
    public float turnStrength = 30, maxBoost = 100, deltaBoost = 10, boostRecharge = 2;
    public float boostForce;
    public boostBarAnim barAnim;
    public Rigidbody r;
    public LayerMask groundLayer;
    public ParticleSystem jumpVfx;
    public Vector3 offset;
    float vertical, horizontal, boost, defaultDrag;
    wheel[] wheels;
    private bool grounded;
    private bool flying;
    private ParticleSystem.EmissionModule jumpVfxEmission;
    private Transform cartMesh;
    void Start()
    {
        r.transform.parent = null;
        boost = maxBoost;
        wheels = GetComponentsInChildren<wheel>();
        grounded = false;
        defaultDrag = r.drag;
        jumpVfxEmission = jumpVfx.emission;
        cartMesh = transform.GetChild(2).GetChild(0);
    }

    void Update()
    {
        foreach(wheel w in wheels)
        {
            if(grounded)
                w.steer = horizontal;
            else
                w.steer = 0;
        }

        jumpVfxEmission.enabled = flying?true:false;
        cartMesh.eulerAngles = new Vector3(cartMesh.eulerAngles.x,cartMesh.eulerAngles.y, 4 * horizontal * vertical);
    }

    void FixedUpdate()
    {
        jumpBoost();
        vertical = Mathf.MoveTowards(vertical, Input.GetAxis("Vertical"), senstivity);
        throttle(vertical);
        transform.position = r.position + offset;
        turn();


        if(r.velocity.magnitude > maxVelocity)
        {
            r.velocity = r.velocity.normalized * maxVelocity;
        }
    }

    private void jumpBoost()
    {
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position, -transform.up, out hit, 1.7f, groundLayer);

        if(Input.GetKey(KeyCode.Space) && boost>1)
        {
            r.AddForce(0,boostForce,0,ForceMode.Acceleration);
            boost -= deltaBoost * Time.deltaTime;
            barAnim.jumpAnim(boost,true);
            flying = true;
        }

        else if(boost<maxBoost)
        {
            boost += boostRecharge * Time.deltaTime;
            barAnim.jumpAnim(boost,false);
            flying = false;
        }

        else
            flying = false;

        if(!flying && !grounded)
            r.AddForce(0,-2,0,ForceMode.Acceleration);

        if(flying)
            r.drag = 0;

        else if(grounded)
            r.drag = defaultDrag;

        boost = Mathf.Clamp(boost, 0, maxBoost);
    }

    void turn()
    {
        horizontal = Mathf.MoveTowards(horizontal, Input.GetAxis("Horizontal"), senstivity);
        transform.Rotate(0,horizontal * turnStrength * Time.deltaTime * 100,0,Space.Self);
    }


    public void throttle(float value)
    {
        if(r.velocity.magnitude < maxVelocity)
            r.AddForce(value * accn * Time.deltaTime * transform.right * 1000, ForceMode.Acceleration);
    }
}
