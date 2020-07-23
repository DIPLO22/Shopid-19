using UnityEngine;

public class healthCheckpoint : MonoBehaviour
{
    public float spawnDistance = 200;
    public LayerMask layerMask;
    public float healAmount = 30;
    public float spawncoolDown = 10;
    private bool playAnim;
    private float startTime;
    public float speedAnim;
    public Material healthUpMat;

    void Start()
    {
        GetComponentInChildren<Animation>().Play();
        playAnim = false;
    }

    void Update()
    {
        if(playAnim)
        {
            startTime += Time.deltaTime * speedAnim;
            float desiredEffect = Mathf.Sin(startTime);
            if(desiredEffect<0)
            {
                desiredEffect = 0;   
                playAnim = false;
                gameObject.SetActive(false);
            }
            healthUpMat.SetFloat("_effect",desiredEffect);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9 && healthScript.playerHealth.health<100)
        {
            StartCoroutine(healthScript.playerHealth.healer(healAmount));

            playAnim = true;
            startTime = 0;

            setMeshActive(GetComponentsInChildren<MeshRenderer>(),false);

            Invoke("spawner",spawncoolDown);
        }
    }

    void setMeshActive(MeshRenderer[] mrs,bool b)
    {
        foreach (MeshRenderer mr in mrs)
        {
            mr.enabled = b;
        }        
    }

    void spawner()
    {
        setMeshActive(GetComponentsInChildren<MeshRenderer>(),false);
        gameObject.SetActive(true);
        float radius = GetComponent<SphereCollider>().radius*transform.lossyScale.x;
        Vector3 point = utils.getRandomPointOnNav(spawnDistance, radius, layerMask);
        point.y = 0;
        transform.position = point;
    }
}
