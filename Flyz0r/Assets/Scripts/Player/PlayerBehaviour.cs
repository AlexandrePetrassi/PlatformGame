//######################################################################################################################
// PlayerBehavior
// * Armazena a referencia para todos os modulos do player
//######################################################################################################################
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(HideSkillModule))] 
[RequireComponent (typeof(WallSkillModule))] 
[RequireComponent (typeof(DashSkillModule))] 
[RequireComponent (typeof(JumpSkillModule))] 
[RequireComponent (typeof(MoveSkillModule))] 
[RequireComponent (typeof(AttackSkillModule))] 
[RequireComponent (typeof(DamageModule))] 
[RequireComponent (typeof(SoundModule))] 
[RequireComponent (typeof(Rigidbody2D))] 
[RequireComponent (typeof(Animator))] 
[RequireComponent (typeof(SpriteRenderer))] 
[RequireComponent (typeof(Player))] 

public abstract class PlayerBehaviour : MonoBehaviour{

	private Player myPlayer;
	private Animator myAnimator;
	private Rigidbody2D myRigidbody;
	private SpriteRenderer mySpriteRenderer;
	private MoveSkillModule myMove;
	private HideSkillModule myHide;
	private WallSkillModule myWall;
	private DashSkillModule myDash;
	private JumpSkillModule myJump;
	private AttackSkillModule myAttack;
	private DamageModule myDamage;
	private SoundModule mySound;
	private Camera myCam;
	
	public Player player{
		get{
			if(myPlayer == null){
				myPlayer = GetComponent<Player>();
			}
			return myPlayer;
		}
		set{
			myPlayer = value;
		}
	}
	
	public Animator anim{
		get{
			if(myAnimator == null){
				myAnimator = GetComponent<Animator>();
			}
			return myAnimator;
		}
		set{
			myAnimator = value;
		}
	}
	
	public Rigidbody2D rb{
		get{
			if(myRigidbody == null){
				myRigidbody = GetComponent<Rigidbody2D>();
			}
			return myRigidbody;
		}
		set{
			myRigidbody = value;
		}
	}

	public SpriteRenderer sr{
		get{
			if(mySpriteRenderer == null){
				mySpriteRenderer = GetComponent<SpriteRenderer>();
			}
			return mySpriteRenderer;
		}
		set{
			mySpriteRenderer = value;
		}
	}

	public MoveSkillModule move{
		get{
			if(myMove == null){
				myMove = GetComponent<MoveSkillModule>();
			}
			return myMove;
		}
		set{
			myMove = value;
		}
	}

	public HideSkillModule hide{
		get{
			if(myHide == null){
				myHide = GetComponent<HideSkillModule>();
			}
			return myHide;
		}
		set{
			myHide = value;
		}
	}

	public WallSkillModule wall{
		get{
			if(myWall == null){
				myWall = GetComponent<WallSkillModule>();
			}
			return myWall;
		}
		set{
			myWall = value;
		}
	}

	public DashSkillModule dash{
		get{
			if(myDash == null){
				myDash = GetComponent<DashSkillModule>();
			}
			return myDash;
		}
		set{
			myDash = value;
		}
	}

	public JumpSkillModule jump{
		get{
			if(myJump == null){
				myJump = GetComponent<JumpSkillModule>();
			}
			return myJump;
		}
		set{
			myJump = value;
		}
	}

	public DamageModule dmg{
		get{
			if(myDamage == null){
				myDamage = GetComponent<DamageModule>();
			}
			return myDamage;
		}
		set{
			myDamage = value;
		}
	}

	public SoundModule sound{
		get{
			if(mySound == null){
				mySound = GetComponent<SoundModule>();
			}
			return mySound;
		}
		set{
			mySound = value;
		}
	}

	public AttackSkillModule atk{
		get{
			if(myAttack == null){
				myAttack = GetComponent<AttackSkillModule>();
			}
			return myAttack;
		}
		set{
			myAttack = value;
		}
	}
}
