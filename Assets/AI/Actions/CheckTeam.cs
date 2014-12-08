using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Perception.Sensors;

[RAINAction]
public class CheckTeam : RAINAction
{
	public CheckTeam() {
		actionName = "Check if Target is a teammate";
	}

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

		GameObject target = ai.WorkingMemory.GetItem<GameObject>("PotentialTarget");
		if(target == null)
			return ActionResult.FAILURE;
	
		AIRig air = target.GetComponentInChildren<AIRig>();
		if(air == null)
			return ActionResult.FAILURE;

		int targetTeam = air.AI.WorkingMemory.GetItem<int>("team");
		int ownTeam = ai.WorkingMemory.GetItem<int>("team");

		if(targetTeam != ownTeam) {
			if (ai.WorkingMemory.GetItem<GameObject>("shootTarget") == null)
				ai.WorkingMemory.SetItem<GameObject>("shootTarget", target);
			DataHolder.getBotServer(ownTeam).detectVisual(target.transform.position);
		} else {
			ai.WorkingMemory.SetItem<GameObject>("PotentialTarget", null);
		}
	
		//Debug.Log(target+"  "+targetTeam+"  "+ownTeam);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}