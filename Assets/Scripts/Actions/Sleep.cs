using UnityEngine;
using System.Collections.Generic;

public class Sleep : GAction
{

    public override bool PrePerform()
    {
        
        return true;
    }

    public override bool PostPerform()
    {
        beliefs.RemoveState("sleepy");
        return true;
    }

}
