using UnityEngine;

public class Configuracoes : MonoBehaviour {

    [Header("Parametros Gerais")]

    [Tooltip("Gravidade aplicada somente ao jogador no momento do salto")]
    public float gravidadeDoJogador;
    [Tooltip("Quantidade de zoom aplicado quando o jogador utiliza a mira")]
    public float zoomNaMira;
    [Tooltip("Tempo até as particulas instanciadas na cena se destruirem")]
    public float duracaoParticulas;
    [Tooltip("Tempo em que os buracos de bala continuam visiveis")]
    public float duracaoBuracosBala;

    public static Configuracoes instancia;

	void Awake ()
    {
        if (Configuracoes.instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
