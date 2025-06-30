//---------------------------------------------------------
// UI para mostrar estadísticas y logros del jugador
// Alexia Pérez Santana
// Roots Of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente encargado de gestionar la interfaz de usuario para mostrar las estadísticas
/// del jugador y los logros desbloqueados. Permite visualizar datos como plantas plantadas,
/// dinero ganado, cultivos vendidos y el progreso de los diferentes logros disponibles.
/// Se actualiza dinámicamente con los datos del StatsManager.
/// </summary>
public class StatsUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Header("Panel Principal")]
    /// <summary>
    /// Panel principal que contiene toda la UI de estadísticas
    /// </summary>
    [SerializeField] private GameObject StatsPanel;

    /// <summary>
    /// Botón para cerrar el panel de estadísticas
    /// </summary>
    [SerializeField] private Button CloseButton;

    [Header("Estadísticas Generales")]
    /// <summary>
    /// Texto que muestra el número total de plantas plantadas
    /// </summary>
    [SerializeField] private TextMeshProUGUI PlantsPlantedText;

    /// <summary>
    /// Texto que muestra el dinero total ganado
    /// </summary>
    [SerializeField] private TextMeshProUGUI MoneyEarnedText;

    /// <summary>
    /// Texto que muestra el número total de plantas cosechadas
    /// </summary>
    [SerializeField] private TextMeshProUGUI PlantsHarvestedText;

    [Header("Estadísticas por Cultivo")]
    /// <summary>
    /// Texto que muestra el número de lechugas vendidas
    /// </summary>
    [SerializeField] private TextMeshProUGUI LettucesSoldText;

    /// <summary>
    /// Texto que muestra el número de zanahorias vendidas
    /// </summary>
    [SerializeField] private TextMeshProUGUI CarrotsSoldText;

    /// <summary>
    /// Texto que muestra el número de fresas vendidas
    /// </summary>
    [SerializeField] private TextMeshProUGUI StrawberriesSoldText;

    /// <summary>
    /// Texto que muestra el número de maíces vendidos
    /// </summary>
    [SerializeField] private TextMeshProUGUI CornSoldText;

    [Header("Sistema de Logros")]
    /// <summary>
    /// Array de GameObjects que representan cada logro en la UI
    /// </summary>
    [SerializeField] private GameObject[] AchievementObjects;

    /// <summary>
    /// Array de textos que muestran los títulos de los logros
    /// </summary>
    [SerializeField] private TextMeshProUGUI[] AchievementTitles;

    /// <summary>
    /// Array de textos que muestran las descripciones de los logros
    /// </summary>
    [SerializeField] private TextMeshProUGUI[] AchievementDescriptions;

    /// <summary>
    /// Array de imágenes que representan los iconos de los logros
    /// </summary>
    [SerializeField] private Image[] AchievementIcons;

    /// <summary>
    /// Color que se usa para los logros desbloqueados
    /// </summary>
    [SerializeField] private Color UnlockedColor = Color.yellow;

    /// <summary>
    /// Color que se usa para los logros bloqueados
    /// </summary>
    [SerializeField] private Color LockedColor = Color.gray;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia al StatsManager para obtener los datos
    /// </summary>
    private StatsManager _statsManager;

    /// <summary>
    /// Indica si el panel de estadísticas está actualmente visible
    /// </summary>
    private bool _isPanelVisible = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        InitializeReferences();
        SetupUI();
        HideStats();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Muestra el panel de estadísticas y actualiza todos los datos
    /// </summary>
    public void ShowStats()
    {
        if (StatsPanel != null)
        {
            UpdateStatsDisplay();
            UpdateAchievementsDisplay();
            StatsPanel.SetActive(true);
            _isPanelVisible = true;
            Debug.Log("Panel de estadísticas mostrado");
        }
        else
        {
            Debug.LogError("StatsPanel no está asignado en StatsUI");
        }
    }

    /// <summary>
    /// Oculta el panel de estadísticas
    /// </summary>
    public void HideStats()
    {
        if (StatsPanel != null)
        {
            StatsPanel.SetActive(false);
            _isPanelVisible = false;
            Debug.Log("Panel de estadísticas ocultado");
        }
    }

    /// <summary>
    /// Alterna la visibilidad del panel de estadísticas
    /// </summary>
    public void ToggleStats()
    {
        if (_isPanelVisible)
        {
            HideStats();
        }
        else
        {
            ShowStats();
        }
    }

    /// <summary>
    /// Verifica si el panel de estadísticas está visible
    /// </summary>
    /// <returns>True si el panel está visible, false en caso contrario</returns>
    public bool IsStatsVisible()
    {
        return _isPanelVisible;
    }

    /// <summary>
    /// Actualiza manualmente todos los datos mostrados en la UI
    /// </summary>
    public void RefreshAllData()
    {
        if (_isPanelVisible)
        {
            UpdateStatsDisplay();
            UpdateAchievementsDisplay();
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Inicializa las referencias necesarias
    /// </summary>
    private void InitializeReferences()
    {
        _statsManager = StatsManager.Instance;

        if (_statsManager == null)
        {
            Debug.LogError("No se pudo encontrar StatsManager en la escena");
        }
    }

    /// <summary>
    /// Configura la UI inicial y conecta los eventos
    /// </summary>
    private void SetupUI()
    {
        if (CloseButton != null)
        {
            CloseButton.onClick.AddListener(HideStats);
        }
        else
        {
            Debug.LogWarning("CloseButton no está asignado en StatsUI");
        }
    }

    /// <summary>
    /// Actualiza todos los textos de estadísticas con los datos actuales
    /// </summary>
    private void UpdateStatsDisplay()
    {
        if (_statsManager == null)
        {
            Debug.LogError("StatsManager no está disponible para actualizar estadísticas");
            return;
        }

        PlayerStats stats = _statsManager.GetStats();

        // Actualizar estadísticas generales
        UpdateTextIfNotNull(PlantsPlantedText, stats.TotalPlantsPlanted.ToString());
        UpdateTextIfNotNull(MoneyEarnedText, stats.TotalMoneyEarned.ToString() + " RC");
        UpdateTextIfNotNull(PlantsHarvestedText, stats.TotalPlantsHarvested.ToString());

        // Actualizar estadísticas por cultivo
        UpdateTextIfNotNull(LettucesSoldText, stats.LettucesSold.ToString());
        UpdateTextIfNotNull(CarrotsSoldText, stats.CarrotsSold.ToString());
        UpdateTextIfNotNull(StrawberriesSoldText, stats.StrawberriesSold.ToString());
        UpdateTextIfNotNull(CornSoldText, stats.CornSold.ToString());

        Debug.Log("Estadísticas actualizadas en la UI");
    }

    /// <summary>
    /// Actualiza la visualización de todos los logros
    /// </summary>
    private void UpdateAchievementsDisplay()
    {
        if (_statsManager == null)
        {
            Debug.LogError("StatsManager no está disponible para actualizar logros");
            return;
        }

        int maxAchievements = Mathf.Min(AchievementObjects.Length, AchievementTitles.Length);

        for (int i = 0; i < maxAchievements; i++)
        {
            bool isUnlocked = _statsManager.IsAchievementUnlocked(i);

            // Actualizar título
            if (AchievementTitles[i] != null)
            {
                AchievementTitles[i].text = _statsManager.GetAchievementTitle(i);
                AchievementTitles[i].color = isUnlocked ? UnlockedColor : LockedColor;
            }

            // Actualizar descripción
            if (AchievementDescriptions[i] != null)
            {
                AchievementDescriptions[i].text = _statsManager.GetAchievementDescription(i);
                AchievementDescriptions[i].color = isUnlocked ? Color.white : LockedColor;
            }

            // Actualizar icono
            if (AchievementIcons[i] != null)
            {
                AchievementIcons[i].color = isUnlocked ? UnlockedColor : LockedColor;
            }

            // Actualizar visibilidad del objeto completo si es necesario
            if (AchievementObjects[i] != null)
            {
                AchievementObjects[i].SetActive(true);
            }
        }

        Debug.Log("Logros actualizados en la UI");
    }

    /// <summary>
    /// Actualiza un texto solo si no es null
    /// </summary>
    /// <param name="textComponent">Componente de texto a actualizar</param>
    /// <param name="newText">Nuevo texto a asignar</param>
    private void UpdateTextIfNotNull(TextMeshProUGUI textComponent, string newText)
    {
        if (textComponent != null)
        {
            textComponent.text = newText;
        }
        else
        {
            Debug.LogWarning($"Componente de texto no asignado para el valor: {newText}");
        }
    }

    #endregion

} // class StatsUI 
// namespace
