//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo: Iria Docampo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class VentaInterfaz : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private GameObject Interactuar;
    [SerializeField] private GameObject Interfaz;

    [SerializeField] private GameObject MaizButton;
    [SerializeField] private GameObject LechugaButton;
    [SerializeField] private GameObject ZanahoriaButton;
    [SerializeField] private GameObject FresasButton;

    [SerializeField] private GameObject VenderButton;

    [SerializeField] private TextMeshProUGUI DescripcionTexto;
    [SerializeField] private TextMeshProUGUI ContadorTexto;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private ContadorDinero ContadorDinero;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool _colisionando = false;
    private bool _interfazActiva = false;

    private bool _isMaizSelected = false;
    private bool _isLechugaSelected = false;
    private bool _isZanahoriaSelected = false;
    private bool _isFresasSelected = false;
    private bool _isVenderPressed = false;

    // Variables para el número de cada cultivo en el inventario
    //private int _maxMaiz;
    //private int _maxLechuga;
    //private int _maxZanahoria;
    //private int _maxFresas;

    private int _cantidadAVender = 1;
    private int[] _inventario;

    // Precio de cada cultivo
    private int _coste; 
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        ResetInterfaz();
        _inventario = GameManager.Instance.Inventario();
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
                ContadorDinero = ObjetoTexto.GetComponent<ContadorDinero>();
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Interactuar.SetActive(true);
            _colisionando = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Interactuar.SetActive(false);
            _colisionando = false;
        }
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Maiz".
    /// </summary>
    public void ButtonMaizPressed()
    {
        _isMaizSelected = true;
        _isLechugaSelected = _isZanahoriaSelected = _isFresasSelected = false;

        _cantidadAVender = 1; // Reinicia la cantidad al cambiar de cultivo
        DescripcionTexto.text = "1 maíz = 90 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonLechugaPressed()
    {
        _isLechugaSelected = true;
        _isMaizSelected = _isZanahoriaSelected = _isFresasSelected = false;

        _cantidadAVender = 1;
        DescripcionTexto.text = "1 lechuga = 20 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonZanahoriaPressed()
    {
        _isZanahoriaSelected = true;
        _isMaizSelected = _isLechugaSelected = _isFresasSelected = false;

        _cantidadAVender = 1;
        DescripcionTexto.text = "1 zanahoria = 65 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonFresasPressed()
    {
        _isFresasSelected = true;
        _isMaizSelected = _isLechugaSelected = _isZanahoriaSelected = false;

        _cantidadAVender = 1;
        DescripcionTexto.text = "1 fresa = 40 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void AumentarCantidad()
    {
        _cantidadAVender++;
        ActualizarTextoCantidad();
    }

    public void DisminuirCantidad()
    {
        if (_cantidadAVender > 1)
        {
            _cantidadAVender--;
            ActualizarTextoCantidad();
        }

    }


    /// <summary>
    /// Metodo para cuando el jugador pulsa el boton "Vender".
    /// </summary>

    public void ButtonVenderPressed()
    {
        _isVenderPressed = true;
        Debug.Log("Vender presionado");

        int[] i = VentaManager.Instance.VenderArray();
        if (_isMaizSelected)
        {
            _coste = 90;
            MostrarDescripcion(i[0], _inventario[0], _coste);
            ContadorDinero.MaizVendido(_cantidadAVender);
            _isMaizSelected = false; // Desmarcar maíz después de vender
        }
        if (_isLechugaSelected)
        {
            _coste = 20;
            MostrarDescripcion(i[1], _inventario[1], _coste);
            ContadorDinero.LechugaVendida(_cantidadAVender);
            _isLechugaSelected = false; // Desmarcar lechuga después de vender
        }
        if (_isZanahoriaSelected)
        {
            _coste = 65;
            MostrarDescripcion(i[2], _inventario[2], _coste);
            ContadorDinero.ZanahoriaVendida(_cantidadAVender);
            _isZanahoriaSelected = false; // Desmarcar zanahoria después de vender
        }
        if (_isFresasSelected)
        {
            _coste = 40;
            MostrarDescripcion(i[3], _inventario[3], _coste);
            ContadorDinero.FresaVendida(_cantidadAVender);
            _isFresasSelected = false; // Desmarcar fresas después de vender
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void ActualizarTextoCantidad1()
    {
        ContadorTexto.text = $"{_cantidadAVender} = {_cantidadAVender * _coste} RC";
    }


    /// <summary>
    /// Metodo para que la descripcion cambie dependiendo del boton seleccionado.
    /// </summary>
    private void MostrarDescripcion(int actual, int max, int coste)
    {
        if (actual >= max)
        {
            DescripcionTexto.text = "Ya no tienes más cultivos para vender.";
            VenderButton.SetActive(false);
        }
        else
        {
            DescripcionTexto.text = "TOTAL: " + actual * coste + "RC";
            VenderButton.SetActive(true);
        }
        ContadorTexto.text = actual + "/" + max;
    }

    /// <summary>
    /// Metodo para activar la interfaz cuando el jugador pulse "E".
    /// </summary>
    private void EnableInterfaz()
    {
        _interfazActiva = true;
        Interactuar.SetActive(false);
        Interfaz.SetActive(true);
        _isMaizSelected = true;
        _isVenderPressed = true;
        ActualizarInterfaz();
        PlayerMovement.enablemovement = false;
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

    private void ActualizarTextoCantidad()
    {
        if (ContadorTexto == null)
        {
            Debug.LogError("ContadorTexto no está asignado en el Inspector.");
            return;
        }

        int precioUnitario = 0;

        if (_isMaizSelected) precioUnitario = 90;
        else if (_isLechugaSelected) precioUnitario = 20;
        else if (_isZanahoriaSelected) precioUnitario = 65;
        else if (_isFresasSelected) precioUnitario = 40;

        int totalGanado = _cantidadAVender * precioUnitario;

        // Mostrar la cantidad junto con el dinero ganado
        ContadorTexto.text = _cantidadAVender + " = " + totalGanado + " RC";
    }


    /// <summary>
    /// Metodo para actualizar la interfaz dependiendo del boton seleccionado.
    /// </summary>
    private void ActualizarInterfaz()
    {
        VenderButton.SetActive(_isVenderPressed);
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
        VenderButton.SetActive(false);
        DescripcionTexto.text = "";
        ContadorTexto.text = "";
    }

    #endregion

} // class ActivarInterfazVenta 
// namespace
