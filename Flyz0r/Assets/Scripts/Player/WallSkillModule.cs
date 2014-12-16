//######################################################################################################################
// WallSkillModule
// * Modulo do jogador que controla: velocidade de deslizamento; Detecçao de colisao com parede e chao
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class WallSkillModule : PlayerCommand {

	[HideInInspector] public bool platformColliding = false; // true caso o personagem esteja tocando uma parede
	[HideInInspector] public bool groundColliding = false;   // True caso o personagem esteja tocando o chao
	public float wallGlideVelocity = -0.05f;                 // Intensidade da força do deslizamento na parede

	override protected bool commandCondition(){
		if(dmg.death) return false; // bloqueia o comando caso o personagem esteja morto
		if(dmg.damageTimeCount>0) return false; // Bloqueia o comando caso o personagem esteja sofrendo dano
		return platformColliding && move.horizontal != 0; // apenas "gruda" na parede caso o jogador esteja se pressionando contra a parede
	}

	override protected void startCommand(){
		rb.velocity = new Vector2(rb.velocity.x, wallGlideVelocity);
	}

	override protected void endCommand(){
		return;
	}

	override public void runCommand(){

		base.runCommand();
	}

	public void groundCollide(){
		groundColliding = true;
		jump.flyingKick = false;
	}

	public void groundUncollide(){
		groundColliding = false;
		if(jump.airJumpCount == 0)++jump.airJumpCount;
		if(jump.dashJumping || dash.isDashing()) ++dash.airDashCount;
	}
}
