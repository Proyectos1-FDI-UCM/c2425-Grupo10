//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable: Javier Librada Jerez
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MejoraInterfaz : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameObject Interactuar;
    [SerializeField] private GameObject Interfaz;
    [SerializeField] private GameObject MejoraButton;
    [SerializeField] private GameObject AmpliarButton;
    [SerializeField] private GameObject RegaderaButton;
   
    [SerializeField] private GameObject InventarioButton; 
    [SerializeField] private GameObject HuertoButton;
    [SerializeField] private GameObject ComprarButton;
    [SerializeField] private TextMeshProUGUI DescripcionTexto;
    [SerializeField] private TextMeshProUGUI ContadorTexto;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private MoneyManager ContadorDinero;





    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool _colisionando = false;
    private bool _interfazActiva = false;
    private bool _isMejoraSelected = true;
    private bool _isAmpliarSelected = false;
    private bool _seleccionRegadera = false;
    private bool _seleccionHuerto = false;
    private bool _seleccionInventario = false;
    private bool _algoSeleccionado = false; // Controla si hay algo seleccionado para comprar 
  

    // Máximo de mejoras por objeto
    private int maxMejorasRegadera = 3;
    private int maxMejorasHuerto = 4;
    private int maxMejorasInventario = 3;
  

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
        ResetInterfaz();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_colisionando && InputManager.Instance.UsarIsPressed())
        {
            EnableInterfaz();
        }
        if (_interfazActiva && InputManager.Instance.SalirIsPressed())
        {
            DisableInterfaz();
        }
        if (ContadorDinero == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoTexto != null)
            {
                ContadorDinero = ObjetoTexto.GetComponent<MoneyManager>();
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
    /// Detectar cuando el jugador esta en el collider.
    /// </summary>
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Interactuar.SetActive(true);
            _colisionando = true;
        }
    }

    /// <summary>
    /// Detectar cuando el jugador sale del collider.
    /// </summary>
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Interactuar.SetActive(false);
            _colisionando = false;
        }
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Ampliar".
    /// </summary>
    public void ButtonAmpliarPressed()
    {
        _isMejoraSelected = false;
        _isAmpliarSelected = true;
        _algoSeleccionado = false; // Aún no se ha presionado "Huerto" o "Inventory".
        ActualizarInterfaz();
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Mejorar".
    /// </summary>
    public void ButtonMejorarPressed()
    {
        _isMejoraSelected = true;
        _isAmpliarSelected = false;
        _algoSeleccionado = false; // Aún no se ha presionado "Regadera".
        ActualizarInterfaz();
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Regadera".
    /// </summary>
    public void ButtonRegaderaPressed()
    {
        _seleccionRegadera = true;
        _seleccionHuerto = false;
        _seleccionInventario = false;
        _algoSeleccionado = true;

        MostrarDescripcion("Aumenta la capacidad de agua por 1.000 RootCoins.", GameManager.Instance.GetMejorasRegadera(), maxMejorasRegadera);
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Huerto".
    /// </summary>
    public void ButtonHuertoPressed()
    {
        _seleccionRegadera = false;
        _seleccionHuerto = true;
        _seleccionInventario = false;
        _algoSeleccionado = true;

        MostrarDescripcion("Expande el terreno de cultivos.", GameManager.Instance.GetMejorasHuerto(), maxMejorasHuerto);
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Inventory".
    /// </summary>
    public void ButtonInventarioPressed()
    {
        _seleccionRegadera = false;
        _seleccionHuerto = false;
        _seleccionInventario = true;
        _algoSeleccionado = true;

        MostrarDescripcion("Expande la capacidad de almacenamiento.", GameManager.Instance.GetMejorasInventario(), maxMejorasInventario);
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Comprar".
    /// </summary>
    public void ComprarMejora()
    {
        if (_isMejoraSelected && _algoSeleccionado)
        {
            if (_seleccionRegadera && (ContadorDinero.GetMoneyCount() >= 1000) && (GameManager.Instance.GetMejorasRegadera() == 0))
            {
                GameManager.Instance.UpgradeWateringCan();
                MostrarDescripcion("Aumenta la capacidad de agua por 5.000 RootCoins.", GameManager.Instance.GetMejorasRegadera(), maxMejorasRegadera);
            }
            else if (_seleccionRegadera && (ContadorDinero.GetMoneyCount() >= 5000) && (GameManager.Instance.GetMejorasRegadera() == 1))
            {
                GameManager.Instance.UpgradeWateringCan();
                MostrarDescripcion("Aumenta la capacidad de agua por 10.000 RootCoins.", GameManager.Instance.GetMejorasRegadera(), maxMejorasRegadera);
            }
            else if (_seleccionRegadera && (ContadorDinero.GetMoneyCount() >= 10000) && (GameManager.Instance.GetMejorasRegadera() == 2))
            {
                GameManager.Instance.UpgradeWateringCan();
                MostrarDescripcion("Aumenta la capacidad de agua.", GameManager.Instance.GetMejorasRegadera(), maxMejorasRegadera);
            }
        }
        else if (_isAmpliarSelected && _algoSeleccionado)
        {
            if (_seleccionHuerto)
            {
                GameManager.Instance.MejorarHuerto();
                MostrarDescripcion("Expande el terreno de cultivos.", GameManager.Instance.GetMejorasHuerto(), maxMejorasHuerto);
            }
            else if (_seleccionInventario)
            {
                GameManager.Instance.MejorarInventario();
                MostrarDescripcion("Expande la capacidad de almacenamiento.", GameManager.Instance.GetMejorasInventario(), maxMejorasInventario);
            }
        }
    }



    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Metodo para que la descripcion cambie dependiendo del boton seleccionado.
    /// </summary>
    private void MostrarDescripcion(string texto, int mejorasActuales, int maxMejoras)
    {
        if (mejorasActuales >= maxMejoras)
        {
            DescripcionTexto.text = "Ya no quedan más mejoras.";
            ComprarButton.SetActive(false);
        }
        else
        {
            DescripcionTexto.text = texto;
            ComprarButton.SetActive(true);
        }
        ContadorTexto.text = mejorasActuales + "/" + maxMejoras;
    }

    /// <summary>
    /// Metodo para activar la interfaz cuando el jugador pulse "E".
    /// </summary>
    private void EnableInterfaz()
    {
        _interfazActiva = true;
        Interactuar.SetActive(false);
        Interfaz.SetActive(true);
        _isMejoraSelected = true; // Siempre inicia en "Mejorar"
        _isAmpliarSelected = false;
        _algoSeleccionado = false;
        ActualizarInterfaz();
        PlayerMovement.DisablePlayerMovement();
    }

    /// <summary>
    /// Metodo para desactivar la interfaz cuando el jugador pulse "Q".
    /// </summary>
    private void DisableInterfaz()
    {
        _interfazActiva = false;
        Interfaz.SetActive(false);
        Interactuar.SetActive(true);
        _colisionando = true;
        PlayerMovement.EnablePlayerMovement();

    }

    /// <summary>
    /// Metodo para actualizar la interfaz dependiendo del boton seleccionado.
    /// </summary>
    private void ActualizarInterfaz()
    {
        // Mostrar los botones según la pestaña activa
        RegaderaButton.SetActive(_isMejoraSelected);
        HuertoButton.SetActive(_isAmpliarSelected);
        InventarioButton.SetActive(_isAmpliarSelected);
       

        // Ocultar comprar hasta que el jugador seleccione algo
        ComprarButton.SetActive(_algoSeleccionado);
        
        DescripcionTexto.text = "";
        ContadorTexto.text = "";
    }

    /// <summary>
    /// Metodo para resetear la interfaz cada vez que la abrimos.
    /// </summary>
    private void ResetInterfaz()
    {
        Interactuar.SetActive(false);
        Interfaz.SetActive(false);
        ComprarButton.SetActive(false);
        RegaderaButton.SetActive(false);
        HuertoButton.SetActive(false);
        InventarioButton.SetActive(false);
        DescripcionTexto.text = "";
        ContadorTexto.text = "";
    }
    #endregion

} // class MejoraInterfaz 
// namespace
