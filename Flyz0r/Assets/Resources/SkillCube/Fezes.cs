using UnityEngine;
using System.Collections;

public class Fezes : MonoBehaviour {

	public GameObject cubows;

	// Use this for initialization
	void Start () {
		Instantiate(cubows,Vector3.zero,Quaternion.identity);
	}

	void Update(){
		transform.RotateAround(new Vector3(1.0f,1.5f,1.0f),transform.rotation * new Vector3(1,0,0),2*Input.GetAxis("Vertical"));
		transform.RotateAround(new Vector3(1.0f,1.5f,1.0f),transform.rotation * new  Vector3(0,1,0),-2*Input.GetAxis("Horizontal"));
	}

}
