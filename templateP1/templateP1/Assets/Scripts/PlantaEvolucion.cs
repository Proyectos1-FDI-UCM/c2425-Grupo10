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
    [SerializeField] private GameObject PrefabSuelo; // Prefab efecto tierra mojada
    [SerializeField] private GameObject PrefabAvisoRiego; // Prefab regadera (se activa cuando es necesario regar la planta)
    [SerializeField] private GameObject PrefabAvisoRecolecta; // Prefab hoz (se activa cuando es necesario recolectar la planta)
    [SerializeField] private GameObject PrefabMaceta;
    [SerializeField] private string NombrePlanta = "Cultivo";

    // Pruebas
    [SerializeField] private int TiempoCrecimiento = 10;
    [SerializeField] private int TiempoRegado = 20;
    [SerializeField] private int TiempoMuerte = 40;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    private SpriteRenderer _spriteRenderer;
    private GameObject _avisos;
    private GameObject _maceta;
    private CropSpawner _cropSpawner;

    private int EstadoCrecimiento;
    private bool EstadoRiego;
    private float TimerCrecimiento;
    private float TimerRiego;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        //_plantas = transform.parent;
        
        // Se cambia en plantar - para poder implementar un nuevo metodo de inicializacion si es necesario

        //_spriteRenderer.sprite = PlantaFase1;  // Inicia en fase 1
        //EstadoCrecimiento = 0;
        
        EstadoRiego = false;
        TimerCrecimiento = TiempoCrecimiento;
        
    }

    private void Update()
    {
        TimerRiego -= Time.deltaTime;
        // Debug.Log("Timer Riego: " + TimerRiego);
        
        if (TimerRiego <= 0f) AvisoRiego();
        
        if (EstadoCrecimiento == 0 && EstadoRiego)
        {
            TimerCrecimiento -= Time.deltaTime;
            // Debug.Log("Timer Crecimiento: " + TimerCrecimiento);
        }
        else if (EstadoCrecimiento > 0)
        {
            TimerCrecimiento -= Time.deltaTime;
            // Debug.Log("Timer Crecimiento: " + TimerCrecimiento);
        }
        if (TimerCrecimiento <= 0f)
        {
            Crecimiento();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos Públicos

    /// <summary>
    /// Se activa cuando la planta inicia desde una semilla, establece el estado de crecimiento y de riego y el tiempo de crecimiento.
    /// </summary>
    public void Planta(GameObject maceta)
    {
        _maceta = maceta;

        _cropSpawner = _maceta.GetComponent<CropSpawner>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = PlantaFase1;  // Inicia en fase 1

        EstadoCrecimiento = 0;
        EstadoRiego = false;
        TimerCrecimiento = TiempoCrecimiento;

    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Regar la planta
    /// </summary>
    private void Regar()
    {
        Destroy(_avisos); // Se eliminan los avisos de riego
        LevelManager.Instance.Regar(); // Se avisa al level manager para que contabilice el agua de la regadera

        // Se moja la tierra
        GameObject suelo = Instantiate(PrefabSuelo, transform.position, Quaternion.identity); 
        suelo.transform.SetParent(transform);

        // Inicia la cuenta atrás del tiempo de regado
        TimerRiego = TiempoRegado;
        EstadoRiego = true;

    }

    /// <summary>
    /// Cambia el estado de la planta según su crecimiento.
    /// </summary>
    private void Crecimiento()
    {
        if (EstadoCrecimiento == 0)
        {
            _spriteRenderer.sprite = PlantaFase2;
            EstadoCrecimiento = 1;
            TimerCrecimiento = TiempoCrecimiento;
        }
        else if (EstadoCrecimiento == 1)
        {
            _spriteRenderer.sprite = PlantaFase3;
            EstadoCrecimiento = 2;
            TimerCrecimiento = TiempoCrecimiento;
        }
        else if (EstadoCrecimiento == 2)
        {
            _spriteRenderer.sprite = PlantaFase4;
            EstadoCrecimiento = 3;
            TimerCrecimiento = TiempoCrecimiento;
            AvisoRecolecta();
        }
    }

    /// <summary>
    /// Cosecharla planta.
    /// </summary>
    private void Cosechar()
    {
        Destroy(_avisos);
        LevelManager.Instance.AgregarAlInventario(NombrePlanta);

        _cropSpawner.Reactivar();

        Destroy(gameObject); // Elimina la planta del mapa tras recogerla
    }

    // AVISOS ------------------
    /// <summary>
    /// Métodos que instancian los prefabs de avisos para el riego y la recolecta
    /// </summary>
    private void AvisoRiego()
    {
        Destroy(_avisos);

        if (EstadoCrecimiento != 3)
        {
            _avisos = Instantiate(PrefabAvisoRiego, transform.position, Quaternion.identity);
            _avisos.transform.SetParent(transform);

            EstadoRiego = false;
        }
    }

    private void AvisoRecolecta()
    {
        if (_avisos != null) { Destroy(_avisos.gameObject); }
        _avisos = Instantiate(PrefabAvisoRecolecta, transform.position, Quaternion.identity);
        _avisos.transform.SetParent(transform);
        EstadoRiego = true;
    }

    // AVISOS ------------------

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
        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 2 && Regadera > 0 && !EstadoRiego)
        {
            Regar();
        }

        // Si la planta está lista para cosechar y el jugador tiene guantes (Herramienta 1), la recoge
        if (InputManager.Instance.UsarIsPressed() && LevelManager.Instance.Herramientas() == 3 && EstadoCrecimiento == 3)
        {
            Cosechar();
        }
    }

    #endregion
}
