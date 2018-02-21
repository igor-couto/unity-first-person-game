using System.Collections;
using UnityEngine;

public class ArmaAtual : MonoBehaviour {

	public GameObject granada;

    Transform armaPrimaria;
    Transform armaSecundaria;

    PropriedadesDaArma armaSelecionada;

	float zoomOriginal;
	bool estaDandoZoom = false;
	bool atirando = false;

	Camera cameraFPS;

	Animator animator;

	#region Metodos Unity

    void Start ()
    {
		animator = GetComponent<Animator> ();
		cameraFPS = Camera.main;
		zoomOriginal = cameraFPS.fieldOfView;
        armaPrimaria = transform.GetChild(0);
        armaSecundaria = transform.GetChild(1);
        SelecionaArma(1);
    }
	
	void Update ()
    {

		// Se o botão esquerdo do mouse for apertado:
		if (Input.GetMouseButtonDown(0) && armaSelecionada.balasNoPente > 0)
		{
			Dispara();
		}

		if(Input.GetMouseButtonUp(0)){
			atirando = false;
			animator.SetBool ("atira", atirando);
		}

		Mira();

        if (Input.GetKeyDown("r"))
        {
            Reload();
        }

		if (Input.GetKeyDown("g"))
		{
			AtiraGranada();
		}

        if ( Input.GetKeyDown("1") )
        {
            SelecionaArma(1);
        }
        else if ( Input.GetKeyDown("2") )
        {
            SelecionaArma(2);
        }

        // Se esta rodando a rodinha do mouse para cima:
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            SelecionaArma(2);
        }
        // Se esta rodando a rodinha do mouse para baixo:
        else if ( Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            SelecionaArma(1);
        }

