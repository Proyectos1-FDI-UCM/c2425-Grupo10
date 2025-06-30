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

    /// <summary>
    /// Texto que muestra el total de cultivos vendidos
    /// </summary>
    [SerializeField] private TextMeshProUGUI TotalSalesText;

    /// <summary>
    /// Texto que muestra el cultivo favorito del jugador
    /// </summary>
    [SerializeField] private TextMeshProUGUI FavoriteCropText;

    /// <summary>
    /// Texto que muestra el progreso de logros desbloqueados
    /// </summary>
    [SerializeField] private TextMeshProUGUI ProgressText;

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
    /// Array de textos que representan los iconos de los logros (usando emojis)
    /// </summary>
    [SerializeField] private TextMeshProUGUI[] AchievementIcons;

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
        //Debug.Log("ShowStats llamado en StatsUI");
        //InitializeReferences(); // Forzar re-inicialización

        //if (StatsPanel != null)
        //{
        //    UpdateStatsDisplay();
        //    UpdateAchievementsDisplay();
        //    StatsPanel.SetActive(true);
        //    _isPanelVisible = true;
        //    Debug.Log("Panel de estadísticas mostrado");
        //}
        //else
        //{
        //    Debug.LogError("StatsPanel no está asignado en StatsUI");
        //}

        Debug.Log("=== SHOWSTATS LLAMADO ===");
        InitializeReferences();

        if (StatsPanel != null)
        {
            // VERIFICAR QUE LOS DATOS EXISTEN
            if (_statsManager != null)
            {
                PlayerStats stats = _statsManager.GetStats();
                Debug.Log($"DATOS EN STATSMANAGER - Lechugas vendidas: {stats.LettucesSold}");
                Debug.Log($"DATOS EN STATSMANAGER - Dinero ganado: {stats.TotalMoneyEarned}");
                Debug.Log($"DATOS EN STATSMANAGER - Plantas plantadas: {stats.TotalPlantsPlanted}");
            }
            else
            {
                Debug.LogError("_statsManager es NULL");
            }

            // VERIFICAR QUE LAS REFERENCIAS EXISTEN
            Debug.Log($"LettucesSoldText asignado: {LettucesSoldText != null}");
            Debug.Log($"MoneyEarnedText asignado: {MoneyEarnedText != null}");
            Debug.Log($"PlantsPlantedText asignado: {PlantsPlantedText != null}");

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
        // Buscar StatsManager activamente
        _statsManager = StatsManager.Instance;

        // Si sigue siendo null, buscar en la escena
        if (_statsManager == null)
        {
            StatsManager foundManager = FindObjectOfType<StatsManager>();
            if (foundManager != null)
            {
                _statsManager = foundManager;
                Debug.Log("StatsManager encontrado mediante FindObjectOfType");
            }
            else
            {
                Debug.LogError("No se pudo encontrar StatsManager en la escena");
            }
        }
        else
        {
            Debug.Log("StatsManager.Instance encontrado correctamente");
        }
    }

    /// <summary>
    /// Configura la UI inicial y conecta los eventos
    /// </summary>
    private void SetupUI()
    {
        //    if (CloseButton != null)
        //    {
        //        CloseButton.onClick.AddListener(HideStats);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("CloseButton no está asignado en StatsUI");
        //    }
    }

    /// <summary>
    /// Actualiza todos los textos de estadísticas con los datos actuales
    /// </summary>
    private void UpdateStatsDisplay()
    {
        // Intentar obtener StatsManager si es null
        if (_statsManager == null)
        {
            _statsManager = StatsManager.Instance;
        }

        // Verificar nuevamente
        if (_statsManager == null)
        {
            Debug.LogError("StatsManager no está disponible para actualizar estadísticas");
            return;
        }

        PlayerStats stats = _statsManager.GetStats();

        // Actualizar estadísticas generales
        UpdateTextIfNotNull(PlantsPlantedText, "Plantas plantadas: " + stats.TotalPlantsPlanted.ToString());
        UpdateTextIfNotNull(MoneyEarnedText, "Dinero ganado: " + stats.TotalMoneyEarned.ToString() + " RC");
        UpdateTextIfNotNull(PlantsHarvestedText, "Plantas cosechadas: " + stats.TotalPlantsHarvested.ToString());

        // Calcular total de ventas
        int totalSales = stats.LettucesSold + stats.CarrotsSold + stats.StrawberriesSold + stats.CornSold;
        UpdateTextIfNotNull(TotalSalesText, "Total ventas: " + totalSales.ToString());

        // Calcular cultivo favorito
        string favoriteCrop = GetFavoriteCrop(stats);
        UpdateTextIfNotNull(FavoriteCropText, "Cultivo favorito: " + favoriteCrop);

        // Calcular progreso de logros
        int unlockedCount = 0;
        for (int i = 0; i < stats.AchievementsUnlocked.Length; i++)
        {
            if (stats.AchievementsUnlocked[i]) unlockedCount++;
        }
        int progressPercentage = (unlockedCount * 100) / 6; // 6 logros totales
        UpdateTextIfNotNull(ProgressText, "Logros: " + unlockedCount + "/6 (" + progressPercentage + "%)");

        // Actualizar estadísticas por cultivo
        UpdateTextIfNotNull(LettucesSoldText, "Lechugas: " + stats.LettucesSold.ToString());
        UpdateTextIfNotNull(CarrotsSoldText, "Zanahorias: " + stats.CarrotsSold.ToString());
        UpdateTextIfNotNull(StrawberriesSoldText, "Fresas: " + stats.StrawberriesSold.ToString());
        UpdateTextIfNotNull(CornSoldText, "Maíz: " + stats.CornSold.ToString());

        Debug.Log("Estadísticas actualizadas en la UI correctamente");
    }

    /// <summary>
    /// Actualiza la visualización de todos los logros
    /// </summary>
    private void UpdateAchievementsDisplay()
    {
        // Intentar obtener StatsManager si es null
        if (_statsManager == null)
        {
            _statsManager = StatsManager.Instance;
        }

        // Verificar nuevamente
        if (_statsManager == null)
        {
            Debug.LogError("StatsManager no está disponible para actualizar logros");
            return;
        }

        //LIMITAR A 6 LOGROS:
        int maxAchievements = Mathf.Min(6, AchievementObjects.Length, AchievementTitles.Length);

        for (int i = 0; i < maxAchievements; i++)
        {
            bool isUnlocked = _statsManager.IsAchievementUnlocked(i);

            // Actualizar título
            if (i < AchievementTitles.Length && AchievementTitles[i] != null)
            {
                AchievementTitles[i].text = _statsManager.GetAchievementTitle(i);
                AchievementTitles[i].color = isUnlocked ? UnlockedColor : LockedColor;
            }

            // Actualizar descripción
            if (i < AchievementDescriptions.Length && AchievementDescriptions[i] != null)
            {
                AchievementDescriptions[i].text = _statsManager.GetAchievementDescription(i);
                AchievementDescriptions[i].color = isUnlocked ? Color.white : LockedColor;
            }

            // Actualizar icono (ahora es texto con emoji)
            if (i < AchievementIcons.Length && AchievementIcons[i] != null)
            {
                // Cambiar el emoji según si está desbloqueado o no
                AchievementIcons[i].text = isUnlocked ? "■ " : "□";
                AchievementIcons[i].color = isUnlocked ? UnlockedColor : LockedColor;
            }

            // Mostrar/ocultar el objeto del logro si es necesario
            if (i < AchievementObjects.Length && AchievementObjects[i] != null)
            {
                AchievementObjects[i].SetActive(true);
            }
        }

        Debug.Log("Logros actualizados en la UI correctamente");
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

    /// <summary>
    /// Calcula cuál es el cultivo más vendido del jugador
    /// </summary>
    /// <param name="stats">Estadísticas del jugador</param>
    /// <returns>Nombre del cultivo favorito</returns>
    private string GetFavoriteCrop(PlayerStats stats)
    {
        int maxSold = Mathf.Max(stats.LettucesSold, stats.CarrotsSold, stats.StrawberriesSold, stats.CornSold);

        if (maxSold == 0)
        {
            return "Ninguno";
        }

        if (stats.LettucesSold == maxSold) return "Lechuga";
        if (stats.CarrotsSold == maxSold) return "Zanahoria";
        if (stats.StrawberriesSold == maxSold) return "Fresa";
        if (stats.CornSold == maxSold) return "Maíz";

        return "Ninguno";
    }
    #endregion

} // class StatsUI 
// namespace
