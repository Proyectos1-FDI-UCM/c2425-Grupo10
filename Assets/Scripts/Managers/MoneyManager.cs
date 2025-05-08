//---------------------------------------------------------
// Este script maneja el Contador del dinero del jugador
// Natalia
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Cantidad total de dinero del jugador.
    /// </summary>
    [SerializeField] private int MoneyCount;

    [Header("Precios de Semillas")]
    /// <summary>
    /// Precio de la semilla de maíz.
    /// </summary>
    [SerializeField] private int CornSeedPrice;

    /// <summary>
    /// Precio de la semilla de zanahoria.
    /// </summary>
    [SerializeField] private int CarrotSeedPrice;

    /// <summary>
    /// Precio de la semilla de lechuga.
    /// </summary>
    [SerializeField] private int LettuceSeedPrice;

    /// <summary>
    /// Precio de la semilla de fresa.
    /// </summary>
    [SerializeField] private int StrawberrySeedPrice;

    [Header("Precios de Venta de Plantas")]
    /// <summary>
    /// Precio de venta de la planta de maíz.
    /// </summary>
    [SerializeField] private int CornPlantPrice;

    /// <summary>
    /// Precio de venta de la planta de zanahoria.
    /// </summary>
    [SerializeField] private int CarrotPlantPrice;

    /// <summary>
    /// Precio de venta de la planta de lechuga.
    /// </summary>
    [SerializeField] private int LettucePlantPrice;

    /// <summary>
    /// Precio de venta de la planta de fresa.
    /// </summary>
    [SerializeField] private int StrawberryPlantPrice;

    [Header("Precios de Mejora de Regadera")]
    /// <summary>
    /// Array con los precios de mejora de la regadera en cada nivel.
    /// </summary>
    [SerializeField] private int[] WateringCanUpgradePrices;


    [Header("Precios de Mejora de Huerto")]
    /// <summary>
    /// Array con los precios de mejora de la regadera en cada nivel.
    /// </summary>
    [SerializeField] private int[] GardenUpgradePrices;

    /// <summary>
    /// Referencia al UIManager
    /// </summary>
    [SerializeField] private UIManager UIManager;

    #endregion
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Instancia única de MoneyManager
    /// </summary>
    private static MoneyManager _instance;

        /// <summary>
        /// Nivel actual de mejora de la regadera.
        /// </summary>
    private int _wateringCanLevel = 0;

    /// <summary>
    /// Nivel actual de mejora del huerto.
    /// </summary>
    private int _gardenLevel = 0;

    #endregion

    /// <summary>
    /// Se llama al iniciar el script. Configura la instancia singleton
    /// y busca el GameManager en la escena.
    /// </summary>

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    private void Start()
    {
        GameManager.Instance.InitializeMoneyManager();
    }

    /// <summary>
    /// Método que se ejecuta cuando se carga una nueva escena.
    /// Se usa para actualizar la UI del dinero.
    /// </summary>
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Escena_Build")
        {
            UIManager.ShowMoneyUI();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    /// <summary>
    /// Propiedad para acceder a la instancia del LevelManager.
    /// </summary>
    public static MoneyManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Inicializar referencia UIManager
    /// </summary>
    public void InitializeUIManager()
    {
        UIManager = FindObjectOfType<UIManager>();
    }

    /// <summary>
    /// Establecer dinero inicial
    /// </summary>
    /// <param name="amount"></param>
    public void InitialMoney(int amount)
    {
        MoneyCount = amount;
    }

    // ---- MÉTODOS PARA VENDER CULTIVOS ----
    #region Métodos vender
    /// <summary>
    /// Vende una cantidad específica de lechugas.
    /// </summary>
    public void SellLettuce(int Quantity) => Sell(Quantity, LettucePlantPrice, Items.Lettuce);

    /// <summary>
    /// Vende una cantidad específica de maíz.
    /// </summary>
    public void SellCorn(int Quantity) => Sell(Quantity, CornPlantPrice, Items.Corn);

    /// <summary>
    /// Vende una cantidad específica de zanahorias.
    /// </summary>
    public void SellCarrot(int Quantity) => Sell(Quantity, CarrotPlantPrice, Items.Carrot);

    /// <summary>
    /// Vende una cantidad específica de fresas.
    /// </summary>
    public void SellStrawberry(int Quantity) => Sell(Quantity, StrawberryPlantPrice, Items.Strawberry);

    private void Sell(int Quantity, int Price, Items Item)
    {
        // Verifica si el jugador tiene suficientes cultivos en el inventory.
        if (InventoryManager.BoolModifyInventorySubstract(Item, Quantity))
        {
            AddMoney(Quantity * Price); // Agrega el dinero ganado por la venta
            Debug.Log($"Se han vendido {Quantity} {Item} por {Quantity * Price} RC.");
        }
        else
        {
            Debug.Log($"No tienes suficientes {Item} para vender.");
        }
    }

    #endregion

    // ---- MÉTODOS PARA COMPRAR SEMILLAS ----
    #region Métodos comprar
    /// <summary>
    /// Método genérico para comprar semillas
    /// </summary>
    /// <param name="item"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool BuySeed(Items item, int price)
    {
        // Verifica si el jugador tiene suficiente dinero
        if (MoneyCount >= price)
        {
            // Resta el dinero
            MoneyCount -= price;
            // Añade la semilla al inventory (siendo semilla un tipo de Item)
            InventoryManager.ModifyInventory(item, 1);

            // Actualiza la UI del dinero
            UIManager.ShowMoneyUI();

            // Debugging: Verifica si se compró correctamente
            Debug.Log($"Has comprado 1 {item} por {price} RC. Total dinero: {MoneyCount}");

            return true;
        }
        else
        {
            // Si no tiene suficiente dinero
            Debug.Log($"No tienes suficiente dinero para comprar {item}. Necesitas {price} RC.");
            return false;
        }
    }
    /// <summary>
    /// Método específico semilla maíz
    /// </summary>
    public void BuyCornSeed() => BuySeed(Items.CornSeed, CornSeedPrice);

    /// <summary>
    /// Método específico semilla zanahoria
    /// </summary>
    public void BuyCarrotSeed() => BuySeed(Items.CarrotSeed, CarrotSeedPrice);

    /// <summary>
    /// Método específico semilla lechuga
    /// </summary>
    public void BuyLettuceSeed() => BuySeed(Items.LettuceSeed, LettuceSeedPrice);

    /// <summary>
    /// Método específico semilla fresa
    /// </summary>
    public void BuyStrawberrySeed() => BuySeed(Items.StrawberrySeed, StrawberrySeedPrice);

    #endregion


    // ---- MÉTODOS PARA MODIFICAR EL DINERO ----
    #region Métodos Dinero

    /// <summary>
    /// Agrega una cantidad de dinero al jugador.
    /// </summary>
    public void AddMoney(int Quantity)
    {
        MoneyCount += Quantity;
        Debug.Log("Dinero agregado: " + Quantity + ". Total: " + MoneyCount);
        UIManager.ShowMoneyUI();
    }

    /// <summary>
    /// Resta una cantidad de dinero si el jugador tiene suficiente.
    /// </summary>
    public bool DeductMoney(int Quantity)
    {
        if (MoneyCount >= Quantity)
        {
            MoneyCount -= Quantity;
            Debug.Log("Dinero gastado: " + Quantity + ". Total: " + MoneyCount);
            UIManager.ShowMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
            return false;
        }
    }

    /// <summary>
    /// Obtiene la cantidad de dinero actual del jugador.
    /// </summary>
    public int GetMoneyCount()
    {
        return MoneyCount;
    }

    /// <summary>
    /// Modifica la cantidad de dinero (Solo se usa al cargar el juego).
    /// </summary>
    public void SetMoneyCount(int money)
    {
        MoneyCount = money;
    }
    #endregion

    // ---- MÉTODOS PARA LAS MEJORARS ----
    #region Mejoras

    // ---- REGADERA ----

    /// <summary>
    /// Método mejoras regadera
    /// </summary>
    /// <returns></returns>
    public bool UpgradeWateringCan()
    {
        if (_wateringCanLevel < WateringCanUpgradePrices.Length)
        {
            int Price = WateringCanUpgradePrices[_wateringCanLevel];

            if (DeductMoney(Price))
            {
                _wateringCanLevel++;
                Debug.Log("Regadera mejorada a nivel " + _wateringCanLevel);
                UIManager.ShowMoneyUI();
                return true;
            }
        }
        else
        {
            Debug.Log("La regadera ya está al nivel máximo.");
        }
        return false;
    }

    /// <summary>
    /// Método específico mejora regadera nivel 1
    /// </summary>
    /// <returns></returns>
    public bool UpgradeWateringCanLevel1() => UpgradeWateringCan();
    /// <summary>
    /// Método específico mejora regadera nivel 2
    /// </summary>
    /// <returns></returns>
    public bool UpgradeWateringCanLevel2() => UpgradeWateringCan();
    /// <summary>
    /// Método específico mejora regadera nivel 3
    /// </summary>
    /// <returns></returns>
    public bool UpgradeWateringCanLevel3() => UpgradeWateringCan();

    // ---- INVENTARIO ----

    /// <summary>
    /// Método mejoras huerto
    /// </summary>
    /// <returns></returns>
    public bool UpgradeGarden()
    {
        if (_gardenLevel < GardenUpgradePrices.Length)
        {
            int Price = GardenUpgradePrices[_gardenLevel];

            if (DeductMoney(Price))
            {
                _gardenLevel++;
                Debug.Log("Huerto mejorado a nivel " + _gardenLevel);
                UIManager.ShowMoneyUI();
                return true;
            }
        }
        else
        {
            Debug.Log("El Huerto ya está al nivel máximo.");
        }
        return false;
    }

    /// <summary>
    /// Método mejoras específicas huerto nivel 1
    /// </summary>
    /// <returns></returns>
    public bool UpgradeGardenLevel1() => UpgradeGarden();
    /// <summary>
    /// Método mejoras específicas huerto nivel 2
    /// </summary>
    /// <returns></returns>
    public bool UpgradeGardenLevel2() => UpgradeGarden();
    /// <summary>
    /// Método mejoras específicas huerto nivel 3
    /// </summary>
    /// <returns></returns>
    public bool UpgradeGardenLevel3() => UpgradeGarden();
    /// <summary>
    /// Método mejoras específicas huerto nivel 4
    /// </summary>
    /// <returns></returns>
    public bool UpgradeGardenLevel4() => UpgradeGarden();

    #endregion
    
    #endregion // Métodos públicos
}

