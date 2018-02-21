using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olhar : MonoBehaviour {

    public float sensibilidadeDoMouse;
    public float limiteCabeca;

    float zoomOriginal;
    bool estaDandoZoom = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        zoomOriginal = Camera.main.fieldOfView;
    }

	void Update () {
        Rotacao();
        Disparo();
        Mira();
	}

    void Rotacao()
    {
        /*
        // Para os lados
        float rotacaoLados = Input.GetAxisRaw("Mouse X") * sensibilidadeDoMouse;
        transform.Rotate(0, rotacaoLados, 0);

        // Para cima e baixo
        rotacaoCimaBaixo = -Input.GetAxisRaw("Mouse Y") * sensibilidadeDoMouse;

        rotacaoCimaBaixo = Mathf.Clamp(rotacaoCimaBaixo, -limiteCabeca, limiteCabeca);
        Camera.main.transform.localRotation = Quaternion.Euler(rotacaoCimaBaixo, 0, 0);
        */

        float rotacaoLados      = Input.GetAxisRaw("Mouse X") * sensibilidadeDoMouse;
        float rotacaoCimaBaixo  = -Input.GetAxisRaw("Mouse Y") * sensibilidadeDoMouse;

        rotacaoCimaBaixo = Mathf.Clamp(rotacaoCimaBaixo, -limiteCabeca, limiteCabeca);

        Vector3 rotacaoDesejada = transform.rotation.eulerAngles;
        rotacaoDesejada.x = rotacaoDesejada.x + rotacaoCimaBaixo;
        rotacaoDesejada.y = rotacaoDesejada.y + rotacaoLados;

        transform.rotation = Quaternion.Euler(rotacaoDesejada);
    }

    void Disparo()
    {
        // Se o botão esquerdo do mouse for apertado:
        if (Input.GetMouseButtonDown(0))
        {
            // Chama o metodo de disparo da arma que esta sendo utilizada atualmente
            transform.GetChild(0).GetComponent<ArmaAtual>().Dispara();
        }
    }

    void Mira()
    {
        if ( Input.GetMouseButtonDown(1) )
        {
            StartCoroutine(DarZoom(30f));
        }
        else if (Input.GetMouseButtonUp(1) )
        {
            StartCoroutine("TirarZoom");
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
}