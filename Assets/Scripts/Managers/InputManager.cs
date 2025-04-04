//---------------------------------------------------------
// Contiene el componente de InputManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Manager para la gestión del Input. Se encarga de centralizar la gestión
/// de los controles del juego. Es un singleton que sobrevive entre
/// escenas.
/// La configuración de qué controles realizan qué acciones se hace a través
/// del asset llamado InputActionSettings que está en la carpeta Settings.
/// 
/// A modo de ejemplo, este InputManager tiene métodos para consultar
/// el estado de dos acciones:
/// - Move: Permite acceder a un Vector2D llamado MovementVector que representa
/// el estado de la acción Move (que se puede realizar con el joystick izquierdo
/// del gamepad, con los cursores...)
/// - Fire: Se proporcionan 3 métodos (FireIsPressed, FireWasPressedThisFrame
/// y FireWasReleasedThisFrame) para conocer el estado de la acción Fire (que se
/// puede realizar con la tecla Space, con el botón Sur del gamepad...)
///
/// Dependiendo de los botones que se quieran añadir, será necesario ampliar este
/// InputManager. Para ello:
/// - Revisar lo que se hace en Init para crear nuevas acciones
/// - Añadir nuevos métodos para acceder al estado que estemos interesados
///  
/// </summary>
public class InputManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static InputManager _instance;

    /// <summary>
    /// Controlador de las acciones del Input. Es una instancia del asset de 
    /// InputAction que se puede configurar desde el editor y que está en
    /// la carpeta Settings
    /// </summary>
    private InputActionSettings _theController;
    
    /// <summary>
    /// Acción para Fire. Si tenemos más botones tendremos que crear más
    /// acciones como esta (y crear los métodos que necesitemos para
    /// conocer el estado del botón)
    /// </summary>
    private InputAction _fire;
    private InputAction _use;
    private InputAction _select1;
    private InputAction _select2;
    private InputAction _select3;
    private InputAction _select4;
    private InputAction _select5; 
    private InputAction _tab;
    private InputAction _esc;
    private InputAction _exit;
    private InputAction _useWateringCan;
    private InputAction _fillWateringCan;
    private InputAction _useSickle;
    private InputAction _useShovel;
    private InputAction _ShorcutInventory;
    private InputAction _ShorcutSeeds;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// 
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // InputManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Nos destruímos. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer InputManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
    } // Awake

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
    } // OnDestroy

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static InputManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    } // Instance

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>True si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Propiedad para acceder al vector de movimiento.
    /// Según está configurado el InputActionController,
    /// es un vector normalizado 
    /// </summary>
    public Vector2 MovementVector { get; private set; }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool FireIsPressed()
    {
        return _fire.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de use + tool (Usar) está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool UsarIsPressed()
    {
        return _use.IsPressed();
    }
    public bool SalirIsPressed()
    {
        return _exit.IsPressed();
    }
    public bool UseWateringCanIsPressed()
    {
        return _useWateringCan.IsPressed();
    }
    public bool UseSickleIsPressed()
    {
        return _useSickle.IsPressed();
    }
    public bool UseShovelIsPressed()
    {
        return _useSickle.IsPressed();
    }
    public bool FillWateringCanIsPressed()
    {
        return _fillWateringCan.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de Select (1/2/3/4/5) se ha pulsado en este frame
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool Select1WasPressedThisFrame()
    {
        return _select1.WasPressedThisFrame();
    }
    public bool Select2WasPressedThisFrame()
    {
        return _select2.WasPressedThisFrame();
    }
    public bool Select3WasPressedThisFrame()
    {
        return _select3.WasPressedThisFrame();
    }
    public bool Select4WasPressedThisFrame()
    {
        return _select4.WasPressedThisFrame();
    }
    public bool Select5WasPressedThisFrame()
    {
        return _select5.WasPressedThisFrame();
    }
    public bool SalirWasPressedThisFrame()
    {
        return _exit.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Tab está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool TabIsPressed()
    {
        return _tab.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de Esc está pulsado
    /// Devolverá true en todos los frames en los que se mantenga pulsado
    /// <returns>True, si el botón está pulsado</returns>
    /// </summary>
    public bool EscIsPressed()
    {
        return _esc.IsPressed();
    }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool FireWasPressedThisFrame()
    {
        return _fire.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de usar (Usar) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool UsarWasPressedThisFrame()
    {
        return _use.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Use Watering Can (usar regadera) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool UseWateringCanWasPressedThisFrame()
    {
        return _useWateringCan.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Use Sickle (usar hoz) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool UseSickleWasPressedThisFrame()
    {
        return _useSickle.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Use Shovel (usar pala) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool UseShovelWasPressedThisFrame()
    {
        return _useShovel.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Use Watering Can (usar regadera) se ha pulsado en este frame
    /// <returns>Devuelve true, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool FillWateringCanWasPressedThisFrame()
    {
        return _fillWateringCan.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Tab está pulsado
    /// <returns>True, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool TabWasPressedThisFrame()
    {
        return _tab.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de P está pulsado
    /// <returns>True, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool ShorcutSeedWasPressedThisFrame()
    {
        return _ShorcutSeeds.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Tab está pulsado
    /// <returns>True, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool ShorcutInventoryWasPressedThisFrame()
    {
        return _ShorcutInventory.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de Tab está pulsado
    /// <returns>True, si el botón ha sido pulsado en este frame
    /// y false, en otro caso
    /// </returns>
    /// </summary>
    public bool EscWasPressedThisFrame()
    {
        return _esc.WasPressedThisFrame();
    }

    /// <summary>
    /// Método para saber si el botón de disparo (Fire) ha dejado de pulsarse
    /// durante este frame
    /// <returns>Devuelve true, si el botón se ha dejado de pulsar en
    /// este frame; y false, en otro caso.
    /// </returns>
    /// </summary>
    public bool FireWasReleasedThisFrame()
    {
        return _fire.WasReleasedThisFrame();
    }


    public bool SalirWasReleasedThisFrame()
    {
        return _exit.WasReleasedThisFrame();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // Creamos el controlador del input y activamos los controles del jugador
        _theController = new InputActionSettings();
        _theController.Player.Enable();

        // Cacheamos la acción de movimiento
        InputAction movement = _theController.Player.Move;
        // Para el movimiento, actualizamos el vector de movimiento usando
        // el método OnMove
        movement.performed += OnMove;
        movement.canceled += OnMove;

        // Para el disparo solo cacheamos la acción de disparo.
        // El estado lo consultaremos a través de los métodos públicos que 
        // tenemos (FireIsPressed, FireWasPressedThisFrame 
        // y FireWasReleasedThisFrame)
        _fire = _theController.Player.Fire;

        // Usar input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (UsarIsPressed, UsarWasPressedThisFrame )
        _use = _theController.Player.Usar;

        // Salir input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (SalirIsPressed, SalirWasPressedThisFrame, SalirWasReleasedThisFrame )
        _exit = _theController.Player.Salir;

        // Select input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (SelectIsPressed, SelectWasPressedThisFrame )
        _select1 = _theController.Player.Select1;
        _select2 = _theController.Player.Select2;
        _select3 = _theController.Player.Select3;
        _select4 = _theController.Player.Select4;
        _select5 = _theController.Player.Select5;

        // Tab input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (TabIsPressed, TabWasPressedThisFrame )
        _tab = _theController.Player.Tab;

        // Esc input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (EscIsPressed, EscWasPressedThisFrame )
        _esc = _theController.Player.Esc;

        // Use Watering Can input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (UseWateringCanIsPressed, UseWateringCanWasPressedThisFrame )
        _useWateringCan = _theController.Player.UseWateringCan;

        // Fill Watering Can input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (FillWateringCanIsPressed, FillWateringCanWasPressedThisFrame )
        _fillWateringCan = _theController.Player.FillWateringCan;

        // Use Sickle input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (UseSickleIsPressed, UseSickleWasPressedThisFrame )
        _useSickle = _theController.Player.UseSickle;

        // Use Shovel input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (UseShovelIsPressed, UseShovelWasPressedThisFrame )
        _useShovel = _theController.Player.UseShovel;

        // ShortcutInventory input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (UseShovelIsPressed, UseShovelWasPressedThisFrame )
        _ShorcutInventory = _theController.Player.ShorcutInventory;

        // ShortcutInventory input system
        // El estado lo consultaremos a través de los métodos públicos que 
        // (UseShovelIsPressed, UseShovelWasPressedThisFrame )
        _ShorcutSeeds = _theController.Player.ShorcutSeed;
    }

    /// <summary>
    /// Método que es llamado por el controlador de input cuando se producen
    /// eventos de movimiento (relacionados con la acción Move)
    /// </summary>
    /// <param name="context">Información sobre el evento de movimiento</param>
    private void OnMove(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
    }

    #endregion
} // class InputManager 
// namespace