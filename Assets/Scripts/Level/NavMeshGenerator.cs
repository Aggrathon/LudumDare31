using UnityEngine;
using System.Threading;
using RAIN.Navigation.NavMesh;

public class NavMeshGenerator : MonoBehaviour {

	[SerializeField]
	private int _threadCount = 4;
	[SerializeField]
	private GameObject navMeshObject;

	public void Start()
	{
		if(navMeshObject == null)
			navMeshObject = gameObject;
		GenerateNavmesh();
	}
	
	// This will regenerate the navigation mesh when called
	public void GenerateNavmesh()
	{
		NavMeshRig tRig = navMeshObject.GetComponent<NavMeshRig>();
		if(tRig == null) {
			tRig = navMeshObject.AddComponent<NavMeshRig>();
		}

		tRig.NavMesh.UnregisterNavigationGraph();
		
		tRig.NavMesh.StartCreatingContours(tRig, _threadCount);
		while (tRig.NavMesh.Creating)
		{
			tRig.NavMesh.CreateContours();
			Thread.Sleep(10);
		}
		
		tRig.NavMesh.RegisterNavigationGraph();

#if UNITY_EDITOR
			tRig.GenerateAllContourVisuals();
			tRig.NavMesh.DisplayMode = RAIN.Navigation.NavMesh.NavMesh.DisplayModeEnum.NavigationMesh;
#endif
	}
}
