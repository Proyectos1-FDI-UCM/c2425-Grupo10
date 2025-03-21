//---------------------------------------------------------
// Este script maneja el Contador del dinero del jugador
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para manejar el cambio de escenas

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;  // Instancia estática para Singleton

    [SerializeField] private int contadorDinero = 10000; // Cantidad de dinero del jugador
    private int nivelRegadera = 0; // Nivel actual de la regadera

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

    [SerializeField] private GameManager gameManager; // Se asignará desde el Inspector
    [SerializeField] private TextMeshProUGUI dineroTexto; // Texto en UI para mostrar el dinero

    private void Awake()
    {
        // Verificar si ya existe una instancia del MoneyManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // No destruir el objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject);  // Destruir el nuevo objeto si ya hay uno persistente
        }
    }

    private void Start()
    {
        ActualizarUI();

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager no asignado en el Inspector.");
        }

        if (dineroTexto == null)
        {
            Debug.LogWarning("No se ha asignado el texto de dinero en la UI.");
        }

        // Asegurar que el texto se actualice cuando la escena se carga
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Actualizar la interfaz cuando se carga una nueva escena
        ActualizarUI();
    }

    public void VenderLechuga(int cantidad)
    {
        if (gameManager == null) return;

        int precio = precioPlantaLechuga;
        AgregarDinero(cantidad * precio);
        Debug.Log($"Se han vendido {cantidad} lechugas por {cantidad * precio} RC.");
    }

    public void VenderMaiz(int cantidad)
    {
        if (gameManager == null) return;

        int precio = precioPlantaMaiz;
        AgregarDinero(cantidad * precio);
        Debug.Log($"Se han vendido {cantidad} maíces por {cantidad * precio} RC.");
    }

    public void VenderZanahoria(int cantidad)
    {
        if (gameManager == null) return;

        int precio = precioPlantaZanahoria;
        AgregarDinero(cantidad * precio);
        Debug.Log($"Se han vendido {cantidad} zanahorias por {cantidad * precio} RC.");
    }

    public void VenderFresa(int cantidad)
    {
        if (gameManager == null) return;

        int precio = precioPlantaFresa;
        AgregarDinero(cantidad * precio);
        Debug.Log($"Se han vendido {cantidad} fresas por {cantidad * precio} RC.");
    }

    public bool Comprar(int cantidad)
    {
        if (gameManager == null) return false;
        return RestarDinero(cantidad);
    }

    public void Vender(int cantidad)
    {
        if (gameManager == null) return;
        AgregarDinero(cantidad);
    }

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
        // Buscar el objeto con el tag "TextoContador" y actualizar el texto
        GameObject textoContadorObj = GameObject.FindGameObjectWithTag("TextoContador");

        // Verificar si el objeto con el tag "TextoContador" fue encontrado
        if (textoContadorObj != null)
        {
            // Obtener el componente TextMeshProUGUI
            TextMeshProUGUI textoContador = textoContadorObj.GetComponent<TextMeshProUGUI>();

            // Si el componente se encuentra, actualizar el texto con el dinero disponible
            if (textoContador != null)
            {
                textoContador.text = contadorDinero.ToString("F0") ;  // Mostrar dinero sin decimales
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

    // Método principal para mejorar la regadera
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

    // Métodos específicos para que otros scripts puedan llamarlos
    public bool Mejora1Regadera() => MejorarRegadera();
    public bool Mejora2Regadera() => MejorarRegadera();
    public bool Mejora3Regadera() => MejorarRegadera();
}
