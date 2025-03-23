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
    [SerializeField] int WateringCanUpdates = 0;

    /// <summary>
    /// Numero de mejoras activas del Huerto.
    /// <summary>
    [SerializeField] int GardenUpgrades = 0;

    /// <summary>
    /// Numero de mejoras activas del Inventory.
    /// <summary>
    [SerializeField] int MejorasInventario = 0;

    /// <summary>
    /// Referencia al script que maneja el dinero
    /// <summary>
    [SerializeField] private MoneyManager ContadorDinero;
    /// <summary>
    /// Referencia al script que maneja la barra de agua
    /// <summary>
    [SerializeField] private SelectorManager SelectorManager;

    /// <summary>
    /// Referencia al script que maneja la cantidad de agua
    /// <summary>
    [SerializeField] private WateringCanManager WateringCanManager;


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
    private int _maxMejorasRegadera = 3;

    /// <summary>
    /// Numero de máximo de mejoras para el Huerto.
    /// <summary>
    private int _maxMejorasHuerto = 4;

    /// <summary>
    /// Numero de máximo de mejoras para el Inventory.
    /// <summary>
    private int _maxMejorasInventario = 3;

    /// <summary>
    /// Numero de máximo de mejoras para el Inventory.
    /// <summary>
    private int _maxVenderMaiz = 3;
    /// <summary>
    /// Cantidad de agua de la regadera.
    /// <summary>
    private int _amountWater = 6;

    /// <summary>
    /// Array Inventory
    /// <summary>
    private int[] _inventario = new int[10];

    /// <summary>
    /// Array Inventory
    /// <summary>
    private int DineroInvertido = 0;
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
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            InventoryManager.ModifyInventory(Items.Corn, 1);
        }
        if (ContadorDinero == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoTexto != null)
            {
                ContadorDinero = ObjetoTexto.GetComponent<MoneyManager>();
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
    public int GetMejorasRegadera() { return WateringCanUpdates; }
    public int GetMejorasHuerto() { return GardenUpgrades; }
    public int GetMejorasInventario() { return MejorasInventario; }

    /// <summary>
    /// Metodo para aumentar +1 la mejora del Huerto.
    /// <summary>
    public void MejorarHuerto()
    {
        if (GardenUpgrades < _maxMejorasHuerto)
        {
            GardenUpgrades++;
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora del Inventory.
    /// <summary>
    public void MejorarInventario()
    {
        if (MejorasInventario < _maxMejorasInventario)
        {
            MejorasInventario++;
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora de la Regadera.
    /// <summary>
    public void UpgradeWateringCan()
    {
        if (WateringCanUpdates < _maxMejorasRegadera)
        {
            WateringCanUpdates += 1;
        }
        if (WateringCanUpdates == 1)
        {
            ContadorDinero.UpgradeWateringCanLevel1();
        }
        else if (WateringCanUpdates == 2)
        {
            ContadorDinero.UpgradeWateringCanLevel2();
        }
        else if (WateringCanUpdates == 3)
        {
            ContadorDinero.UpgradeWateringCanLevel3();
        }
    }

    public bool Cosechado()
    {
        bool _cosechado = true;
        return _cosechado;
    }
    public int UpdateWaterAmount()
    {
        _amountWater = WateringCanManager.GetAmountWateringCan();
        return _amountWater;
        
    }

    private int ObtenerPrecioCultivo(string tipo)
    {
        switch (tipo)
        {
            case "zanahoria": return 5;
            case "lechuga": return 3;
            case "fresa": return 7;
            case "maíz": return 4;
            default: return 1;
        }
    }

    public int LastWaterAmount()
    {
        return _amountWater;
    }


    public int[] Inventory()
    {
        return _inventario;
    }

    public void AgregarIngreso(float inversion)
    {
        DineroInvertido += Convert.ToInt32(inversion);
    }
    public void RestarIngreso(int inversion)
    {
        DineroInvertido -= inversion;
    }
    public int GetDineroIngresadoTotal()
    {
        return DineroInvertido;
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

    
    #endregion
} // class GameManager 
// namespace