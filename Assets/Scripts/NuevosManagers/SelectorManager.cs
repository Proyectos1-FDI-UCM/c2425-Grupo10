//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SelectorManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Referencia al GameObject que representa la herramienta "Semilla".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject SeedTool;

    /// <summary>
    /// Referencia al GameObject que representa el selector de "Semilla".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject SeedSelector;

    /// <summary>
    /// Referencia al GameObject que representa la herramienta "Pala".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject ShovelTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Pala".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject ShovelSelector;

    /// <summary>
    /// Referencia al GameObject que representa la herramienta "Guantes".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject GlovesTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Guantes".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject GlovesSelector;

    /// <summary>
    /// Referencia al objeto que representa la herramienta "Regadera".
    /// </summary>
    [SerializeField] private GameObject WateringCanTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Regadera".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject WateringCanSelector;

    /// <summary>
    /// Referencia al objeto que representa la herramienta "Hoz".
    /// </summary>
    [SerializeField] private GameObject SickleTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Hoz".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject SickleSelector;

    /// <summary>
    /// Referencia al transform de la mano del jugador donde se mostrarán las herramientas.
    /// </summary>
    [SerializeField] private Transform HandPosition;

    /// <summary>
    /// Referencia al slider que representa la barra de Agua.
    /// </summary>
    [SerializeField] private Slider WaterBar;

    /// <summary>
    /// Referencia al LevelManager.
    /// </summary>
    [SerializeField] private LevelManager LevelManager;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia a la herramienta actualmente seleccionada.
    /// Solo una herramienta puede estar activa a la vez.
    /// </summary>
    private GameObject _currentTool;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Inicializa el estado de las herramientas al inicio del juego,
    /// asegurándose de que ninguna esté activada.
    /// </summary>
    void Start()
    {
        UpdateWaterBar(6, 6);

        DeselectCurrentTool();
        EnableSelector(GlovesSelector);
        ToggleTool(GlovesTool);
        DisableSelector(ShovelTool, SeedTool, WateringCanTool, SickleTool, ShovelSelector, SeedSelector, WateringCanSelector, SickleSelector);

        //WaterBar.SetActive(false);
    }

    /// <summary>
    /// Escucha la entrada del jugador en cada fotograma para cambiar de herramienta.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.Select5WasPressedThisFrame())
        {
            ToggleTool(SeedTool);
            EnableSelector(SeedSelector);
            DisableSelector(ShovelTool, GlovesTool, WateringCanTool, SickleTool, ShovelSelector, GlovesSelector, WateringCanSelector, SickleSelector);
            LevelManager.Instance.CambioHerramienta(5);
            //WaterBar.SetActive(false);
        }

        if (InputManager.Instance.Select4WasPressedThisFrame())
        {
            EnableSelector(ShovelSelector);
            ToggleTool(ShovelTool);
            DisableSelector(GlovesTool, SeedTool, WateringCanTool, SickleTool, GlovesSelector, SeedSelector, WateringCanSelector, SickleSelector);
            LevelManager.Instance.CambioHerramienta(4);
            //WaterBar.SetActive(false);
        }

        if (InputManager.Instance.Select1WasPressedThisFrame())
        {
            EnableSelector(GlovesSelector);
            ToggleTool(GlovesTool);
            DisableSelector(ShovelTool, SeedTool, WateringCanTool, SickleTool, ShovelSelector, SeedSelector, WateringCanSelector, SickleSelector);
            LevelManager.Instance.CambioHerramienta(1);
            //WaterBar.SetActive(false);
        }

        if (InputManager.Instance.Select2WasPressedThisFrame())
        {
            ToggleTool(WateringCanTool);
            EnableSelector(WateringCanSelector);
            DisableSelector(ShovelTool, SeedTool, GlovesTool, SickleTool, ShovelSelector, SeedSelector, GlovesSelector, SickleSelector);
            LevelManager.Instance.CambioHerramienta(2);
            //WaterBar.SetActive(true);
        }

        if (InputManager.Instance.Select3WasPressedThisFrame())
        {
            ToggleTool(SickleTool);
            EnableSelector(SickleSelector);
            DisableSelector(ShovelTool, SeedTool, WateringCanTool, GlovesTool, ShovelSelector, SeedSelector, WateringCanSelector, GlovesSelector);
            LevelManager.Instance.CambioHerramienta(3);
            //WaterBar.SetActive(false);
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public void UpdateWaterBar(float Water, float MaxWaterAmount)
    {
        WaterBar.value = Water / MaxWaterAmount;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Activa la herramienta seleccionada o la deselecciona si ya está activa.
    /// </summary>
    /// <param name="newTool">Herramienta a activar o desactivar</param>
    private void ToggleTool(GameObject newTool)
    {
        if (_currentTool != newTool)
        {
            SelectTool(newTool);
        }

    }


    /// <summary>
    /// Activa la herramienta seleccionada y desactiva la anterior si la hay.
    /// </summary>
    /// <param name="newTool">Herramienta a activar</param>
    private void SelectTool(GameObject newTool)
    {
        if (newTool == null) return;

        // Desactiva la herramienta actualmente seleccionada
        DeselectCurrentTool();

        // Activa la nueva herramienta y la establece como actual
        _currentTool = newTool;
        _currentTool.SetActive(true);

        // Poner la herramienta en la mano del jugador
        _currentTool.transform.SetParent(HandPosition);
        _currentTool.transform.localPosition = Vector3.zero; // Asegurar que esté en la posición exacta de la mano
        _currentTool.transform.localRotation = Quaternion.identity; // Resetear rotación si es necesario
    }

    /// <summary>
    /// Deselecciona la herramienta actualmente en uso, dejando las manos vacías.
    /// </summary>
    private void DeselectCurrentTool()
    {
        if (_currentTool != null)
        {
            _currentTool.SetActive(false);
            _currentTool.transform.SetParent(transform);
            _currentTool = null;
        }
    }

    private void EnableSelector(GameObject herramienta)
    {
        herramienta.SetActive(true);
    }

    private void DisableSelector(GameObject herramienta1, GameObject herramienta2, GameObject herramienta3, GameObject herramienta4, GameObject selector1, GameObject selector2, GameObject selector3, GameObject selector4)
    {
        herramienta1.SetActive(false);
        herramienta2.SetActive(false);
        herramienta3.SetActive(false);
        herramienta4.SetActive(false);
        selector1.SetActive(false);
        selector2.SetActive(false);
        selector3.SetActive(false);
        selector4.SetActive(false);

    }


    #endregion

} // class SelectorManager 
// namespace
