using UnityEngine;
using System.Collections.Generic;
using EZCameraShake;

public class breakableScript : MonoBehaviour
{
    public GameObject[] fracturedGlass;
    public GameObject[] glass;
    public Material simpleMat;
    public bool selfGlass;
    private bool isBroke = false;
    private List<Rigidbody> rb;
    private List<MeshRenderer> mr;
    private List<Rigidbody> frb;
    private List<MeshRenderer> fmr;
    void Awake()
    {
        mr = new List<MeshRenderer>();
        rb = new List<Rigidbody>();
        //if object itself has a glass
        if(selfGlass)
        {
            glass = new GameObject[1];
            glass[0] = gameObject;
            rb.Add(gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody);
            rb[0].isKinematic = true;
            mr.Add(GetComponent<MeshRenderer>());
        }

        //if glass does not have fractured glasses
        else if(fracturedGlass.Length == 0)
        {
            foreach(GameObject g in glass)
            {
                var m = g.GetComponent<MeshRenderer>();

                mr.Add(m);
                
                var r = g.AddComponent(typeof(Rigidbody)) as Rigidbody;
                r.isKinematic = true;

                rb.Add(r);
            }
        }

        //if glass has fractured gameobjects
        else
        {
            frb = new List<Rigidbody>();
            fmr = new List<MeshRenderer>();
            foreach(GameObject g in fracturedGlass)
            {
                frb.Add(g.AddComponent(typeof(Rigidbody)) as Rigidbody);
                var fmc = g.AddComponent(typeof(MeshCollider)) as MeshCollider;
                fmc.convex = true;
                fmr.Add(g.GetComponent<MeshRenderer>());
            }

            foreach(Rigidbody r in frb)
            {
                r.isKinematic = true;
                r.mass = .01f;
            }
        }
    }
    void OnCollisionEnter(Collision c)
    {
        if(c.collider.gameObject.layer == 9 && !isBroke && c.impulse.magnitude > 100)
        {
            if(selfGlass)
            {
                rb[0].isKinematic = false;
                isBroke = true;
                mr[0].material = simpleMat;
                gameObject.layer = 16;
            }

            else if(fracturedGlass.Length==0)
            {
                foreach(Rigidbody r in rb)
                {
                    r.isKinematic = false;
                    r.mass = .1f;
                }

                foreach(MeshRenderer m in mr)
                    m.material = simpleMat;

                isBroke = true;
            }

            else
            {
                foreach(Rigidbody r in frb)
                    r.isKinematic = false;
                foreach(MeshRenderer m in fmr)
                    m.enabled = true;

                isBroke = true;

                foreach(GameObject g in glass)
                    Destroy(g);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9 && !isBroke)
        {
            CameraShaker.Instance.ShakeOnce(4,2.5f,0.1f,2f);
            foreach(Rigidbody r in frb)
                    r.isKinematic = false;
            foreach(MeshRenderer m in fmr)
                m.enabled = true;

            isBroke = true;

            foreach(GameObject g in glass)
                Destroy(g);
        }
    }
}
