using UnityEngine;

public class PropriedadesDaArma : MonoBehaviour {
    
    public int danoCausado;
    public int capacidadeDoPente;
    public int capacidadeTotal;
    public float alcance;
    public float taxaDeDisparo;
    public Transform pontoDisparo;
    public ParticleSystem particulaDisparo;
    public ParticleSystem particulaPontoAcerto;

    public int balasNoPente;
    public int balasNaArma;

    void Awake()
    {
        balasNoPente = capacidadeDoPente;
        balasNaArma = capacidadeTotal;
    }
}
