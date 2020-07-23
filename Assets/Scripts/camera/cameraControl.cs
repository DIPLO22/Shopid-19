using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public bool ifCamInCutscene = false;
    public Transform target;
    public float smoothness = .05f, infectionEffectSmoothness = 0.1f;
    public Vector3 offsetRot;
    private Vector3 startLoc;
    private Rigidbody r;
    private Camera cam;
    private float infectionRotFactor;
    private Vector3 camVelocity = Vector3.zero;

    #region Singleton for Camera
    public static cameraControl main;
    void Awake()
    {
        if(main != null)
            print("wtf");
        else
            main = this;
    }
    #endregion

    void Start()
    {
        startLoc = target.position;
        r = target.GetComponentInParent<CartController>().r;
        cam = GetComponentInChildren<Camera>();
        infectionRotFactor = Random.value>=0.5f ? 1 : -1;
        healthScript.whenInfected += infectCam;
        healthScript.whenDoneInfected += uninfectCam;
    }

    void FixedUpdate()
    {
        if(!ifCamInCutscene)
        {
            Vector3 desiredLoc = target.transform.position;
            desiredLoc.y = startLoc.y;
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref camVelocity, smoothness);
            
            transform.LookAt(target.parent.position);
            transform.rotation*=Quaternion.Euler(offsetRot);
        }
    }

    public void infectCam(float health)
    {
        if(!ifCamInCutscene)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,55,infectionEffectSmoothness);
    }

    public void uninfectCam()
    {
        if(!ifCamInCutscene)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60 + r.velocity.magnitude, infectionEffectSmoothness);
    }
}
