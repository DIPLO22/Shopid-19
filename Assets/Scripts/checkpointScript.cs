using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class checkpointScript : MonoBehaviour
{
    public float spawnDistance = 100;
    public Vector3 offset;
    public Transform pointerPivot, pointer;
    public float speedAnim;
    public Material powerUpMat;
    public LayerMask layerMask;
    private bool playAnim;
    private float startTime;

    void Update()
    {
        Vector3 lookPos = pointerPivot.position - transform.position;
        float distance = lookPos.magnitude;
        distance = utils.map(distance,50,200,5,15);
        Vector3 desiredPos = new Vector3(distance, pointer.localPosition.y, pointer.localPosition.z);
        pointer.localPosition = Vector3.Lerp(pointer.localPosition, desiredPos, 0.005f);
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        pointerPivot.rotation = rotation;
        pointerPivot.rotation *= Quaternion.Euler(0,90,0);

        if(playAnim)
        {
            startTime += Time.deltaTime * speedAnim;
            float desiredEffect = Mathf.Sin(startTime);
            if(desiredEffect<0)
            {
                desiredEffect = 0;   
                playAnim = false;
            }
            powerUpMat.SetFloat("_effect",desiredEffect);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==9)
        {
            StartCoroutine(gameManager.gManager.bodyMaker());
            //make checkpoint disappear fade
            float radius = GetComponent<SphereCollider>().radius*transform.lossyScale.x;
            Vector3 point = utils.getRandomPointOnNav(spawnDistance, radius, layerMask);
            point += offset;
            transform.position = point;
            
            playAnim = true;
            startTime = 0;
        }
    }
}
