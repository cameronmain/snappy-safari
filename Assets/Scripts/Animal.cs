using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this script represents an animal agent in the game. it defines various behaviors and 
 * sub-goals for the animal like exploring, fleeing, eating, and drinking. the agent's 
 * animations and state checks like sleepiness and hunger are also managed within.
 */
public class Animal : GAgent
{
    public GameObject player;

    new void Start()
    {
        // initialize base agent components
        base.Start();

        // define various subgoals for the animal and assign priority values

        // goal to explore the environment
        SubGoal s1 = new SubGoal("explore", 1, false);
        goals.Add(s1, 1);

        // goal to flee or maintain distance from threats (e.g., player, predators)
        SubGoal s2 = new SubGoal("flee", 1, false);
        goals.Add(s2, 10);

        // goal for hunger satisfaction
        SubGoal s3 = new SubGoal("satiated", 1, false);
        goals.Add(s3, 4);

        // goal for thirst satisfaction
        SubGoal s4 = new SubGoal("quenched", 1, false);
        goals.Add(s4, 4);

        // goal for sleep
        SubGoal s5 = new SubGoal("rested", 1, false);
        goals.Add(s5, 5);

        // initialize various health and status values for the animal agent
        health = Random.Range(50, 100);
        hunger = Random.Range(10, 50);
        thirst = Random.Range(10, 50);
        energy = Random.Range(10, 50);

        // set up periodic checks and needs for the agent
        Invoke("NeedDrink", Random.Range(10.0f, 20.0f));
        Invoke("NeedFood", Random.Range(10.0f, 20.0f));
        Invoke("NeedSleep", Random.Range(10.0f, 20f));
        Invoke("CheckSurroundings", Random.Range(1.0f, 2.0f));
    }

    private void Update()
    {
        // handle animal animations based on the current action being taken
        if (currentAction)
        {
            string animAction = currentAction.ToString();
            foreach (Animator animator in animators)
            {
                if (animAction.Contains("GoToVegetation"))
                {
                    animator.Play("Walk");
                    animator.Play("Eyes_LookDown");
                }
                else if (animAction.Contains("GoToWater"))
                {
                    animator.Play("Walk");
                    animator.Play("Eyes_Blink");
                }
                else if (animAction.Contains("Wander"))
                {
                    animator.Play("Idle_A");
                    animator.Play("Eyes_Blink");
                }
                else if (animAction.Contains("Flee"))
                {
                    animator.Play("Run");
                    animator.Play("Eyes_Trauma");
                }
                else if (animAction.Contains("Sleep"))
                {
                    animator.Play("Sit");
                    animator.Play("Eyes_Sleep");
                }
            }
        }
    }

    void CheckSurroundings()
    {
        // check proximity with the player and update "threatened" state accordingly
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance < 10.0f && !beliefs.HasState("threatened"))
        {
            beliefs.ModifyState("threatened", 0);
        }
        else if(distance > 10.0f)
        {
            beliefs.RemoveState("threatened");
        }

        // recursively recheck the surroundings after a short random duration
        Invoke("CheckSurroundings", Random.Range(1.0f, 2.0f));
    }

    void NeedSleep()
    {
        // update the agent's sleep need and modify "sleepy" state
        if (energy <= 15)
        {
            beliefs.ModifyState("sleepy", 0);
            energy += 40;
        }
        // periodically check for sleep needs
        Invoke("NeedSleep", Random.Range(10.0f, 20.0f));
    }

    void NeedDrink()
    {
        // update thirst and energy values for the agent
        thirst -= Random.Range(1, 6);
        energy -= Random.Range(1, 10);

        // if thirst reaches a threshold, update "thirsty" state
        if (thirst <= 5)
        {
            beliefs.ModifyState("thirsty", 0);
            thirst += 40;
        }

        // periodically check for drinking needs
        Invoke("NeedDrink", Random.Range(10.0f, 20.0f));
    }

    void NeedFood()
    {
        // update hunger and energy values for the agent
        hunger -= Random.Range(1, 6);
        energy -= Random.Range(1, 10);

        // if hunger reaches a threshold, update "hungry" state
        if (hunger <= 5)
        {
            beliefs.ModifyState("hungry", 0);
            hunger += 40;
        }

        // periodically check for food needs
        Invoke("NeedFood", Random.Range(10.0f, 20.0f));
    }

    void NeedRelief()
    {
        // this method simulates the agent's need for a toilet break
        beliefs.ModifyState("busting", 0);
        // periodically check for relief needs
        Invoke("NeedRelief", Random.Range(2.0f, 5.0f));
    }
}
