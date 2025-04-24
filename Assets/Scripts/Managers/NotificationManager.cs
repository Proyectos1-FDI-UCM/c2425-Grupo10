//---------------------------------------------------------
// Maneja las notificaciones entre escenas
// Javier librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class NotificationManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    ///<summary>
    ///Ref al uimanager
    /// </summary>
    [SerializeField] private UIManager UIManager;

    ///<summary>
    ///Ref al sound manager
    /// </summary>
    [SerializeField] private SoundManager SoundManager;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints


    ///<summary>
    ///Texto de la notificacion normal
    /// </summary>
    private string _notificationText = "";

    ///<summary>
    ///Texto de la notificacion tutorail
    /// </summary>
    private string _tutorialNotificationText = "";
    ///<summary>
    ///Texto de del contador notificacion normal
    /// </summary>
    private string _notificationCounterText = "";

    ///<summary>
    ///Texto del contador notificacion tutorail
    /// </summary>
    private string _tutorialNotificationCounterText = "";

    ///<summary>
    ///Booleanos para saber si debe haber notificacion de ese tipo activa
    /// </summary>
    private bool _isNotificationCreated = false;
    private bool _isTutorialNotificationCreated = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {

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
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    ///<summary>
    ///Metodo para guardar la notificacion
    /// </summary>
    public void SaveNotification(string text, string counterText, string source)
    {
        if (source == "Tutorial")
        {
            _isTutorialNotificationCreated = true;
            _tutorialNotificationText = text;
            _tutorialNotificationCounterText = counterText;
        }
        else if (source == "NoTutorial")
        {
            _isNotificationCreated = true;
            _notificationText = text;
            _notificationCounterText = counterText;
        }
    }

    ///<summary>
    ///Metodo para cargar las notificaciones
    /// </summary>
    public void LoadNotification(string source)
    {
        if (source == "Tutorial" && _isTutorialNotificationCreated)
        {
            UIManager.ShowNotification(_tutorialNotificationText, _tutorialNotificationCounterText, 2,"Tutorial");
            SoundManager.NextButtonSound();
        }
        else if (source == "NoTutorial" && _isNotificationCreated)
        {
            UIManager.ShowNotification(_notificationText, _notificationCounterText, 1, "NoTutorial");
            SoundManager.NextButtonSound();
        }
    }

    ///<summary>
    ///Metodo para borrar la notificacion guardada
    /// </summary>
    public void DestroyNotification(string source)
    {
        if (source == "Tutorial" && _isTutorialNotificationCreated)
        {
            _tutorialNotificationText = "";
            _tutorialNotificationCounterText = "";
            _isTutorialNotificationCreated &= false;
        }
        else if (source == "NoTutorial" && _isNotificationCreated)
        {
            _notificationText = "";
            _notificationCounterText = "";
            _isNotificationCreated &= false;
        }
    }
    ///<summary>
    ///Metodo para inicializar el soundmanager
    /// </summary>
    public void InitializeSoundManager()
    {
        SoundManager = FindObjectOfType<SoundManager>();
    }

    ///<summary>
    ///Metodo para inicializar el uimanager
    /// </summary>
    public void InitializeUIManager()
    {
        UIManager = FindObjectOfType<UIManager>();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    ///<summary>
    ///Metodo para inicializar referencias
    /// </summary>
    private void InitializeReferences()
    {
        UIManager = FindObjectOfType<UIManager>();
        SoundManager = FindObjectOfType<SoundManager>();
    }
    #endregion

} // class NotificationManager 
// namespace
