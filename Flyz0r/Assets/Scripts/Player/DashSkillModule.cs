//######################################################################################################################
// DashSkillModule
// * Modulo do jogador responsavel por controlar o dash e todas as suas funçoes
//######################################################################################################################
using UnityEngine;
using System.Collections;

public class DashSkillModule : PlayerCommand {

	//Flags
	private bool wasPressed = false;                       // Contem a informaçao de botao de açao estava sendo pressionado na iteraçao anterior
	[HideInInspector] public int dashing = -1000;          // controle do dash. Quando for menor que zero, o dash termina.

	// Dash
	public int   dashTime = 15; 		                   // Tempo de duraçao do dash
	public float speedMultiplyer = 3.0f;                   // Velocidade de movimento

	// Air Dash
	public  bool enableAirDash = true;                     // habilita o dash no ar
	public  int  airDashMax = 2;                           // Quantidade maxima de vezes que o personagem pode realizar um airDash
	public int airDashCount = 0;         // contador de vezes que o airDash foi realizado

	// Efeito visual
	public GameObject dashImage;                           // GameObject do rastro do dash
	public int dashEffectExtaTime = 12;                    // Quantidade extra de frames que o efeito de dash dura depois do fim do dash propriamente dito
	[HideInInspector] public bool continueDashing = false; // flag que indica que o personagem ainda deve ter o rastro do dash presente mesmo apos o dash

	public int dashActivationTimeByWalkTolleranceTime = 20;// Tempo de espera para que o dash seja executado caso o jogador aperte o direcional duas vezes seguidas
	public int dashActionTimeByWalkCount = 20;             // Contador do tempo de espera
	public bool isFacingRight = true;                      // true = direita / false = esquerda;
	public bool dashingByDirection = false;                // Flag que indica que o jogador esta "dashando" usando os direcionais
	public bool wasDirectionPressed = false;               // Flag que indica se o direcional estava sendo pressionado na iteraçao anterior

	//------------------------------------------------------------------------------------------------------------------
	// Tenta executar o comando de personagem ou interrompe-lo
	//------------------------------------------------------------------------------------------------------------------
	override public void runCommand() {
		base.runCommand();
		setGravity();
		dashEffect();
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Checa se as codiçoes para dash sao propicias
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		if(dmg.death) return false; // bloqueia o comando caso o personagem esteja morto
		if(atk.isAttacking()) return false; // Bloqueia o comando caso o personagem esteja realizando um ataque
		if(dmg.damageTimeCount>0) return false; // Bloqueia o comando caso o personagem esteja sofrendo dano
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Hiding")) return false; // Bloqueia o comando caso o personagem esteja se escondendo
		if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Wall")) return false; // Bloqueia o comando caso o personagem esteja escorregando por uma parede
		if(dmg.damageTimeCount > 0) return false; // Interrompe ou bloqueia o dash caso o personagem esteja sofrendo dano
		if(!enableAirDash && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Iddle")) return false; // Bloqueia o airDash caso o personagem nao esteja no chao
		if(dashing > 0 || wasPressed) return false; // Bloqueia o dash caso o dash ja esteja sendo realizado
		if(airDashCount >= airDashMax) return false; // Bloqueia o dash caso o limite de dashes seja atingido
		return isKeyPressed();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Checa se a tecla do comando esta pressionada
	//------------------------------------------------------------------------------------------------------------------
	private bool isKeyPressed() {
		bool val = false;

		// Controle de doubleTap dos direcionais
		++dashActionTimeByWalkCount;
		if(Input.GetAxisRaw("Horizontal")!=0)
			val = isDoubleTapping(Input.GetAxisRaw("Horizontal")>0);
		else{
			dashingByDirection = false;
			wasDirectionPressed = false;
		}

		// Checagem da tecla padrao de dash
		if(!val) val = (Input.GetAxisRaw("Dash") != 0);
		return val;
	}

	//------------------------------------------------------------------------------------------------------------------
	// gerencia e checa se o jogador executou um double tap nos direcionais horizontais
	//------------------------------------------------------------------------------------------------------------------
	private bool isDoubleTapping(bool direction){

		bool val = false;
		if(!wasDirectionPressed && isFacingRight == direction){
			if(dashActionTimeByWalkCount < dashActivationTimeByWalkTolleranceTime){
				dashingByDirection = true;
				val = true;
			}
		}else{
			if(dashing <= 0)
				dashingByDirection = false;
			
			if(dashingByDirection)
				val = true;
		}

		if(!val && !wasDirectionPressed) dashActionTimeByWalkCount = 0;
		//else dashActionTimeByWalkCount = dashActivationTimeByWalkTolleranceTime;

		wasDirectionPressed = true;
		isFacingRight = direction;

		return val;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inicia o Dash
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		dashing = dashTime;
		jump.dashJumping = false; // Interrompe um dashJump caso um airDash seja executado
		continueDashing = true; // Controlador para o efeito visual de dash
		++airDashCount;
		rb.velocity = new Vector2(rb.velocity.x,0);
		sound.soundToPlay = LeonSound.dash;
		wasPressed = true;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Interrompe o dash atual e reabilita o incio de um dash depois que tecla shift for solta
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		if(wall.platformColliding || wall.groundColliding){ // Checa se o personagem tocou alguma plataforma
			airDashCount = 0; // Reabilita o airDash caso o personagem esteja em contato com uma plataforma
			continueDashing = false; // Interrompe o efeito visual do dash
		}
		if(dmg.damageTimeCount>0){
			continueDashing = false; // Interrompe o efeito visual do dash caso o personagem sofra dano
			dashing = -dashEffectExtaTime; // Interrompe o dash subtamente
		}
		if(wall.platformColliding || !isKeyPressed()) dashing = -dashEffectExtaTime; // Interrompe o dash subtamente
		else --dashing; // Interrompe o dash gradativamente
		wasPressed = isKeyPressed();
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// Elimina a gravidade enquanto esta realizando o dash, e volta a gravidade ao normal quando o dash eh interrompido
	//------------------------------------------------------------------------------------------------------------------
	void setGravity(){
		if(dashing > 0 && !jump.dashJumping && rb.velocity.y ==0) rb.gravityScale = 0;
		else rb.gravityScale = 1;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Cria o efeito de dash
	//------------------------------------------------------------------------------------------------------------------
	void dashEffect(){
		if((dashing > -dashEffectExtaTime || continueDashing || jump.dashJumping) && dmg.damageTimeCount<=0 || jump.flyingKick){
			GameObject image = Instantiate(dashImage,this.transform.position,this.transform.rotation) as GameObject;
			image.transform.localScale = transform.localScale;
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Retorna true caso o personagem ainda esteja no meio da execuçao de um dash
	//------------------------------------------------------------------------------------------------------------------
	public bool isDashing(){
		return dashing > 0 || atk.attackType == AttackType.dash;
	}

}
