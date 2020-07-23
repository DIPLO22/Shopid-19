using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcManager : MonoBehaviour
{
    public GameObject npc;
    public int npcCount = 20;
    public float spawnDistance = 300;
    public LayerMask layerMask;

    void Start()
    {
        for(int i = 0; i < npcCount; i++)
        {
            Vector3 pos = utils.getRandomPointOnNav(spawnDistance, npc.GetComponent<SphereCollider>().radius * npc.transform.lossyScale.x, layerMask);
            GameObject g = Instantiate(npc, pos, Quaternion.identity) as GameObject;
            g.transform.parent = transform;
        }
    }
}
