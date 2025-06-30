//---------------------------------------------------------
//Sistema de estadísticas y logros del jugador
// Alexia perez santana
// Roots Of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Estructura serializable que contiene todas las estadísticas del jugador
/// y el estado de los logros desbloqueados
/// </summary>
[Serializable]
public class PlayerStats
{
    public int TotalPlantsPlanted = 0;
    public int TotalMoneyEarned = 0;
    public int TotalPlantsHarvested = 0;
    public int TotalTimePlayed = 0; // en segundos

    // Estadísticas por cultivo
    public int LettucesSold = 0;
    public int CarrotsSold = 0;
    public int StrawberriesSold = 0;
    public int CornSold = 0;

    // Logros desbloqueados
    public bool[] AchievementsUnlocked = new bool[10];
}

/// <summary>
/// Manager que se encarga de gestionar las estadísticas del jugador y el sistema de logros.
/// Funciona como singleton para mantener los datos persistentes entre escenas.
/// Registra automáticamente las acciones del jugador y desbloquea logros según criterios.
/// Se integra con el sistema de guardado del juego.
/// </summary>
public class StatsManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    

    /// <summary>
    /// Estructura que contiene todas las estadísticas del jugador
    /// </summary>
    [SerializeField] private PlayerStats PlayerStats = new PlayerStats();

    /// <summary>
    /// Referencia al UIManager para mostrar notificaciones de logros
    /// </summary>
    [SerializeField] private UIManager UIManager;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
   
    /// <summary>
    /// Instancia única del StatsManager (patrón Singleton)
    /// </summary>
    private static StatsManager _instance;

    /// <summary>
    /// Array con los títulos de todos los logros disponibles
    /// </summary>
    private string[] _achievementTitles = {
        "¡Primer Granjero!",
        "¡Granjero Experto!",
        "¡Millonario!",
        "¡Rey de las Lechugas!",
        "¡Zanahoria Dorada!",
        "¡Fresa Perfecta!",
        "¡Maestro del Maíz!",
        "¡Cosechador Experto!",
        "¡Magnate Agrícola!",
        "¡Leyenda de la Granja!"
    };

    /// <summary>
    /// Array con las descripciones de todos los logros disponibles
    /// </summary>
    private string[] _achievementDescriptions = {
        "Has plantado 10 plantas",
        "Has plantado 50 plantas",
        "Has ganado 1000 RootCoins",
        "Has vendido 20 lechugas",
        "Has vendido 15 zanahorias",
        "Has vendido 10 fresas",
        "Has vendido 5 maíces",
        "Has cosechado 100 plantas",
        "Has ganado 10000 RootCoins",
        "Has plantado 200 plantas"
    };
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Awake se llama cuando se instancia el script. Configura el patrón Singleton
    /// y asegura que el objeto persista entre escenas.
    /// </summary>
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        InitializeUIManager();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Propiedad para acceder a la instancia única del StatsManager
    /// </summary>
    public static StatsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<StatsManager>();
            return _instance;
        }
    }

    /// <summary>
    /// Incrementa el contador de plantas plantadas y verifica logros
    /// </summary>
    public void AddPlantPlanted()
    {
        PlayerStats.TotalPlantsPlanted++;
        CheckAchievements();
        Debug.Log($"Plantas plantadas: {PlayerStats.TotalPlantsPlanted}");
    }

    /// <summary>
    /// Añade dinero ganado al total y verifica logros
    /// </summary>
    /// <param name="amount">Cantidad de dinero ganado</param>
    public void AddMoneyEarned(int amount)
    {
        PlayerStats.TotalMoneyEarned += amount;
        CheckAchievements();
        Debug.Log($"Dinero total ganado: {PlayerStats.TotalMoneyEarned}");
    }

    /// <summary>
    /// Incrementa el contador de plantas cosechadas y verifica logros
    /// </summary>
    public void AddPlantHarvested()
    {
        PlayerStats.TotalPlantsHarvested++;
        CheckAchievements();
        Debug.Log($"Plantas cosechadas: {PlayerStats.TotalPlantsHarvested}");
    }

    /// <summary>
    /// Registra la venta de cultivos específicos y verifica logros
    /// </summary>
    /// <param name="cropType">Tipo de cultivo vendido</param>
    /// <param name="quantity">Cantidad vendida</param>
    public void AddCropSold(Items cropType, int quantity)
    {
        switch (cropType)
        {
            case Items.Lettuce:
                PlayerStats.LettucesSold += quantity;
                break;
            case Items.Carrot:
                PlayerStats.CarrotsSold += quantity;
                break;
            case Items.Strawberry:
                PlayerStats.StrawberriesSold += quantity;
                break;
            case Items.Corn:
                PlayerStats.CornSold += quantity;
                break;
        }
        CheckAchievements();
        Debug.Log($"{cropType} vendidos: {quantity}");
    }

    /// <summary>
    /// Devuelve las estadísticas actuales del jugador
    /// </summary>
    /// <returns>Estructura PlayerStats con todas las estadísticas</returns>
    public PlayerStats GetStats()
    {
        return PlayerStats;
    }

    /// <summary>
    /// Verifica si un logro específico está desbloqueado
    /// </summary>
    /// <param name="index">Índice del logro a verificar</param>
    /// <returns>True si el logro está desbloqueado, false en caso contrario</returns>
    public bool IsAchievementUnlocked(int index)
    {
        if (index >= 0 && index < PlayerStats.AchievementsUnlocked.Length)
            return PlayerStats.AchievementsUnlocked[index];
        return false;
    }

    /// <summary>
    /// Carga las estadísticas desde un archivo de guardado
    /// </summary>
    /// <param name="stats">Estadísticas a cargar</param>
    public void LoadStats(PlayerStats stats)
    {
        PlayerStats = stats;
        Debug.Log("Estadísticas cargadas correctamente");
    }

    /// <summary>
    /// Reinicia todas las estadísticas a sus valores por defecto
    /// </summary>
    public void ResetStats()
    {
        PlayerStats = new PlayerStats();
        Debug.Log("Estadísticas reiniciadas");
    }

    /// <summary>
    /// Inicializa la referencia al UIManager
    /// </summary>
    public void InitializeUIManager()
    {
        if (UIManager == null)
            UIManager = FindObjectOfType<UIManager>();
    }

    /// <summary>
    /// Devuelve el título de un logro específico
    /// </summary>
    /// <param name="index">Índice del logro</param>
    /// <returns>Título del logro</returns>
    public string GetAchievementTitle(int index)
    {
        if (index >= 0 && index < _achievementTitles.Length)
            return _achievementTitles[index];
        return "Logro Desconocido";
    }

    /// <summary>
    /// Devuelve la descripción de un logro específico
    /// </summary>
    /// <param name="index">Índice del logro</param>
    /// <returns>Descripción del logro</returns>
    public string GetAchievementDescription(int index)
    {
        if (index >= 0 && index < _achievementDescriptions.Length)
            return _achievementDescriptions[index];
        return "Descripción no disponible";
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Verifica todas las condiciones de logros y desbloquea los que correspondan
    /// </summary>
    private void CheckAchievements()
    {
        // Logro 0: Plantar 10 plantas
        if (PlayerStats.TotalPlantsPlanted >= 10 && !PlayerStats.AchievementsUnlocked[0])
        {
            UnlockAchievement(0);
        }

        // Logro 1: Plantar 50 plantas
        if (PlayerStats.TotalPlantsPlanted >= 50 && !PlayerStats.AchievementsUnlocked[1])
        {
            UnlockAchievement(1);
        }

        // Logro 2: Ganar 1000 RootCoins
        if (PlayerStats.TotalMoneyEarned >= 1000 && !PlayerStats.AchievementsUnlocked[2])
        {
            UnlockAchievement(2);
        }

        // Logro 3: Vender 20 lechugas
        if (PlayerStats.LettucesSold >= 20 && !PlayerStats.AchievementsUnlocked[3])
        {
            UnlockAchievement(3);
        }

        // Logro 4: Vender 15 zanahorias
        if (PlayerStats.CarrotsSold >= 15 && !PlayerStats.AchievementsUnlocked[4])
        {
            UnlockAchievement(4);
        }

        // Logro 5: Vender 10 fresas
        if (PlayerStats.StrawberriesSold >= 10 && !PlayerStats.AchievementsUnlocked[5])
        {
            UnlockAchievement(5);
        }

        // Logro 6: Vender 5 maíces
        if (PlayerStats.CornSold >= 5 && !PlayerStats.AchievementsUnlocked[6])
        {
            UnlockAchievement(6);
        }

        // Logro 7: Cosechar 100 plantas
        if (PlayerStats.TotalPlantsHarvested >= 100 && !PlayerStats.AchievementsUnlocked[7])
        {
            UnlockAchievement(7);
        }

        // Logro 8: Ganar 10000 RootCoins
        if (PlayerStats.TotalMoneyEarned >= 10000 && !PlayerStats.AchievementsUnlocked[8])
        {
            UnlockAchievement(8);
        }

        // Logro 9: Plantar 200 plantas
        if (PlayerStats.TotalPlantsPlanted >= 200 && !PlayerStats.AchievementsUnlocked[9])
        {
            UnlockAchievement(9);
        }
    }

    /// <summary>
    /// Desbloquea un logro específico y muestra la notificación correspondiente
    /// </summary>
    /// <param name="index">Índice del logro a desbloquear</param>
    private void UnlockAchievement(int index)
    {
        if (index >= 0 && index < PlayerStats.AchievementsUnlocked.Length)
        {
            PlayerStats.AchievementsUnlocked[index] = true;
            ShowAchievementNotification(index);
            Debug.Log($"¡Logro desbloqueado! {_achievementTitles[index]}: {_achievementDescriptions[index]}");
        }
    }

    /// <summary>
    /// Muestra una notificación cuando se desbloquea un logro
    /// </summary>
    /// <param name="index">Índice del logro desbloqueado</param>
    private void ShowAchievementNotification(int index)
    {
        if (UIManager != null && index < _achievementTitles.Length)
        {
            string title = "🏆 " + _achievementTitles[index];
            string description = _achievementDescriptions[index];
            UIManager.ShowNotification(title, description, 5, "Achievement");
        }
    }

    #endregion

} // class StatsManager 
// namespace
