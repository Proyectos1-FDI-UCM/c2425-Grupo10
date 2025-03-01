//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Script que maneja la evolución de la planta.
/// Cambia de fase de la planta después de un tiempo.
/// </summary>
public class PlantaEvolucion : MonoBehaviour
{
    [SerializeField] private Sprite PlantaFase1;
    [SerializeField] private Sprite PlantaFase2;
    [SerializeField] private Sprite PlantaFase3;

    private SpriteRenderer spriteRenderer;
    private int faseActual = 0;

    private bool _riego = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Comienza con la fase 1
        spriteRenderer.sprite = PlantaFase1;
    }

    /// <summary>
    /// Método que inicia la evolución de la planta.
    /// Cambia el sprite de la planta después de 3 segundos en cada fase.
    /// </summary>
    public void Planta()
    {
        // Llama a la función de evolución con un retardo
        // Invoke("EvolucionarPlanta", 3f);  // Primer cambio a los 3 segundos
        
        
    }

    private void EvolucionarPlanta()
    {
        if (faseActual == 0)
        {
            spriteRenderer.sprite = PlantaFase2;  // Cambia a la fase 2
            faseActual = 1;
            _riego = false;
            // Invoke("EvolucionarPlanta", 3f);  // Siguiente fase después de otros 3 segundos
        }
        else if (faseActual == 1)
        {
            spriteRenderer.sprite = PlantaFase3;  // Cambia a la fase 3
            faseActual = 2;
        }
    }

    private void OnCollisionStay2D()
    {
        Debug.Log("CollisionConPlanta");
        int Regadera = LevelManager.Instance.Regadera();
        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 2 && Regadera > 0 && _riego == false)
        {
            Invoke("EvolucionarPlanta", 3f);
            LevelManager.Instance.Regar();
            _riego = true;
        }

    }
}
