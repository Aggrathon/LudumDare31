using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class CheckSound : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
	{
		GameObject target = ai.WorkingMemory.GetItem<GameObject>("sound");
		if(target == null)
			return ActionResult.FAILURE;
		
		AIRig air = target.GetComponentInChildren<AIRig>();
		if(air == null)
			return ActionResult.FAILURE;
		
		int targetTeam = air.AI.WorkingMemory.GetItem<int>("team");
		int ownTeam = ai.WorkingMemory.GetItem<int>("team");
		
		if(targetTeam != ownTeam) {
			DataHolder.getBotServer(ownTeam).detectSound(target.transform.position);
		}
		ai.WorkingMemory.SetItem<GameObject>("sound", null);
		
		//Debug.Log(target+"  "+targetTeam+"  "+ownTeam);
		return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}