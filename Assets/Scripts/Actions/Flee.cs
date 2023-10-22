using UnityEngine;
using System.Collections.Generic;

public class Flee : GAction
{
    private float runAwayDistance = 20.0f; // distance at which the agent will start running away from the player
    private Vector3 targetPosition; // target position to run to

    public override bool PrePerform()
    {
        GameObject player = GameObject.FindWithTag("Player");  // find the player game object
        Vector3 playerPosition = player.transform.position;    // get the player's current position
        float distance = Vector3.Distance(transform.position, playerPosition);  // calculate distance between the agent and player

        //Debug.Log("Distance: " + distance);

        if (distance < runAwayDistance)
        {
            Vector3 dirToPlayer = transform.position - playerPosition;  // get direction away from the player
            Vector3 newPos = transform.position + dirToPlayer;           // calculate new position to run to

            this.agent.SetDestination(newPos);   // set the new destination for the agent
        }

        return true;
    }

    public override bool PostPerform()
    {
        beliefs.RemoveState("threatened");  // remove the threatened state from beliefs
        return true;
    }
}
