//######################################################################################################################
// DashImage
// * Componente que controla o comportamento da sombra do Dash do jogador
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class DashImage : MonoBehaviour {

	SpriteRenderer sr; // sprite Renderer
	public float fadeSpeed = 0.1f;

	//------------------------------------------------------------------------------------------------------------------
	// Inicializaçao a imagem
	//------------------------------------------------------------------------------------------------------------------
	void Awake(){
		sr = this.GetComponent<SpriteRenderer>();
		sr.sprite = Player.player.GetComponent<SpriteRenderer>().sprite; // captura a sprite atual do player
	}

	//------------------------------------------------------------------------------------------------------------------
	// update da imagem
	//------------------------------------------------------------------------------------------------------------------
	void FixedUpdate () {
		sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,sr.color.a-fadeSpeed); // diminue a opacidade gradativamente
		if(sr.color.a <= 0) Destroy(gameObject); // Deleta o objeto quando a opacidade for zero
	}
}
