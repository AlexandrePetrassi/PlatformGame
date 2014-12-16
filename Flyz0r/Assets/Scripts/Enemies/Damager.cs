//######################################################################################################################
// Damager
// * Script responsavel por informar ao Player que ele esta sofrendo dano
//######################################################################################################################
using UnityEngine;
using System.Collections;

public abstract class Damager : MonoBehaviour {

	public float damage = 10;             // Quantidade de dano causado por este objeto
	private bool hasHitPlayer = false;    // Flag que armazena a informaçao se o jogador ja foi atingido por este objeto
	public float knockbackPower = 1;      // Força com que o personagem eh empurrado para tras quando eh atingido
	public bool deleteOnTouch = true;     // Caso seja true, este objeto eh deletado apos causar dano no player (ex, projeteis)
	public bool ignoreHiding = false;     // caso seja true, este objeto causa dano mesmo quando o personagem estiver oculto

	void OnTriggerStay2D(Collider2D obj){
		if(!hasHitPlayer && obj.tag == "Player"){
			DamageModule dmg = Player.player.GetComponent<DamageModule>();
			if(dmg.damageTimeCount <= 0 && dmg.invencibilityCount <= 0){ // Apenas atinge o jogador se ele nao estiver sofrendo outro dano ou nao estiver invencivel
				dmg.damager = this;
				hasHitPlayer = deleteOnTouch;
			}
		}
		if(deleteOnTouch && obj.tag == "Platform") Destroy(gameObject);
		//if(obj.tag == "Platform" && deleteOnPlatform)
	}
	
	void OnTriggerExit2D(Collider2D obj){
		if(deleteOnTouch && hasHitPlayer) Destroy(this.gameObject);
	}

}
