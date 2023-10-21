using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : GAction
{
    public override bool PrePerform()
    {
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            return false;
        }

        Vector3 randomDirection = Random.insideUnitSphere * 20;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 5, 1);
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);

        return true;
    }

    public override bool PostPerform()
    {

        return true;
    }
}
