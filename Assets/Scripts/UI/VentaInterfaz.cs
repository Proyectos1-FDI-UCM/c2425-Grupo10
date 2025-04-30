//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo: Iria Docampo y Natalia Nita
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
    [SerializeField] private MoneyManager ContadorDinero;

    [SerializeField] private TextMeshProUGUI TextoCantidadMaiz;
    [SerializeField] private TextMeshProUGUI TextoCantidadLechuga;
    [SerializeField] private TextMeshProUGUI TextoCantidadZanahoria;
    [SerializeField] private TextMeshProUGUI TextoCantidadFresa;

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
        _inventario = GameManager.Instance.Inventory();
        ActualizarCantidadUI(); // Llamamos al iniciar para que muestre los valores correctos
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
    public void ButtonCornPressed()
    {
        _isMaizSelected = true;
        _isLechugaSelected = _isZanahoriaSelected = _isFresasSelected = false;

        _cantidadAVender = 1; // Reinicia la cantidad al cambiar de cultivo
        DescripcionTexto.text = "1 maíz = 90 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonLettucePressed()
    {
        _isLechugaSelected = true;
        _isMaizSelected = _isZanahoriaSelected = _isFresasSelected = false;

        _cantidadAVender = 1;
        DescripcionTexto.text = "1 lechuga = 20 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonCarrotPressed()
    {
        _isZanahoriaSelected = true;
        _isMaizSelected = _isLechugaSelected = _isFresasSelected = false;

        _cantidadAVender = 1;
        DescripcionTexto.text = "1 zanahoria = 65 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonStrawberriesPressed()
    {
        _isFresasSelected = true;
        _isMaizSelected = _isLechugaSelected = _isZanahoriaSelected = false;

        _cantidadAVender = 1;
        DescripcionTexto.text = "1 fresa = 40 RootCoins.";
        ActualizarTextoCantidad();
    }


    public void AumentarCantidad()
    {
        int maxCantidad = 0;

        if (_isMaizSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Corn);
        else if (_isLechugaSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Letuce);
        else if (_isZanahoriaSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Carrot);
        else if (_isFresasSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Strawberry);

        if (_cantidadAVender < maxCantidad)
        {
            _cantidadAVender++;
            ActualizarTextoCantidad();
        }
    }

    public void ActualizarCantidadUI()
    {
        TextoCantidadMaiz.text = "x" + InventoryManager.GetInventoryItem(Items.Corn);
        TextoCantidadLechuga.text = "x" + InventoryManager.GetInventoryItem(Items.Letuce);
        TextoCantidadZanahoria.text = "x" + InventoryManager.GetInventoryItem(Items.Carrot);
        TextoCantidadFresa.text = "x" + InventoryManager.GetInventoryItem(Items.Strawberry);

    }


    /// <summary>
    /// Metodo para cuando el jugador pulsa el boton "Sell".
    /// </summary>
    public void ButtonVenderPressed()
    {
        _isVenderPressed = true;
        Debug.Log("Sell presionado");

        int cantidadDisponible = 0;
        int precioUnitario = 0;

        if (_isMaizSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Corn);
            precioUnitario = 90;
        }
        else if (_isLechugaSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Letuce);
            precioUnitario = 20;
        }
        else if (_isZanahoriaSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Carrot);
            precioUnitario = 65;
        }
        else if (_isFresasSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Strawberry);
            precioUnitario = 40;
        }

        // Si no hay cultivos disponibles, mostrar mensaje y salir
        if (cantidadDisponible <= 0)
        {
            DescripcionTexto.text = "No tienes cultivos de este tipo para vender.";
            return;
        }

        // Verifica que no intente vender más de los que tiene
        if (_cantidadAVender > cantidadDisponible)
        {
            _cantidadAVender = cantidadDisponible;
        }

        // Realizar la venta
        int totalGanado = _cantidadAVender * precioUnitario;
        ContadorDinero.AddMoney(totalGanado);

        // Restar del inventory
        InventoryManager.ModifyInventorySubstract(
            _isMaizSelected ? Items.Corn :
            _isLechugaSelected ? Items.Letuce :
            _isZanahoriaSelected ? Items.Carrot :
            Items.Strawberry, _cantidadAVender
        );

        DescripcionTexto.text = $"Has vendido {_cantidadAVender} por {totalGanado} RC.";
        _cantidadAVender = 1; // Reiniciar cantidad
        ActualizarTextoCantidad();

        ActualizarCantidadUI(); // ⬅️ Llamamos esto para refrescar la UI después de vender
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
            DescripcionTexto.text = "No tienes cultivos para vender.";
            VenderButton.SetActive(true);
        }
        else
        {
            DescripcionTexto.text = "TOTAL: " + actual * coste + "RC";
            VenderButton.SetActive(true);
            ContadorTexto.text = actual + "/" + max;

        }
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
