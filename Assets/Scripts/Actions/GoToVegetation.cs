using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToVegetation : GAction
{
    public override bool PrePerform()
    {
        // get free vegetation
        target = GWorld.Instance.GetQueue("vegetation").RemoveResource();
        // check if we got valid vegetation
        if (target == null) return false;
        // add the vegetation to the inventory
        inventory.AddItem(target);
        // remove its availability from the world
        GWorld.Instance.GetWorld().ModifyState("FreeVeg", -1);
        return true;
    }

    public override bool PostPerform()
    {
        // return the vegetation to the pool
        GWorld.Instance.GetQueue("vegetation").AddResource(target);
        // remove the vegetation from the inventory
        inventory.RemoveItem(target);
        // make the vegetation available again in the world
        GWorld.Instance.GetWorld().ModifyState("FreeVeg", 1);
        // remove the satiated belief so the agent doesn't remain in this state indefinitely
        beliefs.RemoveState("satiated");
        return true;
    }
}
