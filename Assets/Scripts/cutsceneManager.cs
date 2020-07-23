using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneManager : MonoBehaviour
{
    public CartController player;
    private Coroutine co;

    void Start()
    {
        cameraControl.main.ifCamInCutscene = true;
        co = StartCoroutine(autoThrottle());
        Invoke("cutSceneCloser",3.5f);
    }

    IEnumerator autoThrottle()
    {
        for(;;)
        {
            player.throttle(1);
            yield return null;
        }
    }

    void cutSceneCloser()
    {
        cameraControl.main.ifCamInCutscene = false;
        StopCoroutine(co);
    }
}
