using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Graph;

[RAINAction]
public class BotDestinationSelector : RAINAction
{
	private float _wanderLength;
	private BotServer _server;
	private int _timesWandered;

	public BotDestinationSelector() {
		actionName = "BotDestinationSelector";
	}

    public override void Start(RAIN.Core.AI ai)
    {
		base.Start(ai);
		
		_wanderLength = ai.WorkingMemory.GetItem<float>("wanderLength");
		if(_wanderLength == 0)
			_wanderLength = 10;

		_server = DataHolder.getBotServer(ai.WorkingMemory.GetItem<int>("team"));

		_timesWandered = 10;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
	{
		if(ai.WorkingMemory.GetItem<bool>("hasTarget"))
			return ActionResult.SUCCESS;
		ai.WorkingMemory.SetItem<float>("stuckTime",0f);
		AreaPriority apri = _server.getProposedTarget(ai.Body.transform.position);
		if(apri.priority + _timesWandered*10 < DataHolder.getData().wanderThreshold)
			return Wander (ai);
		else
			return Patrol (ai, apri);
	}
	
	private ActionResult Wander(RAIN.Core.AI ai) {
		float hl = _wanderLength/2;
		_timesWandered++;
		ai.WorkingMemory.SetItem<string>("status","Wandering");
		return findDestinationInArea(ai, ai.Kinematic.Position, hl, hl);
	}
	
	private ActionResult Patrol(AI ai, AreaPriority area) {
		_timesWandered = 0;
		ai.WorkingMemory.SetItem<string>("status","Patrolling");
		return findDestinationInArea(ai, area.areaCenter, area.radie, area.radie);
	}

	private ActionResult findDestinationInArea(AI ai, Vector3 center, float halfWidth, float halfHeight) {
		List<RAINNavigationGraph> found;
		int loops = 0;
		Vector3 rand, dest;
		do {
			if(++loops > 20) {
				dest = center + new Vector3(Random.Range(-halfWidth/2, halfWidth/2), 0, Random.Range(-halfHeight/2, halfHeight/2));
				break;
			}

			do {
				rand = new Vector3(Random.Range(-halfWidth,halfWidth),0,Random.Range(-halfHeight, halfHeight));
			} while (rand.sqrMagnitude < halfWidth/2);

			dest = center + rand;

			found = NavigationManager.Instance.GraphsForPoints(ai.Kinematic.Position,
			             dest, ai.Motor.StepUpHeight, NavigationManager.GraphType.Navmesh,
			             ((BasicNavigator)ai.Navigator).GraphTags);

		} while (found.Count == 0);
			
		ai.WorkingMemory.SetItem<Vector3>("moveTarget", dest);
		ai.WorkingMemory.SetItem<bool>("hasTarget", true);
		return ActionResult.SUCCESS;
	}

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}