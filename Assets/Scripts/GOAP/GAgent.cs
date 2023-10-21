using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
    SubGoal:
    Represents a sub-objective or a minor goal that an agent wishes to achieve. 
    Key aspects include:
        - A Dictionary (sGoals) that details the specific goals and their required states.
        - A flag 'remove' that indicates if this goal should be discarded after its completion.
    
    GAgent (extends MonoBehaviour):
    Represents the agent that interacts with the world and attempts to achieve goals. 
    Key aspects include:
        - A list of actions (actions) the agent is capable of performing.
        - A Dictionary of goals (goals) along with their respective priorities.
        - Inventory (inventory) to store items. (No real use for this in the end for this game)
        - Beliefs (beliefs) indicating the agent's perception of the current world state.
        - A planner (planner) that formulates a sequence of actions leading to the achievement of a goal.
        - An action queue (actionQueue) detailing the actions to be executed in order.
        - Mechanisms to keep track of the agent's current action and goal.
        - Various physical attributes like health, thirst, hunger, and energy.
        - Logic to determine the next set of actions to undertake.
        - Logic to evaluate the completion of actions and transition to subsequent ones.
    
*/
public class SubGoal {

    public Dictionary<string, int> sGoals;
    // bol dictates if a goal should be removed after it has been achieved
    public bool remove;
    public SubGoal(string s, int i, bool r) {

        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour {
    //list of actions the agent can perform
    public List<GAction> actions = new List<GAction>();
    // goals with their respective priorities
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public GInventory inventory = new GInventory();
    // beliefs or current state of the world
    public WorldStates beliefs = new WorldStates();

    // planner for the agent to decide its actions
    GPlanner planner;
    // queued actions that the agent plans to execute
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    //
    public Animator[] animators;
    public Rigidbody rigidBody;
    public BoxCollider boxCollider;

    // character stats
    public int health;
    public int thirst;
    public int hunger;
    public int energy;
    //public int strength;
    //public float stamina;
    //public float regenRate;
    //protected float minDist = 1.5f;
    //protected float aggroDist = 5f;
    //protected bool loop = false;
    //protected float maxStamina;
    //

    //target dest
    Vector3 destination = Vector3.zero;

    public void Start() {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
            actions.Add(a);
    }

    //an invoked method to allow an agent to be performing a task for a set location
    bool invoked = false;

    void CompleteAction() {
        // get all GAction components attached to the agent and add them to the actions list
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate() {
        if (currentAction != null && currentAction.running) {
            float distanceToTarget = Vector3.Distance(destination, this.transform.position);
            //Debug.Log(currentAction.agent.hasPath + "   " + distanceToTarget);
            
            // check the agent has a goal and has reached that goal
            if (distanceToTarget < 2f)//currentAction.agent.remainingDistance < 0.5f)
            {
                // Debug.Log("Distance to Goal: " + currentAction.agent.remainingDistance);
                if (!invoked) {
                    //if the action movement is complete wait a certain duration for it to be completed
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if (planner == null || actionQueue == null) {
            planner = new GPlanner();

            // sort & store the goals in descending order
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            // iterate through each goal to find one that has an achievable plan
            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals) {
                actionQueue = planner.plan(actions, sg.Key.sGoals, beliefs);
                // if actionQueue != null then we must have a plan
                if (actionQueue != null) {
                    // set goal
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0) {
            // perform check of  is currentGoal removable?
            if (currentGoal.remove) {
                goals.Remove(currentGoal);
            }
            // reset planner to begin again
            planner = null;
        }

        // check for anymore actions
        if (actionQueue != null && actionQueue.Count > 0) {
            // take the top action
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform()) {
                if (currentAction.target == null && currentAction.targetTag != "")
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);

                if (currentAction.target != null) {
                    // begin action
                    currentAction.running = true;
                    // search for relevant destination pos
                    destination = currentAction.target.transform.position;
                    Transform dest = currentAction.target.transform.Find("Destination");
                    if (dest != null)
                        destination = dest.position;

                    // pass destination to agent
                    currentAction.agent.SetDestination(destination);
                }
            } else {
                // reset to begin anew
                actionQueue = null;
            }

        }

    }
}
