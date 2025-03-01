//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints


    // Herramientas y semillas del jugador (serialized pq aun no se pueden cambiar desde el juego)
    [SerializeField]
    int Herramienta; // Herramientas - Guantes = 1, Semillas = 5

    [SerializeField]
    int Semillas = 1; // Semillas -
                      
    [SerializeField]
    int Regadera = 5; // Regadera (lleno) - 

    // Prefab 
    [SerializeField]
    GameObject PrefabSemilla1;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
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
    /// Método para comprobar que herramienta tiene el jugador
    /// </summary>

    public int Herramientas()
    { 
        return Herramienta;
    }

    /// <summary>
    /// Método para cambiar la herramienta del jugador
    /// </summary>

    public void CambioHerramienta(int i)
    {
        Herramienta = i;
    }


    /// <summary>
    /// Método que planta una semilla en una casilla con posición = position
    /// Más adelante plantará en función de la semilla seleccionada
    /// </summary>

    public bool Plantar()
    {
        // Se activa la animación de plantar
        return (Semillas != 0);
        
    }

    /// <summary>
    /// Método que riega una planta
    /// Más adelante plantará en función de la semilla seleccionada
    /// </summary>
    public bool Regar()
    {
        // Se comprueba la herramienta en la colisión (CropSeed - Script)
        // Se comprueba si la regadera tiene agua
        // Se activa la animación de regar (dependiendo del agua en la regadera)
        
        if (Regadera != 0)
        {
            Regadera--;
        }
        return (Regadera != 0);
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>

    #endregion

    public static bool HasInstance()
    {
        return _instance != null;
    }

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    #endregion
} // class LevelManager 
// namespace