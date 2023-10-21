using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    GAction represents an abstract action that agents can perform. 
    Features:
        - Attributes such as action name, cost, target, and duration.
        - Preconditions that specify the required state of the world for the action to be executable.
        - AfterEffects which describe the resulting state of the world once the action is completed.
        - Logic to check if the action is achievable based on current world conditions.
        - Abstract methods for preparatory and concluding tasks around the execution of the action.
*/

public abstract class GAction : MonoBehaviour {

    // action name
    public string actionName = "Action";
    // action cost
    public float cost = 1.0f;
    // where the action is going to take place
    public GameObject target;

    public string targetTag;
    // action duration
    public float duration = 0.0f;

    // preconditions needed for this action to be executed
    public WorldState[] preConditions;
    // effects or results after the action has been executed
    public WorldState[] afterEffects;

    public NavMeshAgent agent;


    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    // agent state
    public WorldStates agentBeliefs;
    public GInventory inventory;
    public WorldStates beliefs;

    public bool running = false;


    public GAction() {

        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    private void Awake() {

        // populate the agent with its preconditions, aftereffects, inv & beliefs
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if (preConditions != null) {

            foreach (WorldState w in preConditions) {
                preconditions.Add(w.key, w.value);
            }
        }

        if (afterEffects != null) {

            foreach (WorldState w in afterEffects) {
                effects.Add(w.key, w.value);
            }
        }
        inventory = this.GetComponent<GAgent>().inventory;
        beliefs = this.GetComponent<GAgent>().beliefs;
    }

    public bool IsAchievable() {

        return true;
    }

    // check if the action is achievable given the world conditions & try to match with the action preconditions
    public bool IsAhievableGiven(Dictionary<string, int> conditions) {

        foreach (KeyValuePair<string, int> p in preconditions) {

            if (!conditions.ContainsKey(p.Key)) {

                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
