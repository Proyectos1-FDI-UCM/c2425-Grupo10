//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este script efectúa el minijuego de pesca, que consta de presionar un botón cuando lo indique una señal. Al colisionar con el detectable "Pesca", y presionar la "E", comenzará un contador que determinará si se ha ganado o no. Si consigues pescar, se guardará en el inventario y si no, tendrás que alejarte y volver al muelle para volver a pescar. 
/// </summary>
public class FishingManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    ///<summary>
    ///Ref al PlayerMovement
    /// </summary>

    /// <summary>
    /// Icono "Pulsar E"
    /// </summary>
    [SerializeField] private GameObject EIcon;

    /// <summary>
    /// Icono para indicar cuando presionar
    /// </summary>
    [SerializeField] private GameObject Signal;

    /// <summary>
    /// Notificación con instrucciones
    /// </summary>
    [SerializeField] private GameObject Notif;

    /// <summary>
    /// Animator
    /// </summary>
    [SerializeField] private Animator ButtonAnimator;

    ///<summary> 
    /// referencia al animator del player
    /// </summary>
    [SerializeField] private Animator PlayerAnimator;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    

    /// <summary>
    /// Referencia al NotificationManager
    /// </summary>
    private NotificationManager _notificationManager;

    /// <summary>
    /// Referencia al UIManager
    /// </summary>
    private UIManager _uIManager;


    /// <summary>
    /// Referencia al SoundManager
    /// </summary>
    private SoundManager _soundManager;

    /// <summary>
    /// Referencia al Timer
    /// </summary>
    private Timer _timer;

    /// <summary>
    /// Referencia al PlayerMovement
    /// </summary>
    private PlayerMovement _playerMovement;

    /// <summary>
    /// Referencia al Animator del jugador para manejar las animaciones.
    /// </summary>
    private Animator _playerAnimator;


    /// <summary>
    /// Indica si estamos en la posición desde la que se puede realizar la pesca
    /// </summary>
    private bool _fishing;

    /// <summary>
    /// Indica si estamos en la posición desde la que se puede realizar la pesca
    /// </summary>
    private bool _fishingStarted;

    /// <summary>
    /// Timer del minijuego
    /// </summary>
    private float _miniGameTimer;

    /// <summary>
    /// Referencia al tiempo del juego (para la aparición de los peces)
    /// </summary>
    private float _currentTime;

    /// <summary>
    /// Tiempo de juego en segundos
    /// </summary>
    private int _miniGameDuration;

    /// <summary>
    /// Contador de aciertos
    /// </summary>
    private int _checks;


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
        InitializeReferences();
        InitializeTimers();

       

        _fishing = false;
        _fishingStarted = false;


        Notif.SetActive(false);
        EIcon.SetActive(false);
        Signal.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
        if (_fishing)
        {
            // LLamar a notificacion "Presiona E para pescar"
            _uIManager.ShowNotification("Presiona E \npara pescar", "NoCounter", 6, "Fishing");
            
            if (InputManager.Instance.UsarWasPressedThisFrame()) // Empieza el juego
            {
                _fishingStarted = true;
                
                PlayerAnimator.SetBool("IsFishing", true);
                Notif.SetActive(true);
                EIcon.SetActive(true);
            }

            if (_fishingStarted)
            {
                _uIManager.HideNotification("Fishing");
                
                // mgTimer = Tiempo del minijuego sin decimales
                _miniGameTimer += Time.deltaTime;
                int mgTimer = Mathf.FloorToInt(_miniGameTimer);
                //Debug.Log(mgTimer);

                if (_miniGameTimer >= 1) Notif.SetActive(false); // Desactivar notif

                if (mgTimer == 3)
                {
                    Signal.SetActive(true);
                    if (InputManager.Instance.UsarWasPressedThisFrame())
                    {
                        
                        ButtonAnimator.Play("Press");
                        PlayerAnimator.SetTrigger("PressFishing");
                        _checks = 1;
                        Debug.Log("CHECK 1");
                    }
                }
                        
                else if (mgTimer == 4)
                {
                    Signal.SetActive(false);
                    if (_checks == 0) // Fallo
                    {
                        EIcon.SetActive(false);

                        PlayerAnimator.SetBool("IsFishing", false);
                        PlayerAnimator.SetBool("WonFIshing", false);
                        
                        Debug.Log("FALLO 1");
                        ResetMiniGame();
                    }
                        
                }

                if (mgTimer == 6)
                {
                    Signal.SetActive(true);
                    if (InputManager.Instance.UsarWasPressedThisFrame())
                    {
                        ButtonAnimator.Play("Press");
                        PlayerAnimator.SetTrigger("PressFishing");
                        _checks = 2;
                        Debug.Log("CHECK 2");
                    }
                }
                else if (mgTimer == 7)
                {
                    Signal.SetActive(false);
                    if (_checks == 1) // Fallo
                    {
                        EIcon.SetActive(false);

                        PlayerAnimator.SetBool("IsFishing", false);
                        PlayerAnimator.SetBool("WonFIshing", false);
                        
                        Debug.Log("Fallo, vuelve a intentarlo");
                        ResetMiniGame();
                    }
                }

                if (mgTimer == 9)
                {
                    Signal.SetActive(true);
                    if (InputManager.Instance.UsarWasPressedThisFrame())
                    {
                        ButtonAnimator.Play("Press");
                        // PlayerAnimator.SetTrigger("PressFishing");
                        PlayerAnimator.SetBool("WonFIshing", true);
                        PlayerAnimator.SetBool("IsFishing", false);
                        _checks = 3;
                        Debug.Log("CHECK 2");
                    }
                }
                else if (mgTimer == 10)
                {
                    Signal.SetActive(false);
                    if (_checks == 2) // Fallo
                    {
                        EIcon.SetActive(false);
                        Debug.Log("Fallo, vuelve a intentarlo");

                        PlayerAnimator.SetBool("IsFishing", false);
                        PlayerAnimator.SetBool("WonFIshing", false);

                        

                        ResetMiniGame();

                    }
                    else if (_checks == 3) // Ganar 
                    {
                        // LLamar al inventario, guardar
                        //PlayerAnimator.SetBool("IsFishing", false);
                       // PlayerAnimator.SetBool("WonFIshing", true);

                        EIcon.SetActive(false);
                        Debug.Log("Has pescado");


                        AddFishInventory();
                        ResetMiniGame();


                    }
                   

                }
               
            }

           


        }
        else _miniGameTimer = 0;

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void OnCollisionStay2D(Collision2D collision)
    {
        _fishing = true;
    }

    
    private void OnCollisionExit2D(Collision2D collision)
    {
        _fishing = false;
        _fishingStarted = false;
        Notif.SetActive(false);
        EIcon.SetActive(false);
        Signal.SetActive(false);
        //_uIManager.HideNotification("NoCounter");

        ResetMiniGame();
        PlayerAnimator.SetBool("IsFishing", false);
        _uIManager.HideNotification("Fishing");

    }

    private void AddFishInventory()
    {
        InventoryManager.BoolModifyInventory(Items.Fish, 1);
        _uIManager.ActualizeInventory();
    }
    private void ResetMiniGame()
    {
        _checks = 0;
        _miniGameTimer = 0;
        _fishingStarted = false;
        
    }
    private void InitializeReferences()
    {
      
        _notificationManager = FindObjectOfType<NotificationManager>();
        _uIManager = FindObjectOfType<UIManager>();
        _soundManager = FindObjectOfType<SoundManager>();
        _timer = FindObjectOfType<Timer>();
        _playerMovement = FindObjectOfType<PlayerMovement>();

    }

    private void InitializeTimers()
    {
        _currentTime = _timer.GetGameTimeInMinutes();
        _miniGameDuration = 12; // Partida = 12 s
        _miniGameTimer = 0;
    }
    #endregion


} // class FishingManager 
// namespace
