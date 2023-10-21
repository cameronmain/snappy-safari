using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : GAction
{
    GameObject resource;
    public override bool PrePerform()
    {
        // Set our target patient and remove them from the Queue
        target = GWorld.Instance.GetQueue("zebras").RemoveResource();
        if (target == null)
        {
            return false;
        }
            // No patient so return false
            
        else
        {

            // No free cubicles so release the patient
            //GWorld.Instance.GetQueue("patients").AddResource(target);
            target = null;
            return false;
        }

        //take away one cubicle being available from the world state
        //GWorld.Instance.GetWorld().ModifyState("FreeCubicle", -1);
        //return true;
    }

    public override bool PostPerform()
    {

        return true;
    }
}
