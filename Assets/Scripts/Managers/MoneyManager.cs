//---------------------------------------------------------
// Este script maneja el Contador del dinero del jugador
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] private int moneyCount = 100000;
    private int wateringCanLevel = 0;

    [Header("Precios de Seeds")]
    [SerializeField] private int cornSeedPrice = 50;
    [SerializeField] private int carrotSeedPrice = 20;
    [SerializeField] private int lettuceSeedPrice = 15;
    [SerializeField] private int strawberrySeedPrice = 30;

    [Header("Precios de Venta de Plantas")]
    [SerializeField] private int cornPlantPrice = 90;
    [SerializeField] private int carrotPlantPrice = 65;
    [SerializeField] private int lettucePlantPrice = 20;
    [SerializeField] private int strawberryPlantPrice = 40;

    [Header("Precios de Mejora de Regadera")]
    [SerializeField] private int[] wateringCanUpgradePrices = { 1000, 5000, 10000 };

    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();

        if (gameManager == null)
            Debug.LogWarning("GameManager no asignado en el Inspector.");

        if (moneyText == null)
            Debug.LogWarning("No se ha asignado el texto de dinero en la UI.");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUI();
    }

    // Métodos para vender cultivos
    public void SellLettuce(int quantity) => Sell(quantity, lettucePlantPrice, Items.Letuce);
    public void SellCorn(int quantity) => Sell(quantity, cornPlantPrice, Items.Corn);
    public void SellCarrot(int quantity) => Sell(quantity, carrotPlantPrice, Items.Carrot);
    public void SellStrawberry(int quantity) => Sell(quantity, strawberryPlantPrice, Items.Strawberry);

    private void Sell(int quantity, int price, Items item)
    {
        if (gameManager == null) return;

        // Verificar si el jugador tiene la cantidad suficiente en el inventario
        if (InventoryManager.GetInventory(item) >= quantity)
        {
            InventoryManager.ModifyInventory(item, -quantity); // Restar cultivos del inventario
            AddMoney(quantity * price);
            Debug.Log($"Se han vendido {quantity} {item} por {quantity * price} RC.");
        }
        else
        {
            Debug.Log($"No tienes suficientes {item} para vender.");
        }
    }

    // Método para comprar semillas
    public bool BuySeed(Items item, int price)
    {
        if (DeductMoney(price))
        {
            InventoryManager.ModifyInventory(item, 1);
            Debug.Log($"Has comprado 1 {item} por {price} RC.");
            return true;
        }
        else
        {
            Debug.Log($"No tienes suficiente dinero para comprar {item}.");
            return false;
        }
    }

    // Métodos específicos para comprar semillas
    public void BuyCornSeed() => BuySeed(Items.CornSeed, cornSeedPrice);
    public void BuyCarrotSeed() => BuySeed(Items.CarrotSeed, carrotSeedPrice);
    public void BuyLettuceSeed() => BuySeed(Items.LetuceSeed, lettuceSeedPrice);
    public void BuyStrawberrySeed() => BuySeed(Items.StrawberrySeed, strawberrySeedPrice);

    public void AddMoney(int quantity)
    {
        moneyCount += quantity;
        Debug.Log("Dinero agregado: " + quantity + ". Total: " + moneyCount);
        UpdateUI();
    }

    public bool DeductMoney(int quantity)
    {
        if (moneyCount >= quantity)
        {
            moneyCount -= quantity;
            Debug.Log("Dinero gastado: " + quantity + ". Total: " + moneyCount);
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
            return false;
        }
    }

    public int GetMoneyCount()
    {
        return moneyCount;
    }

    private void UpdateUI()
    {
        GameObject moneyCounterObj = GameObject.FindGameObjectWithTag("TextoContador");

        if (moneyCounterObj != null)
        {
            TextMeshProUGUI moneyCounterText = moneyCounterObj.GetComponent<TextMeshProUGUI>();

            if (moneyCounterText != null)
            {
                moneyCounterText.text = moneyCount.ToString("F0");
            }
            else
            {
                Debug.LogError("No se encontró el componente TextMeshProUGUI en el objeto con tag 'TextoContador'.");
            }
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'TextoContador'.");
        }
    }

    public bool UpgradeWateringCan()
    {
        if (wateringCanLevel < wateringCanUpgradePrices.Length)
        {
            int price = wateringCanUpgradePrices[wateringCanLevel];

            if (DeductMoney(price))
            {
                wateringCanLevel++;
                Debug.Log("Regadera mejorada a nivel " + wateringCanLevel);
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
}
