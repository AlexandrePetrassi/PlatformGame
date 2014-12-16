//######################################################################################################################
// HideSkillModule
// * Modulo do jogador responsavel por controlar a habilidade do jogador de esconder-se
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class HideSkillModule : PlayerCommand {

	public bool trulyHiding = false; // fica true quando o personagem se esconde efetivamente em um esconderijo valido
	public bool iluminated = false; // fica true caso o personagem esteja sendo iluminado por alguma lampada, impedindo-o de esconder-se efetivamente
	private float alpha = 1;
	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para esconder-se sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(dmg.death) return false; // bloqueia o comando caso o personagem esteja morto
		if(dmg.damageTimeCount>0) return false; // Bloqueia o comando caso o personagem esteja sofrendo dano
		if(atk.isAttacking()) return false; // Bloqueia o comando caso o personagem esteja realizando um ataque
		bool isButtonPressed = (Input.GetAxisRaw("Vertical") > 0f);
		bool isIddleOrHiding = (anim.GetCurrentAnimatorStateInfo(0).IsTag("Iddle") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding"));
		return isButtonPressed && isIddleOrHiding;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Torna o personagem invisivel
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		anim.SetBool("Hiding",true);
		alpha = iluminated ? 1.0f : alpha + ((alpha>0.3)?-0.05f:0);
		sr.sortingOrder = -2;
		if(trulyHiding && !dmg.isInvisible()) sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,iluminated?1.0f:alpha);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Torna o personagem visivel novamente
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		anim.SetBool("Hiding",false);
		if(alpha < 1) alpha = iluminated ? 1.0f : alpha + 0.1f;
		sr.sortingOrder = 0;
		if(sr.color.a < 1 && !dmg.isInvisible() && !dmg.death) sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,iluminated?1.0f:alpha);
	}

}
