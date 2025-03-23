//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín, Alexia Pérez Santana
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections.Generic; // Necesario para manejar el inventario

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
/// 
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas).
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Tool actual en uso (1 = Guantes, 5 = Seeds).
    /// </summary>
    [SerializeField] int Tool;

    /// <summary>
    /// Cantidad de semillas disponibles.
    /// </summary>
    [SerializeField] int SeedCount = 100;

    /// <summary>
    /// Prefab de la primera semilla.
    /// </summary>
    [SerializeField] GameObject SeedPrefab1;

    /// <summary>
    /// Referencia al GameManager para gestionar la información del juego.
    /// </summary>
    [SerializeField] GameManager GameManager;

    #endregion
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    /// <summary>
    /// Inventory de cultivos recolectados.
    /// </summary>
    private int[] _inventory;

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Se llama al iniciar el script. Configura la instancia singleton
    /// y busca el GameManager en la escena.
    /// </summary>
    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        if (GameManager == null)
        {
            GameObject _textObject = GameObject.FindGameObjectWithTag("GameManager");
            if (_textObject != null)
            {
                GameManager = _textObject.GetComponent<GameManager>();
            }
        }

        _inventory = GameManager.Instance.Inventory(); // Inicializa el inventario desde el GameManager

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Propiedad para acceder a la instancia del LevelManager.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve el valor de la herramienta actual.
    /// </summary>
    public int Tools() { return Tool; }

    /// <summary>
    /// Devuelve la cantidad de semillas disponibles.
    /// </summary>
    public int Seeds() { return SeedCount; }

    /// <summary>
    /// Cambia la herramienta actual a la indicada.
    /// </summary>
    /// <param name="i">Nuevo valor de la herramienta.</param>
    public void ChangeTool(int i) { Tool = i; }

    /// <summary>
    /// Disminuye la cantidad de semillas al plantar.
    /// </summary>
    public void Plant() { SeedCount--; }

    /// <summary>
    /// Verifica si existe una instancia del LevelManager.
    /// </summary>
    public static bool HasInstance() { return _instance != null; }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion
}
