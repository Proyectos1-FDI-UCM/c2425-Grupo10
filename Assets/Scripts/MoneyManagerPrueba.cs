//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine;

public class MoneyManagerPrueba : MonoBehaviour
{
    [SerializeField] private int contadorDinero = 10000; // Cantidad de dinero del jugador
    private int nivelRegadera = 0; // Nivel actual de la regadera

    // Precios de semillas (modificables en el Inspector)
    [Header("Precios de Semillas")]
    [SerializeField] private int precioSemillaMaiz = 50;
    [SerializeField] private int precioSemillaZanahoria = 20;
    [SerializeField] private int precioSemillaLechuga = 15;
    [SerializeField] private int precioSemillaFresa = 30;

    // Precios de las plantas (cuando se venden)
    [Header("Precios de Venta de Plantas")]
    [SerializeField] private int precioPlantaMaiz = 90;
    [SerializeField] private int precioPlantaZanahoria = 65;
    [SerializeField] private int precioPlantaLechuga = 20;
    [SerializeField] private int precioPlantaFresa = 40;

    // Precios de mejoras de regadera
    [SerializeField] private int[] preciosMejoraRegadera = { 1000, 5000, 10000 };

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager no encontrado en la escena. Algunas funciones pueden no funcionar correctamente.");
        }
    }

    public void LechugaVendida(int cantidad)
    {
        if (gameManager == null) return;

        int precioLechuga = 20;
        AgregarDinero(cantidad * precioLechuga);
        Debug.Log($"Se han vendido {cantidad} lechugas por {cantidad * precioLechuga} RC.");
    }

    public void MaizVendido(int cantidad)
    {
        if (gameManager == null) return;

        int precioMaiz = 90;
        AgregarDinero(cantidad * precioMaiz);
        Debug.Log($"Se han vendido {cantidad} maíces por {cantidad * precioMaiz} RC.");
    }

    public void ZanahoriaVendida(int cantidad)
    {
        if (gameManager == null) return;

        int precioZanahoria = 65;
        AgregarDinero(cantidad * precioZanahoria);
        Debug.Log($"Se han vendido {cantidad} zanahorias por {cantidad * precioZanahoria} RC.");
    }

    public void FresaVendida(int cantidad)
    {
        if (gameManager == null) return;

        int precioFresa = 40;
        AgregarDinero(cantidad * precioFresa);
        Debug.Log($"Se han vendido {cantidad} fresas por {cantidad * precioFresa} RC.");
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
    }

    public bool RestarDinero(int cantidad)
    {
        if (contadorDinero >= cantidad)
        {
            contadorDinero -= cantidad;
            Debug.Log("Dinero gastado: " + cantidad + ". Total: " + contadorDinero);
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


}

