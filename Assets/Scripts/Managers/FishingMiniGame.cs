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

    /// <summary>
    /// Numero de intentos
    /// </summary>
    [SerializeField] private int Tries = 3;

    /// <summary>
    /// Duración de los intervalos entre intentos
    /// </summary>
    [SerializeField] private int Intervals = 4;
    /// <summary>
    /// Icono "Pulsar E"
    /// </summary>
    [SerializeField] private GameObject EIcon;

    /// <summary>
    /// Icono para indicar cuando presionar
    /// </summary>
    [SerializeField] private GameObject Signal;

    /// <summary>
    /// Animator del icono del botón E
    /// </summary>
    [SerializeField] private Animator ButtonAnimator;

    ///<summary> 
    /// referencia al animator del player
    /// </summary>
    [SerializeField] private Animator PlayerAnimator;

    /// <summary>
    /// reproductor de audio de pasos
    /// </summary>
    [SerializeField] private AudioSource AudioSource;

    /// <summary>
    /// reproductor de audio de pasos
    /// </summary>
    [SerializeField] private AudioClip Splash;

    /// <summary>
    /// reproductor de audio de pasos
    /// </summary>
    [SerializeField] private AudioClip Win;

    /// <summary>
    /// reproductor de audio de pasos
    /// </summary>
    [SerializeField] private AudioClip Lose;

    /// <summary>
    /// reproductor de audio de pasos
    /// </summary>
    [SerializeField] private AudioClip Throw;
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
    /// Indica si estamos en la posición desde la que se puede realizar la pesca
    /// </summary>
    private bool _fishing;

    /// <summary>
    /// Indica si se está llevando a cabo el minijuego
    /// </summary>
    private bool _fishingStarted;

    /// <summary>
    /// Timer del minijuego
    /// </summary>
    private float _miniGameTimer;

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
            _uIManager.ShowNotification("Presiona E \npara pescar", "NoCounter", 6, "Fishing");
            
            if (InputManager.Instance.UsarWasPressedThisFrame()) // Empieza el juego
            {
                _fishingStarted = true;
     
                PlayerAnimator.SetBool("IsFishing", true);

                //Sonido
                AudioSource.clip = Throw;
                AudioSource.Play();
            }

            if (_fishingStarted)
            {
                _uIManager.HideNotification("Fishing");
                
                _miniGameTimer += Time.deltaTime;  // mgTimer = Tiempo del minijuego sin decimales
                int mgTimer = Mathf.FloorToInt(_miniGameTimer);

                for (int i = 1; i < Tries + 1; i++)
                {
                    if (mgTimer == i * Intervals) 
                    {
                        EIcon.SetActive(true);

                        ButtonAnimator.Play("Effect");

                        if (InputManager.Instance.UsarWasPressedThisFrame()) // Acierto
                        {
                           ButtonAnimator.SetTrigger("Press");
                            

                            if (AudioSource.clip != null)
                            {
                                AudioSource.clip = Splash;
                                AudioSource.Play();
                            }

                            _checks++;

                            if (i == Tries) // Final de partida (ganar)
                            {
                                // Animaciones
                                PlayerAnimator.SetBool("WonFIshing", true);
                                PlayerAnimator.SetBool("IsFishing", false);
                               

                                //Sonido
                                if (AudioSource.clip != null)
                                {
                                    AudioSource.clip = Win;
                                    AudioSource.Play();
                                }

                                AddFishInventory();
                                ResetMiniGame();
                            }
                            else PlayerAnimator.SetTrigger("PressFishing");
                        }
                        
                    }

                    else if (mgTimer == i * Intervals + 1)
                    {
                      //  EIcon.SetActive(false);

                        if (_checks == i - 1) // Fallo
                        {
                            PlayerAnimator.SetBool("IsFishing", false);
                            PlayerAnimator.SetBool("WonFIshing", false);

                            //Sonido
                            if (AudioSource.clip != null) 
                            {
                                AudioSource.clip = Throw;
                                AudioSource.Play();
                            }

                            ResetMiniGame();
                        }
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
        EIcon.SetActive(false);
        Signal.SetActive(false);

        ResetMiniGame();

        PlayerAnimator.SetBool("IsFishing", false);
        _uIManager.HideNotification("Fishing");
    }

    /// <summary>
    /// Añade 1 pez al inventario
    /// </summary>
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
        Signal.SetActive(false);
        EIcon.SetActive(false);
    }

    /// <summary>
    /// Inicializar referencias a otros scripts
    /// </summary>
    private void InitializeReferences()
    {
        _uIManager = FindObjectOfType<UIManager>();
        _soundManager = FindObjectOfType<SoundManager>();
        _timer = FindObjectOfType<Timer>();
    }

    /// <summary>
    /// Inicializa los timers
    /// </summary>
    private void InitializeTimers()
    {
        _miniGameDuration = Tries * Intervals + 1; // Cada intento se lleva a cabo cada 3s + comprobación del último intento (1s)
        _miniGameTimer = 0;
    }
    #endregion


} // class FishingManager 
// namespace
