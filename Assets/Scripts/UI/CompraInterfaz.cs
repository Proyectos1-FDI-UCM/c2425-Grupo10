//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;

/// <summary>
/// Maneja la interfaz de compra de semillas.
/// Permite seleccionar un cultivo, ver su precio, elegir la cantidad y comprar.
/// </summary>
public class CompraInterfaz : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private GameObject Interactuar;
    [SerializeField] private GameObject Interfaz;

    [SerializeField] private GameObject MaizButton;
    [SerializeField] private GameObject LechugaButton;
    [SerializeField] private GameObject ZanahoriaButton;
    [SerializeField] private GameObject FresasButton;

    [SerializeField] private GameObject ComprarButton;

    [SerializeField] private TextMeshProUGUI DescripcionTexto;
    [SerializeField] private TextMeshProUGUI ContadorTexto;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private MoneyManager moneyCount;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool _colisionando = false;
    private bool _interfazActiva = false;

    private bool _isMaizSelected = false;
    private bool _isLechugaSelected = false;
    private bool _isZanahoriaSelected = false;
    private bool _isFresasSelected = false;

    private int _cantidadAComprar = 1;
    private int _coste;
    private int[] _inventario;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    void Start()
    {
        ResetInterfaz();
        _inventario = GameManager.Instance.Inventory();
    }

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
        if (moneyCount == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoTexto != null)
            {
                moneyCount = ObjetoTexto.GetComponent<MoneyManager>();
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

    public void ButtonMaizPressed()
    {
        _isMaizSelected = true;
        _isLechugaSelected = _isZanahoriaSelected = _isFresasSelected = false;

        _cantidadAComprar = 1;
        _coste = 50;
        DescripcionTexto.text = "1 semilla de maíz = 50 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonLechugaPressed()
    {
        _isLechugaSelected = true;
        _isMaizSelected = _isZanahoriaSelected = _isFresasSelected = false;

        _cantidadAComprar = 1;
        _coste = 15;
        DescripcionTexto.text = "1 semilla de lechuga = 15 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonZanahoriaPressed()
    {
        _isZanahoriaSelected = true;
        _isMaizSelected = _isLechugaSelected = _isFresasSelected = false;

        _cantidadAComprar = 1;
        _coste = 20;
        DescripcionTexto.text = "1 semilla de zanahoria = 20 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void ButtonFresasPressed()
    {
        _isFresasSelected = true;
        _isMaizSelected = _isLechugaSelected = _isZanahoriaSelected = false;

        _cantidadAComprar = 1;
        _coste = 30;
        DescripcionTexto.text = "1 semilla de fresa = 30 RootCoins.";
        ActualizarTextoCantidad();
    }

    public void AumentarCantidad()
    {
        if (_cantidadAComprar >= 30)
        {
            DescripcionTexto.text = "Este es el máximo de semillas que puedes tener en el inventario.";
            return;
        }

        int totalCosto = (_cantidadAComprar + 1) * _coste;

        if (totalCosto > moneyCount.GetMoneyCount())
        {
            DescripcionTexto.text = "No tienes suficiente dinero para más semillas.";
            return;
        }

        _cantidadAComprar++;
        ActualizarTextoCantidad();
    }


    public void DisminuirCantidad()
    {
        if (_cantidadAComprar > 1)
        {
            _cantidadAComprar--;
            ActualizarTextoCantidad();
        }
    }
    public void ButtonComprarPressed()
    {
        int totalPrecio = _cantidadAComprar * _coste;

        if (moneyCount.GetMoneyCount() >= totalPrecio)
        {
            moneyCount.DeductMoney(totalPrecio); // Resta el dinero correctamente

            // Actualiza el inventory según el tipo de semilla seleccionada
            if (_isMaizSelected)
            {
                InventoryManager.ModifyInventory(Items.CornSeed, _cantidadAComprar);
            }
            if (_isLechugaSelected)
            {
                InventoryManager.ModifyInventory(Items.LettuceSeed, _cantidadAComprar);
            }
            if (_isZanahoriaSelected)
            {
                InventoryManager.ModifyInventory(Items.CarrotSeed, _cantidadAComprar);
            }
            if (_isFresasSelected)
            {
                InventoryManager.ModifyInventory(Items.StrawberrySeed, _cantidadAComprar);
            }

            DescripcionTexto.text = "Compra realizada con éxito.";
        }

        else
        {
            DescripcionTexto.text = "No tienes suficiente dinero.";  // Mensaje si no hay dinero suficiente
        }

        ActualizarTextoCantidad();  // Actualiza la UI después de la compra
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private void ActualizarTextoCantidad()
    {
        if (ContadorTexto == null)
        {
            Debug.LogError("ContadorTexto no está asignado en el Inspector.");
            return;
        }

        int totalCosto = _cantidadAComprar * _coste;
        ContadorTexto.text = $"{_cantidadAComprar} = {totalCosto} RC";
    }

    private void EnableInterfaz()
    {
        _interfazActiva = true;
        Interactuar.SetActive(false);
        Interfaz.SetActive(true);
        _isMaizSelected = true;
        ActualizarInterfaz();
        PlayerMovement.DisablePlayerMovement();
    }

    private void DisableInterfaz()
    {
        _interfazActiva = false;
        Interfaz.SetActive(false);
        Interactuar.SetActive(true);
        _colisionando = true;
        PlayerMovement.EnablePlayerMovement();
    }

    private void ActualizarInterfaz()
    {
        ComprarButton.SetActive(true);
        DescripcionTexto.text = "";
        ContadorTexto.text = "";
    }

    private void ResetInterfaz()
    {
        Interactuar.SetActive(false);
        Interfaz.SetActive(false);
        ComprarButton.SetActive(false);
        DescripcionTexto.text = "";
        ContadorTexto.text = "";
    }
    #endregion
}
