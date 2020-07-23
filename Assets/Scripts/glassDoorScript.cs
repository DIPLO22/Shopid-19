using EZCameraShake;
using UnityEngine;

public class glassDoorScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            CameraShaker.Instance.ShakeOnce(4,4,0.1f,2f);
            Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();

            foreach(Rigidbody rb in rbs)
                rb.isKinematic = false;
        }
    }
}
