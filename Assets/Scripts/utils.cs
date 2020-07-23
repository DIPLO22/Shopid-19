using UnityEngine;
using System.Reflection;
using UnityEngine.AI;

public class utils
{
    public static float map(float value, float OldMin, float OldMax, float NewMin, float NewMax)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((value - OldMin) * NewRange) / OldRange) + NewMin;
    
        return(Mathf.Clamp(NewValue,Mathf.Min(NewMin,NewMax),Mathf.Max(NewMin,NewMax)));
    }

    public static bool checkDirectLOS(Vector3 source, Vector3 target)
    {
        RaycastHit hit;
        Physics.Linecast(source, target, out hit);
        if(hit.collider.gameObject.layer == 11)
        {
            return false;
        }
        return true;
    }

    // public static void clearConsole()
    // {
    //     var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //     var type = assembly.GetType("UnityEditor.LogEntries");
    //     var method = type.GetMethod("Clear");
    //     method.Invoke(new object(), null);
    // }

    public static Vector3 getRandomPointOnNav(float spawnDistance, float radius, LayerMask layerMask)
    {
        NavMeshHit hit;

        Vector3 randomPoint = new Vector3(270,0,-60) + (Random.insideUnitSphere * spawnDistance);
        NavMesh.SamplePosition(randomPoint, out hit, spawnDistance, 1);

        if(Physics.OverlapSphere(hit.position,radius - 1f,layerMask).Length > 0)
            return getRandomPointOnNav(spawnDistance,radius,layerMask);
        
        else
            return hit.position;
    }
}
