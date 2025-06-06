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

    ///<summary>
    ///Ref al uimanager
    /// </summary>
    [SerializeField] private UIManager UIManager;

    ///<summary>
    ///ref al menumanager
    /// </summary>
    [SerializeField] private MenuManager MenuManager;

    ///<summary>
    ///ref al player movement
    /// </summary>
    [SerializeField] private PlayerMovement PlayerMovement;


    [SerializeField] private GameObject CanvasRootWood;
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
    private bool _shouldShowCursor = true;
    private bool _shouldHideCursor = true;


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
    private bool _newGame = false;

    ///<summary>
    ///bool para saber si hay mando
    /// </summary>
    private bool _isGameController = false;

    ///<summary>
    ///int para saber cuantos mandos hay conectados
    /// </summary>
    private int numberOfGamepads = 0;

    ///<summary>
    ///Booleano para saber si el jugador esta en la cinematica inicial
    /// </summary>
    private bool _isInCinematic = false;

    /// <summary>
    /// Int para contar cuantas lechugas has vendido de cada cosa
    /// </summary>
    private int _amountOfLettuceSold = 0;

    /// <summary>
    /// Int para contar cuantas zanahorias has vendido de cada cosa
    /// </summary>
    private int _amountOfCarrotSold = 0;

    /// <summary>
    /// Int para contar cuantas fresas has vendido de cada cosa
    /// </summary>
    private int _amountOfStrawberrySold = 0;

    /// <summary>
    /// Int para contar cuantos maices has vendido de cada cosa
    /// </summary>
    private int _amountOfCornSold = 0;

    /// <summary>
    /// nombre de la escena
    /// </summary>
    private string _scene;

    /// <summary>
    /// booleanos de tipo de escena
    /// </summary>
    private bool _isBuildScene;
    private bool _isShopScene;
    
    /// <summary>
    /// bool para saber si esta activado el menupausa
    /// </summary>
    private bool _isPause;

    /// <summary>
    /// bool para saber si esta activado el dialogo
    /// </summary>
    private bool _isDialogue;

    /// <summary>
    /// bool para saber si esta actiada la enciclopedia
    /// </summary>
    private bool _isLibrary;

    /// <summary>
    /// bool para saber si esta activada la interfaz de ui
    /// </summary>
    private bool _isUI;

    /// <summary>
    /// bools para saber si ya esta la escena final
    /// </summary>
    private bool _isFinal;
    private bool _gameCharged = false;
    private bool _isFinalScene = false;

    // 0: Lechuga, 1: Zanahoria, 2: Fresa, 3: Maíz
    private bool[] _unlockedCrops = new bool[4] { true, false, false, false };

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

    private void Start()
    {
        LoadGame();
    }
    private void Update()
    {
        
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad)
            {
                numberOfGamepads += 1;
                Debug.Log($"Gamepad detectado al inicio: {device.name}");
                _isGameController = true;
                //SetCursorState(false);
                break; // Suponemos que solo quieres ocultar el cursor al detectar el primer gamepad
            }
        }
        if (numberOfGamepads == 0)
        {
            _isGameController = false;
        }

        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change == InputDeviceChange.Added)
            {
                if (device is Gamepad && !_isGameController)
                {
                    Debug.Log($"Gamepad conectado: {device.name}");
                    _isGameController = true;
                    SetCursorState(false);

                }
            }
            else if (change == InputDeviceChange.Removed)
            {
                if (device is Gamepad)
                {
                    Debug.Log($"Gamepad desconectado: {device.name}");
                    _isGameController = false;
                    SetCursorState(true);
                    if (SceneManager.GetActiveScene().name == "Menu")
                    {
                        MenuManager.UpdateControllers();
                    }
                }
            }
        };
        if(InputManager.Instance.CheatsActivatorWasPressedThisFrame())
        {
            Build = !Build;
        }
        if (!Build)
        {
            if (InputManager.Instance.AddMoneyTestWasPressedThisFrame())
            {
                MoneyCount.AddMoney(100000);
            }
            if (InputManager.Instance.NextDialogueWasPressedThisFrame())
            {
                TutorialManager.NextDialogue();
            }
        }
        if ((_isShopScene || _isBuildScene) && PlayerMovement == null)
        {
            PlayerMovement = FindObjectOfType<PlayerMovement>();
        }
        if (SceneManager.GetActiveScene().name == "Escena_Build")
        {
            if (_isFinalScene && PlayerMovement.IsMovementEnable())
            {
                PlayerMovement.DisablePlayerMovement();
                CanvasRootWood.SetActive(false);
            }
        }
        
        if (InputManager.Instance.ExitWasPressedThisFrame()) // Menu Pausa
        {
            if (SceneManager.GetActiveScene().name == "Escena_Build")
            {
                InventoryManager.ModifyPlayerPosition(FindObjectOfType<PlayerMovement>().transform.position);
                SaveTime();
            }
        }
        if (MoneyCount == null)
        {
            InitializeMoneyManager();
        }
        if (GardenManager == null)
        {
            InitializeGardenManager();
        }
        if (WateringCanManager == null)
        {
            InitializeWateringCanManager();
        }
        if (SceneManager.GetActiveScene().name != "Menu" && _isInCinematic)
        {
            Invoke("EndCinematic", 2f);
        }

         FindActualScene();
         _isBuildScene = _scene == "Escena_Build";
         _isShopScene = _scene == "Escena_Compra" || _scene == "Escena_Banco" || _scene == "Escena_Venta" || _scene == "Escena_Mejora";
        
        if(SceneManager.GetActiveScene().name != "Menu")
        {
            _isPause = UIManager.GetPauseMenu();
            _isDialogue = UIManager.GetDialogueActive();
            _isLibrary = UIManager.GetLibraryActive();
            _isUI = UIManager.GetUIActive();
            _isFinal = GetFinalScene();
             _shouldShowCursor = (_isDialogue || _isLibrary || _isPause || _isUI || _isFinal);
             _shouldHideCursor = !_isDialogue && !_isLibrary && !_isPause && !_isUI && !_isFinal;
        }

        if ((_isBuildScene || _isShopScene) && _shouldShowCursor && !_isCursorVisible && !_isGameController)
        {
            SetCursorState(true);
        }
        else if (_isShopScene && _shouldHideCursor && _isCursorVisible)
        {
            SetCursorState(false);
        }
        else if (_isBuildScene && _shouldHideCursor && _isCursorVisible)
        {
            SetCursorState(false);
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
    /// Metodos para obtener la cantidad de mejoras que tiene la Regadera/Huerto.
    /// <summary>
    public int GetWateringCanUpgrades() { return _wateringCanUpgrades; }
    public int GetGardenUpgrades() { return _gardenUpgrades; }

    /// <summary>
    /// devuelve si es una nueva partida
    /// </summary>
    /// <returns></returns>
    public bool GetNewGame() { return _newGame; }

    /// <summary>
    /// devuelve si hay mando activo
    /// </summary>
    /// <returns></returns>
    public bool GetControllerUsing() { return _isGameController; }

    /// <summary>
    /// devuelve si hay cheats o es build
    /// </summary>
    /// <returns></returns>
    public bool GetBuild() { return Build; }

    public void SetTimer(Timer timercomponent)
    {
        timer = timercomponent;
        timer.SetRealTime(realTime);
    }
    /// <summary>
    /// guarda en el gameManager el tiempo actual para volverlo a cargar cuando vuelvas a la escena
    /// </summary>
    public void SaveTime()
    {
        realTime = timer.GetRealTime();
    }
    /// <summary>
    /// activa el bool de escena final
    /// </summary>
    public void FinalScene()
    {
        _isFinalScene = true;
    }
    /// <summary>
    /// devuelve el bool de si es escena final
    /// </summary>
    /// <returns></returns>
    public bool GetFinalScene()
    {
        return _isFinalScene;
    }
    /// <summary>
    /// metodo al que le pasas el time que se ha guardado en el archivo guardar partida para cargar una partida
    /// </summary>
    /// <param name="time"></param>
    public void SaveTime(float time)
    {
        realTime = time;
    }
    /// <summary>
    /// busca cual es el nombre de la escena actual
    /// </summary>
    public void FindActualScene()
    {
        _scene = SceneManager.GetActiveScene().name;
    }

    ///<summary>
    ///metodo para obtener el contador de venta de un cultivo
    /// </summary>
    public int GetAmountSold(string type)
    {
        switch (type)
        {
            case "Lettuce":
                return _amountOfLettuceSold;
            case "Carrot":
                return _amountOfCarrotSold;
            case "Strawberry":
                return _amountOfStrawberrySold;
            case "Corn":
                return _amountOfCornSold;
        }
        return 0;
    }

    ///<summary>
    ///metodo para añadir 1 al contador de venta de un cultivo
    /// </summary>
    public void AddAmountSold(Items item, int amount)
    {
        switch (item)
        {
            case Items.Lettuce:
                 _amountOfLettuceSold += amount;
                break;
            case Items.Carrot:
                _amountOfCarrotSold += amount;
                break;
            case Items.Strawberry:
                 _amountOfStrawberrySold += amount;
                break;
            case Items.Corn:
                 _amountOfCornSold += amount;
                break;
        }
        CheckCropUnlocks();
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
    /// establece el canvas del juego(no final) para luego desactivarlo desde otro metodo
    /// </summary>
    /// <param name="canvas"></param>
    public void SetCanvas(GameObject canvas)
    {
        CanvasRootWood = canvas;
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

    /// <summary>
    /// Metodo para activar/desactivar el raton
    /// </summary>
    /// <param name="visible"></param>
    public void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        _isCursorVisible = visible;
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
    ///Metodo para inicializar UIManager
    /// </summary>
    public void InitializeUIManager()
    {
        UIManager = FindObjectOfType<UIManager>();

    }

    ///<summary>
    ///Metodo para inicializar MenuManager
    /// </summary>
    public void InitializeMenuManager()
    {
        MenuManager = FindObjectOfType<MenuManager>();

    }

    ///<summary>
    ///Metodo para inicializar MoneyManager
    /// </summary>
    public void InitializeMoneyManager()
    {
        MoneyCount = FindObjectOfType<MoneyManager>();

    }
    /// <summary>
    /// reinicia datos para una nueva partida
    /// </summary>
    public void NewGame()
    {
        _wateringCanUpgrades = 0;
        _gardenUpgrades = 0;
        _moneyInvested = 0;
        _amountWater = 6;
        _isInCinematic = true;
        ResetInitialMoney();
        _amountOfCarrotSold = 0;
        _amountOfLettuceSold = 0;
        _amountOfCornSold = 0;
        _amountOfStrawberrySold = 0;
        TutorialManager.ResetTutorialManager();
        NotificationManager.ResetNotificationManager();
        GardenData.ResetGarden();
        InventoryManager.ResetInventory();
        InventoryManager.ModifyPlayerPosition(new Vector3(14.14f, -9.62f, 0));
        SaveTime(0f);
        _newGame = false;
        Build = true;
        _isFinalScene = false;
        _unlockedCrops = new bool[4];
        _unlockedCrops[0] = true; // Lechuga desbloqueada por defecto


        Debug.Log("Partida Reiniciada correctamente");

    }

    /// <summary>
    /// este metodo llama al moneymanager para reiniciar su dinero inicial
    /// </summary>
    public void ResetInitialMoney()
    {
        MoneyCount.InitialMoney(InitialAmountMoney);

    }

    /// <summary>
    /// metodo para guardar el juego en save data
    /// </summary>
    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.SetPlayerPosition(InventoryManager.GetPlayerPosition());
        data.SetInventory(InventoryManager.GetInventory());
        data.SetActivePlants( GardenData.GetActivePlants());
        data.SetGarden(GardenData.GetGarden());
        data.SetTimer(realTime);
        data.SetMoney (MoneyCount.GetMoneyCount());
        data.SetwaterUpdate (_wateringCanUpgrades);
        data.SetGardenUpdate( _gardenUpgrades);

        data.Setnotification(NotificationManager.SaveNotifications());
        data.SettextNotifications(NotificationManager.SaveNotificationText());
        data.Setchecks(NotificationManager.SaveChecks());

        data.SetTutorialPhase(TutorialManager.GetTutorialPhase());
        data.SetTutorialPhaseEscena(TutorialManager.GetTutorialPhaseEscena());
        data.SetTutorialPhaseMejora(TutorialManager.GetTutorialPhaseMejora());
        data.SetTutorialPhaseBanco(TutorialManager.GetTutorialPhaseBanco());
        data.SetUnlockedCrops(_unlockedCrops);

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        Debug.Log("Partida guardada en " + Application.persistentDataPath);
    }

    /// <summary>
    /// metodo apra cargar el juego desde el archivo
    /// </summary>
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            InventoryManager.SetPlayerPosition(data.GetPlayerPosition());
            InventoryManager.SetInventory(data.GetInventory());
            GardenData.SetActivePlants(data.GetActivePlants());
            GardenData.SetGarden(data.GetGarden());
            SaveTime(data.GetTimer());
            MoneyCount.SetMoneyCount(data.GetMoney());
            _wateringCanUpgrades = data.GetwaterUpdate();
            _gardenUpgrades = data.GetGardenUpdate();

            NotificationManager.SetNotifications(data.Getnotification());
            NotificationManager.SetNotificationText(data.GettextNotifications());
            NotificationManager.SetChecks(data.Getchecks());

            TutorialManager.SetTutorialPhase(data.GetTutorialPhase());
            TutorialManager.SetTutorialPhaseEscena(data.GetTutorialPhaseEscena());
            TutorialManager.SetTutorialPhaseMejora(data.GetTutorialPhaseMejora());
            TutorialManager.SetTutorialPhaseBanco(data.GetTutorialPhaseBanco());
            _unlockedCrops = data.GetUnlockedCrops();

            Debug.Log("Partida cargada correctamente");

            _newGame = false;
        }
        else
        {
            Debug.LogWarning("No existe partida guardada.");
            _newGame= true;
        }
    }

    /// <summary>
    /// booleano que comprueba si el cultivo esta desbloqueado
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsCropUnlocked(int index)
    {
        return _unlockedCrops[index];
    }

    /// <summary>
    /// estabalece un cultivo como desbloqueado
    /// </summary>
    /// <param name="crops"></param>
    public void SetUnlockedCrops(bool[] crops)
    {
        _unlockedCrops = crops;
    }

    /// <summary>
    /// metodo para obtener el array de booleanos con cultivos desbloqueados
    /// </summary>
    /// <returns></returns>
    public bool[] GetUnlockedCrops()
    {
        return _unlockedCrops;
    }

    /// <summary>
    /// Actualiza el aviso 
    /// </summary>
    public void UpdateAllPlantsWater()
    {
        if (GardenManager != null)
        {
            GardenManager.UpdateAllPlantsWater();
        }
    }

    /// <summary>
    /// metodo para desactivar los avisos del huerto cuando el tiempo esta rapido
    /// </summary>
    /// <param name="isFastMode"></param>
    public void OnTimeSpeedChanged(bool isFastMode)
    {
        if (GardenManager != null)
        {
            GardenManager.HandleTimeSpeedChange(isFastMode);

            // Si se activa el tiempo rápido, también limpiar todos los avisos visuales
            if (isFastMode)
            {
                GardenManager.ClearAllWarningSprites();
            }
        }
    }


    /// <summary>
    /// este metodo comprueba las condiciones para desbloquear un cultivo, y manda notificacion en caso de ser asi
    /// </summary>
    public void CheckCropUnlocks()
    {
        // Verifica si se desbloquea la zanahoria (al vender 10 lechugas)
        if (!_unlockedCrops[1] && _amountOfLettuceSold >= 10)
        {
            _unlockedCrops[1] = true;

            // Mostrar la notificación en la UI
            UIManager.ShowNotification("¡Has desbloqueado \nla zanahoria!", "NoCounter", 4, "Tool");
            Invoke("HideSeedNotification", 2f);


        }

        // Verifica si se desbloquea la fresa (al vender 30 zanahorias)
        if (!_unlockedCrops[2] && _amountOfCarrotSold >= 30)
        {
            _unlockedCrops[2] = true;

            // Mostrar la notificación en la UI
            UIManager.ShowNotification("¡Has desbloqueado \nla fresa!", "NoCounter", 4, "Tool");
            Invoke("HideSeedNotification", 2f);

        }

        // Verifica si se desbloquea el maíz (al vender 50 fresas)
        if (!_unlockedCrops[3] && _amountOfStrawberrySold >= 50)
        {
            _unlockedCrops[3] = true;

            // Mostrar la notificación en la UI
            UIManager.ShowNotification("¡Has desbloqueado \nel maíz!", "NoCounter", 4, "Tool");
            Invoke("HideSeedNotification", 2f);
        }
    }
    /// <summary>
    /// este metodo desactiva el bool de la cinematca inicial(no existe)
    /// </summary>
    public void EndCinematic()
    {
        _isInCinematic = false;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

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
        MenuManager = FindObjectOfType<MenuManager>();
    }

    /// <summary>
    /// este metodo oculta la notificacion lanzada al desbloquear semilla
    /// </summary>
    private void HideSeedNotification()
    {
        UIManager.HideNotification("Tool");
    }
    #endregion
} // class GameManager 
// namespace