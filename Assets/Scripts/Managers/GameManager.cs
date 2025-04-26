//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    ///<summary>
    ///Bool que activa/desactiva las características para la build
    /// </summary>
    [SerializeField] private bool Build = false;

    /// <summary>
    /// Numero de mejoras activas de la Regadera.
    /// <summary>
    [SerializeField] private int _wateringCanUpgrades = 0;

    /// <summary>
    /// Numero de mejoras activas del Huerto.
    /// <summary>
    [SerializeField] private int _gardenUpgrades = 0;


    [SerializeField] private int InitialAmountMoney;
    /// <summary>
    /// Referencia al script que maneja el dinero
    /// <summary>
    [SerializeField] private MoneyManager MoneyCount;

    /// <summary>
    /// Referencia al script que maneja la cantidad de agua
    /// <summary>
    [SerializeField] private WateringCanManager WateringCanManager;
    ///<summary>
    /// Ref al GardenManager
    /// </summary>
    [SerializeField] private GardenManager GardenManager;

    ///<summary>
    ///Ref al TutorialManager
    /// </summary>
    [SerializeField] private TutorialManager TutorialManager;

    ///<summary>
    ///Ref al Notification manager
    /// </summary>
    [SerializeField] private NotificationManager NotificationManager;

    ///<summary>
    ///Prefab de GameManager
    /// </summary>
    [SerializeField] private GameObject GameManagerPrefab;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Numero de máximo de mejoras para la Regadera.
    /// <summary>
    private int _maxWateringCanUpgrades = 3;

    /// <summary>
    /// Numero de máximo de mejoras para el Huerto.
    /// <summary>
    private int _maxGardenUpgrades = 4;

    private bool _isCursorVisible = true;

    /// <summary>
    /// Cantidad de agua de la regadera.
    /// <summary>
    private int _amountWater = 6;

    /// <summary>
    /// Array Inventory
    /// <summary>
    private int[] _inventory = new int[10];

    /// <summary>
    /// Array Inventory
    /// <summary>
    private int _moneyInvested = 0;

    /// <summary>
    /// Posición del Jugador
    /// <summary>
    private Transform _playerPosition;

    /// <summary>
    /// Timer
    /// <summary>
    private Timer timer;

    /// <summary>
    /// Tiempo que ha pasado en la escena principal
    /// </summary>
    private float realTime;

    ///<summary>
    ///Booleano para saber si es una nueva partida
    /// </summary>
    [SerializeField] private bool _newGame = true;

    ///<summary>
    ///bool para saber si hay mando
    /// </summary>
    private bool _isGameController = false;

    ///<summary>
    ///Booleano para saber si el jugador esta en la cinematica inicial
    /// </summary>
    [SerializeField] private bool _isInCinematic = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        InitializeReferences();
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al GameManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            TransferSceneState();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
        if (_isCursorVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        _isInCinematic = true;
    }
    private void Update()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad)
            {
                Debug.Log($"Gamepad detectado al inicio: {device.name}");
                _isGameController = true;
                HideCursor();
                break; // Suponemos que solo quieres ocultar el cursor al detectar el primer gamepad
            }
        }

        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change == InputDeviceChange.Added)
            {
                if (device is Gamepad && !_isGameController)
                {
                    Debug.Log($"Gamepad conectado: {device.name}");
                    _isGameController = true;
                    HideCursor();
                }
            }
            else if (change == InputDeviceChange.Removed)
            {
                if (device is Gamepad)
                {
                    Debug.Log($"Gamepad desconectado: {device.name}");
                    _isGameController = false;
                    ShowCursor();
                }
            }
        };

        if (!Build)
        {
            if (InputManager.Instance.ShorcutInventoryWasPressedThisFrame())
            {
                InventoryManager.ModifyInventory(Items.Corn, 1);
            }
            if (InputManager.Instance.ShorcutSeedWasPressedThisFrame())
            {
                InventoryManager.ModifyInventory(Items.CornSeed, 1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TutorialManager.NextDialogue();
            }

        }
        if (InputManager.Instance.ExitWasPressedThisFrame()) // Menu Pausa
        {
            if (SceneManager.GetActiveScene().name == "Escena_Build")
            {
                InventoryManager.ModifyPlayerPosition(FindObjectOfType<PlayerMovement>().transform.position);
                SaveTime();
            }
            
            //Application.Quit();
            // 
        }
        if (MoneyCount == null)
        {
            GameObject TextObject = GameObject.FindGameObjectWithTag("GameManager");
            if (TextObject != null)
            {
                MoneyCount = TextObject.GetComponent<MoneyManager>();
            }
        }
        if (GardenManager == null)
        {
            GameObject ObjetoGarden = GameObject.FindGameObjectWithTag("GardenManager");
            if (ObjetoGarden != null)
            {
                GardenManager = ObjetoGarden.GetComponent<GardenManager>();
            }
        }
        if (WateringCanManager == null)
        {
            GameObject WCObject = GameObject.FindGameObjectWithTag("WateringCan");
            if ( WCObject != null )
            {
                WateringCanManager = WCObject.GetComponent<WateringCanManager>();
            }
        }
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            EndCinematic();
        }
    }
    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal  
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    /// <summary>
    /// Metodo para obtener la cantidad de mejoras que tiene la Regadera/Huerto/Inventory.
    /// <summary>
    public int GetWateringCanUpgrades() { return _wateringCanUpgrades; }
    public int GetGardenUpgrades() { return _gardenUpgrades; }
    public bool GetNewGame() { return _newGame; }
    public bool GetControllerUsing() { return _isGameController; }
    public void SetTimer(Timer timercomponent)
    {
        timer = timercomponent;
        timer.SetRealTime(realTime);
    }

    public void SaveTime()
    {
        realTime = timer.GetRealTime();
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora del Huerto.
    /// <summary>
    public void UpgradeGarden()
    {
        if (_gardenUpgrades < _maxGardenUpgrades)
        {
            _gardenUpgrades += 1;
        }
        if (_gardenUpgrades == 1)
        {
            MoneyCount.UpgradeGardenLevel1();
        }
        else if (_gardenUpgrades == 2)
        {
            MoneyCount.UpgradeGardenLevel2();
        }
        else if (_gardenUpgrades == 3)
        {
            MoneyCount.UpgradeGardenLevel3();
        }
        else if (_gardenUpgrades == 4)
        {
            MoneyCount.UpgradeGardenLevel4();
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora de la Regadera.
    /// <summary>
    public void UpgradeWateringCan()
    {
        if (_wateringCanUpgrades < _maxWateringCanUpgrades)
        {
            _wateringCanUpgrades += 1;
        }
        if (_wateringCanUpgrades == 1)
        {
            MoneyCount.UpgradeWateringCanLevel1();
        }
        else if (_wateringCanUpgrades == 2)
        {
            MoneyCount.UpgradeWateringCanLevel2();
        }
        else if (_wateringCanUpgrades == 3)
        {
            MoneyCount.UpgradeWateringCanLevel3();
        }
    }

    /// <summary>
    /// Indica si la cosecha ha sido realizada.
    /// </summary>
    /// <returns>Siempre devuelve true, indicando que la cosecha ha sido realizada.</returns>
    public bool Harvested()
    {
        bool _harvested = true; // Variable que indica que la cosecha ha sido realizada
        return _harvested; // Retorna el estado de la cosecha
    }

    public void HideCursor()
    {
        _isCursorVisible = false;
        // Ocultar el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        _isCursorVisible =true;
        // Desbloquear el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Actualiza y obtiene la cantidad de agua disponible en la regadera.
    /// </summary>
    /// <returns>La cantidad actual de agua disponible.</returns>
    public int UpdateWaterAmount()
    {
        _amountWater = WateringCanManager.GetAmountWateringCan(); // Obtiene la cantidad de agua de la regadera
        return _amountWater; // Retorna la cantidad de agua

    }

    /// <summary>
    /// Obtiene el precio de un cultivo basado en su tipo.
    /// </summary>
    /// <param name="_tipe">El tipo de cultivo (ej. "zanahoria", "lechuga").</param>
    /// <returns>El precio del cultivo correspondiente.</returns>
    private int GetCropPrice(string _tipe)
    {
        switch (_tipe) // Evalúa el tipo de cultivo
        {
            case "zanahoria": return 5;
            case "lechuga": return 3;
            case "fresa": return 7;
            case "maíz": return 4;
            default: return 1; // Precio por defecto para otros cultivos
        }
    }

    /// <summary>
    /// Devuelve la última cantidad de agua utilizada.
    /// </summary>
    /// <returns>La cantidad de agua más reciente.</returns>
    public int LastWaterAmount()
    {
        return _amountWater;
    }

    /// <summary>
    /// Devuelve el inventory de cultivos.
    /// </summary>
    /// <returns>Un array que representa el inventory de cultivos.</returns>
    public int[] Inventory()
    {
        return _inventory;
    }

    /// <summary>
    /// Añade ingresos al dinero invertido del jugador.
    /// </summary>
    /// <param name="_investment">La cantidad de dinero a invertir.</param>
    public void AddIncome(float _investment)
    {
        _moneyInvested += Convert.ToInt32(_investment); // Convierte y suma la inversión al dinero invertido
    }

    /// <summary>
    /// Deduce una cantidad específica de dinero del total invertido.
    /// </summary>
    /// <param name="_investment">La cantidad de dinero a deducir.</param>
    public void DeductDepositedMoney(int _investment)
    {
        _moneyInvested -= _investment; // Resta la inversión del dinero invertido
    }

    /// <summary>
    /// Obtiene el total de dinero que ha sido depositado.
    /// </summary>
    /// <returns>El total de dinero invertido.</returns>
    public int GetTotalMoneyDeposited()
    {
        return _moneyInvested;
    }
    
    ///<summary>
    ///Metodo para saber si la cinematica se esta reproduciendo
    /// </summary>
    public bool GetCinematicState()
    {
        return _isInCinematic;
    }

    ///<summary>
    ///Metodo para inicializar GardenManager
    /// </summary>
    public void InitializeGardenManager()
    {
        GardenManager = FindObjectOfType<GardenManager>();

    }

    ///<summary>
    ///Metodo para inicializar WateringCanManager
    /// </summary>
    public void InitializeWateringCanManager()
    {
        WateringCanManager = FindObjectOfType<WateringCanManager>();

    }

    ///<summary>
    ///Metodo para inicializar MoneyManager
    /// </summary>
    public void InitializeMoneyManager()
    {
        MoneyCount = FindObjectOfType<MoneyManager>();

    }

    public void NewGame()
    {
        _wateringCanUpgrades = 0;
        _gardenUpgrades = 0;
        _moneyInvested = 0;
        _amountWater = 6;
        _isInCinematic = true;
        ResetInitialMoney();
        _newGame = false;
        TutorialManager.ResetTutorialManager();
        NotificationManager.ResetNotificationManager();
    }

    
    public void ResetInitialMoney()
    {
        MoneyCount.InitialMoney(InitialAmountMoney);

    }
#endregion

// ---- MÉTODOS PRIVADOS ----

#region Métodos Privados

/// <summary>
/// Dispara la inicialización.
/// </summary>
private void Init()
    {
        // De momento no hay nada que inicializar
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    ///<summary>
    ///Metodo para inicializar las referencias en awake
    /// </summary>
    private void InitializeReferences()
    {

        MoneyCount = FindObjectOfType<MoneyManager>();
        GardenManager = FindObjectOfType<GardenManager>();
        WateringCanManager = FindObjectOfType<WateringCanManager>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        NotificationManager = FindObjectOfType<NotificationManager>();
    }
    public void EndCinematic()
    {
        if (_isInCinematic && Input.GetKeyDown(KeyCode.Return)) // Return = Enter
        {
            _isInCinematic = false;
            //TutorialManager.NextDialogue();
        }
    }
    #endregion
} // class GameManager 
// namespace