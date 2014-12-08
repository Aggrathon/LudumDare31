using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;

[System.Serializable]
public class BotServer {

#region setup
	public float patrolPriority = 0.5f;
	public float sightPriority = 3.5f;
	public float soundPriority = 2f;
	public float botsInRoomBonus = 40f;
	public float adjacentRoomBonus = 20f;
	public float distancePriority = 10f;
	public float botsInRoomCapasity = 6f;
	public float timeCap = 30f;
	public float randomness = 60f;

	private RoomData[] rooms;
	private List<AI> bots;

	public bool defeated = false;

	
	public BotServer(Data data) : this() {
		patrolPriority = data.patrolPriority;
		sightPriority = data.sightPriority;
		soundPriority = data.soundPriority;
		botsInRoomBonus = data.botsInRoomBonus;
		adjacentRoomBonus = data.adjacentRoomBonus;
		distancePriority = data.distancePriority;
		botsInRoomCapasity = data.botsInRoomCapasity;
		timeCap = data.timeCap;
		randomness = data.randomness;
	}
	public BotServer() {
		rooms = new RoomData[9];
		rooms[0] = new RoomData( new int[] {1,3} );
		rooms[1] = new RoomData( new int[] {0,2,4} );
		rooms[2] = new RoomData( new int[] {2,5} );
		rooms[3] = new RoomData( new int[] {0,4,6} );
		rooms[4] = new RoomData( new int[] {1,3,5,7} );
		rooms[5] = new RoomData( new int[] {4,3,8} );
		rooms[6] = new RoomData( new int[] {7,3} );
		rooms[7] = new RoomData( new int[] {4,7,8} );
		rooms[8] = new RoomData( new int[] {5,7} );

		bots = new List<AI>();
	}

	public void RegisterBot(AI bot) {
		bots.Add(bot);
	}
	 public void UnregisterBot(AI bot) {
		bots.Remove(bot);
		if(bots.Count == 0)
			defeated = true;
	}
#endregion

	public void Update (float timeDelta) {
		foreach (RoomData rd in rooms) {
			rd.timeForward(timeDelta);
		}
	}

#region calculatePriorities
	public void recalculatePriorities() {
		calculateBotLocations();
		for (int i = 0; i< rooms.Length; i++)
			calculateRoomPriority(i);
	}

	public void calculateBotLocations() {
		foreach(RoomData d in rooms)
			d.botsComing = d.botsInRoom = 0;

		foreach(AI bot in bots) {
			if(bot == null)
				continue;

			int pos = getRoom(bot.Body.transform.position);
			if(pos >= 0)
				rooms[pos].botsInRoom++;
			
			if(!bot.WorkingMemory.GetItem<bool>("hasTarget")) {
				if(pos >= 0)
					rooms[pos].botsComing++;
			} else {
				Vector3 des = bot.WorkingMemory.GetItem<Vector3>("moveTarget");
				int posdes = getRoom(des);
				if(posdes<0) {
					if(pos >= 0)
						rooms[pos].botsComing++;
				} else
					rooms[posdes].botsComing++;
			}

		}
	}

	public float calculateRoomPriority(int roomNr) {
		RoomData room = rooms[roomNr];
		float pri = 0;
		
		pri += Mathf.Min(room.lastPatrolled, timeCap)*patrolPriority;
		pri += (50f - Mathf.Min(room.lastSighted, timeCap))*sightPriority;
		pri += (50f - Mathf.Min(room.lastSound, timeCap))*soundPriority;

		foreach (int i in room.connectedRooms) {
			if(rooms[i].botsInRoom>0) {
				pri += adjacentRoomBonus;
				break;
			}
		}

		float roomBots = room.botsInRoom+room.botsComing;
		if(roomBots < botsInRoomCapasity && roomBots > 0)
			pri += botsInRoomBonus;
		if(roomBots>botsInRoomCapasity*2)
			pri -= botsInRoomBonus;

		if(roomNr == 4) 
			pri -= 30f;
		else if(roomNr%2 ==0)
			pri += 30f;

		return rooms[roomNr].priority = pri;
	}
#endregion

#region getters
	public static int getRoom(Vector3 position) {
		int x = (int)((position.x+15)/10);
		int y = (int)((position.z-15)/-10);
		if(x>2 || y>2 || x<0 || y<0) 
			return -1;
		return x+y*3;
	}
	
	public RoomData getRoomData(int x, int y) {
		return getRoomData(x+3*y);
	}
	public RoomData getRoomData(int i) {
		if(i<rooms.Length && i>=0)
			return rooms[i];
		else
			return null;
	}
	

	public AreaPriority getProposedTarget(Vector3 position) {
		recalculatePriorities();
		int roomPos = getRoom(position);
		int roomPri = 0;
		float maxPri = -10f;

		for(int i = 0; i <rooms.Length; i++) {
			float prir = rooms[i].priority;
			int dist = 3;
			if (i == roomPos) 
				dist = 1;
			else {
				foreach (int j in rooms[i].connectedRooms) {
					if( roomPos == j ) {
						dist = 0;
						break;
					}
					foreach (int k in rooms[j].connectedRooms) {
						if( roomPos == k) {
							dist = 2;
							break;
						}
					}
				}
			}
			prir += (3-dist)*distancePriority;
			if(maxPri < prir+ Random.Range(-randomness, randomness) ) {
				roomPri = i;
				maxPri = prir;
			}
		}
		float x = (roomPri%3-1)*10f;
		float y = (((int)(roomPri/3))*(-1)+1)*10f;

		return new AreaPriority( maxPri, new Vector3(x, 0, y ), 4.5f);	//0:-10 1:0 2:10  0:10 1:0 2:-10
	}
#endregion

#region alerts
	
	public void detectSound(Vector3 pos) {
		int r = getRoom(pos);
		if(r>=0) {
			rooms[r].lastSound = 0;
			recalculatePriorities();
		}
	}
	public void detectVisual(Vector3 pos) {
		int r = getRoom(pos);
		if(r>=0) {
			rooms[r].lastSighted = 0;
			recalculatePriorities();
		}
	}
	public void patrolRoom(Vector3 pos) {
		int r = getRoom(pos);
		if(r>=0) {
			rooms[r].lastPatrolled = 0;
			recalculatePriorities();
		}
	}

	public void muteRoom(Vector3 pos) {
		int r = getRoom(pos);
		if(r>=0) {
			rooms[r].lastSound += timeCap/2;
			rooms[r].lastSighted += timeCap/2;
			recalculatePriorities();
		}
	}

#endregion
}

#region helperClasses

public class RoomData {
	public int[] connectedRooms;

	public float lastPatrolled;
	public float lastSighted;
	public float lastSound;

	public int botsInRoom;
	public int botsComing;

	public float priority;

	public RoomData(int[] conRooms) {
		connectedRooms = conRooms;
		lastPatrolled = 0f;
		lastSighted = 30f;
		lastSound = 30f;

		botsInRoom = 0;
		botsComing = 0;
	}

	public void timeForward (float timeStep) {
		lastSound += timeStep;
		lastSighted += timeStep;
		if(botsInRoom == 0)
			lastPatrolled += timeStep;
		else 
			lastPatrolled = 0;
	}
	
}

public class AreaPriority {
	public float priority;
	public Vector3 areaCenter;
	public float radie;

	public AreaPriority (float priority, Vector3 areaCenter, float radie)
	{
		this.priority = priority;
		this.areaCenter = areaCenter;
		this.radie = radie;
	}
		
}
#endregion
