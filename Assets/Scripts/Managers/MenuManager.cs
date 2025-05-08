//---------------------------------------------------------
// Maneja el menú, todos los botones, reinicio de partida...
// Javier Librada
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
///  Maneja el menú, todos los botones, reinicio de partida...
/// </summary>
public class MenuManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Botones de la interfaz
    /// </summary>
    [SerializeField] private Button NewGameButton;

    /// <summary>
    /// Botones de la interfaz
    /// </summary>
    [SerializeField] private Button ContinueButton;

    /// <summary>
    /// Botones de la interfaz
    /// </summary>
    [SerializeField] private Button NOButton;

    /// <summary>
    /// Botones de la interfaz
    /// </summary>
    [SerializeField] private Button YESButton;

    /// <summary>
    /// Botones de la interfaz
    /// </summary>
    [SerializeField] private Button CreditsButton;

    /// <summary>
    /// Botones de la interfaz
    /// </summary>
    [SerializeField] private Button ExitGameButton;

    /// <summary>
    /// Transición
    /// </summary>
    [SerializeField] private GameObject Continue;

    /// <summary>
    /// Transición
    /// </summary>
    [SerializeField] private GameObject LoadingScreen;

    /// <summary>
    /// PopUp de Reiniciar Partida
    /// </summary>
    [SerializeField] private GameObject PopUp;

    /// <summary>
    /// Texto del PopUp de Reiniciar Partida
    /// </summary>
    [SerializeField] private TextMeshProUGUI PopUpText;

    /// <summary>
    /// Script CambiarEscena
    /// </summary>
    [SerializeField] private CambiarEscena CambiarEscena;


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
    /// Bool que se activa si hay un archivo de partida guardado
    /// </summary>
    private bool _newGameSelected = false;

    /// <summary>
    /// Bool que se activa si se sale del juego
    /// </summary>
    private bool _exitGameSelected = false;
    
    /// <summary>
    /// Bool que se encarga de los controles (mando, teclado...)
    /// </summary>
    private bool _updateControllers = true;
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
        YESButton.onClick.AddListener(GameManager.Instance.NewGame);
        LoadingScreen.SetActive(false);
        PopUp.SetActive(false);
        if(GameManager.Instance.GetNewGame() && GameManager.Instance.GetControllerUsing() == true)
        {
            NewGameButton.Select();
        }
        else if (!GameManager.Instance.GetNewGame() && GameManager.Instance.GetControllerUsing() == true)
        {
            ContinueButton.Select();
        }
        GameManager.Instance.InitializeMenuManager();

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(GameManager.Instance.GetNewGame())
        {
            Continue.SetActive(false);
        }
        else
        {
            Continue.SetActive(true);
        }
        if (_updateControllers)
        {
            if (GameManager.Instance.GetNewGame() && GameManager.Instance.GetControllerUsing() == true)
            {
                NewGameButton.Select();
                _updateControllers = false;
            }
            else if (!GameManager.Instance.GetNewGame() && GameManager.Instance.GetControllerUsing() == true)
            {
                ContinueButton.Select();
                _updateControllers = false;
            }
        }
        
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
    /// Método continuar, se carga una nueva pantalla
    /// </summary>
    public void ContinueOrYesPressed()
    {
        LoadingScreen.SetActive(true);
        PopUp.SetActive(false);
    }

    /// <summary>
    /// Método botones de la interfaz 
    /// </summary>
    public void NoButtonPressed()
    {
        if (_newGameSelected && GameManager.Instance.GetControllerUsing() == true)
        {
            NewGameButton.Select();
        }
        else if (_exitGameSelected && GameManager.Instance.GetControllerUsing() == true)
        {
            ExitGameButton.Select();
        }
        PopUp.SetActive(false);
        CreditsButton.interactable = true;
        ContinueButton.interactable = true;
        ExitGameButton.interactable = true;
        NewGameButton.interactable = true;
    }

    /// <summary>
    /// Modificar controladores
    /// </summary>
    public void UpdateControllers()
    {
        _updateControllers = true;
    }

    /// <summary>
    /// Método salir del juego
    /// </summary>
    public void ExitButtonPressed()
    {
        if(GameManager.Instance.GetControllerUsing() == true)
        {
            YESButton.Select();
        }
        PopUp.SetActive(true);
        
        _exitGameSelected |= true;
        PopUpText.text = "¿Estás seguro de que quieres salir?";
        YESButton.onClick.AddListener(CambiarEscena.Exit);
        YESButton.onClick.RemoveListener(GameManager.Instance.NewGame);
        YESButton.onClick.RemoveListener(CambiarEscena.ChangeScene);

        CreditsButton.interactable = false;
        ContinueButton.interactable = false;
        ExitGameButton.interactable = false;
        NewGameButton.interactable = false;

    }

    /// <summary>
    /// Método reinicio
    /// </summary>
    public void NewGameButtonPressed()
    {
        if(!GameManager.Instance.GetNewGame())
        {
            if(GameManager.Instance.GetControllerUsing()) 
            {
                YESButton.Select();
            }
            PopUp.SetActive(true);
            _newGameSelected |= true;
            PopUpText.text = "¿Estás seguro de que quieres crear una nueva partida?\n Tu partida actual se eliminará.";
            YESButton.onClick.AddListener(GameManager.Instance.NewGame);
            YESButton.onClick.AddListener(CambiarEscena.ChangeScene);
            YESButton.onClick.RemoveListener(CambiarEscena.Exit);
            CreditsButton.interactable = false;
            ContinueButton.interactable = false;
            ExitGameButton.interactable = false;
            NewGameButton.interactable = false;
        }
        else
        {
            ContinueOrYesPressed();
            GameManager.Instance.NewGame();
            CambiarEscena.ChangeScene();
        }
        
    }
    #endregion

} // class MenuManager 
// namespace
