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
    ///Texto de la notificacion normal
    /// </summary>
    private string _energyNotificationText = "";
    ///<summary>
    ///Texto de la notificacion normal
    /// </summary>
    private string _toolNotificationText = "";
    ///<summary>
    ///Texto de la notificacion normal
    /// </summary>
    private string _wcNotificationText = "";
    ///<summary>
    ///Texto de la notificacion normal
    /// </summary>
    private string _inventoryNotificationText = "";

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
    ///int para saber si el check1 esta activo
    /// </summary>
    private int _tutorialNotificationCheck1;

    ///<summary>
    ///int para saber si el check2 esta activo
    /// </summary>
    private int _tutorialNotificationCheck2;

    ///<summary>
    ///int para saber si el check3 esta activo
    /// </summary>
    private int _tutorialNotificationCheck3;

    ///<summary>
    ///Booleanos para saber si debe haber notificacion de ese tipo activa
    /// </summary>
    [SerializeField]private bool _isNotificationCreated = false;
    private bool _isTutorialNotificationCreated = false;
    private bool _isEnergyNotificationCreated = false;
    private bool _isToolNotificationCreated = false;
    private bool _isWcNotificationCreated = false;
    private bool _isInventoryNotificationCreated = false;

    [SerializeField]  private bool _check1 = false;
    [SerializeField] private bool _check2 = false;
    [SerializeField] private bool _check3 = false;

    [SerializeField] private int _intCheck1 = 0;
    [SerializeField] private int _intCheck2 = 0;
    [SerializeField] private int _intCheck3 = 0;
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
    public void SaveNotification(string text, string counterText, string source, int check1 = 0, int check2 = 0, int check3 = 0)
    {
        if (source == "Tutorial")
        {
            _isTutorialNotificationCreated = true;
            _tutorialNotificationText = text;
            _tutorialNotificationCounterText = counterText;
            if (check1 == 1)
            {
                _check1 = true;
            }
            if (check2 == 1)
            {
                _check2 = true;
            }
            if(check3 == 1)
            {
                _check3 = true;
            }

        }
        else if (source == "NoTutorial")
        {
            _isNotificationCreated = true;
            _notificationText = text;
        }
        else if (source == "Energy")
        {
            _isEnergyNotificationCreated = true;
            _energyNotificationText = text;
        }
        else if(source == "WC")
        {
            _isWcNotificationCreated = true;
            _wcNotificationText = text;
        }
        else if (source == "Tool")
        {
            _isToolNotificationCreated = true;
            _toolNotificationText = text;
        }
        else if (source == "Inventory")
        {
            _isInventoryNotificationCreated = true;
            _inventoryNotificationText = text;
        }

    }
    ///<summary>
    ///Metodo para guardar la notificacion
    /// </summary>
    public void EditNotification(int check)
    {
        Debug.Log("Noti Mod check" + check);

        if (check == 0)
        {
            _check1 = true;
        }
        else if (check == 1)
        {
            _check2 = true;
        }
        else if (check == 2)
        {
            _check3 = true;
        }

    }

    ///<summary>
    ///Metodo para cargar las notificaciones
    /// </summary>
    public void LoadNotification(string source)
    {
        if (source == "Tutorial" && _isTutorialNotificationCreated)
        {
            if (_check1) _intCheck1 = 1;
            if (_check2) _intCheck2 = 1;
            if (_check3) _intCheck3 = 1;

            UIManager.ShowNotification(_tutorialNotificationText, _tutorialNotificationCounterText, 2, "Tutorial", _intCheck1, _intCheck2, _intCheck3);
            SoundManager.NextButtonSound();

            // Reseteo de checks para que no queden marcados al recargar
            _check1 = false;
            _check2 = false;
            _check3 = false;
            _intCheck1 = 0;
            _intCheck2 = 0;
            _intCheck3 = 0;
        }
        else if (source == "NoTutorial" && _isNotificationCreated)
        {
            UIManager.ShowNotification(_notificationText, _notificationCounterText, 1, "NoTutorial");
            SoundManager.NextButtonSound();
        }
        else if (source == "Energy" && _isEnergyNotificationCreated)
        {
            UIManager.ShowNotification(_energyNotificationText, "NoCounter", 1, "NoTutorial");
            SoundManager.NextButtonSound();
        }
        else if (source == "Tool" && _isToolNotificationCreated)
        {
            UIManager.ShowNotification(_toolNotificationText, "NoCounter", 1, "NoTutorial");
            SoundManager.NextButtonSound();
        }
        else if (source == "WC" && _isWcNotificationCreated)
        {
            UIManager.ShowNotification(_wcNotificationText, "NoCounter", 1, "NoTutorial");
            SoundManager.NextButtonSound();
        }
        else if (source == "Inventory" && _isInventoryNotificationCreated)
        {
            UIManager.ShowNotification(_wcNotificationText, "NoCounter", 1, "NoTutorial");
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
            _isTutorialNotificationCreated = false;
            _check1 = false;
            _check2 = false;
            _check3 = false;
            _intCheck1 = 0;
            _intCheck2 = 0; 
            _intCheck3 = 0;
        }
        else if (source == "NoTutorial" && _isNotificationCreated)
        {
            _notificationText = "";
            _notificationCounterText = "";
            _isNotificationCreated = false;
        }
        else if (source == "Energy" && _isEnergyNotificationCreated)
        {
            _energyNotificationText = "";
            _isEnergyNotificationCreated = false;
        }
        else if (source == "WC" && _isWcNotificationCreated)
        {
            _wcNotificationText = "";
            _isWcNotificationCreated = false;
        }
        else if (source == "Tool" && _isToolNotificationCreated)
        {
            _toolNotificationText = "";
            _isToolNotificationCreated = false;
        }
        else if (source == "Inventory" && _isInventoryNotificationCreated)
        {
            _inventoryNotificationText = "";
            _isInventoryNotificationCreated = false;
        }
    }
    ///<summary>
    ///Metodo para inicializar el soundmanager
    /// </summary>
    public void InitializeSoundManager()
    {
        SoundManager = FindObjectOfType<SoundManager>();
    }

    public bool GetCheckActive(int i)
    {
        if (i == 0)
        {
            return _check1;
        }
        else if (i == 1)
        {
            return _check2;
        }
        else if (i == 2)
        {
            return _check3;
        }
        return false;
    }
    ///<summary>
    ///Metodo para inicializar el uimanager
    /// </summary>
    public void InitializeUIManager()
    {
        UIManager = FindObjectOfType<UIManager>();
    }

    public void ResetNotificationManager()
    {
        _isNotificationCreated = false;
        _isTutorialNotificationCreated = false;
        _isEnergyNotificationCreated = false;
        _isToolNotificationCreated = false;
        _isWcNotificationCreated= false;
        _check1 = false;
        _check2 = false;
        _check3 = false;
        _intCheck3 = 0;
        _intCheck1 = 0;
        _intCheck2 = 0;
        _notificationText = "";
        _tutorialNotificationText = "";
        _toolNotificationText = "";
        _energyNotificationText = "";
        _wcNotificationText = "";
        _inventoryNotificationText = "";
        _notificationCounterText = "";
        _tutorialNotificationCounterText = "";
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
