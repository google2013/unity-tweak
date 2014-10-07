using UnityEngine;
using System.Collections;

public class CameraTTTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		print ("camera position is " + camera.transform.position.ToString() );
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);

		RaycastHit hit;	
		if ( Physics.Raycast(ray , out hit , 100 ))
		{
			if( Input.GetMouseButtonDown(0))
				print ("I'm looking at " + hit.transform.name);
		}
	}
}
