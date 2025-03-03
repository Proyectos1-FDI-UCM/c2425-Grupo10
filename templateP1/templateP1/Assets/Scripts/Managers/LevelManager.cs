//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín, Alexia Pérez Santana
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections.Generic; // Necesario para manejar el inventario

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
/// 
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas).
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int Herramienta; // Herramientas - Guantes = 1, Semillas = 5
    [SerializeField] int CantidadSemillas = 100; // Semillas
    [SerializeField] int AguaRegadera = 50; // Regadera (lleno)
    [SerializeField] GameObject PrefabSemilla1;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    /// <summary>
    /// Inventario de cultivos recolectados.
    /// </summary>
    private Dictionary<string, int> inventario = new Dictionary<string, int>();

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Init();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Metodos que devuelven y modifican el valor de las herramientas, semillas y regaderas
    /// </summary>
    public int Herramientas() { return Herramienta; }
    public int Semillas() { return CantidadSemillas; }
    public int Regadera() { return AguaRegadera; }
    public void CambioHerramienta(int i) { Herramienta = i; }
    public void LlenarRegadera(int i) { AguaRegadera = i; }
    public void Plantar() { CantidadSemillas--; }
    public void Regar() { AguaRegadera--; }
    public static bool HasInstance() { return _instance != null; }

    /// <summary>
    /// Agrega una planta al inventario cuando se recolecta.
    /// </summary>
    public void AgregarAlInventario(string nombrePlanta)
    {
        if (inventario.ContainsKey(nombrePlanta))
        {
            inventario[nombrePlanta]++;
        }
        else
        {
            inventario[nombrePlanta] = 1;
        }
        Debug.Log(nombrePlanta + " añadida al inventario. Cantidad: " + inventario[nombrePlanta]);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void Init()
    {
        inventario = new Dictionary<string, int>();
    }

    #endregion
}
