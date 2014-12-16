//######################################################################################################################
// Platform
// * Gerencia a criaçao dos colliders das plataformas.
//    Este script cria automaticamente BoxColliders unificados, ou seja, colliders adjacentes compartilham o mesmo
// BoxCollider, evitando problemas de transiçao de Colliders em terrenos planos.
//######################################################################################################################
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Platform : MonoBehaviour {

	private static bool done = false;                                               // Flag que indica que o processo de criaçao dos colliders ja foi realizado
	private static List<List<bool>> colliders = new List<List<bool>>();             // Lista Bidimensional que armazena a posiçao de cada tile criado
	private static List<List<Vector4>> JoinedColliders = new List<List<Vector4>>(); // Lista Bidimensional de Colliders unidos

	//------------------------------------------------------------------------------------------------------------------
	// Adciona Adciona uma flag True no array de posiçao dos colliders, informando a posiçao deste tile
	//------------------------------------------------------------------------------------------------------------------
	private void addToArray(int x, int y){
		if(colliders.Count <= y){
			increaseArraySize<List<bool>>(colliders, y);
		}
		if(colliders[y].Count <= x){
			increaseArraySize<bool>(colliders[y], x);
		}
		colliders[y][x] = true;
	}

	//------------------------------------------------------------------------------------------------------------------
	// Metodo que aumenta o tamanho do array para o tamanho especificado, preenchendo-o com valores vazios
	//------------------------------------------------------------------------------------------------------------------
	private void increaseArraySize<T>(List<T> array,int size)where T:new(){
		for(int i = array.Count; i <= size; ++i){
			array.Add(new T());
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Retorna a posiçao central de um tile, dado as suas dimensoes definidas em um Vector4
	//------------------------------------------------------------------------------------------------------------------
	private Vector3 getCenterPosition(Vector4 pos){
		return new Vector3(
			pos.x + pos.z/2 - 0.5f,
			pos.y + pos.w/2 - 0.5f,
			0.0f
		);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Ao inicializar, adiciona no array colliders o valor True na posiçao (x,y) deste tile no mundo
	//------------------------------------------------------------------------------------------------------------------
	void Awake(){
		addToArray(
			(int)transform.position.x,
			(int)transform.position.y
		);
	}

	//------------------------------------------------------------------------------------------------------------------
	// Inicializaçao, chamada depois que todos os Awakes dos outros objetos foram chamados
	//------------------------------------------------------------------------------------------------------------------
	void Start(){
		if(done) return;
		JoinColliders();
		instantiateColliders();
	}

	//------------------------------------------------------------------------------------------------------------------
	//  Itera pela matriz Colliders, procurando por colliders adjantes, criando uma nova matriz de Vector4 com as 
	// posiçoes de cada Collider unificado
	//------------------------------------------------------------------------------------------------------------------
	private static void JoinColliders(){
		done = true;
		// Juntar na horizontal

		for(int i = 0; i < colliders.Count;++i){
			int w = 0; // Largura do Collider
			List<Vector4> line = new List<Vector4>();
			for(int j = 0; j < colliders[i].Count; ++j){
				++w;
				if(colliders[i][j] != true || j == colliders[i].Count-1){
					if(w-(colliders[i][j] != true?1:0) > 0){
						line.Add(new Vector4(j-w+1,i,w-(colliders[i][j] != true?1:0),1));
					}
					w = 0;
				}
			}
			JoinedColliders.Add(line);
		}
	}

	//------------------------------------------------------------------------------------------------------------------
	// Instancia os todos colliders que foram unidos
	//------------------------------------------------------------------------------------------------------------------
	void instantiateColliders(){
		foreach(List<Vector4> line in JoinedColliders)
			foreach(Vector4 vec in line){
				GameObject collider = 
					new GameObject(
						"Collider " + vec.x + ":" + vec.y +  ":" + vec.z + ":" + vec.w
					);
				collider.transform.position = getCenterPosition(vec);
				collider.AddComponent<BoxCollider2D>().size = new Vector3(vec.z,vec.w,0.0f);
				collider.AddComponent<PlatformCollider>();
			}
		colliders = null;
	}
}

