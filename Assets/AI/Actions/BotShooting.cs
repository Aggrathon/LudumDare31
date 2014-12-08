using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Perception.Sensors;

[RAINAction]
public class BotShooting : RAINAction
{
	Gun gun;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
		gun = ai.Body.GetComponentInChildren<Gun>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		
		GameObject shooting = ai.WorkingMemory.GetItem<GameObject>("shootTarget");
		if(shooting != null) {
			VisualSensor eyes = (VisualSensor)ai.Senses.GetSensor("visual");
			Vector3 offset = eyes.PositionOffset;
			Vector3 direction = shooting.transform.position-ai.Body.transform.position;
			RaycastHit hit;
			if(Physics.Raycast(offset+ai.Body.transform.position, direction, out hit, eyes.Range)) {
				if(hit.transform == shooting.transform) {
					if(Vector3.Angle(ai.Body.transform.forward, direction) < 20) {
						gun.Shoot(shooting.transform.position+gun.transform.position-gun.transform.parent.position);
						shooting.GetComponent<BotData>().health -= ai.WorkingMemory.GetItem<float>("damage");
					}
					return ActionResult.RUNNING;
				}
			}
			ai.WorkingMemory.SetItem<GameObject>("shootTarget", null);
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}