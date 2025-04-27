//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
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

    [SerializeField] private Button ContinueButton;

    [SerializeField] private Button NOButton;

    [SerializeField] private Button YESButton;

    [SerializeField] private Button CreditsButton;

    [SerializeField] private Button ExitGameButton;

    [SerializeField] private GameObject Continue;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private CambiarEscena CambiarEscena;

    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private GameObject PopUp;
    [SerializeField] private TextMeshProUGUI PopUpText;



    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _newGameSelected = false;
    private bool _exitGameSelected = false;
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
        GameManager = FindObjectOfType<GameManager>();
        YESButton.onClick.AddListener(GameManager.NewGame);
        LoadingScreen.SetActive(false);
        PopUp.SetActive(false);
        if(GameManager.GetNewGame() && GameManager.GetControllerUsing() == true)
        {
            NewGameButton.Select();
        }
        else if (!GameManager.GetNewGame() && GameManager.GetControllerUsing() == true)
        {
            ContinueButton.Select();
        }
        GameManager.InitializeMenuManager();

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(GameManager.GetNewGame())
        {
            Continue.SetActive(false);
        }
        else
        {
            Continue.SetActive(true);
        }
        if (_updateControllers)
        {
            if (GameManager.GetNewGame() && GameManager.GetControllerUsing() == true)
            {
                NewGameButton.Select();
                _updateControllers = false;
            }
            else if (!GameManager.GetNewGame() && GameManager.GetControllerUsing() == true)
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
    public void ContinueorYesPressed()
    {
        LoadingScreen.SetActive(true);
        PopUp.SetActive(false);
    }
    public void NoButtonPressed()
    {
        if (_newGameSelected && GameManager.GetControllerUsing() == true)
        {
            NewGameButton.Select();
        }
        else if (_exitGameSelected && GameManager.GetControllerUsing() == true)
        {
            ExitGameButton.Select();
        }
        PopUp.SetActive(false);
        CreditsButton.interactable = true;
        ContinueButton.interactable = true;
        ExitGameButton.interactable = true;
        NewGameButton.interactable = true;
    }

    public void UpdateControllers()
    {
        _updateControllers = true;
    }
    public void ExitButtonPressed()
    {
        if(GameManager.GetControllerUsing() == true)
        {
            YESButton.Select();
        }
        PopUp.SetActive(true);
        
        _exitGameSelected |= true;
        PopUpText.text = "¿Estás seguro de que quieres salir?";
        YESButton.onClick.AddListener(CambiarEscena.Exit);
        YESButton.onClick.RemoveListener(GameManager.NewGame);
        YESButton.onClick.RemoveListener(CambiarEscena.ChangeScene);

        CreditsButton.interactable = false;
        ContinueButton.interactable = false;
        ExitGameButton.interactable = false;
        NewGameButton.interactable = false;

    }
    public void NewGameButtonPressed()
    {
        if(!GameManager.GetNewGame())
        {
            if(GameManager.GetControllerUsing()) 
            {
                YESButton.Select();
            }
            PopUp.SetActive(true);
            _newGameSelected |= true;
            PopUpText.text = "¿Estás seguro de que quieres crear una nueva partida?\n Tu partida actual se eliminará.";
            YESButton.onClick.AddListener(GameManager.NewGame);
            YESButton.onClick.AddListener(CambiarEscena.ChangeScene);
            YESButton.onClick.RemoveListener(CambiarEscena.Exit);
            CreditsButton.interactable = false;
            ContinueButton.interactable = false;
            ExitGameButton.interactable = false;
            NewGameButton.interactable = false;
        }
        else
        {
            ContinueorYesPressed();
            GameManager.NewGame();
            CambiarEscena.ChangeScene();
        }
        
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class MenuManager 
// namespace
