using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    public float velocidadeDeMovimento;
    public float velocidadeDoPulo;
    CharacterController controlador;

    void Start()
    {
        controlador = GetComponent < CharacterController > ();
    }

    void Update ()
    {
        Movimento();
        Correndo();
    }

    void Movimento()
    {
        float lados = Input.GetAxisRaw("Horizontal") * velocidadeDeMovimento * Time.deltaTime;
        float frente = Input.GetAxisRaw("Vertical") * velocidadeDeMovimento * Time.deltaTime;

        Vector3 movimento = new Vector3(lados, 0.0f, frente);

        movimento = Camera.main.transform.rotation * movimento;
        controlador.Move(movimento);

         /*
        // Se o jogador está no chão
        if (controlador.isGrounded)
        {
            // Se apertou o botão de pulo
            if (Input.GetKeyDown("space"))
            {
                movimento.y = velocidadeDoPulo;
            }
        }
        movimento.y -= 5.0f * Time.deltaTime;
        controlador.Move(movimento);
        */
    }

    void Correndo()
    {
        if ( Input.GetKeyDown(KeyCode.LeftShift) )
        {
            velocidadeDeMovimento = velocidadeDeMovimento * 4;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) )
        {
            velocidadeDeMovimento = velocidadeDeMovimento / 4;
        }
    }
}