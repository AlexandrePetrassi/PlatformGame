//######################################################################################################################
// PlatformCollider
// * Gerencia os coliders das plataformas.
//    Este script cria automaticamente BoxColliders unificados, ou seja, colliders adjacentes compartilham o mesmo
// BoxCollider, evitando problemas de transiçao de Colliders em terrenos planos.
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class PlatformCollider : MonoBehaviour {

	//------------------------------------------------------------------------------------------------------------------
	// Informa ao player que ele estah colidindo com esta plataforma
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerStay2D(Collider2D obj){
		if(obj.tag == Tags.PlayerFeet){
			Player.player.wall.groundCollide();
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Informa ao player que ele nao esta mais colidindo com esta plataforma
	//------------------------------------------------------------------------------------------------------------------
	void OnTriggerExit2D(Collider2D obj){
		if(obj.tag == Tags.PlayerFeet){
			Player.player.wall.groundUncollide();
		}
	}
}
