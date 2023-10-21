using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
    The following classes, ResourceQueue and GWorld, manage world resources and states.

    ResourceQueue:
    Represents a collection of GameObject resources that can be used by agents. 
    The main functionalities include:
        - Enqueueing and dequeueing resources.
        - Searching for resources based on tags.
        - Modifying the world states based on the availability of resources.
        - Providing the ability to add or remove specific resources.
    
    GWorld:
    This class serves as a centralized access point for all the world's data, 
    ensuring that there's only a single instance of world states and resources. 
    Key responsibilities include:
        - Holding the global instance of world states.
        - Managing multiple resource queues, such as waterholes and vegetation.
        - Providing methods to retrieve specific resource queues or the entire world state.
    
*/


public class ResourceQueue {

    public Queue<GameObject> que = new Queue<GameObject>();
    public string tag;
    public string modState;

    public ResourceQueue(string t, string ms, WorldStates w) {

        tag = t;
        modState = ms;
        if (tag != "") {

            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject r in resources) {

                que.Enqueue(r);
            }
        }

        if (modState != "") {

            w.ModifyState(modState, que.Count);
        }
    }

    public void AddResource(GameObject r) {

        que.Enqueue(r);
    }


    public GameObject RemoveResource() {

        if (que.Count == 0) return null;

        return que.Dequeue();
    }

    public void RemoveResource(GameObject r) {
        que = new Queue<GameObject>(que.Where(p => p != r));
    }
}

public sealed class GWorld {

    private static readonly GWorld instance = new GWorld();
    // the world states
    private static WorldStates world;

    private static ResourceQueue waterholes;
    private static ResourceQueue vegetation;


    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();

    static GWorld() {

        // create world
        world = new WorldStates();

        // create wateringhole array
        waterholes = new ResourceQueue("Waterhole", "FreeWater", world);
        resources.Add("waterholes", waterholes);

        // create vegetation(food) array
        vegetation = new ResourceQueue("GroundVegetation", "FreeVeg", world);
        resources.Add("vegetation", vegetation);

        //Time.timeScale = 5.0f;
    }

    public ResourceQueue GetQueue(string type) {

        return resources[type];
    }

    private GWorld() {

    }

    public static GWorld Instance {

        get { return instance; }
    }

    public WorldStates GetWorld() {

        return world;
    }
}
