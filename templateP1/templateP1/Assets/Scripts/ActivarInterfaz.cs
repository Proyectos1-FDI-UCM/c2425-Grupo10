//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
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
public class ActivarInterfaz : MonoBehaviour
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





    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool colisionando = false;
    private bool interfazActiva = false;
    private bool isMejoraSelected = true;  // Siempre inicia en "Mejorar"
    private bool isAmpliarSelected = false;
    private bool algoSeleccionado = false; // Controla si hay algo seleccionado para comprar

    // Contadores de mejoras
    private int mejorasRegadera = 0;
    private int mejorasHuerto = 0;
    private int mejorasInventario = 0;

    // Máximo de mejoras por objeto
    private const int maxMejorasRegadera = 3;
    private const int maxMejorasHuerto = 4;
    private const int maxMejorasInventario = 3;
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
        if (colisionando && InputManager.Instance.UsarIsPressed())
        {
            EnableInterfaz();
        }
        if (interfazActiva && Input.GetKeyDown(KeyCode.Escape))
        {
            DisableInterfaz();
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
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Interactuar.SetActive(true);
            colisionando = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Interactuar.SetActive(false);
            colisionando = false;
        }
    }

    public void ButtonAmpliarPressed()
    {
        isMejoraSelected = false;
        isAmpliarSelected = true;
        algoSeleccionado = false; // Aún no ha elegido Huerto/Inventario
        ActualizarInterfaz();
    }

    public void ButtonMejorarPressed()
    {
        isMejoraSelected = true;
        isAmpliarSelected = false;
        algoSeleccionado = false; // Aún no ha elegido Regadera
        ActualizarInterfaz();
    }

    public void ButtonRegaderaPressed()
    {
        algoSeleccionado = true;
        MostrarDescripcion("Mejora la velocidad de riego.", mejorasRegadera, maxMejorasRegadera);
    }

    public void ButtonHuertoPressed()
    {
        algoSeleccionado = true;
        MostrarDescripcion("Aumenta el espacio para más cultivos.", mejorasHuerto, maxMejorasHuerto);
    }

    public void ButtonInventarioPressed()
    {
        algoSeleccionado = true;
        MostrarDescripcion("Expande la capacidad de almacenamiento.", mejorasInventario, maxMejorasInventario);
    }

    public void ComprarMejora()
    {
        if (isMejoraSelected && algoSeleccionado)
        {
            if (RegaderaButton.activeSelf && mejorasRegadera < maxMejorasRegadera)
            {
                mejorasRegadera++;
                MostrarDescripcion("Mejora la velocidad de riego.", mejorasRegadera, maxMejorasRegadera);
            }
        }
        else if (isAmpliarSelected && algoSeleccionado)
        {
            if (HuertoButton.activeSelf && mejorasHuerto < maxMejorasHuerto)
            {
                mejorasHuerto++;
                MostrarDescripcion("Aumenta el espacio para más cultivos.", mejorasHuerto, maxMejorasHuerto);
            }
            else if (InventarioButton.activeSelf && mejorasInventario < maxMejorasInventario)
            {
                mejorasInventario++;
                MostrarDescripcion("Expande la capacidad de almacenamiento.", mejorasInventario, maxMejorasInventario);
            }
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

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
    private void EnableInterfaz()
    {
        interfazActiva = true;
        Interactuar.SetActive(false);
        Interfaz.SetActive(true);
        isMejoraSelected = true; // Siempre inicia en "Mejorar"
        isAmpliarSelected = false;
        algoSeleccionado = false;
        ActualizarInterfaz();
    }

    private void DisableInterfaz()
    {
        interfazActiva = false;
        Interfaz.SetActive(false);
        Interactuar.SetActive(true);
        colisionando = true;
    }

    private void ActualizarInterfaz()
    {
        // Mostrar los botones según la pestaña activa
        RegaderaButton.SetActive(isMejoraSelected);
        HuertoButton.SetActive(isAmpliarSelected);
        InventarioButton.SetActive(isAmpliarSelected);

        // Ocultar comprar hasta que el jugador seleccione algo
        ComprarButton.SetActive(algoSeleccionado);
        DescripcionTexto.text = "";
        ContadorTexto.text = "";
    }

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

} // class ActivarInterfaz 
// namespace
