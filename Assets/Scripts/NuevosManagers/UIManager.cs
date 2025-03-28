//---------------------------------------------------------
// Script para gestionar la visibilidad del inventario.
// Responsable: Alexia Pérez Santana, Iria Docampo Zotes, Julia Vera Ruiz
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;

// Añadir aquí el resto de directivas using
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// La Clase InventoryCultivos se encarga de mostrar el inventario al pulsar la tecla TAB 
/// Actualiza su información en función de InventoryManager
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    /// <summary>
    /// Ref al Panel del inventario
    /// </summary>
    [SerializeField] private RectTransform InventoryPanel;

    /// <summary>
    /// Ref a la Barra de acceso rápido
    /// </summary>
    [SerializeField] private RectTransform QuickAccessBar;  

    /// <summary>
    /// Ref a los Iconos del Inventory
    /// </summary>
    [SerializeField] private GameObject InventoryIcons;

    ///<summary>
    ///Ref al PlayerMovement
    /// </summary>
    [SerializeField] private PlayerMovement PlayerMovement;

    /// <summary>
    /// Ref al moneymanager
    /// </summary>
    [SerializeField] private MoneyManager MoneyManager;


    [Header("UI del Banco")]
    ///<summary>
    ///Pop up del npc cuando te acercas para interactuar con el
    /// </summary>
    [SerializeField] private GameObject NpcMessage;

    /// <summary>
    /// Conjunto de objetos que forman la interfaz
    /// </summary>
    [SerializeField] private GameObject BankUI;

    /// <summary>
    /// Boton para ingresar
    /// </summary>
    [SerializeField] private GameObject DepositButton;

    /// <summary>
    /// Boton de mudanza
    /// </summary>
    [SerializeField] private GameObject MovingButton;
    
    /// <summary>
    /// Boton de la Casa en la Playa
    /// </summary>
    [SerializeField] private GameObject BeachHouseButton;

    /// <summary>
    /// Boton para mudarse
    /// </summary>
    [SerializeField] private GameObject MoveButton;

    /// <summary>
    /// Boton para aceptar el dinero a ingresar en el banco
    /// </summary>
    [SerializeField] private GameObject AcceptButton;

    /// <summary>
    /// Slider para que el jugador elija la cantidad de dinero que quiere depositar (su maximo es el dinero del jugador)
    /// </summary>
    [SerializeField] private Slider AmountMoneyToDeposit;

    /// <summary>
    /// Texto donde pone la descripcion de lo que vas a hacer en el banco
    /// </summary>
    [SerializeField] private TextMeshProUGUI DescriptionText;

    /// <summary>
    /// Texto donde sale la cantidad de dinero que va a ingresar el jugador(se actualiza con el slider)
    /// </summary>
    [SerializeField] private TextMeshProUGUI AmountToDepositText;

    /// <summary>
    /// Texto donde sale la cantidad de dinero ingresado en el banco en total
    /// </summary>
    [SerializeField] private TextMeshProUGUI AmountDepositedText;

    /// <summary>
    /// Titulo para el texto anterior (cantidad ingresada)
    /// </summary>
    [SerializeField] private TextMeshProUGUI AmountDepositedTitleText;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Estado del inventario
    /// </summary>
    private bool _isInventoryVisible = false; 

    /// <summary>
    /// Posiciones y velocidades
    /// </summary>
    private float _quickBarBaseY;           // Posición base de la QuickAccessBar (se mantiene siempre visible)
    private float _visibleY = 300f;         // Posición Y del inventario cuando está visible
    private float _hiddenY = -300f;         // Posición Y del inventario cuando está oculto
    private float _quickBarOffset = 100f;   // Espacio entre inventario y QuickAccessBar
    private float _transitionSpeed = 10f;   // Velocidad de animación

    /// <summary>
    ///  Capacidad de cada Slot del inventario
    /// </summary>
    private int _slotsCapacity = 10;

    /// <summary>
    /// Booleano para saber si esta en el area de interaccion con el npc
    /// </summary>
    private bool _isInNpcArea = false;

    /// <summary>
    /// Booleano para saber si esta activa la interfaz
    /// </summary>
    private bool _uiActive = false;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton ingresar
    /// </summary>
    private bool _isDepositSelected = true;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton mudanza
    /// </summary>
    private bool _isMovingSelected = false;

    /// <summary>
    /// Booleano para saber si el juagdor ha pulsado el boton de la casa en la playa
    /// </summary>
    private bool _isBeachHouseSelected = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Escena_Build")
        {
            // Guardamos la posición inicial de la QuickAccessBar para que siempre sea visible
            _quickBarBaseY = QuickAccessBar.anchoredPosition.y;
            // Inicializamos la posición del inventario en oculto
            InventoryPanel.anchoredPosition = new Vector2(InventoryPanel.anchoredPosition.x, _hiddenY);
        }
        
        if (SceneManager.GetActiveScene().name == "Escena_Banco" || SceneManager.GetActiveScene().name == "Escena_Venta" || SceneManager.GetActiveScene().name == "Escena_Mejora" || SceneManager.GetActiveScene().name == "Escena_Compra")
        {
            ResetInterfaz();
        }

    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Escena_Build")
        {
            // La subida/Bajada del inventario se activa con el TAB.
            if (InputManager.Instance.TabWasPressedThisFrame())
            {
                ToggleInventory();
                ActualizeInventory();
            }

            // Define la posición objetivo del inventario
            float targetInventoryY = _isInventoryVisible ? _visibleY : _hiddenY;

            // Define la posición de la QuickAccessBar
            float targetQuickBarY = _isInventoryVisible ? (_visibleY + _quickBarOffset) : _quickBarBaseY;

            // Movimiento suave del inventario
            InventoryPanel.anchoredPosition = Vector2.Lerp
            (
                InventoryPanel.anchoredPosition,
                new Vector2(InventoryPanel.anchoredPosition.x, targetInventoryY),
                Time.deltaTime * _transitionSpeed
            );

            // Movimiento suave de la QuickAccessBar para que suba con el inventario
            QuickAccessBar.anchoredPosition = Vector2.Lerp
            (
                QuickAccessBar.anchoredPosition,
                new Vector2(QuickAccessBar.anchoredPosition.x, targetQuickBarY),
                Time.deltaTime * _transitionSpeed
            );
        }
        if (SceneManager.GetActiveScene().name == "Escena_Banco" || SceneManager.GetActiveScene().name == "Escena_Venta" || SceneManager.GetActiveScene().name == "Escena_Mejora" || SceneManager.GetActiveScene().name == "Escena_Compra")
        {
            if (_isInNpcArea && InputManager.Instance.UsarIsPressed())
            {
                EnableInterfaz();
            }
            if (_uiActive && InputManager.Instance.SalirIsPressed())
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
            if (_isDepositSelected)
            {
                AcceptButton.SetActive(AmountMoneyToDeposit.value > 0);
            }
        }
        
    }
    #endregion

    // ---- BUILD ----
    #region


    // ---- METODOS PUBLICOS (BUILD) ----
    #region
    /// <summary>
    /// Metodo para saber si el inventario esta visible en pantalla
    /// </summary>
    /// <returns></returns>
    public bool GetInventoryVisible()
    {
        return _isInventoryVisible;
    }
    #endregion
    #endregion

    // ---- BANCO ----
    #region

    // ---- METODOS PUBLICOS (BANCO) ----
    #region

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NpcMessage.SetActive(true);
            _isInNpcArea = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NpcMessage.SetActive(false);
            _isInNpcArea = false;
        }
    }
    public void ButtonDepositPressed()
    {
        _isDepositSelected = true;
        _isMovingSelected = false;
        _isBeachHouseSelected = false;
        UpdateUI();
    }

    public void ButtonMovingPressed()
    {
        _isDepositSelected = false;
        _isMovingSelected = true;
        _isBeachHouseSelected = false;
        UpdateUI();
    }

    public void ButtonBeachHousePressed()
    {
        _isBeachHouseSelected = true;
        DescriptionText.text = "Has seleccionado la Casa Playa. Pulsa 'Mudarse' para continuar.";
        MoveButton.SetActive(true);
    }

    public void ButtonMovePressed()
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

    public void ButtonAcceptPressed()
    {
        float AmountDeposited = AmountMoneyToDeposit.value;
        if (AmountDeposited > 0 && MoneyManager.GetMoneyCount() >= AmountDeposited)
        {
            // Restar el dinero después de convertir la cantidad a int
            MoneyManager.DeductMoney(Mathf.FloorToInt(AmountDeposited));  // O usar Mathf.RoundToInt
            GameManager.Instance.AgregarIngreso(AmountDeposited);
            UpdateSlider();
            AmountMoneyToDeposit.value = 0;
        }
    }

    public void UpdateSlider()
    {
        AmountMoneyToDeposit.maxValue = MoneyManager.GetMoneyCount();
        AmountMoneyToDeposit.interactable = AmountMoneyToDeposit.maxValue > 0;
        AmountDepositedText.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
        AmountToDepositText.text = "Dinero a ingresar: " + Convert.ToInt32(AmountMoneyToDeposit.value);

    }
    #endregion

    // ---- METODOS PRIVADOS (BANCO)
    #region
    private void UpdateUI()
    {
        if (_isDepositSelected)
        {
            AmountDepositedTitleText.gameObject.SetActive(true);
            DescriptionText.text = "En el banco puedes ingresar tu dinero.";
            AmountMoneyToDeposit.gameObject.SetActive(true);
            AmountToDepositText.gameObject.SetActive(true);
            BeachHouseButton.SetActive(false);
            AmountDepositedText.gameObject.SetActive(true);
            AmountDepositedText.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
            MoveButton.SetActive(false);
            AcceptButton.SetActive(AmountMoneyToDeposit.value > 0);
            UpdateSlider();
        }
        else if (_isMovingSelected)
        {
            AmountDepositedTitleText.gameObject.SetActive(false);
            DescriptionText.text = "Selecciona la casa a la que deseas mudarte.";
            AmountMoneyToDeposit.gameObject.SetActive(false);
            AmountToDepositText.gameObject.SetActive(false);
            BeachHouseButton.SetActive(true);
            AmountDepositedText.gameObject.SetActive(false);
            MoveButton.SetActive(false);
        }
    }

    private void EnableInterfaz()
    {
        _uiActive = true;
        BankUI.SetActive(true);
        NpcMessage.SetActive(false);
        PlayerMovement.DisablePlayerMovement();
        ButtonDepositPressed();
        MovingButton.SetActive(true);
    }

    private void DisableInterfaz()
    {
        _uiActive = false;
        BankUI.SetActive(false);
        NpcMessage.SetActive(true);
        PlayerMovement.EnablePlayerMovement();
    }

    private void ResetInterfaz()
    {
        NpcMessage.SetActive(false);
        BankUI.SetActive(false);
    }
    #endregion

    #endregion



    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Alterna la visibilidad del inventario y mueve la QuickAccessBar con él.
    /// </summary>
    private void ToggleInventory()
    {
        _isInventoryVisible = !_isInventoryVisible;
    }

    /// <summary>
    /// Actualiza la cantidad de los items del inventario
    /// No comprueba si hay inventario suficiente para mostrar los items porque ya lo comprueba InventoryManager
    /// </summary>
    public void ActualizeInventory()
    {
        TextMeshProUGUI _units;

        // Muestra las semillas
        for (int i = 0; i < (int)Items.Count/2; i++)
        {
            GameObject _crops = InventoryIcons.transform.GetChild(i).gameObject;
            if (InventoryManager.GetInventory(i) != 0)
            {
                _crops.SetActive(true);
                _units = _crops.GetComponentInChildren<TextMeshProUGUI>();
                _units.text = InventoryManager.GetInventory(i) + "x";
            }
            else _crops.SetActive(false);
        }

        // Muestra los cultivos
        for (int i = (int)Items.Count / 2; i < (int)Items.Count; i++) 
        {
            if (InventoryManager.GetInventory(i) != 0)
            {
                int actualSlot = 1; // El Slot actual que está estableciendo
                bool fullSlot = false; // Es true si el Slot es igual que la cantidad máxima por Slot
                
                while (actualSlot < 5 && !fullSlot)
                {
                    GameObject _crops = InventoryIcons.transform.GetChild(i * actualSlot).gameObject;
                    _crops.SetActive(true);
                    _units = _crops.GetComponentInChildren<TextMeshProUGUI>();
                    if (InventoryManager.GetInventory(i) / (actualSlot * _slotsCapacity) != 0)
                    {
                        _units.text = _slotsCapacity + "x";
                    }
                    else if (InventoryManager.GetInventory(i) - ((actualSlot - 1) * _slotsCapacity) != 0)
                    {
                        _units.text = InventoryManager.GetInventory(i) - ((actualSlot - 1) * _slotsCapacity) + "x";
                        fullSlot = true;
                    }
                    else
                    {
                        _crops.SetActive(false);
                        fullSlot = true;
                    }
                    actualSlot++;
                }
            }
            else InventoryIcons.transform.GetChild(i).gameObject.SetActive(false);
        }
        
    }
        #endregion

} // class InventoryUI
