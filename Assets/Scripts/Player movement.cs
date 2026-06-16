using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Playermovement : MonoBehaviour
{
    [Header("Movimiento Lateral (Carriles)")]
    [SerializeField] private GameObject[] carrilCentro;
    public float velocidadMovimiento = 5f;
    public float velocidadCambioCarril = 10f; 
    private int carrilActual = 1;
    private float objetivoX;

    [Header("Físicas y Salto")]
    [SerializeField] private float fuerzaSalto = 8f;
    private Rigidbody rb;
    private bool quiereSaltar = false; 
    private bool estaEnElSuelo = true; 

    [Header("Componentes y Animación")]
    [SerializeField] private Animator animator;

    [Header("Interfaz y Estado del Juego")]
    [SerializeField] private GameObject menu;
    [SerializeField] private TMP_Text score;
    private float roundScore;
    private bool isGameOver;
    private bool isShield;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip shieldSound;
    [SerializeField] private AudioClip crashSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem coinParticle;
    [SerializeField] private ParticleSystem shieldParticle;
    [SerializeField] private ParticleSystem crashParticle;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        
        // Verificación de seguridad para los carriles
        if (carrilCentro != null && carrilCentro.Length > carrilActual)
        {
            transform.position = carrilCentro[carrilActual].transform.position;
            objetivoX = transform.position.x;
        }
    }

    void Update()
    {
        velocidadMovimiento = 5 + roundScore * 0.1f;
        
        if (!isGameOver)
        {
            // Contador de puntuación
            roundScore += Time.deltaTime;
            if (score != null)
            {
                score.text = "Score: " + roundScore.ToString("f1");
            }

            // 1. Inputs de movimiento lateral
            if (Input.GetKeyDown(KeyCode.A) && carrilActual > 0)
            {
                carrilActual--;
                objetivoX = carrilCentro[carrilActual].transform.position.x;
            }
            else if (Input.GetKeyDown(KeyCode.D) && carrilActual < carrilCentro.Length - 1) 
            {
                carrilActual++;
                objetivoX = carrilCentro[carrilActual].transform.position.x;
            }

            // 2. Input de salto
            if (Input.GetKeyDown(KeyCode.Space) && estaEnElSuelo)
            {
                quiereSaltar = true;
                audioSource.PlayOneShot(jumpSound);
            }

            // 3. Control de animación de salto
            if (animator != null)
            {
                animator.SetBool("Jump", !estaEnElSuelo);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isGameOver)
        {
            // Movimiento constante hacia adelante
            Vector3 avanceAdelante = transform.forward * velocidadMovimiento * Time.fixedDeltaTime;

            // Movimiento suave lateral usando Lerp
            float nuevoX = Mathf.Lerp(rb.position.x, objetivoX, velocidadCambioCarril * Time.fixedDeltaTime);
            Vector3 posicionObjetivo = new Vector3(nuevoX, rb.position.y, rb.position.z) + avanceAdelante;

            rb.MovePosition(posicionObjetivo);

            // Aplicar física del salto
            if (quiereSaltar)
            {
                rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
                estaEnElSuelo = false; 
                quiereSaltar = false;  
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detección de aterrizaje en el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnElSuelo = true;
        }

        // Detección de colisión con obstáculos
        if (collision.gameObject.CompareTag("Obstacle") && !isShield)
        {
            isGameOver = true;
            audioSource.PlayOneShot(crashSound);
            
            // CORREGIDO: Se almacena como ParticleSystem y se destruye su .gameObject
            ParticleSystem vfx = Instantiate(crashParticle, transform.position, transform.rotation);
            Destroy(vfx.gameObject, 2f);
            
            if (menu != null)
            {
                menu.SetActive(true);
            }

            if (animator != null)
            {
                animator.SetBool("Dead", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detección de Monedas
        if (other.gameObject.CompareTag("Money"))
        {
            audioSource.PlayOneShot(coinSound);
            
            // CORREGIDO: Se almacena como ParticleSystem y se destruye su .gameObject
            ParticleSystem vfx = Instantiate(coinParticle, other.transform.position, transform.rotation);
            Destroy(vfx.gameObject, 2f);

            roundScore += 5;
            Destroy(other.gameObject);
            
            if (score != null)
            {
                score.text = "Score: " + roundScore.ToString("f1");
            }
        }

        // Detección de Escudo
        if (other.gameObject.CompareTag("Shield"))
        {
            audioSource.PlayOneShot(shieldSound);
            
            // CORREGIDO: Se almacena como ParticleSystem y se destruye su .gameObject
            ParticleSystem vfx = Instantiate(shieldParticle, other.transform.position, transform.rotation);
            Destroy(vfx.gameObject, 2f);
            
            Destroy(other.gameObject);
            isShield = true;
            Invoke("DesactivateShield", 5f);
        }
    }

    private void DesactivateShield()
    {
        isShield = false;
    }
}