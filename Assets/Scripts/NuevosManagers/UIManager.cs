//---------------------------------------------------------
// Script para gestionar la UI del juego en cada escena
// Responsable: Alexia Pérez Santana, Iria Docampo Zotes, Julia Vera Ruiz, Javier Librada Jerez
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
/// La Clase UIManager se encarga de mostrar la UI del juego correcta para cada escena, ya sea la principal o las del mercado
/// Actualiza su información en función de InventoryManager
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector


    [Header("UI de Mercado Comunes")]
    ///<summary>
    ///Pop up del npc cuando te acercas para interactuar con el
    /// </summary>
    [SerializeField] private GameObject NpcMessage;

    /// <summary>
    /// Conjunto de objetos que forman la interfaz
    /// </summary>
    [SerializeField] private GameObject UI;

    /// <summary>
    /// Texto donde pone la descripcion de lo que va a hacer el jugador en la interfaz
    /// </summary>
    [SerializeField] private TextMeshProUGUI DescriptionText;


    [Header("UI de BUILD")]
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


    [Header("UI de Mejora")]
    ///<summary>
    ///Boton para elegir que mejora hacer
    /// </summary>
    [SerializeField] private GameObject UpgradeButton;

    /// <summary>
    /// Boton para elegir que ampliacion hacer
    /// </summary>
    [SerializeField] private GameObject ExtendButton;

    /// <summary>
    /// Boton para mejorar la regadera
    /// </summary>
    [SerializeField] private GameObject WateringCanButton;

    /// <summary>
    /// Boton para ampliar el inventario
    /// </summary>
    [SerializeField] private GameObject InventoryButton;

    /// <summary>
    /// Boton para ampliar el huerto
    /// </summary>
    [SerializeField] private GameObject GardenButton;

    /// <summary>
    /// Boton para comprar la mejora/ampliacion
    /// </summary>
    [SerializeField] private GameObject BuyButton;

    /// <summary>
    /// Contador de mejoras/ampliaciones compradas
    /// </summary>
    [SerializeField] private TextMeshProUGUI AmountOfUpgradesText;
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
    
    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton mejora
    /// </summary>
    private bool _isUpgradeSelected = true;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton ampliar
    /// </summary>
    private bool _isExtendSelected = false;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton regadera
    /// </summary>
    private bool _isWateringCanSelected = false;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton huerto
    /// </summary>
    private bool _isGardenSelected = false;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton inventario
    /// </summary>
    private bool _isInventorySelected = false;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado algun boton
    /// </summary>
    private bool _isSomethingSelected = false;


    /// <summary>
    /// Numero maximo de mejoras de la WateringCan
    /// </summary>
    private int _maxWCUpgrades = 3;

    /// <summary>
    /// Numero maximo de mejoras del Huerto
    /// </summary>
    private int _maxGardenUpgrades = 4;

    /// <summary>
    /// Numero maximo de mejoras del Inventario
    /// </summary>
    private int _maxInventoryUpgrades = 3;
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

        if (SceneManager.GetActiveScene().name == "Escena_Banco")
        {
            if (_isDepositSelected)
            {
                AcceptButton.SetActive(AmountMoneyToDeposit.value > 0);
            }
        } 
    }
    #endregion

    // ---- METODOS PUBLICOS GENERALES ----
    #region Metodos Publicos Generales
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
    #endregion

    // ---- MÉTODOS PRIVADOS GENERALES ----
    #region Métodos Privados Generales
    private void UpdateUI()
    {
        if (SceneManager.GetActiveScene().name == "Escena_Build")
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

        else if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            WateringCanButton.SetActive(_isUpgradeSelected);
            GardenButton.SetActive(_isExtendSelected);
            InventoryButton.SetActive(_isExtendSelected);
            BuyButton.SetActive(_isSomethingSelected);
            DescriptionText.text = "";
            AmountOfUpgradesText.text = "";
        }
    }

    private void EnableInterfaz()
    {
        _uiActive = true;
        UI.SetActive(true);
        NpcMessage.SetActive(false);
        PlayerMovement.DisablePlayerMovement();

        if (SceneManager.GetActiveScene().name == "Escena_Build")
        {
            ButtonDepositPressed();
            MovingButton.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            _isUpgradeSelected = true; // Siempre inicia en "Mejorar"
            _isExtendSelected = false;
            _isSomethingSelected = false;
        }
    }

    private void DisableInterfaz()
    {
        _uiActive = false;
        UI.SetActive(false);
        NpcMessage.SetActive(true);
        _isInNpcArea = true;
        PlayerMovement.EnablePlayerMovement();
    }

    private void ResetInterfaz()
    {
        NpcMessage.SetActive(false);
        UI.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            BuyButton.SetActive(false);
            WateringCanButton.SetActive(false);
            GardenButton.SetActive(false);
            InventoryButton.SetActive(false);
            DescriptionText.text = "";
            AmountOfUpgradesText.text = "";
        }
    }

    #endregion

    // ---- BUILD ----
    #region Build


    // ---- METODOS PUBLICOS (BUILD) ----
    #region Metodos Publicos (Build)
    /// <summary>
    /// Metodo para saber si el inventario esta visible en pantalla
    /// </summary>
    /// <returns></returns>
    public bool GetInventoryVisible()
    {
        return _isInventoryVisible;
    }

    /// <summary>
    /// Actualiza la cantidad de los items del inventario
    /// No comprueba si hay inventario suficiente para mostrar los items porque ya lo comprueba InventoryManager
    /// </summary>
    public void ActualizeInventory()
    {
        TextMeshProUGUI _units;

        // Muestra las semillas
        for (int i = 0; i < (int)Items.Count / 2; i++)
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

    // ---- METODOS PRIVADOS (BUILD) ----
    #region Metodos Privados (Build)
    /// <summary>
    /// Alterna la visibilidad del inventario y mueve la QuickAccessBar con él.
    /// </summary>
    private void ToggleInventory()
    {
        _isInventoryVisible = !_isInventoryVisible;
    }
    #endregion

    #endregion

    // ---- BANCO ----
    #region Banco

    // ---- METODOS PUBLICOS (BANCO) ----
    #region Metodos Publicos (Banco)
    /// <summary>
    /// Metodo para actualizar la ui cuando se pulsa el boton depositar
    /// </summary>
    public void ButtonDepositPressed()
    {
        _isDepositSelected = true;
        _isMovingSelected = false;
        _isBeachHouseSelected = false;
        UpdateUI();
    }

    /// <summary>
    /// Metodo para actualizar la ui cuando se pulsa el boton mudanza
    /// </summary>
    public void ButtonMovingPressed()
    {
        _isDepositSelected = false;
        _isMovingSelected = true;
        _isBeachHouseSelected = false;
        UpdateUI();
    }

    /// <summary>
    /// Metodo para actualizar la ui cuando se pulsa el boton de la casa de la playa
    /// </summary>
    public void ButtonBeachHousePressed()
    {
        _isBeachHouseSelected = true;
        DescriptionText.text = "Has seleccionado la Casa Playa. Pulsa 'Mudarse' para continuar.";
        MoveButton.SetActive(true);
    }

    /// <summary>
    /// Metodo para mudarse a la playa si tienes suficiente dinero en el banco
    /// </summary>
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

    /// <summary>
    /// Metodo para aceptar el dinero que quieres depositar en el banco y añadirlo
    /// </summary>
    public void ButtonAcceptPressed()
    {
        float AmountDeposited = AmountMoneyToDeposit.value;
        if (AmountDeposited > 0 && MoneyManager.GetMoneyCount() >= AmountDeposited)
        {
            MoneyManager.DeductMoney(Mathf.FloorToInt(AmountDeposited));  // O usar Mathf.RoundToInt
            GameManager.Instance.AddIncome(AmountDeposited);
            UpdateSlider();
            AmountMoneyToDeposit.value = 0;
        }
    }

    /// <summary>
    /// Metodo para actualizar los valores del slider
    /// </summary>
    public void UpdateSlider()
    {
        AmountMoneyToDeposit.maxValue = MoneyManager.GetMoneyCount();
        AmountMoneyToDeposit.interactable = AmountMoneyToDeposit.maxValue > 0;
        AmountDepositedText.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
        AmountToDepositText.text = "Dinero a ingresar: " + Convert.ToInt32(AmountMoneyToDeposit.value);

    }
    #endregion

    // ---- METODOS PRIVADOS (BANCO)
    #region Metodos Privados (Banco)

    #endregion

    #endregion

    // ---- MEJORA ----
    #region Mejora

    // ---- METODOS PUBLICOS (MEJORA) ----
    #region Metodos Publicos (Mejora)
    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Ampliar".
    /// </summary>
    public void ButtonExtendPressed()
    {
        _isUpgradeSelected = false;
        _isExtendSelected = true;
        _isSomethingSelected = false;
        UpdateUI();
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Mejorar".
    /// </summary>
    public void ButtonUpgradePressed()
    {
        _isUpgradeSelected = true;
        _isExtendSelected = false;
        _isSomethingSelected = false;
        UpdateUI();
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Regadera".
    /// </summary>
    public void ButtonWateringCanPressed()
    {
        _isWateringCanSelected = true;
        _isGardenSelected = false;
        _isInventorySelected = false;
        _isSomethingSelected = true;

        ShowDescription("Aumenta la capacidad de agua por 1.000 RootCoins.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Huerto".
    /// </summary>
    public void ButtonGardenPressed()
    {
        _isWateringCanSelected = false;
        _isGardenSelected = true;
        _isInventorySelected = false;
        _isSomethingSelected = true;

        ShowDescription("Expande el terreno de cultivos por 5.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Inventory".
    /// </summary>
    public void ButtonInventoryPressed()
    {
        _isWateringCanSelected = false;
        _isGardenSelected = false;
        _isInventorySelected = true;
        _isSomethingSelected = true;

        ShowDescription("Expande la capacidad de almacenamiento por 2.000 RootCoins.", GameManager.Instance.GetInventoryUpgrades(), _maxInventoryUpgrades);
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Comprar".
    /// </summary>
    public void BuyUpgrade()
    {
        if (_isUpgradeSelected && _isSomethingSelected)
        {
            if (_isWateringCanSelected && (MoneyManager.GetMoneyCount() >= 1000) && (GameManager.Instance.GetWateringCanUpgrades() == 0))
            {
                GameManager.Instance.UpgradeWateringCan();
                ShowDescription("Aumenta la capacidad de agua por 5.000 RootCoins.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
            }
            else if (_isWateringCanSelected && (MoneyManager.GetMoneyCount() >= 5000) && (GameManager.Instance.GetWateringCanUpgrades() == 1))
            {
                GameManager.Instance.UpgradeWateringCan();
                ShowDescription("Aumenta la capacidad de agua por 10.000 RootCoins.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
            }
            else if (_isWateringCanSelected && (MoneyManager.GetMoneyCount() >= 10000) && (GameManager.Instance.GetWateringCanUpgrades() == 2))
            {
                GameManager.Instance.UpgradeWateringCan();
                ShowDescription("Aumenta la capacidad de agua.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
            }
        }
        else if (_isExtendSelected && _isSomethingSelected)
        {
            if (_isGardenSelected)
            {
                if ((MoneyManager.GetMoneyCount() >= 5000) && (GameManager.Instance.GetGardenUpgrades() == 0))
                {
                    GameManager.Instance.MejorarHuerto();
                    ShowDescription("Expande el terreno de cultivos por 10.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 10000) && (GameManager.Instance.GetGardenUpgrades() == 1))
                {
                    GameManager.Instance.MejorarHuerto();
                    ShowDescription("Expande el terreno de cultivos por 15.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 15000) && (GameManager.Instance.GetGardenUpgrades() == 2))
                {
                    GameManager.Instance.MejorarHuerto();
                    ShowDescription("Expande el terreno de cultivos por 20.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 20000) && (GameManager.Instance.GetGardenUpgrades() == 3))
                {
                    GameManager.Instance.MejorarHuerto();
                    ShowDescription("Expande el terreno de cultivos por 30.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 30000) && (GameManager.Instance.GetGardenUpgrades() == 4))
                {
                    GameManager.Instance.MejorarHuerto();
                    ShowDescription("Expande el terreno de cultivos.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }

            }
            else if (_isInventorySelected)
            {
                if (_isInventorySelected && (MoneyManager.GetMoneyCount() >= 2000) && (GameManager.Instance.GetInventoryUpgrades() == 0))
                {
                    GameManager.Instance.MejorarInventario();
                    ShowDescription("Expande la capacidad de almacenamiento por 5.000 RootCoins.", GameManager.Instance.GetInventoryUpgrades(), _maxInventoryUpgrades);
                }
                else if (_isInventorySelected && (MoneyManager.GetMoneyCount() >= 5000) && (GameManager.Instance.GetInventoryUpgrades() == 1))
                {
                    GameManager.Instance.MejorarInventario();
                    ShowDescription("Expande la capacidad de almacenamiento.", GameManager.Instance.GetInventoryUpgrades(), _maxInventoryUpgrades);
                }
            }
        }
    }

    #endregion

    // ---- METODOS PRIVADOS (MEJORA) ----
    #region Metodos Privados (Mejora)
    /// <summary>
    /// Metodo para que la descripcion cambie dependiendo del boton seleccionado.
    /// </summary>
    private void ShowDescription(string text, int actualUpgrades, int maxUpgrades)
    {
        if (actualUpgrades >= maxUpgrades)
        {
            DescriptionText.text = "Ya no quedan más mejoras.";
            BuyButton.SetActive(false);
        }
        else
        {
            DescriptionText.text = text;
            BuyButton.SetActive(true);
        }
        AmountOfUpgradesText.text = actualUpgrades + "/" + maxUpgrades;
    }
    #endregion


    #endregion
} // class UIManager
