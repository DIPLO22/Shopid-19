using System.Collections;
using TMPro;
using UnityEngine;
using EZCameraShake;

public class gameManager : MonoBehaviour
{
    public GameObject bodyPrefab, tail;
    public int carts;
    public TextMeshProUGUI score;
    public Transform cartParent;

    public delegate void sceneUnloaderEvent();
    public static sceneUnloaderEvent sceneUnloader;

    #region Singleton of game manager
    public static gameManager gManager;
    void Awake()
    {
        if(gManager!=null)
            print("Error");
        else
            gManager = this;
    }
    #endregion

    
    void Start()
    {
        carts = 4;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            StartCoroutine(bodyMaker());
        }
    }

    //new body spawner, call this to create a body
    public IEnumerator bodyMaker()
    {
        bodyMove tailScript = tail.GetComponent<bodyMove>();
        tailScript.spawnTrigger = true;
        yield return new WaitUntil(() => tailScript.counter >= tailScript.reqTrailLen);
        GameObject newObj = Instantiate(bodyPrefab, tailScript.trail[tailScript.trail.Count-1], Quaternion.identity) as GameObject;
        newObj.transform.LookAt(tail.transform);
        newObj.transform.rotation *= Quaternion.Euler(0,-90,0);
        tailScript.canSpawn = false;
        tailScript.spawnTrigger = false;
        tailScript.isTail = false;
        tailScript.body = newObj;
        tailScript = newObj.GetComponent<bodyMove>();
        tailScript.isTail = true;
        tailScript.canSpawn = false;
        tailScript.spawnTrigger = false;
        tailScript.next = tail.transform;
        tail = newObj;
        tail.transform.parent = cartParent;
        changeScore(1);
    }

    public void changeScore(int change)
    {
        carts += change;
        score.text = carts.ToString();
        if(change>0)
            score.GetComponent<Animation>().Play();
    }
}
