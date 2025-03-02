//---------------------------------------------------------
// Script para manejar la evolución y recolección de la planta.
// Responsable: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Maneja la evolución de la planta.
/// Permite su recolección cuando alcanza su fase máxima.
/// </summary>
public class PlantaEvolucion : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    [SerializeField] private Sprite PlantaFase1;
    [SerializeField] private Sprite PlantaFase2;
    [SerializeField] private Sprite PlantaFase3;
    [SerializeField] private GameObject PrefabSuelo;
    [SerializeField] private string nombrePlanta = "Cultivo";

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    private float _tiempoRegado;
    private float _tiempoCrecimiento;
    private SpriteRenderer spriteRenderer;
    private int faseActual = 0;
    private bool _riego = false;
    private bool _listaParaCosechar = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = PlantaFase1;  // Inicia en fase 1
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Inicializa la evolución de la planta con tiempos específicos.
    /// </summary>
    public void Planta(float TiempoCrecimiento, float TiempoRegado)
    {
        _tiempoCrecimiento = TiempoCrecimiento;
        _tiempoRegado = TiempoRegado;
    }

    /// <summary>
    /// Regar la planta para activar su evolución.
    /// </summary>
    public void Regar()
    {
        Debug.Log("Regando...");
        GameObject suelo = Instantiate(PrefabSuelo, transform.position, Quaternion.identity);
        suelo.transform.SetParent(transform);
        Invoke("ActivaRiego", _tiempoRegado);

        if (faseActual == 0)
        {
            Invoke("EvolucionarPlanta", _tiempoCrecimiento);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void ActivaRiego()
    {
        _riego = false;
    }

    /// <summary>
    /// Cambia la fase de la planta según su crecimiento.
    /// </summary>
    private void EvolucionarPlanta()
    {
        if (faseActual == 0)
        {
            spriteRenderer.sprite = PlantaFase2;
            faseActual = 1;
            Invoke("EvolucionarPlanta", _tiempoCrecimiento);
        }
        else if (faseActual == 1)
        {
            spriteRenderer.sprite = PlantaFase3;
            faseActual = 2;
            _listaParaCosechar = true;  // Marca la planta como lista para cosechar
        }
    }

    /// <summary>
    /// Método para cosechar la planta y agregarla al inventario.
    /// </summary>
    private void Cosechar()
    {
        Debug.Log("Cosechando planta...");
        LevelManager.Instance.AgregarAlInventario(nombrePlanta);
        Destroy(gameObject); // Elimina la planta del mapa tras recogerla
    }

    #endregion

    // ---- EVENTOS ----
    #region Eventos

    /// <summary>
    /// Detecta si el jugador interactúa con la planta.
    /// </summary>
    private void OnCollisionStay2D()
    {
        Debug.Log("Colisión con planta detectada.");

        // Si el jugador tiene la regadera y presiona el botón, riega la planta
        int Regadera = LevelManager.Instance.Regadera();
        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 2 && Regadera > 0 && !_riego)
        {
            _riego = true;
            LevelManager.Instance.Regar();
            Regar();
        }

        // Si la planta está lista para cosechar y el jugador tiene guantes (Herramienta 1), la recoge
        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 3 && _listaParaCosechar)
        {
            Cosechar();
        }
    }

    #endregion
}
