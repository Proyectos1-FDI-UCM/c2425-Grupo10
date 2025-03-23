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

    [SerializeField] private int contadorDinero = 100000;
    private int nivelRegadera = 0;

    [Header("Precios de Semillas")]
    [SerializeField] private int precioSemillaMaiz = 50;
    [SerializeField] private int precioSemillaZanahoria = 20;
    [SerializeField] private int precioSemillaLechuga = 15;
    [SerializeField] private int precioSemillaFresa = 30;

    [Header("Precios de Venta de Plantas")]
    [SerializeField] private int precioPlantaMaiz = 90;
    [SerializeField] private int precioPlantaZanahoria = 65;
    [SerializeField] private int precioPlantaLechuga = 20;
    [SerializeField] private int precioPlantaFresa = 40;

    [Header("Precios de Mejora de Regadera")]
    [SerializeField] private int[] preciosMejoraRegadera = { 1000, 5000, 10000 };

    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI dineroTexto;

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
        ActualizarUI();

        if (gameManager == null)
            Debug.LogWarning("GameManager no asignado en el Inspector.");

        if (dineroTexto == null)
            Debug.LogWarning("No se ha asignado el texto de dinero en la UI.");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ActualizarUI();
    }

    // Métodos para vender cultivos
    public void VenderLechuga(int cantidad) => Vender(cantidad, precioPlantaLechuga, Items.Letuce);
    public void VenderMaiz(int cantidad) => Vender(cantidad, precioPlantaMaiz, Items.Corn);
    public void VenderZanahoria(int cantidad) => Vender(cantidad, precioPlantaZanahoria, Items.Carrot);
    public void VenderFresa(int cantidad) => Vender(cantidad, precioPlantaFresa, Items.Strawberry);

    private void Vender(int cantidad, int precio, Items item)
    {
        if (gameManager == null) return;

        // Verificar si el jugador tiene la cantidad suficiente en el inventario
        if (InventoryManager.GetInventory(item) >= cantidad)
        {
            InventoryManager.ModifyInventory(item, -cantidad); // Restar cultivos del inventario
            AgregarDinero(cantidad * precio);
            Debug.Log($"Se han vendido {cantidad} {item} por {cantidad * precio} RC.");
        }
        else
        {
            Debug.Log($"No tienes suficientes {item} para vender.");
        }
    }

    // Método para comprar semillas
    public bool ComprarSemilla(Items item, int precio)
    {
        if (RestarDinero(precio))
        {
            InventoryManager.ModifyInventory(item, 1);
            Debug.Log($"Has comprado 1 {item} por {precio} RC.");
            return true;
        }
        else
        {
            Debug.Log($"No tienes suficiente dinero para comprar {item}.");
            return false;
        }
    }

    // Métodos específicos para comprar semillas
    public void ComprarSemillaMaiz() => ComprarSemilla(Items.CornSeed, precioSemillaMaiz);
    public void ComprarSemillaZanahoria() => ComprarSemilla(Items.CarrotSeed, precioSemillaZanahoria);
    public void ComprarSemillaLechuga() => ComprarSemilla(Items.LetuceSeed, precioSemillaLechuga);
    public void ComprarSemillaFresa() => ComprarSemilla(Items.StrawberrySeed, precioSemillaFresa);

    public void AgregarDinero(int cantidad)
    {
        contadorDinero += cantidad;
        Debug.Log("Dinero agregado: " + cantidad + ". Total: " + contadorDinero);
        ActualizarUI();
    }

    public bool RestarDinero(int cantidad)
    {
        if (contadorDinero >= cantidad)
        {
            contadorDinero -= cantidad;
            Debug.Log("Dinero gastado: " + cantidad + ". Total: " + contadorDinero);
            ActualizarUI();
            return true;
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
            return false;
        }
    }

    public int GetContadorDinero()
    {
        return contadorDinero;
    }

    private void ActualizarUI()
    {
        GameObject textoContadorObj = GameObject.FindGameObjectWithTag("TextoContador");

        if (textoContadorObj != null)
        {
            TextMeshProUGUI textoContador = textoContadorObj.GetComponent<TextMeshProUGUI>();

            if (textoContador != null)
            {
                textoContador.text = contadorDinero.ToString("F0");
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

    public bool MejorarRegadera()
    {
        if (nivelRegadera < preciosMejoraRegadera.Length)
        {
            int precio = preciosMejoraRegadera[nivelRegadera];

            if (RestarDinero(precio))
            {
                nivelRegadera++;
                Debug.Log("Regadera mejorada a nivel " + nivelRegadera);
                return true;
            }
        }
        else
        {
            Debug.Log("La regadera ya está al nivel máximo.");
        }
        return false;
    }

    public bool Mejora1Regadera() => MejorarRegadera();
    public bool Mejora2Regadera() => MejorarRegadera();
    public bool Mejora3Regadera() => MejorarRegadera();
}
