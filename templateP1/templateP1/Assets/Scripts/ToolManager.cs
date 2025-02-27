//---------------------------------------------------------
// Script para gestionar la selección y deselección de herramientas.
// Permite al jugador cambiar entre diferentes herramientas usando las teclas asignadas.
// Responsable: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase encargada de gestionar la selección de herramientas 
/// en el inventario del jugador. Al pulsar las teclas específicas, 
/// se activa una herramienta y se desactiva la anterior.
/// </summary>
public class ToolManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Referencia al GameObject que representa la herramienta "Semilla".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject SeedTool;

    /// <summary>
    /// Referencia al GameObject que representa el selector de Semillas.
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject SeedSelector;

    /// <summary>
    /// Referencia al GameObject que representa la herramienta "Pala".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject ShovelTool;

    /// <summary>
    /// Referencia al GameObject que representa el selector de Pala.
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject ShovelSelector;

    /// <summary>
    /// Referencia al GameObject que representa la herramienta "Guantes".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject GlovesTool;

    /// <summary>
    /// Referencia al GameObject que representa el selector de Guantes.
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject GlovesSelector;

    /// <summary>
    /// Referencia al objeto que representa la herramienta "Regadera".
    /// </summary>
    [SerializeField] private GameObject WateringCanTool;

    /// <summary>
    /// Referencia al GameObject que representa el selector de Regadera.
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject WateringCanSelector;

    /// <summary>
    /// Referencia al objeto que representa la herramienta "Azada".
    /// </summary>
    [SerializeField] private GameObject HoeTool;

    /// <summary>
    /// Referencia al GameObject que representa el selector de Hoz.
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject HoeSelector;

    /// <summary>
    /// Referencia al transform de la mano del jugador donde se mostrarán las herramientas.
    /// </summary>
    [SerializeField] private Transform HandPosition;

    /// <summary>
    /// Referencia al transform de la mano del jugador donde se guardan las herramientas.
    /// </summary>
    [SerializeField] private Transform ToolsPosition;

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
        DeselectCurrentTool();
        ShowSelector(GlovesSelector);
        DisableSelector(HoeSelector);
        DisableSelector(SeedSelector);
        DisableSelector(WateringCanSelector);
        DisableSelector(ShovelSelector);
    }

    /// <summary>
    /// Escucha la entrada del jugador en cada fotograma para cambiar de herramienta.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ToggleTool(SeedTool);
            ShowSelector(SeedSelector);
            DisableSelector(HoeSelector);
            DisableSelector(GlovesSelector);
            DisableSelector(WateringCanSelector);
            DisableSelector(ShovelSelector);
            LevelManager.Instance.CambioHerramientas(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleTool(ShovelTool);
            ShowSelector(ShovelSelector);
            DisableSelector(HoeSelector);
            DisableSelector(SeedSelector);
            DisableSelector(WateringCanSelector);
            DisableSelector(GlovesSelector);
            LevelManager.Instance.CambioHerramientas(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleTool(GlovesTool);
            ShowSelector(GlovesSelector);
            DisableSelector(HoeSelector);
            DisableSelector(SeedSelector);
            DisableSelector(WateringCanSelector);
            DisableSelector(ShovelSelector);
            LevelManager.Instance.CambioHerramientas(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleTool(WateringCanTool);
            ShowSelector(WateringCanSelector);
            DisableSelector(HoeSelector);
            DisableSelector(SeedSelector);
            DisableSelector(GlovesSelector);
            DisableSelector(ShovelSelector);
            LevelManager.Instance.CambioHerramientas(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleTool(HoeTool);
            ShowSelector(HoeSelector);
            DisableSelector(GlovesSelector);
            DisableSelector(SeedSelector);
            DisableSelector(WateringCanSelector);
            DisableSelector(ShovelSelector);
            LevelManager.Instance.CambioHerramientas(3);
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Activa la herramienta seleccionada o la deselecciona si ya está activa.
    /// </summary>
    /// <param name="newTool">Herramienta a activar o desactivar</param>
    private void ToggleTool(GameObject newTool)
    {
        if (_currentTool == newTool)
        {
            DeselectCurrentTool();
        }
        else
        {
            SelectTool(newTool);
        }
    }
    
    private void ShowSelector(GameObject selector)
    {
        selector.SetActive(true);
    }

    private void DisableSelector(GameObject selector)
    {
        selector.SetActive(false);
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
            _currentTool.transform.SetParent(ToolsPositions);
            _currentTool = null;
        }
    }

    #endregion
}
 // class ToolManager 
// namespace
