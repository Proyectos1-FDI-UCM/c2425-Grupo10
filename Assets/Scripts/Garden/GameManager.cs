//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;


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

    /// <summary>
    /// Numero de mejoras activas de la Regadera.
    /// <summary>
    [SerializeField] int WateringCanUpgrades = 0;

    /// <summary>
    /// Numero de mejoras activas del Huerto.
    /// <summary>
    [SerializeField] int GardenUpgrades = 0;

    /// <summary>
    /// Numero de mejoras activas del Inventory.
    /// <summary>
    [SerializeField] int InventoryUpgrades = 0;

    /// <summary>
    /// Referencia al script que maneja el dinero
    /// <summary>
    [SerializeField] private MoneyManager MoneyCount;
    /// <summary>
    /// Referencia al script que maneja la barra de agua
    /// <summary>
    [SerializeField] private SelectorManager SelectorManager;

    /// <summary>
    /// Referencia al script que maneja la cantidad de agua
    /// <summary>
    [SerializeField] private WateringCanManager WateringCanManager;
    ///<summary>
    /// Ref al GardenManager
    /// </summary>
    [SerializeField] private GardenManager GardenManager;   


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

    /// <summary>
    /// Numero de máximo de mejoras para el Inventory.
    /// <summary>
    private int _maxInventoryUpgrades = 2;

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
    }
    private void Update()
    {
        if (InputManager.Instance.SalirWasPressedThisFrame())
        {
            Application.Quit();
        }
        if (InputManager.Instance.ShorcutInventoryWasPressedThisFrame())
        {
            InventoryManager.ModifyInventory(Items.Corn, 1);
        }
        if (InputManager.Instance.ShorcutSeedWasPressedThisFrame())
        {
            InventoryManager.ModifyInventory(Items.CornSeed, 1);
        }
        if (MoneyCount == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoTexto != null)
            {
                MoneyCount = ObjetoTexto.GetComponent<MoneyManager>();
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
    public int GetWateringCanUpgrades() { return WateringCanUpgrades; }
    public int GetGardenUpgrades() { return GardenUpgrades; }
    public int GetInventoryUpgrades() { return InventoryUpgrades; }

    /// <summary>
    /// Metodo para aumentar +1 la mejora del Huerto.
    /// <summary>
    public void UpgradeGarden()
    {
        if (GardenUpgrades < _maxGardenUpgrades)
        {
            GardenUpgrades += 1;
        }
        if (GardenUpgrades == 1)
        {
            MoneyCount.UpgradeGardenLevel1();
        }
        else if (GardenUpgrades == 2)
        {
            MoneyCount.UpgradeGardenLevel2();
        }
        else if (GardenUpgrades == 3)
        {
            MoneyCount.UpgradeGardenLevel3();
        }
        else if (GardenUpgrades == 4)
        {
            MoneyCount.UpgradeGardenLevel4();
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora del Inventory.
    /// <summary>
    public void MejorarInventario()
    {
        if (InventoryUpgrades < _maxInventoryUpgrades)
        {
            InventoryUpgrades += 1;
        }
        if (InventoryUpgrades == 1)
        {
            MoneyCount.UpgradeInventoryLevel1();
        }
        else if (InventoryUpgrades == 2)
        {
            MoneyCount.UpgradeInventoryLevel2();
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora de la Regadera.
    /// <summary>
    public void UpgradeWateringCan()
    {
        if (WateringCanUpgrades < _maxWateringCanUpgrades)
        {
            WateringCanUpgrades += 1;
        }
        if (WateringCanUpgrades == 1)
        {
            MoneyCount.UpgradeWateringCanLevel1();
        }
        else if (WateringCanUpgrades == 2)
        {
            MoneyCount.UpgradeWateringCanLevel2();
        }
        else if (WateringCanUpgrades == 3)
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
    /// Devuelve el inventario de cultivos.
    /// </summary>
    /// <returns>Un array que representa el inventario de cultivos.</returns>
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
    }
    #endregion
} // class GameManager 
// namespace