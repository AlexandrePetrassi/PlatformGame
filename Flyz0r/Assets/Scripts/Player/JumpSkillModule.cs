//######################################################################################################################
// JumpSkillModule
// * Modulo do jogador responsavel por controlar a movimentaçao vertical e todos os tipos de saltos e suas variaçoes
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class JumpSkillModule : PlayerCommand {

	//Flags
	[HideInInspector] public bool wasPressed = false;    // Contem a informaçao de botao de açao estava sendo pressionado na iteraçao anterior
	[HideInInspector] public bool dashJumping = false;   // Indica se o personagem esta realizando um dashJump
	[HideInInspector] public bool flyingKick = false;    // Indica que o personagem esta realizando uma voadora

	// Jump
	public float jumpPower = 6;		                     // Força do salto

	//Wall Jump
	[HideInInspector] public float wallJumpCooldown = 0; // cooldown do wallJump
	public float wallJumpTime = 8;                       // Tempo inicial do coolDown do wallJump

	// Air Jump
	public float airJumpMax = 2;                         // Maximo do saltos sem tocar o chao
	[HideInInspector] public float airJumpCount = 0;     // Quantidade de saltos realizados sem tocar o chao
	public GameObject airJumpVisualEffect;               // GameObject responsavel pelo efeito visual do airJump

	//------------------------------------------------------------------------------------------------------------------
	// Sobrecarga do comando principal
	//------------------------------------------------------------------------------------------------------------------
	public override void runCommand(){
		checkCollision();
		base.runCommand();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para salto sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(dmg.death) return false; // bloqueia o comando caso o personagem esteja morto
		if(dmg.damageTimeCount>0) return false; // Bloquei/a o comando caso o personagem esteja sofrendo dano
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding")) return false; // Salto bloquado enquanto se esconde
		if(atk.isAttacking()) return false; // Bloqueia o comando caso o personagem esteja realizando um ataque
		return isKeyPressed(); // Tecla necessaria para realizar a açao
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se a tecla do comando esta pressionada
	//------------------------------------------------------------------------------------------------------------------
	public bool isKeyPressed() {
		return (Input.GetAxisRaw("Jump") != 0);
	}

	//------------------------------------------------------------------------------------------------------------------
	// tenta realizar um salto
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		--wallJumpCooldown; // Diminue gradativamente o Cooldown para que o personagem possa se movimentar novamente
		if(!wasPressed){
			if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Wall"))
				wallJump();
			else if(airJumpCount < airJumpMax){
				if(Input.GetAxisRaw("Vertical") < 0f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Jumping")) FlyingKick(); // Caso o jogador esteja apertando a tecla para baixo, ao inves de pular o personagem executa uma voadora
				else normalJump(); // salto normal
			}

		}
		wasPressed = true;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Realiza um WallJump
	//------------------------------------------------------------------------------------------------------------------
	void wallJump(){
		dashJumping = (Input.GetAxisRaw("Fire2") != 0);
		wallJumpCooldown = wallJumpTime;
		float horizontal = jumpPower*-Mathf.Sign(transform.localScale.x)/(dashJumping?1:2);
		rb.velocity = new Vector2(horizontal,jumpPower);
		++airJumpCount;
		if(dashJumping) sound.soundToPlay = LeonSound.dash;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Realiza um Salto normal, airJump ou dashJump
	//------------------------------------------------------------------------------------------------------------------
	void normalJump(){
		dashJumping = dash.isDashing() || dashJumping; // dashJump so pode ser realizado se o personagem estiver em meio a um dash
		if(!wall.groundColliding) ++airJumpCount; // Apenas adciona +1 no contador caso o personagem esteja no ar, pois esse +1 eh adcionado automaticamente na checagem de colisao com o chao quando o personagem cai de uma plataforma
		float horizontal = dashJumping?rb.velocity.x:0;
		rb.velocity = new Vector2(horizontal,jumpPower);
		airJumpVisualImage();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia uma unica imagem de dash no ar quando o personagem realiza um airJump
	//------------------------------------------------------------------------------------------------------------------
	void airJumpVisualImage(){
		if(airJumpCount > 1){
			GameObject image = Instantiate(airJumpVisualEffect,this.transform.position,this.transform.rotation) as GameObject;
			image.transform.localScale = transform.localScale;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Realiza uma voadora
	//------------------------------------------------------------------------------------------------------------------
	void FlyingKick(){
		flyingKick = true;
		rb.velocity = new Vector2(jumpPower*Mathf.Sign(transform.localScale.x),-jumpPower);
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Zera a velocidade vertical, e reabilita o salto do personagem
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		if(wasPressed && rb.velocity.y>0) rb.velocity = new Vector2(rb.velocity.x,0);
		wasPressed = false;
		wallJumpCooldown = 0; // zera o o Cooldown para que o personagem possa se movimentar novamente
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se o personagem colidiu com plataformas, para resetar a quantidade de saltos que ele pode executar
	//------------------------------------------------------------------------------------------------------------------
	private void checkCollision(){
		if((wall.platformColliding && anim.GetCurrentAnimatorStateInfo(0).IsTag("Wall")) || wall.groundColliding){
			airJumpCount = 0; //Reabilita o airJump caso o personagem toque o chao ou uma plataforma
			dashJumping = false;
		}
	}
}
