using System;
using UnityEngine;

public class npcDamage : MonoBehaviour
{
    public float maxDamage = 100;
    public ParticleSystem rangeCircle;
    public LayerMask layerMask;
    public Transform fresnel;
    private float radius;
    private Material intersectMat, fresnelMat;
    private string fresnelEffect, intersectEffect;
    private ParticleSystem.EmissionModule emissionCircle;
    private ParticleSystem.MainModule mainCircle;
    private Color startCircleColor;
    private npcAI parent;

    //delegates and events
    //one event for when player enters sphere collider
    //one event for when player exits sphere collider
    delegate void whenInfectingEvent(float dps);
    whenInfectingEvent whenInfecting;
    delegate void whenDoneInfectingEvent();
    whenDoneInfectingEvent whenDoneInfecting;

    void Start()
    {
        radius = GetComponent<SphereCollider>().radius * transform.lossyScale.x;
        intersectMat = GetComponent<MeshRenderer>().material;
        fresnelMat = fresnel.GetComponent<MeshRenderer>().material;
        fresnelEffect = "_effectFresnel";
        intersectEffect = "_effectIntersect";
        emissionCircle = rangeCircle.emission;
        mainCircle = rangeCircle.GetComponent<ParticleSystem>().main;
        startCircleColor = rangeCircle.main.startColor.color;
        parent = GetComponentInParent<npcAI>();

        whenInfecting += updateWhenInfecting;
        whenDoneInfecting += updateWhenDoneInfecting;
        gameManager.sceneUnloader+=unload;

        transform.parent = null;
    }

    void Update()
    {
        // bug fix of animations going on even if the player is not around the npc
        // sees if player is in range -> if not stops all the animations slowly
        if(emissionCircle.rateOverTime.constant > 0)
            scanColliderForPlayer(GetComponent<SphereCollider>(), radius);

        transform.position = parent.transform.position;

        if (parent.isGrouned)
        {
            //do something when knocked out
        }
    }

    void scanColliderForPlayer(SphereCollider item, float radius)
    {
        Vector3 center = item.transform.position;
        float r = item.radius * transform.lossyScale.x;

        Collider[] otherColliders = Physics.OverlapSphere(center, radius, layerMask);

        if(otherColliders.Length>0)
            return;

        fresnelMat.SetFloat(fresnelEffect,fresnelMat.GetFloat(fresnelEffect) - 0.1f);
        intersectMat.SetFloat(intersectEffect,intersectMat.GetFloat(intersectEffect) - 0.1f);

        emissionCircle.rateOverTime = emissionCircle.rateOverTime.constant - 0.5f;

        checkNegative();
    }

    void checkNegative()
    {
        if(fresnelMat.GetFloat(fresnelEffect) < 0)
            fresnelMat.SetFloat(fresnelEffect, 0);
        if(intersectMat.GetFloat(intersectEffect) < 0)
            intersectMat.SetFloat(intersectEffect, 0);
    }

    void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.CompareTag("Player"))
        {
            print("yes");
            if(utils.checkDirectLOS(transform.position, other.transform.position))
            {
                float effect = Vector3.Distance(other.transform.position,transform.position);

                whenInfecting?.Invoke(effect);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(utils.checkDirectLOS(transform.position, other.transform.position))
            {
                whenDoneInfecting?.Invoke();
            }
        }
    }

    void updateWhenInfecting(float effect)
    {
        healthScript.playerHealth.doDamage(maxDamage * utils.map(effect,0,radius,1,0.1f));

        fresnelMat.SetFloat(fresnelEffect, utils.map(effect,0,radius,1,0.05f));
        intersectMat.SetFloat(intersectEffect, utils.map(effect,0,radius,1,0.05f));
        
        Color tempColor = startCircleColor;
        tempColor.a = utils.map(effect, 0, radius, 1.5f, .5f);
        mainCircle.startColor = tempColor;
        emissionCircle.rateOverTime = utils.map(effect,0,radius,4,1);
    }

    void updateWhenDoneInfecting()
    {
        fresnelMat.SetFloat(fresnelEffect,0);
        intersectMat.SetFloat(intersectEffect,0);
        healthScript.playerHealth.resetSliderSize();

        emissionCircle.rateOverTime = 0;
    }

    void unload()
    {
        whenDoneInfecting = null;
        whenInfecting = null;
    }
}
