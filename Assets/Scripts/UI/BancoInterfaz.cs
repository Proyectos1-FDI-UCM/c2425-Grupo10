using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BancoInterfaz : MonoBehaviour
{
    [SerializeField] private GameObject Interactuar;
    [SerializeField] private GameObject Interfaz;
    [SerializeField] private GameObject IngresarButton;
    [SerializeField] private GameObject MudanzaButton;
    [SerializeField] private GameObject CasaPlayaButton;
    [SerializeField] private GameObject MudarseButton;
    [SerializeField] private GameObject AceptarButton;
    [SerializeField] private Slider CantidadIngresar;
    [SerializeField] private TextMeshProUGUI DescripcionTexto;
    [SerializeField] private TextMeshProUGUI CantidadTexto;
    [SerializeField] private TextMeshProUGUI CantidadIngresadaTexto;
    [SerializeField] private TextMeshProUGUI DineroEnBancoTexto;
    [SerializeField] private MoneyManager MoneyManager;
    [SerializeField] private PlayerMovement PlayerMovement;
    private bool _colisionando = false;
    private bool _interfazActiva = false;
    private bool _isIngresarSelected = true;
    private bool _isMudanzaSelected = false;
    private bool _isCasaPlayaSelected = false;

    void Start()
    {
        ResetInterfaz();
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
        if (MoneyManager == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoTexto != null)
            {
                MoneyManager = ObjetoTexto.GetComponent<MoneyManager>();
            }
        }
        if (_isIngresarSelected)
        {
            AceptarButton.SetActive(CantidadIngresar.value > 0);
        }
    }

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

    public void ButtonIngresarPressed()
    {
        _isIngresarSelected = true;
        _isMudanzaSelected = false;
        _isCasaPlayaSelected = false;
        ActualizarInterfaz();
    }

    public void ButtonMudanzaPressed()
    {
        _isIngresarSelected = false;
        _isMudanzaSelected = true;
        _isCasaPlayaSelected = false;
        ActualizarInterfaz();
    }

    public void ButtonCasaPlayaPressed()
    {
        _isCasaPlayaSelected = true;
        DescripcionTexto.text = "Has seleccionado la Casa Playa. Pulsa 'Mudarse' para continuar.";
        MudarseButton.SetActive(true);
    }

    public void ButtonMudarsePressed()
    {
        if (GameManager.Instance.GetTotalMoneyDeposited() >= 100000)
        {
            Debug.Log("Mudanza realizada con éxito.");
            GameManager.Instance.DeductDepositedMoney(100000);

        }
        else
        {
            Debug.Log("No tienes suficiente dinero para mudarte.");
        }
    }

    public void ButtonAceptarPressed()
    {
        float cantidadIngresada = CantidadIngresar.value;
        if (cantidadIngresada > 0 && MoneyManager.GetMoneyCount() >= cantidadIngresada)
        {
            // Restar el dinero después de convertir la cantidad a int
            MoneyManager.DeductMoney(Mathf.FloorToInt(cantidadIngresada));  // O usar Mathf.RoundToInt
            GameManager.Instance.AgregarIngreso(cantidadIngresada);
            ActualizarSliderInversion();
            CantidadIngresar.value = 0;
        }
    }

    public void ActualizarSliderInversion()
    {
        CantidadIngresar.maxValue = MoneyManager.GetMoneyCount();
        CantidadIngresar.interactable = CantidadIngresar.maxValue > 0;
        CantidadIngresadaTexto.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
        CantidadTexto.text = "Dinero a ingresar: " + Convert.ToInt32(CantidadIngresar.value);

    }
    private void ActualizarInterfaz()
    {
        if (_isIngresarSelected)
        {
            DineroEnBancoTexto.gameObject.SetActive(true);
            DescripcionTexto.text = "En el banco puedes ingresar tu dinero.";
            CantidadIngresar.gameObject.SetActive(true);
            CantidadTexto.gameObject.SetActive(true);
            CasaPlayaButton.SetActive(false);
            CantidadIngresadaTexto.gameObject.SetActive(true);
            CantidadIngresadaTexto.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
            MudarseButton.SetActive(false);
            AceptarButton.SetActive(CantidadIngresar.value > 0);
            ActualizarSliderInversion();
        }
        else if (_isMudanzaSelected)
        {
            DineroEnBancoTexto.gameObject.SetActive(false);
            DescripcionTexto.text = "Selecciona la casa a la que deseas mudarte.";
            CantidadIngresar.gameObject.SetActive(false);
            CantidadTexto.gameObject.SetActive(false);
            CasaPlayaButton.SetActive(true);
            CantidadIngresadaTexto.gameObject.SetActive(false);
            MudarseButton.SetActive(false);
        }
    }

    private void EnableInterfaz()
    {
        _interfazActiva = true;
        Interfaz.SetActive(true);
        Interactuar.SetActive(false);
        PlayerMovement.DisablePlayerMovement();
        ButtonIngresarPressed();
        MudanzaButton.SetActive(true);
    }

    private void DisableInterfaz()
    {
        _interfazActiva = false;
        Interfaz.SetActive(false);
        Interactuar.SetActive(true);
        PlayerMovement.EnablePlayerMovement();
    }

    private void ResetInterfaz()
    {
        Interactuar.SetActive(false);
        Interfaz.SetActive(false);
    }

    
}
