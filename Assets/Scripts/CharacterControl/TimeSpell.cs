using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

internal class TimeSpell
{
    public static Collider[] StopTime( Vector3 playerPosition, LayerMask colliderMask)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, 30f, colliderMask);
        foreach (var hitCollider in hitColliders)
        {
            toggleEnableComponents(hitCollider, false);
        }

        return hitColliders;
    }

    private static void toggleEnableComponents(Collider collider, bool enable)
    {
        Rigidbody rb = collider.GetComponent<Rigidbody>();
        Animator anim = collider.GetComponent<Animator>();
        NavMeshAgent navMeshAgent = collider.GetComponent<NavMeshAgent>();

        if (rb != null) { rb.velocity = Vector3.zero; rb.isKinematic = !enable; }
        if (anim != null) { anim.enabled = enable; }
        if (navMeshAgent != null) { navMeshAgent.ResetPath(); }

        //Disable all logic for that game object
        MonoBehaviour[] behavior = collider.GetComponents<MonoBehaviour>();

        foreach(MonoBehaviour behaviour in behavior)
        {
            behaviour.enabled = enable;
        }
    }

    public static void ResumeTime(Collider[] colliders)
    {
        foreach(Collider collider in colliders){
            toggleEnableComponents(collider, true);
        }
    }
}