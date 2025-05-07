//---------------------------------------------------------
// Script para gestionar la UI del juego en cada escena
// Responsable: Alexia Pérez Santana, Iria Docampo Zotes, Julia Vera Ruiz, Javier Librada Jerez
// Nombre del juego: Roots of Life
// Curso 2024-25
//---------------------------------------------------------


// Añadir aquí el resto de directivas using
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// La Clase UIManager se encarga de mostrar la UI del juego correcta para cada escena, ya sea la principal o las del mercado
/// Actualiza su información en función de InventoryManager
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector


    [Header("UI Comunes")]

    /// <summary>
    /// Conjunto de objetos que forman la interfaz
    /// </summary>
    [SerializeField] private GameObject UI;

    /// <summary>
    /// Texto donde pone la descripcion de lo que va a hacer el jugador en la interfaz
    /// </summary>
    [SerializeField] private TextMeshProUGUI DescriptionText;

    /// <summary>
    /// Texto para escribir el dinero en pantalla
    /// </summary>
    [SerializeField] private TextMeshProUGUI MoneyText;

    ///<summary>
    ///Ref al PlayerMovement
    /// </summary>
    [SerializeField] private PlayerMovement PlayerMovement;

    ///<summary>
    ///Tamaño del jugador
    /// </summary>
    [SerializeField] private Transform Player;
    [SerializeField] private Vector3 PlayerScale;
    [SerializeField] private Transform PlayerMarker;

    /// <summary>
    /// Ref al moneymanager
    /// </summary>
    [SerializeField] private MoneyManager MoneyManager;



    /// <summary>
    /// Ref al tutorial manager
    /// </summary>
    [SerializeField] private TutorialManager TutorialManager;

    /// <summary>
    /// Ref al notification manager
    /// </summary>
    [SerializeField] private NotificationManager NotificationManager;

    /// <summary>
    /// Boton de menu de pausa
    /// </summary>
    [SerializeField] private Button PauseMenuButton;

    /// <summary>
    /// Menu de pausa
    /// </summary>
    [SerializeField] private GameObject PauseMenu;

    /// <summary>
    /// boton del menu pausa para volver a la escena
    /// </summary>
    [SerializeField] private Button ContinueButton;

    [Header("UI de CONTROLES")]
    ///<summary>
    /// controles
    /// </summary>
    [SerializeField] private TMP_Dropdown ControlsDropdown;

    ///<summary>
    ///Gameobject con todos las partes de la ui
    /// </summary>
    [SerializeField] private GameObject[] Controls;



    [Header("UI de TUTORIAL")]
    ///<summary>
    ///Gameobject con todos las partes de la ui
    /// </summary>
    [SerializeField] private GameObject TutorialUI;

    /// <summary>
    /// Texto donde mostrar todo el dialogo
    /// </summary>
    [SerializeField] private TextMeshProUGUI TutorialText;

    /// <summary>
    /// Texto del boton para salir / continuar
    /// </summary>
    [SerializeField] private TextMeshProUGUI ButtonTutorialText;

    /// <summary>
    /// Button del tutorial
    /// </summary>
    [SerializeField] private Button TutorialButton;

    /// <summary>
    /// Checks de las tareas del tutorial
    /// </summary>
    [SerializeField] private GameObject[] CheckBox = new GameObject[3];

    /// <summary>
    /// GameObject del boton del dialogo
    /// </summary>
    [SerializeField] private GameObject TutorialUIButton;

    [Header("UI de ENCICLOPEDIA")]
    ///<summary>
    /// lugares de rootwood
    /// </summary>
    [SerializeField] private TMP_Dropdown PlacesDropdown;

    ///<summary>
    /// personajes de rootwood
    /// </summary>
    [SerializeField] private TMP_Dropdown CharactersDropdown;

    ///<summary>
    /// cultivos de rootwood
    /// </summary>
    [SerializeField] private TMP_Dropdown PlantsDropdown;

    ///<summary>
    /// herramientas de rootwood
    /// </summary>
    [SerializeField] private TMP_Dropdown ToolsDropdown;

    /// <summary>
    /// UI de enciclopedia
    /// </summary>
    [SerializeField] private GameObject Library;

    /// <summary>
    /// descripcion de enciclopedia
    /// </summary>
    [SerializeField] private TextMeshProUGUI LibraryDescription;

    /// <summary>
    /// boton de enciclopedia
    /// </summary>
    [SerializeField] private GameObject LibraryButton;

    /// <summary>
    /// lugares
    /// </summary>
    [SerializeField] private GameObject[] PlacesRootWood;

    /// <summary>
    /// lugares
    /// </summary>
    [SerializeField] private GameObject[] CharactersRootWood;

    /// <summary>
    /// lugares
    /// </summary>
    [SerializeField] private GameObject[] PlantsRootWood;

    /// <summary>
    /// herramientas
    /// </summary>
    [SerializeField] private GameObject[] ToolsRootWood;

    /// <summary>
    /// Descripciones de cultivos
    /// </summary>
    [SerializeField] private TextMeshProUGUI CarrotDescription;
    [SerializeField] private TextMeshProUGUI StrawberryDescription;
    [SerializeField] private TextMeshProUGUI CornDescription;


    [Header("UI de BUILD")]
    /// <summary>
    /// Ref al Panel del inventory
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

    /// <summary>
    /// Ref al mensaje de estar cansado
    /// </summary>
    [SerializeField] private GameObject TiredMessage;

    /// <summary>
    /// Ref al slider de la energia
    /// </summary>
    [SerializeField] private Slider EnergySlider;

    /// <summary>
    /// GameObject del mapa
    /// </summary>
    [SerializeField] private GameObject Map;

    /// <summary>
    /// Conjunto de notificaciones
    /// </summary>
    [SerializeField] private GameObject NotificationsContainer;

    /// <summary>
    /// Mensaje de regadera
    /// </summary>
    [SerializeField] private GameObject WaterMessage;

    /// <summary>
    /// barra de regadera
    /// </summary>
    [SerializeField] private Slider WaterBar;

    /// <summary>
    /// text agua en regadera
    /// </summary>
    [SerializeField] private TextMeshProUGUI WaterBarText;

    /// <summary>
    /// Numeros quickacccesbar
    /// </summary>
    [SerializeField] private GameObject[] BarNumbersKeyBoard;
    [SerializeField] private GameObject[] BarNumbersController;

    [SerializeField] private GameObject FinalCanvas;
    [SerializeField] private Button FinalButton;


    [Header("Notificacion 0")]
    /// <summary>
    /// Gameobject de la notificacion
    /// </summary>
    [SerializeField] private GameObject Notification0;

    /// <summary>
    /// Texto de la notificacion
    /// </summary>
    [SerializeField] private TextMeshProUGUI Notification0Text;

    [Header("Notificacion 1")]
    /// <summary>
    /// Gameobject de la notificacion
    /// </summary>
    [SerializeField] private GameObject Notification1;

    /// <summary>
    /// Texto de la notificacion
    /// </summary>
    [SerializeField] private TextMeshProUGUI TextNotification1;

    [Header("Notificacion 2")]
    /// <summary>
    /// Gameobject de la notificacion
    /// </summary>
    [SerializeField] private GameObject Notification2;

    /// <summary>
    /// Texto de la notificacion
    /// </summary>
    [SerializeField] private TextMeshProUGUI Notification2Text;

    /// <summary>
    /// Gameobject del contador de la notificacion
    /// </summary>
    [SerializeField] private GameObject Notification2Counter;

    /// <summary>
    /// Texto del contador de la notificacion
    /// </summary>
    [SerializeField] private TextMeshProUGUI Notification2CounterText;
    ///<summary>
    /// boton de la notificacion
    /// </summary>
    [SerializeField] private Button Notification2Button;

    [Header("Notificacion 3")]
    /// <summary>
    /// Gameobject de la notificacion
    /// </summary>
    [SerializeField] private GameObject Notification3;

    /// <summary>
    /// Texto de la notificacion
    /// </summary>
    [SerializeField] private TextMeshProUGUI Notification3Text;

    [Header("Notificacion 4")]
    /// <summary>
    /// Gameobject de la notificacion
    /// </summary>
    [SerializeField] private GameObject Notification4;

    /// <summary>
    /// Texto de la notificacion
    /// </summary>
    [SerializeField] private TextMeshProUGUI Notification4Text;

    [Header("Notificacion 5")]
    /// <summary>
    /// Gameobject de la notificacion
    /// </summary>
    [SerializeField] private GameObject Notification5;

    /// <summary>
    /// Texto de la notificacion
    /// </summary>
    [SerializeField] private TextMeshProUGUI Notification5Text;


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


    [Header("UI de Mercado")]

    ///<summary>
    ///Bloqueadores de compra/venta para el tutorial
    /// </summary>
    [SerializeField] private GameObject[] BlockMarketSeeds;
    [SerializeField] private GameObject[] BlockMarketPlants;
    [SerializeField] private Button LettuceButton;
    [SerializeField] private Button CarrotsButton;
    [SerializeField] private Button StrawberriesButton;
    [SerializeField] private Button CornsButton;


    [SerializeField] private Button LettuceSeedsButton;
    [SerializeField] private Button CarrotSeedsButton;
    [SerializeField] private Button StrawberrySeedsButton;
    [SerializeField] private Button CornSeedsButton;

    [SerializeField] private Button DepositeMoneyButton;
    [SerializeField] private Button ExtendButton;


    [Header("UI de Mejora")]
    ///<summary>
    ///Boton para elegir que mejora hacer
    /// </summary>
    [SerializeField] private GameObject UpgradeButton;

    /// <summary>
    /// Boton para elegir que ampliacion hacer
    /// </summary>
    [SerializeField] private GameObject ExtendButtonObj;

    /// <summary>
    /// Boton para mejorar la regadera
    /// </summary>
    [SerializeField] private GameObject WateringCanButton;

    /// <summary>
    /// Boton para ampliar el huerto
    /// </summary>
    [SerializeField] private GameObject GardenButton;

    /// <summary>
    /// Boton para comprar la mejora/ampliacion
    /// </summary>
    [SerializeField] private GameObject BuyUpgradeObj;
    /// <summary>
    /// Boton para comprar la mejora/ampliacion
    /// </summary>
    [SerializeField] private Button BuyUpgradeButton;

    /// <summary>
    /// Contador de mejoras/ampliaciones compradas
    /// </summary>
    [SerializeField] private TextMeshProUGUI AmountOfUpgradesText;

    [Header("UI de Venta")]

    /// <summary>
    /// Botón maíz
    /// </summary>
    [SerializeField] private GameObject CornButton;

    /// <summary>
    /// Botón lechuga
    /// </summary>
    [SerializeField] private GameObject LettuceButtonObj;

    /// <summary>
    /// Botón zanahoria
    /// </summary>
    [SerializeField] private GameObject CarrotButton;

    /// <summary>
    /// Botón fesas
    /// </summary>
    [SerializeField] private GameObject StrawberryButton;

    /// <summary>
    ///Botón de vender
    /// </summary>
    [SerializeField] private GameObject SellObj;

    /// <summary>
    ///Botón de vender
    /// </summary>
    [SerializeField] private Button SellButton;
    /// <summary>
    /// Botón para sumar 1 cultivo para vender
    /// </summary>
    [SerializeField] private GameObject PlusButton;

    /// <summary>
    /// Botón para restar 1 cultivo para vender
    /// </summary>
    [SerializeField] private GameObject MinusButton;

    /// <summary>
    /// Texto del contador
    /// </summary>
    [SerializeField] private TextMeshProUGUI Counter;


    /// <summary>
    /// Textos de la cantidad de cultivos que tienes en el inventario
    /// </summary>
    [SerializeField] private TextMeshProUGUI CornText;
    [SerializeField] private TextMeshProUGUI LettuceText;
    [SerializeField] private TextMeshProUGUI CarrotText;
    [SerializeField] private TextMeshProUGUI StrawberryText;


    [Header("UI de Compra")]

    ///<summary>
    ///Texto para mostras el precio de las semillas seleccionadas
    /// </summary>
    [SerializeField] private TextMeshProUGUI PriceAmountToBuy;

    /// <summary>
    /// Boton para comprar la mejora/ampliacion
    /// </summary>
    [SerializeField] private GameObject BuySeedsObj;

    /// <summary>
    /// Boton para comprar la mejora/ampliacion
    /// </summary>
    [SerializeField] private Button BuySeedsButton;
    /// <summary>
    /// Boton para comprar la mejora/ampliacion
    /// </summary>
    [SerializeField] private GameObject IncreaseAmountButton;

    /// <summary>
    /// Boton para comprar la mejora/ampliacion
    /// </summary>
    [SerializeField] private GameObject DecreaseAmountButton;

    /// <summary>
    /// Textos de la cantidad de cultivos que tienes en el inventario
    /// </summary>
    [SerializeField] private TextMeshProUGUI SeedCornText;
    [SerializeField] private TextMeshProUGUI SeedLettuceText;
    [SerializeField] private TextMeshProUGUI SeedCarrotText;
    [SerializeField] private TextMeshProUGUI SeedStrawberryText;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    ///<summary>
    ///boool para saber si ya se esta mostrando el canvas con el boton de volver al final de la partida
    /// </summary>
    private bool _isFinalCanvasActive = false;

    /// <summary>
    /// Estado del inventory
    /// </summary>
    private bool _isInventoryVisible = false;

    /// <summary>
    /// Posiciones y velocidades
    /// </summary>
    private float _quickBarBaseY;           // Posición base de la QuickAccessBar (se mantiene siempre visible)
    private float _visibleY = 300f;         // Posición Y del inventory cuando está visible
    private float _hiddenY = -300f;         // Posición Y del inventory cuando está oculto
    private float _quickBarOffset = 100f;   // Espacio entre inventory y QuickAccessBar
    private float _transitionSpeed = 10f;   // Velocidad de animación

    /// <summary>
    ///  Capacidad de cada Slot del inventory
    /// </summary>
    private int _slotsCapacity = 10;

    /// <summary>
    /// Booleano para saber si esta en el area de interaccion con el npc
    /// </summary>
    [SerializeField] private bool _isInNpcArea = false;

    /// <summary>
    /// Booleano para saber si esta activa la interfaz
    /// </summary>
    [SerializeField] private bool _uiActive = false;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton ingresar
    /// </summary>
    private bool _isDepositSelected = false;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton de ingresar
    /// </summary>
    private bool _isWithdrawSelected = false;

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
    /// Booleano para saber si el jugador ha pulsado el boton inventory
    /// </summary>
    private bool _isInventorySelected = false;


    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton de venta
    /// </summary>
    private bool _isSellPressed = false;

    private int _amountBuying = 1;
    private int _cost;
    private int[] _inventory;

    /// <summary>
    /// Booleano para saber si el jugador ha pulsado el boton de maiz
    /// </summary>
    private bool _isCornSelected = false;
    private bool _isLettuceSelected = false;
    private bool _isCarrotSelected = false;
    private bool _isStrawberriesSelected = false;


    /// <summary>
    /// Booleano para saber si el jugador ha pulsado algun boton
    /// </summary>
    private bool _isSomethingSelected = false;

    /// <summary>
    /// nombre de la semilla seleccionada
    /// </summary>
    private string _actualSeedSelected = "";


    /// <summary>
    /// Numero maximo de mejoras de la WateringCan
    /// </summary>
    private int _maxWCUpgrades = 3;

    /// <summary>
    /// Numero maximo de mejoras del Huerto
    /// </summary>
    private int _maxGardenUpgrades = 4;


    /// <summary>
    /// Booleano para saber si el mapa esta visible
    /// </summary>
    private bool _isMapVisible = false;

    /// <summary>
    /// Texto a poner en la descripcion
    /// </summary>
    private string _newDescriptionText;

    /// <summary>
    /// Tutorial notificacion activa
    /// </summary>
    private bool _isTutorialNotification = false;
    ///<summary>
    ///Otra notification activa
    /// </summary>
    private bool _isOtherNotification = false;

    ///<summary>
    ///Energia notification activa
    /// </summary>
    private bool _isEnergyNotification = false;

    ///<summary>
    ///regadera vacia notification activa
    /// </summary>
    private bool _isWcNotification = false;

    ///<summary>
    ///herramienta notification activa
    /// </summary>
    private bool _isToolNotification = false;

    ///<summary>
    ///herramienta notification activa
    /// </summary>
    private bool _isInventoryNotification = false;
    /// <summary>
    /// Cual es la notificacion del tutorial
    /// </summary>
    [SerializeField] private int _tutorialNotificationID = 0;
    /// <summary>
    /// Booleano para saber si el menú de pausa está mostrado
    /// </summary>
    [SerializeField] private bool _isPauseMenuActive = false;

    /// <summary>
    /// Booleano para saber si el dialogo esta activado
    /// </summary>
    [SerializeField] private bool _isDialogueActive = false;

    /// <summary>
    /// bool para saber si la enciclopedia esta activa
    /// </summary>
    private bool _isLibraryActive = false;


    /// <summary>
    /// bool para saber si los controles están activos
    /// </summary>
    private bool _isControlsActive;

    private Vector3 _newHousePosition = new Vector3(70f, -55f, 0f);


    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        InitializeReferences();
        GameManager.Instance.InitializeUIManager();
        

        if (SceneManager.GetActiveScene().name == "Escena_Build")
        {
            // Guardamos la posición inicial de la QuickAccessBar para que siempre sea visible
            _quickBarBaseY = QuickAccessBar.anchoredPosition.y;
            // Inicializamos la posición del inventory en oculto
            InventoryPanel.anchoredPosition = new Vector2(InventoryPanel.anchoredPosition.x, _hiddenY);
            Map.SetActive(false);

        }

        if (SceneManager.GetActiveScene().name == "Escena_Banco" || SceneManager.GetActiveScene().name == "Escena_Venta" || SceneManager.GetActiveScene().name == "Escena_Mejora" || SceneManager.GetActiveScene().name == "Escena_Compra")
        {
            ResetInterfaz();
        }

        MoneyManager.InitializeUIManager();
        NotificationManager.InitializeUIManager();
        ShowMoneyUI();
        Notification2Button.onClick.AddListener(TutorialManager.ActualDialogue);

        NotificationManager.LoadNotification("Tutorial");
        NotificationManager.LoadNotification("NoTutorial");
        PlacesDropdown.onValueChanged.AddListener(delegate { UpdateLibrary(0); });
        CharactersDropdown.onValueChanged.AddListener(delegate { UpdateLibrary(1); });
        PlantsDropdown.onValueChanged.AddListener(delegate { UpdateLibrary(2); });
        UpdateLibrary(0);

        ControlsDropdown.value = 0;
        UpdateControls(0);
    }

    void Update()
    {

        if(InventoryManager.GetBoolInventoryFull())
        {
            ShowNotification("Inventario Lleno", "NoCounter", 5, "Inventory");
            InventoryManager.InventoryNotFull();
        }

         if (InputManager.Instance.SalirIsPressed() && _isLibraryActive == true)
         {
            HideLibrary();
         }
        
        if (InputManager.Instance.ExitWasPressedThisFrame() && _isPauseMenuActive == false)
        {
            ShowPauseMenu();
            _isPauseMenuActive = true;
        }
        else if(InputManager.Instance.ExitWasPressedThisFrame() && _isPauseMenuActive == true && !_isControlsActive)
        {
            HidePauseMenu();
            _isPauseMenuActive = false;
        }
        if (SceneManager.GetActiveScene().name == "Escena_Build")
        {
            // La subida/Bajada del inventario se activa con el TAB.
            if (!_isMapVisible && InputManager.Instance.TabWasPressedThisFrame() && !_isDialogueActive)
            {
                ToggleInventory();
                ActualizeInventory();
            }


            // Define la posición objetivo del inventory
            float targetInventoryY = _isInventoryVisible ? _visibleY : _hiddenY;

            // Define la posición de la QuickAccessBar
            float targetQuickBarY = _isInventoryVisible ? (_visibleY + _quickBarOffset) : _quickBarBaseY;

            // Movimiento suave del inventory
            InventoryPanel.anchoredPosition = Vector2.Lerp
            (
                InventoryPanel.anchoredPosition,
                new Vector2(InventoryPanel.anchoredPosition.x, targetInventoryY),
                Time.deltaTime * _transitionSpeed
            );

            // Movimiento suave de la QuickAccessBar para que suba con el inventory
            QuickAccessBar.anchoredPosition = Vector2.Lerp
            (
                QuickAccessBar.anchoredPosition,
                new Vector2(QuickAccessBar.anchoredPosition.x, targetQuickBarY),
                Time.deltaTime * _transitionSpeed
            );
            if (!_isMapVisible && !_isInventoryVisible && InputManager.Instance.MapWasPressedThisFrame() && !_isDialogueActive && !_isPauseMenuActive && !_isControlsActive && !_isLibraryActive)
            {
                Map.SetActive(true);
                _isMapVisible = true;
                PlayerMovement.DisablePlayerMovement();
                //Player.localScale = new Vector3 (15f, 15f, 1f);

                if (TutorialManager.GetTutorialPhase() == 3) // Verifica si es la fase 3 o la fase que corresponda
                {
                    Check(0);
                    Invoke("NextDialogue", 0.6f);
                    
                }
            }
            else if (_isMapVisible && InputManager.Instance.MapWasPressedThisFrame())
            {
                Map.SetActive(false);
                _isMapVisible = false;
                PlayerMovement.EnablePlayerMovement();
                Player.localScale = new Vector3(7f, 7f, 1f);

            }

            if (GameManager.Instance.GetControllerUsing())
            {
                for (int i = 0; i < BarNumbersKeyBoard.Length; i++)
                {
                    BarNumbersKeyBoard[i].SetActive(false);
                    BarNumbersController[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < BarNumbersKeyBoard.Length; i++)
                {
                    BarNumbersKeyBoard[i].SetActive(true);
                    BarNumbersController[i].SetActive(false);
                }
            }

            if (GameManager.Instance.GetFinalScene() && !_isFinalCanvasActive)
            {
                FinalCanvas.SetActive(true);
                _isFinalCanvasActive = true;
                FinalButton.Select();
            }
        }
        if (TutorialManager.GetTutorialPhase() >= 25)
        {
            LibraryButton.SetActive(true);
        }
        else
        {
            LibraryButton.SetActive(false);
        }
        if (_isDepositSelected)
        {
            AcceptButton.SetActive(AmountMoneyToDeposit.value > 0);
        }
        if (_isInNpcArea)
        {
            ShowNotification("Presiona E para\nhablar", "NoCounter", 1, "NoTutorial");
            if (InputManager.Instance.UsarIsPressed())
            {
                if (TutorialManager.GetTutorialPhase() == 9) // Tutorial
                {
                    TutorialManager.ModifyNotification("Mi primera \ncompra", "[ ] Compra una\r\n    semilla de\r\n    lechuga");
                    TutorialManager.NextDialogue();
                }
                if (TutorialManager.GetTutorialPhase() == 20)
                {
                    TutorialManager.ModifyNotification("Vende tu \nprimera cosecha", "[ ] Vende\n una lechuga");
                    TutorialManager.NextDialogue();
                }
                if (TutorialManager.GetTutorialPhaseMejora() == 3)
                {
                    //TutorialManager.ModifyNotification("Vende tu \nprimera cosecha", "[ ] Vende\n una lechuga");
                    TutorialManager.NextDialogue();
                }
                if (TutorialManager.GetTutorialPhaseBanco() == 3)
                {
                    //TutorialManager.ModifyNotification("Vende tu \nprimera cosecha", "[ ] Vende\n una lechuga");
                    TutorialManager.NextDialogue();
                }
                if (SceneManager.GetActiveScene().name == "Escena_Mejora" || SceneManager.GetActiveScene().name == "Escena_Venta")
                {
                    Check(0);
                    //if (TutorialManager.GetTutorialPhase() == 20 ) NextDialogue();
                }
                EnableInterfaz();
            }
            
        }
        if (_uiActive && InputManager.Instance.SalirIsPressed())
        {
            DisableInterfaz();
        }
        
        for (int i = 0; i < CheckBox.Length; i++) 
        {
            if (CheckBox[i].activeSelf)
            {
                NotificationManager.EditNotification(i);
            }
        }
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            if(_isControlsActive && InputManager.Instance.ExitWasPressedThisFrame())
            {
                HideControls();
            }
        }
    }


    #endregion

    // ---- METODOS PUBLICOS GENERALES ----
    #region Metodos Publicos Generales

    /// <summary>
    /// Metodo para actualizar la cantidad de dinero en pantalla
    /// </summary>
    public void ShowMoneyUI()
    {
        MoneyText.text = "x" + Convert.ToString(MoneyManager.GetMoneyCount());
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInNpcArea = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Escena_Banco" || SceneManager.GetActiveScene().name == "Escena_Compra" || SceneManager.GetActiveScene().name == "Escena_Mejora" || SceneManager.GetActiveScene().name == "Escena_Venta")
            {
                _isInNpcArea = false;
                HideNotification("NoTutorial");
            }
        }
    }


    /// <summary>
    /// Método para mostrar una notificación
    /// </summary>
    public void ShowNotification(string text, string counterText, int notificationID, string source, int check1 = 0, int check2 = 0, int check3 = 0)
    {
        GameObject notif = null;
        TMP_Text notifText = null;
        if (notificationID == 0)
        {
            notif = Notification0;
            notifText = Notification0Text;
        }
        else if (notificationID == 1)
        {
            notif = Notification1;
            notifText = TextNotification1;
        }
        else if (notificationID == 2)
        {
            notif = Notification2;
            notifText = Notification2Text;
        }
        else if (notificationID == 3)
        {
            notif = Notification3;
            notifText = Notification3Text;
        }
        else if (notificationID == 4)
        {
            notif = Notification4;
            notifText = Notification4Text;
        }
        else if (notificationID == 5)
        {
            notif = Notification5;
            notifText = Notification5Text;
        }


        if (source == "Tutorial" && !_isTutorialNotification)
        {
            notif.SetActive(true);
            notifText.text = text;

            // Colocar al final del Vertical Layout Group
            if (counterText != "NoCounter")
            {
                Notification2Counter.SetActive(true);
                Notification2CounterText.text = counterText;
                if (check1 == 1) CheckBox[0].SetActive(true);
                    if (check2 == 1) CheckBox[1].SetActive(true);
                if (check3 == 1) CheckBox[2].SetActive(true);
            }
            else
            {
                Notification2Counter.SetActive(false);
            }

            _isTutorialNotification = true;
            _tutorialNotificationID = notificationID;
        }
        else if (source == "NoTutorial" && !_isOtherNotification)
        {
            notif.SetActive(true);
            notifText.text = text;

            // Colocar al principio del Vertical Layout Group
            notif.transform.SetSiblingIndex(0);

            _isOtherNotification = true;
        }
        else if (source == "Energy" && !_isEnergyNotification)
        {
            notif.SetActive(true);
            notifText.text = text;

            // Colocar al principio del Vertical Layout Group
            notif.transform.SetSiblingIndex(0);

            _isEnergyNotification = true;
        }
        else if (source == "WC" && !_isWcNotification)
        {
            notif.SetActive(true);
            notifText.text = text;

            // Colocar al principio del Vertical Layout Group
            notif.transform.SetSiblingIndex(0);

            _isWcNotification = true;
        }
        else if (source == "Tool" && !_isToolNotification)
        {
            notif.SetActive(true);
            notifText.text = text;

            // Colocar al principio del Vertical Layout Group
            notif.transform.SetSiblingIndex(0);

            _isToolNotification = true;
        }
        else if (source == "Inventory" && !_isInventoryNotification)
        {
            notif.SetActive(true);
            notifText.text = text;

            // Colocar al principio del Vertical Layout Group
            notif.transform.SetSiblingIndex(0);

            _isInventoryNotification = true;
            Invoke("NotificationInventory", 1.5f);
        }
        NotificationManager.SaveNotification(text, counterText, source);
    }

    /// <summary>
    /// Método para saber qué notificación está disponible
    /// </summary>
    public int GetAvailableNotification()
    {
        if (!Notification1.activeSelf && !Notification2.activeSelf)
            return 1;

        if (Notification1.activeSelf && !Notification2.activeSelf)
            return 2;

        return 1;
    }

    /// <summary>
    /// Método para ocultar la notificación
    /// </summary>
    public void HideNotification(string source)
    {
        if (source == "Tutorial" && _isTutorialNotification)
        {
            Notification2.SetActive(false);
            Notification2Counter.SetActive(false);
            _isTutorialNotification = false;
            for (int i = 0; i < CheckBox.Length; i++)
            {
                CheckBox[i].SetActive(false);                
            }
            NotificationManager.DestroyNotification(source);
        }
        else if (source == "NoTutorial" && _isOtherNotification)
        {
            Notification1.SetActive(false);
            NotificationManager.DestroyNotification(source);
            _isOtherNotification = false;
        }
        else if (source == "Energy" && _isEnergyNotification)
        {
            Notification0.SetActive(false);
            NotificationManager.DestroyNotification(source);
            _isEnergyNotification = false;
        }
        else if (source == "WC" && _isWcNotification)
        {
            Notification3.SetActive(false);
            NotificationManager.DestroyNotification(source);
            _isWcNotification = false;
        }
        else if (source == "Tool" && _isToolNotification)
        {
            Notification4.SetActive(false);
            NotificationManager.DestroyNotification(source);
            _isToolNotification = false;
        }
        else if (source == "Inventory" && _isInventoryNotification)
        {
            Notification5.SetActive(false);
            NotificationManager.DestroyNotification(source);
            _isInventoryNotification = false;
        }
    }
    public void NotificationInventory()
    {
        HideNotification("Inventory");
    }

    public void ShowWaterBar()
    {
        WaterMessage.SetActive(true);
    }
    public void HideWaterBar()
    {
        WaterMessage.SetActive(false);
        HideNotification("WC");
    }
    public void UpdateWaterBar(float Water, float MaxWaterAmount)
    {
        if (Water == 0)
        {
            WaterBarText.color = new Color32(0xAB, 0x00, 0x2C, 0xFF);
            WaterBarText.text = Water.ToString();
            ShowNotification("Regadera Vacía", "NoCounter", 3, "WC");
        }
        else if ( Water < (MaxWaterAmount/2))
        {
            WaterBarText.color = Color.black;
            WaterBarText.text = Water.ToString();
            HideNotification("WC");
        }
        else
        {
            WaterBarText.color = Color.white;
            WaterBarText.text = Water.ToString();
            HideNotification("WC");
        }
        WaterBar.value = Water / MaxWaterAmount;
    }

    /// <summary>
    /// Metodo para mostrar el dialogo actual del tutorial
    /// </summary>
    public void ShowDialogue(string dialogueText, string buttonText)
    {
        _isDialogueActive = true;
        PlayerMovement.DisablePlayerMovement();
        TutorialUI.SetActive(true);
        if (!TutorialUIButton.activeSelf)
        {
            Invoke("ShowDialogueButton", 0.5f);
        }
        TutorialText.text = dialogueText;
        ButtonTutorialText.text = buttonText;
        // Quitar listeners anteriores por seguridad
        TutorialButton.onClick.RemoveAllListeners();

        // Asignar nueva acción según el botón
        TutorialButton.onClick.AddListener(() =>
        {
            TutorialManager.OnTutorialButtonPressed(buttonText);
        });
    }
    /// <summary>
    /// Metodo para saber si el dialogo se esta mostrando
    /// </summary>
    /// <returns></returns>
    public bool GetDialogueActive()
    {
        return _isDialogueActive;
    }

    ///<summary>
    ///Metodo para activar el boton de continuar/cerrar en el dialogo
    /// </summary>
    public void ShowDialogueButton()
    {
        TutorialUIButton.SetActive(true);
        if(GameManager.Instance.GetControllerUsing())
        {
            TutorialButton.Select();
        }
        
    }

    ///<summary>
    ///Metodo para desactivar el boton de continuar/cerrar en el dialogo
    /// </summary>
    public void HideDialogueButton()
    {
        TutorialUIButton.SetActive(false);
    }

    ///<summary>
    ///Metodo para ocultar el dialogo del tutorial
    /// </summary>
    public void HideDialogue()
    {
        _isDialogueActive = false;
        Debug.Log("Cerrando diálogo");
        TutorialUI.SetActive(false);
        TutorialUIButton.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Escena_Venta" || SceneManager.GetActiveScene().name == "Escena_Compra" || SceneManager.GetActiveScene().name == "Escena_Banco" || SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            if (!_uiActive)
            {

                PlayerMovement.EnablePlayerMovement();
            }
            else if (_uiActive && GameManager.Instance.GetControllerUsing())
            {
                if(SceneManager.GetActiveScene().name == "Escena_Compra")
                {
                    LettuceSeedsButton.Select();
                }
                else if (SceneManager.GetActiveScene().name == "Escena_Venta")
                {
                    LettuceButton.Select();
                }
            }
            else
            {
                PlayerMovement.EnablePlayerMovement();
            }
        }
    }

    /// <summary>
    /// Marca la tarea del tutorial como completa
    /// </summary>
    /// <param name="i"></param>
    public void Check(int i)
    {
        if (i < CheckBox.Length) CheckBox[i].SetActive(true);
        NotificationManager.EditNotification(i++);
    }

    ///<summary>
    ///Metodo para saber el estado del mapa
    /// </summary>
    public void HideMap()
    {
        Player.localScale = new Vector3(7f, 7f, 1f);
        Map.SetActive(false);
        _isMapVisible = false;
        PlayerMovement.EnablePlayerMovement();

    }
    ///<summary>
    ///Metodo para mostrar el menu de pausa
    /// </summary>
    public void ShowPauseMenu()
    {
        HideLibrary();

        ContinueButton.Select();

        TutorialButton.interactable = false;
        PauseMenu.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Escena_Build")
        {
            HideMap();
            if (_isInventoryVisible)
            {
                ToggleInventory();
            }
        }
        else if (GetUIActive())
        {
            DisableInterfaz();
        }
        PlayerMovement.DisablePlayerMovement();
        _isPauseMenuActive = true;
    }
    ///<summary>
    ///Metodo para esconder el menu de pausa 
    /// </summary>
    public void HidePauseMenu()
    {
        if (GameManager.Instance.GetControllerUsing() == false)
        {
            ContinueButton.Select();
        }
        TutorialButton.interactable = true;
        if(_isDialogueActive && GameManager.Instance.GetControllerUsing() == true)
        {
            Debug.Log("boton tuto selec");
            TutorialButton.Select();
        }
        else if (!_isDialogueActive && !_isLibraryActive && !PlayerMovement.GetPlayerTired())
        {
            PlayerMovement.EnablePlayerMovement();

        }
        PauseMenu.SetActive(false);
        _isPauseMenuActive = false;
    }
    public bool GetPauseMenu()
    {
        return _isPauseMenuActive;
    }
    public bool GetUIActive()
    {
        return _uiActive;
    }
    public bool GetMapVisible()
    {
        return _isMapVisible;
    }

    public void CanExitGame()
    {
        if (SceneManager.GetActiveScene().name == "Escena_Banco")
        {
            if (TutorialManager.GetTutorialPhaseBanco() < 9)
            {
                ShowNotification("Termina el tutorial\n para ir al menú.", "NoCounter", 4, "Tool");
                HidePauseMenu();
                Invoke("HideExitGameMessage", 1.5f);
            }
            else
            {
                SceneTransition.Instance.ChangeScene("Menu");
            }
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            if (TutorialManager.GetTutorialPhaseBanco() < 10)
            {
                ShowNotification("Termina el tutorial\n para ir al menú.", "NoCounter", 4, "Tool");
                HidePauseMenu();
                Invoke("HideExitGameMessage", 1.5f);
            }
            else
            {
                SceneTransition.Instance.ChangeScene("Menu");
            }
        }
        else
        {
            if (TutorialManager.GetTutorialPhase() < 26)
            {
                ShowNotification("Termina el tutorial\n para ir al menú.", "NoCounter", 4, "Tool");
                HidePauseMenu();
                Invoke("HideExitGameMessage", 1.5f);
            }
            else
            {
                SceneTransition.Instance.ChangeScene("Menu");
            }
        }
            
    }
    public void HideExitGameMessage()
    {
        HideNotification("Tool");
    }
    public void ShowLibrary()
    {

            Library.SetActive(true);
            _isLibraryActive = true;
            PlacesDropdown.value = 0;
            CharactersDropdown.value = 0;
            CharactersDropdown.Select();
            PlantsDropdown.value = 0;
            if (GameManager.Instance.GetAmountSold("Lettuce") >= 10)
            {
                CarrotDescription.text = "Fiel y subterránea, crece bajo tierra como los secretos del bosque. A todos les encanta, y a los comerciantes también.";
            }
            else
            {
                CarrotDescription.text = "Cultivo no descubierto, vende 10 Lechugas para desbloquear. Lechugas vendidas:\n" + GameManager.Instance.GetAmountSold("Lettuce");
                StrawberryDescription.text = "Desbloquea el cultivo anterior para mas información.";
                CornDescription.text = "Desbloquea el cultivo anterior para mas información.";
            }


            if (GameManager.Instance.GetAmountSold("Carrot") >= 30)
            {
                StrawberryDescription.text = "Pequeña, dulce y jugosa. Aunque tarda un poco más, su valor es alto. Ideal para quienes cultivan con amor (y paciencia).";
            }
            else if (GameManager.Instance.GetAmountSold("Carrot") < 30 && GameManager.Instance.GetAmountSold("Lettuce") >= 10)
            {
                StrawberryDescription.text = "Cultivo no descubierto, vende 30 zanahorias para desbloquear. Zanahorias vendidas:\n" + GameManager.Instance.GetAmountSold("Carrot");
                CornDescription.text = "Desbloquea el cultivo anterior para mas información.";
        }


            if (GameManager.Instance.GetAmountSold("Strawberry") >= 50)
            {
                CornDescription.text = "Alto y orgulloso. Su crecimiento es lento pero produce mucho. Cuando lo cosechas, suena a victoria. Literalmente, crack.";
            }
            else if (GameManager.Instance.GetAmountSold("Strawberry") < 50 && GameManager.Instance.GetAmountSold("Carrot") >= 30)
            {
                CornDescription.text = "Cultivo no descubierto, vende 30 zanahorias para desbloquear. Zanahorias vendidas:\n" + GameManager.Instance.GetAmountSold("Carrot");
            }


            if((SceneManager.GetActiveScene().name == "Escena_Banco" || SceneManager.GetActiveScene().name == "Escena_Mejora" || SceneManager.GetActiveScene().name == "Escena_Venta" || SceneManager.GetActiveScene().name == "Escena_Compra") && _uiActive)
            {
               DisableInterfaz();
            }
            else if(SceneManager.GetActiveScene().name == "Escena_Build")
            {
                if(_isInventoryVisible)
                {
                    ToggleInventory();
                }
                else if(_isMapVisible)
                {
                    HideMap();
                }
            }
        PlayerMovement.DisablePlayerMovement();
    }
    public void HideLibrary()
    {
        PlayerMovement.EnablePlayerMovement();
        Library.SetActive(false);
        _isLibraryActive = false;
    }

    public bool GetLibraryActive()
    {
        return _isLibraryActive;
    }

    ///<summary>
    ///library cambios
    /// </summary>
    public void UpdateLibrary(int changedDropdown)
    {
        // Desactivar todo
        foreach (var obj in PlacesRootWood) obj.SetActive(false);
        foreach (var obj in CharactersRootWood) obj.SetActive(false);
        foreach (var obj in PlantsRootWood) obj.SetActive(false);
        foreach (var obj in ToolsRootWood) obj.SetActive(false);

        LibraryDescription.text = "Busca información de todo lo descubierto en la Enciclopedia.";

        // Desconectar temporalmente los listeners para evitar llamadas recursivas
        PlacesDropdown.onValueChanged.RemoveAllListeners();
        CharactersDropdown.onValueChanged.RemoveAllListeners();
        PlantsDropdown.onValueChanged.RemoveAllListeners();
        ToolsDropdown.onValueChanged.RemoveAllListeners();

        switch (changedDropdown)
        {
            case 0: // Lugar seleccionado
                if (PlacesDropdown.value > 0)
                {
                    LibraryDescription.text = "";
                    CharactersDropdown.value = 0;
                    PlantsDropdown.value = 0;
                    ToolsDropdown.value = 0;

                    int index = PlacesDropdown.value - 1;
                    if (index < PlacesRootWood.Length)
                        PlacesRootWood[index].SetActive(true);
                }
                break;

            case 1: // Personaje seleccionado
                if (CharactersDropdown.value > 0)
                {
                    LibraryDescription.text = "";
                    PlacesDropdown.value = 0;
                    PlantsDropdown.value = 0;
                    ToolsDropdown.value = 0;


                    int index = CharactersDropdown.value - 1;
                    if (index < CharactersRootWood.Length)
                        CharactersRootWood[index].SetActive(true);
                }
                break;

            case 2: // Planta seleccionada
                if (PlantsDropdown.value > 0)
                {
                    LibraryDescription.text = "";
                    PlacesDropdown.value = 0;
                    CharactersDropdown.value = 0;
                    ToolsDropdown.value = 0;


                    int index = PlantsDropdown.value - 1;
                    if (index < PlantsRootWood.Length)
                        PlantsRootWood[index].SetActive(true);
                }
                break;
            case 3: //Herramienta seleccionada
                if (ToolsDropdown.value > 0)
                {
                    LibraryDescription.text = "";
                    PlacesDropdown.value = 0;
                    CharactersDropdown.value = 0;
                    PlantsDropdown.value = 0;

                    int index = ToolsDropdown.value - 1;
                    if (index < ToolsRootWood.Length)
                        ToolsRootWood[index].SetActive(true);
                }
                break;
        }

        // Reconectar los listeners
        PlacesDropdown.onValueChanged.AddListener(delegate { UpdateLibrary(0); });
        CharactersDropdown.onValueChanged.AddListener(delegate { UpdateLibrary(1); });
        PlantsDropdown.onValueChanged.AddListener(delegate { UpdateLibrary(2); });
        ToolsDropdown.onValueChanged.AddListener(delegate { UpdateLibrary(3); });

    }

    public void HideControls()
    {
        _isControlsActive = false;
        ControlsDropdown.value = 0;
        UpdateControls(0);
    }

    public void UpdateControls(int changedDropdown)
    {
        // Desactivar todo
        foreach (var obj in Controls) obj.SetActive(false);
        // Desconectar temporalmente los listeners para evitar llamadas recursivas
        ControlsDropdown.onValueChanged.RemoveAllListeners();

        switch (changedDropdown)
        {
            case 0: // Lugar seleccionado
                if (ControlsDropdown.value > 0)
                {
                    int index = ControlsDropdown.value - 1;
                    if (index < Controls.Length)
                        Controls[index].SetActive(true);
                    _isControlsActive = true;
                }
                break;
        }
        ControlsDropdown.onValueChanged.AddListener(delegate { UpdateControls(0); });
    }
    #endregion

    // ---- MÉTODOS PRIVADOS GENERALES ----
    #region Métodos Privados Generales




    /// <summary>
    /// Metodo para actualizar la interfaz dependiendo de la escena activa
    /// </summary>
    /// <summary>
    /// Metodo para actualizar la interfaz dependiendo de la escena activa
    /// </summary>
    private void UpdateUI()
    {
        if (SceneManager.GetActiveScene().name == "Escena_Banco")
        {
            DepositeMoneyButton.Select();
            if (_isDepositSelected)
            {
                AmountDepositedTitleText.gameObject.SetActive(true);
                DescriptionText.text = "En el banco puedes ingresar tu dinero.";
                AmountMoneyToDeposit.gameObject.SetActive(true);
                AmountToDepositText.gameObject.SetActive(true);
                AmountToDepositText.text = "Dinero a ingresar: " + Convert.ToInt32(AmountMoneyToDeposit.value);
                BeachHouseButton.SetActive(false);
                AmountDepositedText.gameObject.SetActive(true);
                AmountDepositedText.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
                MoveButton.SetActive(false);
                AcceptButton.SetActive(true);
                UpdateSlider();
            }
            else if (_isMovingSelected)
            {
                AmountDepositedTitleText.gameObject.SetActive(false);
                DescriptionText.text = "Selecciona la casa a la que deseas mudarte.";
                AmountMoneyToDeposit.gameObject.SetActive(false);
                AmountToDepositText.gameObject.SetActive(false);
                BeachHouseButton.SetActive(true);
                AcceptButton.SetActive(false);
                AmountDepositedText.gameObject.SetActive(false);
                MoveButton.SetActive(false);
            }
            else if (_isWithdrawSelected)
            {
                AmountDepositedTitleText.gameObject.SetActive(true);
                DescriptionText.text = "Selecciona la cantidad que deseas retirar.";
                AmountMoneyToDeposit.gameObject.SetActive(true);
                AmountToDepositText.gameObject.SetActive(true);
                AmountToDepositText.text = "Dinero a retirar: " + Convert.ToInt32(AmountMoneyToDeposit.value);
                BeachHouseButton.SetActive(false);
                AmountDepositedText.gameObject.SetActive(true);
                AmountDepositedText.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
                MoveButton.SetActive(false);
                AcceptButton.SetActive(true);
                UpdateSlider();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            ExtendButton.Select();
            WateringCanButton.SetActive(_isUpgradeSelected);
            GardenButton.SetActive(_isExtendSelected);
            BuyUpgradeObj.SetActive(_isSomethingSelected);
            DescriptionText.text = "";
            AmountOfUpgradesText.text = "";
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Venta")
        {
            SellObj.SetActive(_isSomethingSelected);
            PlusButton.SetActive(_isSomethingSelected);
            MinusButton.SetActive(_isSomethingSelected);
            if (GameManager.Instance.GetAmountSold("Lettuce") >= 10)
            {
                BlockMarketPlants[0].SetActive(false);
                CarrotsButton.interactable = true;
            }
            else
            {
                CarrotsButton.interactable = false;
            }
            if (GameManager.Instance.GetAmountSold("Carrot") >= 30)
            {
                BlockMarketPlants[1].SetActive(false);
                StrawberriesButton.interactable = true;
            }
            else
            {
                StrawberriesButton.interactable = false;
            }
            if (GameManager.Instance.GetAmountSold("Strawberry") >= 50)
            {
                BlockMarketPlants[2].SetActive(false);
                CornsButton.interactable = true;
            }
            else
            {
                CornsButton.interactable = false;
            }
            DescriptionText.text = "";
            Counter.text = "";
            int totalCosto = _amountBuying * _cost;
            Counter.text = $"{_amountBuying} = {totalCosto} RC";
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Compra")
        {
            BuySeedsObj.SetActive(_isSomethingSelected);
            IncreaseAmountButton.SetActive(_isSomethingSelected);
            DecreaseAmountButton.SetActive(_isSomethingSelected);

            // Botón Lechuga (siempre activo)
            Navigation navLettuce = new Navigation { mode = Navigation.Mode.Explicit };

            if (GameManager.Instance.GetAmountSold("Lettuce") >= 10)
            {
                BlockMarketSeeds[0].SetActive(false);
                CarrotSeedsButton.interactable = true;

                // Lechuga -> Zanahoria
                navLettuce.selectOnRight = CarrotSeedsButton;
                navLettuce.selectOnDown = BuySeedsButton;
                LettuceSeedsButton.navigation = navLettuce;

                // Zanahoria <- Lechuga
                Navigation navCarrot = new Navigation { mode = Navigation.Mode.Explicit };
                navCarrot.selectOnLeft = LettuceSeedsButton;
                navCarrot.selectOnDown = BuySeedsButton;
                CarrotSeedsButton.navigation = navCarrot;


                if (GameManager.Instance.GetAmountSold("Carrot") >= 30)
                {
                    BlockMarketSeeds[1].SetActive(false);
                    StrawberrySeedsButton.interactable = true;

                    // Zanahoria -> Fresa
                    navCarrot.selectOnRight = StrawberrySeedsButton;
                    navCarrot.selectOnLeft = LettuceSeedsButton;
                    navCarrot.selectOnDown = BuySeedsButton;
                    CarrotSeedsButton.navigation = navCarrot;

                    // Fresa <- Zanahoria
                    Navigation navStrawberry = new Navigation { mode = Navigation.Mode.Explicit };
                    navStrawberry.selectOnLeft = CarrotSeedsButton;
                    navStrawberry.selectOnDown = BuySeedsButton;
                    StrawberrySeedsButton.navigation = navStrawberry;



                    if (GameManager.Instance.GetAmountSold("Strawberry") >= 50)
                    {
                        BlockMarketSeeds[2].SetActive(false);
                        CornSeedsButton.interactable = true;

                        // Fresa -> Maíz
                        navStrawberry.selectOnRight = CornSeedsButton;
                        navStrawberry.selectOnLeft = CarrotSeedsButton;
                        navStrawberry.selectOnDown = BuySeedsButton;
                        StrawberrySeedsButton.navigation = navStrawberry;

                        // Maíz <- Fresa
                        Navigation navCorn = new Navigation { mode = Navigation.Mode.Explicit };
                        navCorn.selectOnLeft = StrawberrySeedsButton;
                        navCorn.selectOnDown = BuySeedsButton;
                        CornSeedsButton.navigation = navCorn;
                    }
                    else
                    {
                        CornSeedsButton.interactable = false;
                        StrawberrySeedsButton.navigation = navStrawberry;
                    }
                }
                else
                {
                    StrawberrySeedsButton.interactable = false;
                    CarrotSeedsButton.navigation = navCarrot;
                }
            }
            else
            {
                CarrotSeedsButton.interactable = false;
                navLettuce.selectOnDown = BuySeedsButton;
                LettuceSeedsButton.navigation = navLettuce;
            }

            DescriptionText.text = "";
            PriceAmountToBuy.text = "";
            int totalCosto = _amountBuying * _cost;
            PriceAmountToBuy.text = _amountBuying + " " + _actualSeedSelected + " = " + totalCosto + "RC";
        }
    }

    private void EnableInterfaz()
    {
        _uiActive = true;
        UI.SetActive(true);
        _isInNpcArea = false;
        HideNotification("NoTutorial");
        PlayerMovement.DisablePlayerMovement();
        if (SceneManager.GetActiveScene().name == "Escena_Banco")
        {
            ButtonDepositPressed();
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            _isUpgradeSelected = true; // Siempre inicia en Mejoras
            _isExtendSelected = false;
            _isSomethingSelected = false;
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Venta")
        {
            _isLettuceSelected = true;
            LettuceButton.Select();
            ActualizarCantidadUI();
        }
        else if (SceneManager.GetActiveScene().name == "Escena_Compra")
        {
            _isLettuceSelected = true;
            LettuceSeedsButton.Select();
            ActualizarCantidadSeedsUI();
        }
    }

    private void DisableInterfaz()
    {
        _uiActive = false;
        UI.SetActive(false);
        if(!_isPauseMenuActive)
        {
            _isInNpcArea = true;
        }
        ShowNotification("Presiona E para\nhablar", "NoCounter", 1, "NoTutorial");
        PlayerMovement.EnablePlayerMovement();
    }

    private void ResetInterfaz()
    {
        UI.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Escena_Mejora")
        {
            BuyUpgradeObj.SetActive(false);
            WateringCanButton.SetActive(false);
            GardenButton.SetActive(false);
            DescriptionText.text = "";
            AmountOfUpgradesText.text = "";
        }
        if (SceneManager.GetActiveScene().name == "Escena_Venta")
        {
            SellObj.SetActive(_isSomethingSelected);
            PlusButton.SetActive(_isSomethingSelected);
            MinusButton.SetActive(_isSomethingSelected);
            DescriptionText.text = "";
            Counter.text = "";
        }

        if (SceneManager.GetActiveScene().name == "Escena_Compra")
        {
            BuySeedsObj.SetActive(_isSomethingSelected);
            IncreaseAmountButton.SetActive(_isSomethingSelected);
            DecreaseAmountButton.SetActive(_isSomethingSelected);
            DescriptionText.text = "";
            PriceAmountToBuy.text = "";
        }
    }
    /// <summary>
    /// Metodo para asignar las referencias de este script
    /// </summary>
    private void InitializeReferences()
    {
        MoneyManager = FindObjectOfType<MoneyManager>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        NotificationManager = FindObjectOfType<NotificationManager>();
    }



    
    #endregion

    // ---- BUILD ----
    #region Build


    // ---- METODOS PUBLICOS (BUILD) ----
    #region Metodos Publicos (Build)
    /// <summary>
    /// Metodo para saber si el inventory esta visible en pantalla
    /// </summary>
    /// <returns></returns>
    public bool GetInventoryVisible()
    {
        return _isInventoryVisible;
    }

    /// <summary>
    /// Actualiza la cantidad de los items del inventory
    /// No comprueba si hay inventory suficiente para mostrar los items porque ya lo comprueba InventoryManager
    /// </summary>
    public void ActualizeInventory()
    {
        TextMeshProUGUI _units;

        // Muestra las semillas
        for (int i = 0; i < (int)Items.Count / 2; i++)
        {
            GameObject _crops = InventoryIcons.transform.GetChild(i).gameObject;
            if (InventoryManager.GetInventoryItem(i) != 0)
            {
                _crops.SetActive(true);
                _units = _crops.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                _units.text = "x" + InventoryManager.GetInventoryItem(i);
            }
            else _crops.SetActive(false);
        }

        // Muestra los cultivos
        for (int i = (int)Items.Count / 2; i < (int)Items.Count; i++)
        {
            if (InventoryManager.GetInventoryItem(i) != 0)
            {
                int actualSlot = 1; // El Slot actual que está estableciendo
                bool fullSlot = false; // Es true si el Slot es igual que la cantidad máxima por Slot

                while (actualSlot < 5 && !fullSlot)
                {
                    GameObject _crops = InventoryIcons.transform.GetChild(i * actualSlot).gameObject;
                    _crops.SetActive(true);
                    _units = _crops.GetComponentInChildren<TextMeshProUGUI>();
                    if (InventoryManager.GetInventoryItem(i) / (actualSlot * _slotsCapacity) != 0)
                    {
                        _units.text = "x" + _slotsCapacity;
                    }
                    else if (InventoryManager.GetInventoryItem(i) - ((actualSlot - 1) * _slotsCapacity) != 0)
                    {
                        _units.text = InventoryManager.GetInventoryItem(i) - ((actualSlot - 1) * _slotsCapacity) + "x";
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
    public void UpdateEnergyBar(float current, int max)
    {
        EnergySlider.maxValue = max;
        EnergySlider.value = current;
    }

    public void ShowTiredMessage(bool show)
    {
        TiredMessage.SetActive(show);
    }
    #endregion

    /// <summary>
    /// Alterna la visibilidad del inventory y mueve la QuickAccessBar con él.
    /// </summary>
    public void ToggleInventory()
    {
        _isInventoryVisible = !_isInventoryVisible;
        bool desactivate = false;
        if (TutorialManager.GetTutorialPhase() == 7) // Verifica si es la fase 3 o la fase que corresponda
        {
            Check(0);
            Invoke("NextDialogue", 0.6f);
        }


    }

    /// <summary>
    /// Invoca Next Dialogue de TutorialManager
    /// </summary>
    public void NextDialogue()
    {
        TutorialManager.NextDialogue();
    }

    // ---- METODOS PRIVADOS (BUILD) ----
    #region Metodos Privados (Build)

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
        _isWithdrawSelected = false; // Añadimos esta línea
        UpdateUI();
        if (TutorialManager.GetTutorialPhaseBanco() == 6)
        {
            Check(0);
        }
    }

    /// <summary>
    /// Metodo para actualizar la ui cuando se pulsa el boton mudanza
    /// </summary>
    public void ButtonMovingPressed()
    {
        _isDepositSelected = false;
        _isMovingSelected = true;
        _isBeachHouseSelected = false;
        _isWithdrawSelected = false; // Añadimos esta línea
        UpdateUI();

        if (TutorialManager.GetTutorialPhaseBanco() == 8)
        {
            Check(0);
        }
    }

    /// <summary>
    /// Metodo para actualizar la ui cuando se pulsa el boton de la casa de la playa
    /// </summary>
    public void ButtonBeachHousePressed()
    {
        _isBeachHouseSelected = true;
        _isDepositSelected = false; // Añadimos esta línea
        _isMovingSelected = false; // Añadimos esta línea
        _isWithdrawSelected = false; // Añadimos esta línea
        DescriptionText.text = "Has seleccionado la Casa Playa.\n ¡Compra esta casa por solo 100.000 RootCoins!.";
        MoveButton.SetActive(true);
        if (TutorialManager.GetTutorialPhaseBanco() == 8) // Verifica si ha pulsado el botón + de venta
        {
            Check(1);
            Invoke("NextDialogue", 0.6f);
        }
    }

    /// <summary>
    /// Metodo para actualizar la ui cuando se pulsa el boton retirar
    /// </summary>
    public void ButtonWithdrawPressed()
    {
        _isDepositSelected = false;
        _isMovingSelected = false;
        _isBeachHouseSelected = false;
        _isWithdrawSelected = true;
        UpdateUI();
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
            InventoryManager.SetPlayerPosition(_newHousePosition);
            SceneTransition.Instance.FinalCamera();
            Cloud.Instance.FinalCamera();
            GameManager.Instance.FinalScene();
            SceneTransition.Instance.ChangeScene("Escena_Build");

        }
        else
        {
            DescriptionText.text = "No tienes suficiente dinero.";
            _newDescriptionText = "Has seleccionado la Casa Playa. Pulsa 'Mudarse' para continuar.";
            Invoke("ChangeDescription", 1f);
            Debug.Log("No tienes suficiente dinero para mudarte.");

        }
    }

    /// <summary>
    /// Metodo para aceptar el dinero que quieres depositar en el banco y añadirlo
    /// </summary>
    public void ButtonAcceptPressed()
    {
        float AmountDeposited = AmountMoneyToDeposit.value;
        if (_isDepositSelected && AmountDeposited > 0 && MoneyManager.GetMoneyCount() >= AmountDeposited)
        {
            MoneyManager.DeductMoney(Mathf.FloorToInt(AmountDeposited));  // O usar Mathf.RoundToInt
            GameManager.Instance.AddIncome(AmountDeposited);
            UpdateSlider();
            AmountMoneyToDeposit.value = 0;
            if (TutorialManager.GetTutorialPhaseBanco() == 6)
            {
                Check(2);
                Invoke("NextDialogue", 0.6f);

            }
        }
        else if (_isWithdrawSelected && AmountDeposited > 0 && GameManager.Instance.GetTotalMoneyDeposited() >= AmountDeposited)
        {
            GameManager.Instance.DeductDepositedMoney(Convert.ToInt32(AmountDeposited));
            MoneyManager.AddMoney(Mathf.FloorToInt(AmountDeposited)); // O usar Mathf.RoundToInt
            UpdateSlider();
            AmountMoneyToDeposit.value = 0;
        }
    }

    /// <summary>
    /// Metodo para actualizar los valores del slider
    /// </summary>
    public void UpdateSlider()
    {
        if (_isDepositSelected)
        {
            AmountMoneyToDeposit.maxValue = MoneyManager.GetMoneyCount();
            AmountMoneyToDeposit.interactable = AmountMoneyToDeposit.maxValue > 0;
            AmountToDepositText.text = "Dinero a ingresar: " + Convert.ToInt32(AmountMoneyToDeposit.value);
        }
        else if (_isWithdrawSelected)
        {
            AmountMoneyToDeposit.maxValue = GameManager.Instance.GetTotalMoneyDeposited();
            AmountMoneyToDeposit.interactable = AmountMoneyToDeposit.maxValue > 0;
            AmountToDepositText.text = "Dinero a retirar: " + Convert.ToInt32(AmountMoneyToDeposit.value);
        }
        AmountDepositedText.text = GameManager.Instance.GetTotalMoneyDeposited() + " RC";
    }
    #endregion

    // ---- METODOS PRIVADOS (BANCO)
    #region Metodos Privados (Banco)
    private void ChangeDescription()
    {
        DescriptionText.text = _newDescriptionText;
    }
    #endregion
    #endregion

    // ---- VENTA ----
    #region Venta

    // ---- METODOS PUBLICOS (VENTA) ----
    #region Metodos Publicos (Venta)

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el botón "Maíz".
    /// </summary>
    public void ButtonCornPressed()
    {
        _isSomethingSelected = true;
        _isCornSelected = true;
        _isLettuceSelected = _isCarrotSelected = _isStrawberriesSelected = false;
        SellButton.Select();
        _amountBuying = 1; // Reinicia la cantidad al cambiar de cultivo
        _cost = 90;
        ActualizarTextoCantidad();
        
        DescriptionText.text = "1 maíz = 90 RootCoins.";      
        UpdateUI();
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el botón "Lechuga".
    /// </summary>
    public void ButtonLettucePressed()
    {
        _isSomethingSelected = true;
        _isLettuceSelected = true;
        _isCornSelected = _isCarrotSelected = _isStrawberriesSelected = false;
        SellButton.Select();

        _amountBuying = 1; // Reinicia la cantidad al cambiar de cultivo
        _cost = 20;
        ActualizarTextoCantidad();

        DescriptionText.text = "1 lechuga = 20 RootCoins.";
        UpdateUI();

        if (TutorialManager.GetTutorialPhase() == 21) // Verifica que ha puslsado el botón de lechuga venta
        {
            Check(0);
            Invoke("NextDialogue", 0f);
        }

    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el botón "Zanahoria".
    /// </summary>
    public void ButtonCarrotPressed()
    {
        _isSomethingSelected = true;
        _isCarrotSelected = true;
        _isLettuceSelected = _isCornSelected = _isStrawberriesSelected = false;
        SellButton.Select();

        _amountBuying = 1; // Reinicia la cantidad al cambiar de cultivo
        _cost = 65;
        ActualizarTextoCantidad();
        
        DescriptionText.text = "1 zanahoria = 65 RootCoins.";
        UpdateUI();
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el botón "Maíz".
    /// </summary>
    public void ButtonStrawberriesPressed()
    {
        _isSomethingSelected = true;
        _isStrawberriesSelected = true;
        _isLettuceSelected = _isCarrotSelected = _isCornSelected = false;
        SellButton.Select();

        _amountBuying = 1; // Reinicia la cantidad al cambiar de cultivo
        ActualizarTextoCantidad();
        _cost = 40;
        DescriptionText.text = "1 fresa = 40 RootCoins.";
        UpdateUI();
    }

    /// <summary>
    /// Metodo para detectar cuando el jugador pulsa el boton "Sell".
    /// </summary>
    public void ButtonSellPressed()
    {
        _isSellPressed = true;
        Debug.Log("Vender presionado");
        _isSomethingSelected = false;

        int cantidadDisponible = 0;
        int precioUnitario = 0;

        if (_isCornSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Corn);
            precioUnitario = 90;
        }
        else if (_isLettuceSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Lettuce);
            precioUnitario = 20;
        }
        else if (_isCarrotSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Carrot);
            precioUnitario = 65;
        }
        else if (_isStrawberriesSelected)
        {
            cantidadDisponible = InventoryManager.GetInventoryItem(Items.Strawberry);
            precioUnitario = 40;
        }

        // Si no hay cultivos disponibles, mostrar mensaje y salir
        if (cantidadDisponible <= 0)
        {
            DescriptionText.text = "No tienes cultivos de este tipo para vender.";
            return;
        }

        // Verifica que no intente vender más de los que tiene
        if (_amountBuying > cantidadDisponible)
        {
            _amountBuying = cantidadDisponible;
        }

        // Realizar la venta
        int totalGanado = _amountBuying * precioUnitario;
        MoneyManager.AddMoney(totalGanado);

        // Restar del inventario
        InventoryManager.ModifyInventorySubstract(
            _isCornSelected ? Items.Corn :
            _isLettuceSelected ? Items.Lettuce :
            _isCarrotSelected ? Items.Carrot :
            Items.Strawberry, _amountBuying
        );
        if ( _isLettuceSelected )
        {
            GameManager.Instance.AddAmountSold("Lettuce", _amountBuying);
        }
        else if (_isCarrotSelected)
        {
            GameManager.Instance.AddAmountSold("Carrot", _amountBuying);
        }
        else if (_isStrawberriesSelected)
        {
            GameManager.Instance.AddAmountSold("Strawberry", _amountBuying);
        }
        else if (_isCornSelected)
        {
            GameManager.Instance.AddAmountSold("Corn", _amountBuying);
        }

        DescriptionText.text = $"Has vendido {_amountBuying} por {totalGanado} RC.";
        _amountBuying = 1; // Reiniciar cantidad
        ActualizarTextoCantidad();

        ActualizarCantidadUI(); // ⬅️ Llamamos esto para refrescar la UI después de vender
        
        if (TutorialManager.GetTutorialPhase() == 23) // Verifica que ha pulsado el boton de venta
        {
            //Check(0);
            Invoke("NextDialogue", 0f);

        }
        UpdateUI();

    }


    public void ButtonPlusPressed()
    {
        int maxCantidad = 0;

        if (_isCornSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Corn);
        else if (_isLettuceSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Lettuce);
        else if (_isCarrotSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Carrot);
        else if (_isStrawberriesSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Strawberry);

        if (_amountBuying < maxCantidad)
        {
            _amountBuying++;
            ActualizarTextoCantidad();
        }

        if (TutorialManager.GetTutorialPhase() == 22) // Verifica si ha pulsado el botón + de venta
        {
            //Check(0);
            Invoke("NextDialogue", 0f);

        }
    }

    public void ButtonMinusPressed()
    {
        int maxCantidad = 0;

        if (_isCornSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Corn);
        else if (_isLettuceSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Lettuce);
        else if (_isCarrotSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Carrot);
        else if (_isStrawberriesSelected) maxCantidad = InventoryManager.GetInventoryItem(Items.Strawberry);

        if (_amountBuying < maxCantidad)
        {
            _amountBuying--;
            ActualizarTextoCantidad();
        }
    }

    #endregion

    // ---- METODOS PRIVADOS (VENTA) ----
    #region Metodos Privados (venta)
    /// <summary>
    /// Metodo para que la descripcion cambie dependiendo del boton seleccionado.
    /// </summary>

    private void ActualizarTextoCantidad()
    {
        if (MoneyManager == null)
        {
          //  Debug.LogError("ContadorTexto no está asignado en el Inspector.");
            return;
        }

        int precioUnitario = 0;

        if (_isCornSelected) precioUnitario = 90;
        else if (_isLettuceSelected) precioUnitario = 20;
        else if (_isCarrotSelected) precioUnitario = 65;
        else if (_isStrawberriesSelected) precioUnitario = 40;

        int totalGanado = _amountBuying * precioUnitario;

        // Mostrar la cantidad junto con el dinero ganado
        Counter.text = _amountBuying + " = " + totalGanado + " RC";
    }


    /// <summary>
    /// Metodo para que la descripcion cambie dependiendo del boton seleccionado.
    /// </summary>
    private void MostrarDescripcion(int actual, int max, int coste)
    {
        if (actual >= max)
        {
            DescriptionText.text = "No tienes cultivos para vender.";
            SellObj.SetActive(true);
        }
        else
        {
            DescriptionText.text = "TOTAL: " + actual * coste + "RC";
            SellObj.SetActive(true);
            //ContadorTexto.text = actual + "/" + max;
            Counter.text = actual + "/" + max;

        }
    }

    public void ActualizarCantidadUI()
    {
        CornText.text = "x" + InventoryManager.GetInventoryItem(Items.Corn);
        LettuceText.text = "x" + InventoryManager.GetInventoryItem(Items.Lettuce);
        CarrotText.text = "x" + InventoryManager.GetInventoryItem(Items.Carrot);
        StrawberryText.text = "x" + InventoryManager.GetInventoryItem(Items.Strawberry);

    }
    #endregion


    #endregion

    // ---- COMPRA ----
    #region

    // ---- METODOS PUBLICOS (COMPRA) ----
    #region
    public void ButtonCornSeedPressed()
    {
        _isSomethingSelected = true;
        _isCornSelected = true;
        _isLettuceSelected = _isCarrotSelected = _isStrawberriesSelected = false;
        BuySeedsButton.Select();

        _amountBuying = 1;
        if (_amountBuying <= 1)
        {
            _actualSeedSelected = "Semilla de Maiz";

        }
        else
        {
            _actualSeedSelected = "Semillas de Maiz";
        }
        _cost = 50;
        DescriptionText.text = ""; // Reseteamos el mensaje de "máximo de semillas"

        UpdateUI();
    }

    public void ButtonLettuceSeedPressed()
    {
        if((TutorialManager.GetTutorialPhase() == 11) || (TutorialManager.GetTutorialPhase() >= 14))
        {
            BuySeedsButton.Select();
            _isSomethingSelected = true;
            _isLettuceSelected = true;
            _isCornSelected = _isCarrotSelected = _isStrawberriesSelected = false;

            _amountBuying = 1;
            if (_amountBuying <= 1)
            {
                _actualSeedSelected = "Semilla de Lechuga";

            }
            else
            {
                _actualSeedSelected = "Semillas de Lechuga";
            }
            _cost = 15;
            DescriptionText.text = ""; // Reseteamos el mensaje de "máximo de semillas"

            UpdateUI();

            if (TutorialManager.GetTutorialPhase() == 11) // Verifica si es la fase 3 o la fase que corresponda
            {
                Check(0);
                Invoke("NextDialogue", 0f);
            }
        }
    }

    public void ButtonCarrotSeedPressed()
    {
        BuySeedsButton.Select();
        _isSomethingSelected = true;
        _isCarrotSelected = true;
        _isCornSelected = _isLettuceSelected = _isStrawberriesSelected = false;

        _amountBuying = 1;
        if (_amountBuying <= 1)
        {
            _actualSeedSelected = "Semilla de Zanahoria";

        }
        else
        {
            _actualSeedSelected = "Semillas de Zanahoria";
        }
        _cost = 20;
        DescriptionText.text = ""; // Reseteamos el mensaje de "máximo de semillas"

        UpdateUI();
    }

    public void ButtonStrawberriesSeedPressed()
    {
        BuySeedsButton.Select();
        _isSomethingSelected = true;
        _isStrawberriesSelected = true;
        _isCornSelected = _isLettuceSelected = _isCarrotSelected = false;

        _amountBuying = 1;
        if (_amountBuying <= 1)
        {
            _actualSeedSelected = "Semilla de Fresa";

        }
        else
        {
            _actualSeedSelected = "Semillas de Fresa";
        }
        _cost = 30;
        DescriptionText.text = ""; // Reseteamos el mensaje de "máximo de semillas"

        UpdateUI();
    }


    public void IncreaseAmount()
    {
        // Verifica si se ha alcanzado el máximo de semillas para la semilla seleccionada
        if (_isCornSelected && InventoryManager.GetInventoryItem(Items.CornSeed) + _amountBuying >= 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de maíz (30).";
            return;
        }
        if (_isCarrotSelected && InventoryManager.GetInventoryItem(Items.CarrotSeed) + _amountBuying >= 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de zanahoria (30).";
            return;
        }
        if (_isLettuceSelected && InventoryManager.GetInventoryItem(Items.LettuceSeed) + _amountBuying >= 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de lechuga (30).";
            return;
        }
        if (_isStrawberriesSelected && InventoryManager.GetInventoryItem(Items.StrawberrySeed) + _amountBuying >= 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de fresa (30).";
            return;
        }

        int totalCost = (_amountBuying + 1) * _cost;

        if (totalCost > MoneyManager.GetMoneyCount())
        {
            DescriptionText.text = "No tienes suficiente dinero para más semillas.";
            return;
        }

        _amountBuying++;
        UpdateUI();

        if (TutorialManager.GetTutorialPhase() == 12) // Verifica si es la fase 3 o la fase que corresponda
        {
            Check(0);
            Invoke("NextDialogue", 0f);
        }
    }




    public void DecreaseAmount()
    {
        if (_amountBuying > 1)
        {
            _amountBuying--;
            UpdateUI();
        }
    }
    public void ButtonBuyPressed()
    {
        // Verifica el máximo de semillas solo para el tipo seleccionado
        if (_isCornSelected && InventoryManager.GetInventoryItem(Items.CornSeed) + _amountBuying > 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de maíz (30).";
            return;
        }
        if (_isCarrotSelected && InventoryManager.GetInventoryItem(Items.CarrotSeed) + _amountBuying > 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de zanahoria (30).";
            return;
        }
        if (_isLettuceSelected && InventoryManager.GetInventoryItem(Items.LettuceSeed) + _amountBuying > 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de lechuga (30).";
            return;
        }
        if (_isStrawberriesSelected && InventoryManager.GetInventoryItem(Items.StrawberrySeed) + _amountBuying > 30)
        {
            DescriptionText.text = "Ya tienes el máximo de semillas de fresa (30).";
            return;
        }

        int TotalCost = _amountBuying * _cost;

        if (MoneyManager.GetMoneyCount() >= TotalCost)
        {
            MoneyManager.DeductMoney(TotalCost); // Resta el dinero correctamente

            // Actualiza el inventory según el tipo de semilla seleccionada
            if (_isCornSelected)
            {
                InventoryManager.ModifyInventory(Items.CornSeed, _amountBuying);
            }
            if (_isLettuceSelected)
            {
                InventoryManager.ModifyInventory(Items.LettuceSeed, _amountBuying);
            }
            if (_isCarrotSelected)
            {
                InventoryManager.ModifyInventory(Items.CarrotSeed, _amountBuying);
            }
            if (_isStrawberriesSelected)
            {
                InventoryManager.ModifyInventory(Items.StrawberrySeed, _amountBuying);
            }

            DescriptionText.text = "Compra realizada con éxito.";
        }
        else
        {
            DescriptionText.text = "No tienes suficiente dinero.";  // Mensaje si no hay dinero suficiente
        }

        UpdateUI();  // Actualiza la UI después de la compra
        ActualizarCantidadSeedsUI(); // Llamamos esto para refrescar la UI después de vender

        if (TutorialManager.GetTutorialPhase() == 13) // Verifica si es la fase 3 o la fase que corresponda
        {
            Check(0);
            Invoke("NextDialogue", 0f);
        }
    }



    #endregion

    // ---- METODOS PRIVADOS (COMPRA) ----
    #region
    public void ActualizarCantidadSeedsUI()
    {
        SeedCornText.text = "x" + InventoryManager.GetInventoryItem(Items.CornSeed);
        SeedLettuceText.text = "x" + InventoryManager.GetInventoryItem(Items.LettuceSeed);
        SeedCarrotText.text = "x" + InventoryManager.GetInventoryItem(Items.CarrotSeed);
        SeedStrawberryText.text = "x" + InventoryManager.GetInventoryItem(Items.StrawberrySeed);

    }
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
        if (TutorialManager.GetTutorialPhaseMejora() == 5) Check(0);

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
        if (TutorialManager.GetTutorialPhaseMejora() == 8) Check(0);
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

        ShowDescriptionUpgrade("Aumenta la capacidad de agua por 1.000 RootCoins.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
        if (TutorialManager.GetTutorialPhaseMejora() == 8)
        {
            Check(2);
            Invoke("NextDialogue", 0f);
            if (TutorialManager.GetTutorialPhaseMejora() == 8) Check(0);
        }
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

        ShowDescriptionUpgrade("Expande el terreno de cultivos por 5.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
        if (TutorialManager.GetTutorialPhaseMejora() == 5)
        {
            Check(2);
            Invoke("NextDialogue", 0f);
        }
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
                ShowDescriptionUpgrade("Aumenta la capacidad de agua por 5.000 RootCoins.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
            }
            else if (_isWateringCanSelected && (MoneyManager.GetMoneyCount() >= 5000) && (GameManager.Instance.GetWateringCanUpgrades() == 1))
            {
                GameManager.Instance.UpgradeWateringCan();
                ShowDescriptionUpgrade("Aumenta la capacidad de agua por 10.000 RootCoins.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
            }
            else if (_isWateringCanSelected && (MoneyManager.GetMoneyCount() >= 10000) && (GameManager.Instance.GetWateringCanUpgrades() == 2))
            {
                GameManager.Instance.UpgradeWateringCan();
                ShowDescriptionUpgrade("Aumenta la capacidad de agua.", GameManager.Instance.GetWateringCanUpgrades(), _maxWCUpgrades);
            }
        }
        else if (_isExtendSelected && _isSomethingSelected)
        {
            if (_isGardenSelected)
            {
                if ((MoneyManager.GetMoneyCount() >= 5000) && (GameManager.Instance.GetGardenUpgrades() == 0))
                {
                    GameManager.Instance.UpgradeGarden();
                    ShowDescriptionUpgrade("Expande el terreno de cultivos por 10.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 10000) && (GameManager.Instance.GetGardenUpgrades() == 1))
                {
                    GameManager.Instance.UpgradeGarden();
                    ShowDescriptionUpgrade("Expande el terreno de cultivos por 15.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 15000) && (GameManager.Instance.GetGardenUpgrades() == 2))
                {
                    GameManager.Instance.UpgradeGarden();
                    ShowDescriptionUpgrade("Expande el terreno de cultivos por 20.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 20000) && (GameManager.Instance.GetGardenUpgrades() == 3))
                {
                    GameManager.Instance.UpgradeGarden();
                    ShowDescriptionUpgrade("Expande el terreno de cultivos por 30.000 RootCoins.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
                }
                else if ((MoneyManager.GetMoneyCount() >= 30000) && (GameManager.Instance.GetGardenUpgrades() == 4))
                {
                    GameManager.Instance.UpgradeGarden();
                    ShowDescriptionUpgrade("Expande el terreno de cultivos.", GameManager.Instance.GetGardenUpgrades(), _maxGardenUpgrades);
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
    private void ShowDescriptionUpgrade(string text, int actualUpgrades, int maxUpgrades)
    {
        if (actualUpgrades >= maxUpgrades)
        {
            DescriptionText.text = "Ya no quedan más mejoras.";
            BuyUpgradeObj.SetActive(false);
        }
        else
        {
            DescriptionText.text = text;
            BuyUpgradeObj.SetActive(true);
        }
        AmountOfUpgradesText.text = actualUpgrades + "/" + maxUpgrades;
    }
    #endregion


    #endregion
} // class UIManager
