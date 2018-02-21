using UnityEngine;
using UnityEngine.UI;

public class GerenciadorDeJogo : MonoBehaviour {

	public static GerenciadorDeJogo instance;

	public Text municaoText;
	public Text saudeText;

	public void Init(){
		SetSaude (100);
	}

	public void SetMunicao(int municaoNoPente, int municaoNaArma){
		municaoText.text = "Ammo " + municaoNoPente.ToString () + " / " + municaoNaArma.ToString () ;
	}

	public void SetSaude(int saude){
		saudeText.text = "Health " + saude.ToString ();
	}

	void Awake () {
		if (instance == null) {
			instance = this;
			Init ();
		} else {
			Destroy (this);
		}
	}
		
}
