using UnityEngine;
using System.Collections.Generic;

public class Flee : GAction
{
    private float runAwayDistance = 20.0f; // distance at which the agent will start running away from the player
    private Vector3 targetPosition; // target position to run to


    public override bool PrePerform()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPosition = player.transform.position;
        float distance = Vector3.Distance(transform.position, playerPosition);

        //Debug.Log("Distance: " + distance);

        if (distance < runAwayDistance)
        {
            Vector3 dirToPlayer = transform.position - playerPosition;
            Vector3 newPos = transform.position + dirToPlayer;

            this.agent.SetDestination(newPos);
        }

        return true;
    }

    public override bool PostPerform()
    {
        beliefs.RemoveState("threatened");
        return true;
    }

}
