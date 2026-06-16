using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015 Jean Moreno
// Versión Ultra-Segura: Evita colisiones de destrucción al acumular muchas partículas en pantalla

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
    [Header("Configuración de Destrucción")]
    [Tooltip("Si está marcado, desactiva el objeto en lugar de destruirlo de la escena.")]
    public bool OnlyDeactivate;
    
    void OnEnable()
    {
        StartCoroutine(CheckIfAlive());
    }
    
    IEnumerator CheckIfAlive()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        
        if (ps != null)
        {
            // 1. Calcular de forma segura cuánto va a durar el efecto completo
            // main.duration es el tiempo base y main.startLifetime el extra que pueden vivir las partículas
            float tiempoDeVida = ps.main.duration + ps.main.startLifetime.constantMax;
            
            // 2. Esperar pacientemente a que termine el tiempo calculado + un margen de seguridad de 0.5s
            yield return new WaitForSeconds(tiempoDeVida + 0.5f);
        }
        else
        {
            // Si por algún motivo no hay partículas, salimos con una espera mínima estándar
            yield return new WaitForSeconds(1.0f);
        }
        
        // 3. Ejecutar la remoción del GameObject completo
        if (OnlyDeactivate)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}