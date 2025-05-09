//---------------------------------------------------------
// Cambiar de una escena a otra
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Método que gestiona los cambios de escena
/// </summary>
public class CambiarEscena : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    
    /// <summary>
    /// Variable con el nombre de la escena
    /// </summary>
    [SerializeField] private string sceneName;
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
    /// Referencia al TutorialManager
    /// </summary>
    private TutorialManager _tutorialManager;

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
    /// Variable con el nombre de la escena actual
    /// </summary>
    private string _currentSceneName;

    /// <summary>
    /// Bool que se activa para usar la transición de las nubes
    /// </summary>
    private bool _shouldUseClouds = false;
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
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Método que gestiona los cambios de escena
    /// </summary>
    public void ChangeScene()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;

        LoadScene();
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            _soundManager.InitialSound();
        }
    }

    /// <summary>
    /// Método que cierra el programa
    /// </summary>
    public void Exit()
    {
        GameManager.Instance.SaveGame(); //Comentado para cargar una nueva partida rapidamente
        Application.Quit();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Escena_Build")
            {
                SavePosition();
            }
            if (SceneManager.GetActiveScene().name == "Escena_Compra")
            {
                if(_tutorialManager.GetTutorialPhase() >= 14)
                {
                    SceneTransition.Instance.ChangeScene(sceneName);
                    Debug.Log("Cambiando a escena: " + sceneName);
                }
                else
                {
                    _uIManager.ShowNotification("Avanza en el tutorial \npara salir", "NoCounter", 1, "NoTutorial");
                }
            }
            if (SceneManager.GetActiveScene().name == "Escena_Venta")
            {
                if (_tutorialManager.GetTutorialPhase() >= 23)
                {
                    SceneTransition.Instance.ChangeScene(sceneName);
                    Debug.Log("Cambiando a escena: " + sceneName);
                }
                else
                {
                    _uIManager.ShowNotification("Avanza en el tutorial \npara salir", "NoCounter", 1, "NoTutorial");
                }
            }
            if (SceneManager.GetActiveScene().name == "Escena_Banco")
            {
                if (_tutorialManager.GetTutorialPhaseBanco() >= 9)
                {
                    SceneTransition.Instance.ChangeScene(sceneName);
                    Debug.Log("Cambiando a escena: " + sceneName);
                }
                else
                {
                    _uIManager.ShowNotification("Avanza en el tutorial \npara salir", "NoCounter", 1, "NoTutorial");
                }
            }
            if (SceneManager.GetActiveScene().name == "Escena_Mejora")
            {
                if (_tutorialManager.GetTutorialPhaseMejora() >= 10)
                {
                    SceneTransition.Instance.ChangeScene(sceneName);
                    Debug.Log("Cambiando a escena: " + sceneName);
                }
                else
                {
                    _uIManager.ShowNotification("Avanza en el tutorial \npara salir", "NoCounter", 1, "NoTutorial");
                }
            }

            if (sceneName == "Escena_Compra")
                {
                    if (_tutorialManager.GetTutorialPhase() >= 9)
                    {
                        SceneTransition.Instance.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        _uIManager.ShowNotification("Avanza en el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                    Vector3 newPosition = InventoryManager.GetPlayerPosition() + new Vector3(0, -1, 0);
                    InventoryManager.SetPlayerPosition(newPosition);
                }
                else if (sceneName == "Escena_Venta")
                {
                    if (_tutorialManager.GetTutorialPhase() >= 16)
                    {
                        SceneTransition.Instance.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        _uIManager.ShowNotification("Avanza en el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                    Vector3 newPosition = InventoryManager.GetPlayerPosition() + new Vector3(0, -1, 0);
                    InventoryManager.SetPlayerPosition(newPosition);
                }
                else if (sceneName == "Escena_Banco")
                {
                    if (_tutorialManager.GetTutorialPhase() >= 26)
                    {
                        SceneTransition.Instance.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        _uIManager.ShowNotification("Avanza en el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                    Vector3 newPosition = InventoryManager.GetPlayerPosition() + new Vector3(0, -1, 0);
                    InventoryManager.SetPlayerPosition(newPosition);
                }
                else if (sceneName == "Escena_Mejora")
                {
                    if (_tutorialManager.GetTutorialPhase() >= 26)
                    {
                        SceneTransition.Instance.ChangeScene(sceneName);
                        Debug.Log("Cambiando a escena: " + sceneName);
                    }
                    else
                    {
                        _uIManager.ShowNotification("Avanza en el tutorial \npara entrar", "NoCounter", 1, "NoTutorial");
                    }
                    Vector3 newPosition = InventoryManager.GetPlayerPosition() + new Vector3(0, -1, 0);
                    InventoryManager.SetPlayerPosition(newPosition);
                }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _uIManager.HideNotification("NoTutorial");
        }
    }

    /// <summary>
    /// Método que carga una nueva escena
    /// </summary>
    private void LoadScene()
    {
        SceneTransition.Instance.ChangeScene(sceneName);
    }

    /// <summary>
    /// Método que guarda la posición del personaje al cambiar de escena
    /// </summary>
    private void SavePosition()
    {
        InventoryManager.ModifyPlayerPosition(FindObjectOfType<PlayerMovement>().transform.position);
        Debug.Log("SavePosition" + InventoryManager.GetPlayerPosition());
        GameManager.Instance.SaveTime();
    }

    /// <summary>
    /// Método que inicializa las referencias
    /// </summary>
    private void InitializeReferences()
    {
        _tutorialManager = FindObjectOfType<TutorialManager>();
        _notificationManager = FindObjectOfType<NotificationManager>();
        _uIManager = FindObjectOfType<UIManager>();
        _soundManager = FindObjectOfType<SoundManager>();
    }
    #endregion

} // class CambiarEscena 
// namespace
