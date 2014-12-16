//######################################################################################################################
// PunchSkillModule
// * Modulo do jogador responsavel por controlar a habilidade de soco
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class AttackSkillModule : PlayerCommand {
	
	[HideInInspector] public bool wasPressed = false;    // Contem a informaçao de botao de açao estava sendo pressionado na iteraçao anterior
	[HideInInspector] public AttackType attackType;      // Informaçao se o persoangem esta realizano um ataque

	//CoolDown
	public int attackTime_Normal = 13;                   // Tempo que o personagem fica travado realizando o ataque
	public int attackTime_Dash = 20;                     // Tempo que o personagem fica travado realizando o ataque
	public int attackTime_Air = 20;                      // Tempo que o personagem fica travado realizando o ataque
	[HideInInspector] public int attackCount = 0;        // Contador do tempo de ataque

	public bool autoAttack = false;                      // Caso seja true, o personagem ataca continuamente enquanto o botao de ataque estiver pressionado, senao, e necessario que o jogador solte e aperte a tecla novamente para que um novo ataque seja realizado
	[HideInInspector] public bool autoAttacked = false;  // Flag que contem a informaçao se o jogador ja realizou o auto ataque ou nao

	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para esconder-se sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(isAttacking()) return false;
		if(!autoAttack && autoAttacked) return false;
		return isKeyPressed(); // Tecla necessaria para realizar a açao
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Checa se a tecla do comando esta pressionada
	//------------------------------------------------------------------------------------------------------------------
	private bool isKeyPressed() {
		return (Input.GetAxisRaw("Attack") != 0);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Checa qual tipo de ataque devera ser desferido pelo personagem
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		attackType = getAttackType();
		switch(attackType){
		case AttackType.normal:
			startAttack_Normal();
			break;
		case AttackType.dash:
			startAttack_Dash();
			break;
		case AttackType.air:
			startAttack_Air();
			break;
		default:
			endCommand();
			break;
		}
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Torna o personagem visivel novamente
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		if(!isKeyPressed()) autoAttacked = false;
		if(dmg.damageTimeCount > 0){
			attackType = AttackType.none;
			attackCount = 0;
			return;
		}
		
		if(!isAttacking()) return;
		if(--attackCount > 0) return;

		attackType = AttackType.none;
		attackCount = 0;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa as condiçoes para adequar o tipo de ataque desferido em relaçao ao estado do personagem
	//------------------------------------------------------------------------------------------------------------------
	private AttackType getAttackType(){
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Dash")) return AttackType.dash; // DashAttack
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump")) return AttackType.air; // DashAttack
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Iddle")) return AttackType.normal; // DashAttack
		return AttackType.none;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inicia um ataque normal
	//------------------------------------------------------------------------------------------------------------------
	private void startAttack_Normal(){
		attackCount = attackTime_Normal;
		autoAttacked = true;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inicia um dash Attack
	//------------------------------------------------------------------------------------------------------------------
	private void startAttack_Dash(){
		attackCount = attackTime_Dash;
		autoAttacked = true;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inicia um ataque aereo
	//------------------------------------------------------------------------------------------------------------------
	private void startAttack_Air(){
		attackCount = attackTime_Air;
		autoAttacked = true;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se o personagem esta atacando
	//------------------------------------------------------------------------------------------------------------------
	public bool isAttacking(){
		return attackType != AttackType.none;
	}

}

public enum AttackType{
	none,
	normal,
	dash,
	air
}