		if(Input.GetKeyDown("e")){
			Interage ();
		}
    }

	#endregion

	void Interage(){
		
		RaycastHit objetoAcertado;
		float alcance = 2f;

		// Se acertou em alguma coisa
		if (Physics.Raycast (cameraFPS.transform.position, cameraFPS.transform.forward, out objetoAcertado, alcance)) {
			IInteragivel acao = objetoAcertado.transform.gameObject.GetComponent<IInteragivel> ();
			if(acao != null){
				acao.Interacao ();
			}
		}
	}

    void SelecionaArma( int numeroDaArma )
    {
        if (numeroDaArma == 1)
        {
            armaSelecionada = armaPrimaria.GetChild(0).GetComponent<PropriedadesDaArma>();
            armaPrimaria.gameObject.SetActive(true);
            armaSecundaria.gameObject.SetActive(false);
        }
        else if (numeroDaArma == 2)
        {
            armaSelecionada = armaSecundaria.GetChild(0).GetComponent<PropriedadesDaArma>();
            armaPrimaria.gameObject.SetActive(false);
            armaSecundaria.gameObject.SetActive(true);
        }
		AtualizaUIArma ();
    }

    public void Dispara()
    {
		armaSelecionada.balasNoPente--;
		AtualizaUIArma ();

		atirando = true;
		animator.SetBool ("atira", atirando);

        // Aciona a particula de tiro, na ponta do cano da arma
		//GameObject g = new GameObject("muzzleflash");
        ParticleSystem particula = Instantiate(armaSelecionada.particulaDisparo, armaSelecionada.pontoDisparo.position, Quaternion.identity);

		//particula.transform.parent = g.transform;
		//g.transform.parent = transform;
        Destroy(particula.gameObject, 0.1f);

        // Raycast para ver o que acertou
        RaycastHit objetoAcertado;
        float alcance = armaSelecionada.alcance;

        // Se acertou em alguma coisa:
		if( Physics.Raycast(cameraFPS.transform.position, cameraFPS.transform.forward, out objetoAcertado, alcance) )
		{
			// Coloca um buraco de bala
			GameObject buracoBala = new GameObject ("BulletSpot");
			buracoBala.transform.position = objetoAcertado.point;
			Instantiate(Resources.Load("Buraco Bala"), objetoAcertado.point + objetoAcertado.normal * 0.01f, Quaternion.FromToRotation(Vector3.up, objetoAcertado.normal), buracoBala.transform);
			buracoBala.transform.parent = objetoAcertado.transform;
            
            //GameObject buracoBala = (GameObject)Instantiate(Resources.Load("Buraco Bala"), objetoAcertado.point + objetoAcertado.normal * 0.01f, Quaternion.FromToRotation(Vector3.up, objetoAcertado.normal), objetoAcertado.transform);
			//buracoBala.transform.localScale = new Vector3 (0,0,0);


			// Coloca o efeito de acerto, faisca e fumaça
            ParticleSystem efeitoAcerto = Instantiate(armaSelecionada.particulaPontoAcerto, objetoAcertado.point, Quaternion.identity, objetoAcertado.transform);
            // Destruir os efeitos de particula depois, para eles não ficarem toda a vida na memória
            Destroy(efeitoAcerto.gameObject, 10f);
            Destroy(buracoBala, 15f);

            // SE REAGE A FISICA, ADICIONA IMPACTO
            Rigidbody massaAtingida = objetoAcertado.transform.gameObject.GetComponent<Rigidbody>();
            if (massaAtingida)
            {
                objetoAcertado.transform.gameObject.GetComponent<Rigidbody>().AddForce(-objetoAcertado.normal * 100);
            }

            // SE FOR INIMIGO, TIRA DANO
			if(objetoAcertado.transform.gameObject.tag == "Inimigo" ){
				//objetoAcertado.transform.gameObject.GetComponent<Inimigo> ();
			}
        }

		if (armaSelecionada.balasNoPente == 0) Reload ();	
		
    }

    void Reload()
    {
        // Se a arma esta com pelo menos uma bala disparada no pente E existem balas na arma
        if (armaSelecionada.balasNoPente < armaSelecionada.capacidadeDoPente && armaSelecionada.balasNaArma > 0)
        {
			animator.SetBool ("recarregando", true);
            int balasNecessarias = armaSelecionada.capacidadeDoPente - armaSelecionada.balasNoPente;

            if (armaSelecionada.balasNaArma >= balasNecessarias)
            {
                armaSelecionada.balasNaArma -= balasNecessarias;
                armaSelecionada.balasNoPente += balasNecessarias;
            }
            else
            {
                armaSelecionada.balasNoPente += armaSelecionada.balasNaArma;
                armaSelecionada.balasNaArma = 0;
            }
			AtualizaUIArma ();
        }
        else
        {
            // Barulho de arma vazia (click)
        }
    }
		
	void Mira()
	{
		if ( Input.GetMouseButtonDown(1) )
		{
			estaDandoZoom = true;
			animator.SetBool ("mirando", estaDandoZoom);
			Camera.main.fieldOfView = 30f;
			//StartCoroutine(DarZoom(30.0f));
		}
		else if (Input.GetMouseButtonUp(1) )
		{
			estaDandoZoom = false;
			animator.SetBool ("mirando", estaDandoZoom);
			Camera.main.fieldOfView = zoomOriginal;
			//StartCoroutine("TirarZoom");
		}
	}

	IEnumerator DarZoom ( float quantidadeDeZoom )
	{
		float zoom = zoomOriginal;
		while (zoom >= quantidadeDeZoom)
		{
			zoom = zoom -1;
			Camera.main.fieldOfView = zoom;
			yield return null;
		}
	}

	IEnumerator TirarZoom()
	{
		float zoom = Camera.main.fieldOfView;
		while (zoom <= zoomOriginal)
		{
			zoom = zoom +1;
			Camera.main.fieldOfView = zoom;
			yield return null;
		}
	}

	void AtualizaUIArma(){
		GerenciadorDeJogo.instance.SetMunicao (armaSelecionada.balasNoPente, armaSelecionada.balasNaArma);
	}

	void AtiraGranada(){
		GameObject granadaArremessada = (GameObject)Instantiate (granada, cameraFPS.transform.position + cameraFPS.transform.forward, Quaternion.identity);
		granadaArremessada.GetComponent<Rigidbody> ().AddForce(cameraFPS.transform.forward * 500);
	}
}