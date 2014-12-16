//######################################################################################################################
// PlayerCommand
// * Superclasse de todos os modulos do jogador. Controla e padroniza o compartamento de todos os comandos
//######################################################################################################################
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Player))] 
public abstract class PlayerCommand : PlayerBehaviour {

	public virtual void runCommand(){
		if(commandCondition())
			startCommand();
		else 
			endCommand();
	}

	protected abstract bool commandCondition();

	protected abstract void startCommand();

	protected abstract void endCommand();
}
