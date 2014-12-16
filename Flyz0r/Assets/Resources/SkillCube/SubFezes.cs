using UnityEngine;
using System.Collections;

public class SubFezes : MonoBehaviour {

	static SubFezes selected = null;

	private Color deselectedColor;
	private Color selectedColor;
	private Color disabledColor = Color.black;
	private Color selectedDisabledColor = Color.gray;
	public string normalMapName = "CrescentSlashSkill";
	private Texture bumpMap;
	private Texture reverseBumpMap;
	public bool enabled = false;

	// Use this for initialization
	void Awake () {

		//bumpMap = Resources.Load("SkillCube/SkillsNormalMaps/" + normalMapName) as Texture;
		//reverseBumpMap = Resources.Load("SkillCube/SkillsNormalMaps/" + normalMapName + "R") as Texture;
		renderer.material.SetTexture("_MainTex",(Texture)Resources.Load("SkillCube/SkillsNormalMaps/" + normalMapName + "T"));

		deselectedColor = renderer.material.color/2;
		selectedColor = renderer.material.color;
		deselect();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter(){
		select(this);
	}

	void OnMouseExit(){
		deselect();
	}

	void select(SubFezes selecting){
		if(selected != null) selected.deselect();
		selected = this;
		renderer.material.color = enabled?selectedColor:selectedDisabledColor;
		selecting.renderer.material.SetTexture("_BumpMap",bumpMap);
	}

	public void deselect(){
		renderer.material.color = enabled?deselectedColor:disabledColor;
		renderer.material.SetTexture("_BumpMap",reverseBumpMap);
		if(selected == this) selected = null;

	}
}
