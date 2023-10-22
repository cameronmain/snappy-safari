using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

        /* 
        The method performs the following steps:
        1. Acquires the NavMeshAgent component from the game object.
        2. Generates a random position within a sphere of radius 20 units around the current position.
        3. Samples a valid position on the NavMesh close to the random position.
        4. Sets the agent's destination to the sampled NavMesh position, causing the agent to wander to that location.
        */
        
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
