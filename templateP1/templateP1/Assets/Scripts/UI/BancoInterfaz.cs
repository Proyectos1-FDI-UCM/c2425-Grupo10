//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable: Javier Librada Jerez
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class BancoInterfaz : MonoBehaviour
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

    [SerializeField] private GameObject MudarseButton;
    [SerializeField] private GameObject InvertirButton;

    [SerializeField] private Slider CantidadInversion;

    [SerializeField] private TextMeshProUGUI DescripcionTexto;
    [SerializeField] private TextMeshProUGUI CantidadTexto;
    [SerializeField] private TextMeshProUGUI CantidadInvertidaTexto;

    [SerializeField] private MoneyManager ContadorDinero;
    [SerializeField] private PlayerMovement PlayerMovement;





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
    private bool _algoSeleccionado = false; // Controla si hay algo seleccionado para comprar 

  

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
    public void ButtonCasaPlayaPressed()
    {
        // Activamos la interfaz para la Casa Playa
        Interfaz.SetActive(true);

        // Solo activamos el botón de Invertir después de que se presione Casa Playa
        MudarseButton.SetActive(false);  // Desactivamos el botón de mudarse
        InvertirButton.SetActive(true);  // Activamos el botón de invertir

        // Activamos el slider y otros elementos de texto solo cuando se presiona Casa Playa
        CantidadInversion.gameObject.SetActive(true);
        DescripcionTexto.gameObject.SetActive(true);
        CantidadTexto.gameObject.SetActive(true);

        // Aseguramos que algo está seleccionado para invertir
        _algoSeleccionado = true;
    }

    /// <summary>
    /// Método para calcular la inversión total realizada.
    /// </summary>
    private float InversionTotal()
    {
        // Aquí calculamos la inversión total. Este valor puede obtenerse de alguna variable que haga seguimiento de las inversiones.
        return GameManager.Instance.GetInversionTotal(); // Asumiendo que tienes un método que lo haga
    }
    /// <summary>
    /// Método para detectar cuando el jugador presiona el botón de invertir.
    /// </summary>
    public void ButtonInvertirPressed()
    {
        float cantidadInvertida = CantidadInversion.value;

        if (cantidadInvertida > 0 && ContadorDinero.GetContadorDinero() >= cantidadInvertida && InversionTotal() + cantidadInvertida <= 100000)
        {
            // Realizar la inversión
            ContadorDinero.RestarDinero(cantidadInvertida); // Descontamos el dinero invertido
            GameManager.Instance.AgregarInversion(cantidadInvertida); // Guardamos la inversión en el GameManager

            // Actualizamos la interfaz después de invertir
            ActualizarSliderInversion();
            CantidadInversion.value = 0;
            // Verificamos si se alcanzaron los 100.000
            if (InversionTotal() >= 100000)
            {
                MudarseButton.SetActive(true); // Activamos el botón de mudarse
            }
        }
    }

    /// <summary>
    /// Método para activar el botón de mudarse cuando se haya alcanzado la inversión de 100.000 monedas.
    /// </summary>
    public void ButtonMudarsePressed()
    {
        if (InversionTotal() >= 100000)
        {
            // Aquí puedes agregar la lógica de mudanza, como cambiar de escena o activar nuevas funcionalidades
            Debug.Log("Mudanza habilitada");
        }
    }
    public void ActualizarSliderInversion()
    {
        CantidadInversion.maxValue = ContadorDinero.GetContadorDinero();
        MostrarDescripcion(CantidadInversion.value);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados



    /// <summary>
    /// Metodo para que la descripcion cambie dependiendo del boton seleccionado.
    /// </summary>
    private void MostrarDescripcion(float Inversion)
    {
        CantidadTexto.text = "Dinero a Invertir: " + Convert.ToString(Convert.ToInt32(Inversion)) + " RC";
        DescripcionTexto.text = "¡Disfruta de la Casa de Tus Sueños por solo 100.000 RootCoins!";
        CantidadInvertidaTexto.text = "Dinero Invertido: " + GameManager.Instance.GetInversionTotal() + " RC";
    }

    /// <summary>
    /// Metodo para activar la interfaz cuando el jugador pulse "E".
    /// </summary>
    private void EnableInterfaz()
    {
        _interfazActiva = true;
        Interfaz.SetActive(true);
        Interactuar.SetActive(false);
        _colisionando = true;
        PlayerMovement.enablemovement = false;
        MudarseButton.SetActive(false);
        InvertirButton.SetActive(true);
        CantidadInversion.gameObject.SetActive(true);
        DescripcionTexto.gameObject.SetActive(true);
        CantidadTexto.gameObject.SetActive(true);
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
        PlayerMovement.enablemovement = true;

    }

    /// <summary>
    /// Metodo para resetear la interfaz cada vez que la abrimos.
    /// </summary>
    private void ResetInterfaz()
    {
        Interactuar.SetActive(false);
        Interfaz.SetActive(false);
    }
    #endregion

} // class MejoraInterfaz 
// namespace
