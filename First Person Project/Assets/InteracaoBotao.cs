using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracaoBotao : MonoBehaviour, IInteragivel {

	Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
	}

	public void Interacao(){
		if(anim.GetBool("Push") == false){
			anim.SetBool ("Push", true);
		}

	}
}
