using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWater : GAction
{
    public override bool PrePerform()
    {

        // get free waterhole space
        target = GWorld.Instance.GetQueue("waterholes").RemoveResource();
        // check if available
        if (target == null) return false;
        // add to inv
        inventory.AddItem(target);
        // remove from world temporarily
        GWorld.Instance.GetWorld().ModifyState("FreeWater", -1);
        return true;
    }

    public override bool PostPerform()
    {
        //release waterhole space back to world
        GWorld.Instance.GetQueue("waterholes").AddResource(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("FreeWater", 1);
        // remove thirst state
        beliefs.RemoveState("thirsty");
        return true;
    }
}
