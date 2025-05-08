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
    [SerializeField] private int MoneyCount;

    /// <summary>
    /// Nivel actual de mejora de la regadera.
    /// </summary>
    private int WateringCanLevel = 0;

    /// <summary>
    /// Nivel actual de mejora del huerto.
    /// </summary>
    private int GardenLevel = 0;


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

    // ---- MÉTODOS PARA VENDER CULTIVOS ----

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

    public void InitializeUIManager()
    {
        UIManager = FindObjectOfType<UIManager>();
    }

    public void InitialMoney(int amount)
    {
        MoneyCount = amount;
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

    public void BuyCornSeed() => BuySeed(Items.CornSeed, CornSeedPrice);
    public void BuyCarrotSeed() => BuySeed(Items.CarrotSeed, CarrotSeedPrice);
    public void BuyLettuceSeed() => BuySeed(Items.LettuceSeed, LettuceSeedPrice);
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
    /// Modifica la cantidad de dinero (Solo se usa al cargar el juego).
    /// </summary>
    public void SetMoneyCount(int money)
    {
        MoneyCount = money;
    }


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

