//---------------------------------------------------------
// Script para manejar la evolución y recolección de la planta.
// Responsable: Natalia Nita, Julia Vera y Alexia Pérez Santana
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
    [SerializeField] private Sprite PlantaFase4;
    [SerializeField] private GameObject PrefabSuelo;
    [SerializeField] private GameObject PrefabRiego;
    [SerializeField] private GameObject PrefabRecolecta;
    [SerializeField] private GameObject PrefabMaceta;
    [SerializeField] private string NombrePlanta = "Cultivo";

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    private float _tiempoRegado;
    private float _tiempoCrecimiento;
    private SpriteRenderer _spriteRenderer;
    private int faseActual = 0;
    private bool _riego = false;
    private bool _listaParaCosechar = false;
    private GameObject _avisos;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = PlantaFase1;  // Inicia en fase 1
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
            Invoke("EvolucionarPlanta", _tiempoCrecimiento); // Si es la primera vez que se riega activa su crecimiento
        }
    }

    public void ActivaRiego()
    {
        if (!_listaParaCosechar) {
            _avisos = Instantiate(PrefabRiego, transform.position, Quaternion.identity);
            _avisos.transform.SetParent(transform);
            _riego = false; }
    }

    public void ActivaRecolecta()
    {
        Destroy(_avisos);
        _avisos = Instantiate(PrefabRecolecta, transform.position, Quaternion.identity);
        _avisos.transform.SetParent(transform);
        _riego = true;
        _listaParaCosechar = true;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Vuelve a activar la posibilidad de riego
    /// </summary>


    /// <summary>
    /// Cambia la fase de la planta según su crecimiento.
    /// </summary>
    private void EvolucionarPlanta()
    {
        if (faseActual == 0)
        {
            _spriteRenderer.sprite = PlantaFase2;
            faseActual = 1;
            Invoke("EvolucionarPlanta", _tiempoCrecimiento);
        }
        else if (faseActual == 1)
        {
            _spriteRenderer.sprite = PlantaFase3;
            faseActual = 2;
            Invoke("EvolucionarPlanta", _tiempoCrecimiento);
        }
        else if (faseActual == 2)
        {
            _spriteRenderer.sprite = PlantaFase4;
            faseActual = 3;
            ActivaRecolecta();
        }
    }

    /// <summary>
    /// Método para cosechar la planta y agregarla al inventario.
    /// </summary>
    private void Cosechar()
    {
        Debug.Log("Cosechando planta...");
        LevelManager.Instance.AgregarAlInventario(NombrePlanta);
        GameObject Maceta = Instantiate(PrefabMaceta, transform.position, Quaternion.identity);
        Maceta.transform.SetParent(transform);
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
            Destroy(_avisos);
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
