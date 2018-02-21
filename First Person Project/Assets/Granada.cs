using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granada : MonoBehaviour {

	public float raio;
	public float forca;
	public float tempoAteExplosao;
	public GameObject efeitoExplosao;

	float cronometro;
	bool explodiu = false;

	void Update () {
		cronometro += Time.deltaTime;
		if(cronometro >= tempoAteExplosao && !explodiu){
			explodiu = true;
			Instantiate (efeitoExplosao,transform.position, Quaternion.identity);
			gameObject.GetComponent<Renderer> ().enabled = false;
			Collider[] objetosAfetados = Physics.OverlapSphere (transform.position, raio);
			foreach(Collider objeto in objetosAfetados){
				Rigidbody corpo = objeto.GetComponent<Rigidbody> ();
				if(corpo){
					corpo.AddExplosionForce (forca, transform.position, raio);
				}
				if(objeto.gameObject.tag == "Inimigo"){
					//TIRA DANO objeto.gameObject.GetComponent<Inimigo>().Dano(forca);
				}
			}
			gameObject.GetComponent<Collider> ().enabled = false;
			Destroy (gameObject, 10f);
		}
	}
}
