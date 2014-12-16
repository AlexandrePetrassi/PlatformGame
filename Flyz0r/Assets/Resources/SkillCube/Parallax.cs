using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	Material mat;
	// Use this for initialization
	void Awake () {
		mat = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		mat.SetTextureOffset("_MainTex",new Vector2(Time.frameCount/200.0f,Time.frameCount/300.0f));
	}
}
