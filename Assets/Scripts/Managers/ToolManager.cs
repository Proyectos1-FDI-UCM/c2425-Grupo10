//---------------------------------------------------------
// Script para gestionar la selección y deselección de herramientas.
// Permite al jugador cambiar entre diferentes herramientas usando las teclas asignadas.
// Responsable: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase encargada de gestionar la selección de herramientas 
/// en el inventory del jugador. Al pulsar las teclas específicas, 
/// se activa una Tool y se desactiva la anterior.
/// </summary>
public class ToolManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Referencia al GameObject que representa la Tool "Semilla".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject SeedTool;

    /// <summary>
    /// Referencia al GameObject que representa el selector de "Semilla".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject SeedSelector;

    /// <summary>
    /// Referencia al GameObject que representa la Tool "Pala".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject ShovelTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Pala".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject ShovelSelector;

    /// <summary>
    /// Referencia al GameObject que representa la Tool "Guantes".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject GlovesTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Guantes".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject GlovesSelector;

    /// <summary>
    /// Referencia al objeto que representa la Tool "Regadera".
    /// </summary>
    [SerializeField] private GameObject WateringCanTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Regadera".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject WateringCanSelector;

    /// <summary>
    /// Referencia al objeto que representa la Tool "Hoz".
    /// </summary>
    [SerializeField] private GameObject HoeTool;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Hoz".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private GameObject HoeSelector;

    /// <summary>
    /// Referencia al transform de la mano del jugador donde se mostrarán las herramientas.
    /// </summary>
    [SerializeField] private Transform HandPosition;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Hoz".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private Slider WaterBar;
    /// <summary>
    /// Referencia al GameObject que representa el selector de "Hoz".
    /// Se activará cuando el jugador presione la tecla correspondiente.
    /// </summary>
    [SerializeField] private LevelManager LevelManager;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia a la Tool actualmente seleccionada.
    /// Solo una Tool puede estar activa a la vez.
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
        EnableSelector(GlovesSelector);
        ToggleTool(GlovesTool);
        DisableSelector(ShovelSelector, SeedSelector, WateringCanSelector, HoeSelector);
        //WaterBar.SetActive(false);
    }

    /// <summary>
    /// Escucha la entrada del jugador en cada fotograma para cambiar de Tool.
    /// </summary>
    void Update()
    {
        if (InputManager.Instance.Select5WasPressedThisFrame())
        {
            ToggleTool(SeedTool);
            EnableSelector(SeedSelector);
            DisableSelector(ShovelSelector, GlovesSelector, WateringCanSelector, HoeSelector);
            LevelManager.Instance.ChangeTool(5);
            //WaterBar.SetActive(false);
        }

        if (InputManager.Instance.Select4WasPressedThisFrame())
        {
            EnableSelector(ShovelSelector);
            ToggleTool(ShovelTool);
            DisableSelector(SeedSelector, GlovesSelector, WateringCanSelector, HoeSelector);
            LevelManager.Instance.ChangeTool(4);
            //WaterBar.SetActive(false);
        }

        if (InputManager.Instance.Select1WasPressedThisFrame())
        {
            EnableSelector(GlovesSelector);
            ToggleTool(GlovesTool);
            DisableSelector(ShovelSelector, SeedSelector, WateringCanSelector, HoeSelector);
            LevelManager.Instance.ChangeTool(1);
            //WaterBar.SetActive(false);
        }

        if (InputManager.Instance.Select2WasPressedThisFrame())
        {
            ToggleTool(WateringCanTool);
            EnableSelector(WateringCanSelector);
            DisableSelector(ShovelSelector, GlovesSelector, SeedSelector, HoeSelector);
            LevelManager.Instance.ChangeTool(2);
            //WaterBar.SetActive(true);
        }

        if (InputManager.Instance.Select3WasPressedThisFrame())
        {
            ToggleTool(HoeTool);
            EnableSelector(HoeSelector);
            DisableSelector(ShovelSelector, GlovesSelector, WateringCanSelector, SeedSelector);
            LevelManager.Instance.ChangeTool(3);
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
    public void BarraAgua(float ActualWater, float MaxWater)
    {
        WaterBar.value = ActualWater / MaxWater;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Activa la Tool seleccionada o la deselecciona si ya está activa.
    /// </summary>
    /// <param name="newTool">Tool a activar o desactivar</param>
    private void ToggleTool(GameObject newTool)
    {
        if (_currentTool != newTool)
        {
            SelectTool(newTool);
        }
        
    }


    /// <summary>
    /// Activa la Tool seleccionada y desactiva la anterior si la hay.
    /// </summary>
    /// <param name="newTool">Tool a activar</param>
    private void SelectTool(GameObject newTool)
    {
        if (newTool == null) return;

        // Desactiva la Tool actualmente seleccionada
        DeselectCurrentTool();

        // Activa la nueva Tool y la establece como actual
        _currentTool = newTool;
        _currentTool.SetActive(true);

        // Poner la Tool en la mano del jugador
        _currentTool.transform.SetParent(HandPosition);
        _currentTool.transform.localPosition = Vector3.zero; // Asegurar que esté en la posición exacta de la mano
        _currentTool.transform.localRotation = Quaternion.identity; // Resetear rotación si es necesario
    }

    /// <summary>
    /// Deselecciona la Tool actualmente en uso, dejando las manos vacías.
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

    private void EnableSelector(GameObject Tool)
    {
        Tool.SetActive(true);
    }

    private void DisableSelector(GameObject Tool1, GameObject Tool2, GameObject Tool3, GameObject Toll4)
    {
        Tool1.SetActive(false);
        Tool2.SetActive(false);
        Tool3.SetActive(false);
        Toll4.SetActive(false);
    }

    
    #endregion
}
 // class ToolManager 
// namespace
