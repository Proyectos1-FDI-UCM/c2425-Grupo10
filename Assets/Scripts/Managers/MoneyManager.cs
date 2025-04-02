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
    /// <summary>
    /// Instancia única de MoneyManager (patrón Singleton).
    /// Permite acceder a MoneyManager desde cualquier parte del código.
    /// </summary>
    public static MoneyManager Instance;

    /// <summary>
    /// Cantidad total de dinero del jugador.
    /// </summary>
    [SerializeField] private int MoneyCount = 100000;

    /// <summary>
    /// Nivel actual de mejora de la regadera.
    /// </summary>
    private int WateringCanLevel = 0;

    /// <summary>
    /// Nivel actual de mejora del huerto.
    /// </summary>
    private int GardenLevel = 0;

    /// <summary>
    /// Nivel actual de mejora del inventario.
    /// </summary>
    private int InventoryLevel = 0;


    [Header("Precios de Semillas")]
    /// <summary>
    /// Precio de la semilla de maíz.
    /// </summary>
    [SerializeField] private int CornSeedPrice = 50;

    /// <summary>
    /// Precio de la semilla de zanahoria.
    /// </summary>
    [SerializeField] private int CarrotSeedPrice = 20;

    /// <summary>
    /// Precio de la semilla de lechuga.
    /// </summary>
    [SerializeField] private int LettuceSeedPrice = 15;

    /// <summary>
    /// Precio de la semilla de fresa.
    /// </summary>
    [SerializeField] private int StrawberrySeedPrice = 30;


    [Header("Precios de Venta de Plantas")]
    /// <summary>
    /// Precio de venta de la planta de maíz.
    /// </summary>
    [SerializeField] private int CornPlantPrice = 90;

    /// <summary>
    /// Precio de venta de la planta de zanahoria.
    /// </summary>
    [SerializeField] private int CarrotPlantPrice = 65;

    /// <summary>
    /// Precio de venta de la planta de lechuga.
    /// </summary>
    [SerializeField] private int LettucePlantPrice = 20;

    /// <summary>
    /// Precio de venta de la planta de fresa.
    /// </summary>
    [SerializeField] private int StrawberryPlantPrice = 40;


    [Header("Precios de Mejora de Regadera")]
    /// <summary>
    /// Array con los precios de mejora de la regadera en cada nivel.
    /// </summary>
    [SerializeField] private int[] WateringCanUpgradePrices = { 1000, 5000, 10000 };


    [Header("Precios de Mejora de Huerto")]
    /// <summary>
    /// Array con los precios de mejora de la regadera en cada nivel.
    /// </summary>
    [SerializeField] private int[] GardenUpgradePrices = { 5000, 10000, 15000, 20000, 30000 };


    [Header("Precios de Mejora de Inventario")]
    /// <summary>
    /// Array con los precios de mejora de la regadera en cada nivel.
    /// </summary>
    [SerializeField] private int[] InventoryUpgradePrices = { 2000, 5000 };

    /// <summary>
    /// Referencia al GameManager del juego.
    /// </summary>
    [SerializeField] private GameManager GameManager;

    /// <summary>
    /// Referencia al UIManager
    /// </summary>
    [SerializeField] private UIManager UIManager;



    private void Awake()
    {

        GameManager.InitializeMoneyManager();
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Asegurar que el dinero se muestra al iniciar
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        UIManager.ShowMoneyUI(); // Llamada extra para asegurar la actualización

        // carga de escenas para actualizar la UI
       // SceneManager.sceneLoaded += OnSceneLoaded;
    }


    /// <summary>
    /// Método que se ejecuta cuando se carga una nueva escena.
    /// Se usa para actualizar la UI del dinero.
    /// </summary>
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    UiManager.ShowMoneyUI();
    //}

    // ---- MÉTODOS PARA VENDER CULTIVOS ----

    /// <summary>
    /// Vende una cantidad específica de lechugas.
    /// </summary>
    public void SellLettuce(int Quantity) => Sell(Quantity, LettucePlantPrice, Items.Letuce);

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
        if (GameManager == null) return;

        // Verifica si el jugador tiene suficientes cultivos en el inventario.
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

    public void InitializeUIManager()
    {
        UIManager = FindObjectOfType<UIManager>();
    }


    // ---- MÉTODOS PARA COMPRAR SEMILLAS ----
    // Método genérico para comprar semillas
    public bool BuySeed(Items item, int price)
    {
        // Verifica si el jugador tiene suficiente dinero
        if (MoneyCount >= price)
        {
            // Resta el dinero
            MoneyCount -= price;
            // Añade la semilla al inventario (siendo semilla un tipo de Item)
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

    public void BuyCornSeed() => BuySeed(Items.CornSeed, CornSeedPrice);
    public void BuyCarrotSeed() => BuySeed(Items.CarrotSeed, CarrotSeedPrice);
    public void BuyLettuceSeed() => BuySeed(Items.LetuceSeed, LettuceSeedPrice);
    public void BuyStrawberrySeed() => BuySeed(Items.StrawberrySeed, StrawberrySeedPrice);




    // ---- MÉTODOS PARA MODIFICAR EL DINERO ----

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
    /// Actualiza la UI con la cantidad de dinero actual.
    /// </summary>
    //private void UpdateUI()
    //{
    //    GameObject MoneyCounterObj = GameObject.FindGameObjectWithTag("TextoContador");

    //    if (MoneyCounterObj != null)
    //    {
    //        TextMeshProUGUI MoneyCounterText = MoneyCounterObj.GetComponent<TextMeshProUGUI>();

    //        if (MoneyCounterText != null)
    //        {
    //            MoneyCounterText.text = "x" + MoneyCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.LogError("No se encontró el componente TextMeshProUGUI en el objeto con tag 'TextoContador'.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("No se encontró un objeto con el tag 'TextoContador'.");
    //    }
    //}

    // ---- MÉTODOS PARA MEJORAR LA REGADERA ----

    public bool UpgradeWateringCan()
    {
        if (WateringCanLevel < WateringCanUpgradePrices.Length)
        {
            int Price = WateringCanUpgradePrices[WateringCanLevel];

            if (DeductMoney(Price))
            {
                WateringCanLevel++;
                Debug.Log("Regadera mejorada a nivel " + WateringCanLevel);
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

    public bool UpgradeWateringCanLevel1() => UpgradeWateringCan();
    public bool UpgradeWateringCanLevel2() => UpgradeWateringCan();
    public bool UpgradeWateringCanLevel3() => UpgradeWateringCan();

    // ---- MÉTODOS PARA MEJORAR EL INVENTARIO ----

    public bool UpgradeInventory()
    {
        if (InventoryLevel < InventoryUpgradePrices.Length)
        {
            int Price = InventoryUpgradePrices[InventoryLevel];

            if (DeductMoney(Price))
            {
                InventoryLevel++;
                Debug.Log("Inventario mejorado a nivel " + InventoryLevel);
                return true;
            }
        }
        else
        {
            Debug.Log("El inventario ya está al nivel máximo.");
        }
        return false;
    }

    public bool UpgradeInventoryLevel1() => UpgradeInventory();
    public bool UpgradeInventoryLevel2() => UpgradeInventory();

    // ---- MÉTODOS PARA MEJORAR EL INVENTARIO ----

    public bool UpgradeGarden()
    {
        if (GardenLevel < GardenUpgradePrices.Length)
        {
            int Price = GardenUpgradePrices[GardenLevel];

            if (DeductMoney(Price))
            {
                GardenLevel++;
                Debug.Log("Huerto mejorado a nivel " + GardenLevel);
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

    public bool UpgradeGardenLevel1() => UpgradeGarden();
    public bool UpgradeGardenLevel2() => UpgradeGarden();
    public bool UpgradeGardenLevel3() => UpgradeGarden();
    public bool UpgradeGardenLevel4() => UpgradeGarden();

}

