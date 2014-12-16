using UnityEngine;
using System.Collections;

public class SoundModule : PlayerCommand {

	public AudioClip deathAudio;                      // Som do personagem morrendo
	public AudioClip damageAudio;                     // Som do personagem sofrendo dano
	public AudioClip dashAudio;                       // Som do personagem executando um dash

	public LeonSound soundToPlay = LeonSound.nothing; // Som que deve ser tocado nesta iteraçao

	//------------------------------------------------------------------------------------------------------------------
	// Checa se o personagem esta em contato com um objeto danoso
	//------------------------------------------------------------------------------------------------------------------
	override protected bool commandCondition(){
		return true;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// aplica o dano
	//------------------------------------------------------------------------------------------------------------------
	override protected void startCommand(){
		switch(soundToPlay){
		case LeonSound.death:
			playDeathAudio();
			break;
		case LeonSound.damage:
			playDamageAudio();
			break;
		case LeonSound.dash:
			playDashAudio();
			break;
		}
		if(soundToPlay != LeonSound.death) soundToPlay = LeonSound.nothing;
	}
	
	//------------------------------------------------------------------------------------------------------------------
	// aplica o efeito de imunidade temporaria ou regeneraçao de vida
	//------------------------------------------------------------------------------------------------------------------
	override protected void endCommand(){
		return;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Toca o som do personagem morrendo
	//------------------------------------------------------------------------------------------------------------------
	private void playDeathAudio(){
		audio.Stop();
		audio.clip = deathAudio;
		audio.pitch = 1;
		audio.Play();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Toca o som do personagem tomando dano
	//------------------------------------------------------------------------------------------------------------------
	private void playDamageAudio(){
		audio.clip = deathAudio;
		audio.pitch = Random.Range(1.1f,1.3f);
		audio.Play();
	}

	//------------------------------------------------------------------------------------------------------------------
	// Toca o som do personagem execudando dash
	//------------------------------------------------------------------------------------------------------------------
	private void playDashAudio(){
		audio.clip = dashAudio;
		audio.pitch = Random.Range(1.0f,1.2f);
		audio.Play();
	}

}

// Enum que indica qual som deve ser tocado
public enum LeonSound{
	nothing,
	death,
	damage,
	dash
}
