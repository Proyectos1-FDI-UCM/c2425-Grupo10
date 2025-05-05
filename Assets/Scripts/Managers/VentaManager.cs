//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín, Alexia Pérez Santana
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections.Generic; // Necesario para manejar el inventory
using UnityEngine.UI; // Necesario para manejar UI

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
public class VentaManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int Herramienta; // Tools - Guantes = 1, Seeds = 5
    [SerializeField] int CantidadSemillas = 100; // Seeds
    [SerializeField] int AguaRegadera = 50; // Regadera (lleno)
    [SerializeField] GameObject PrefabSemilla1;
    [SerializeField] ToolManager ToolManager;

    [SerializeField] private Text mensajeErrorTexto; // Referencia al objeto UI de texto


    int[] _sale = { 0, 0, 0, 0 };

    #endregion
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static VentaManager _instance;

    /// <summary>
    /// Inventory de cultivos recolectados.
    /// </summary>
    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    private int[] _inventory;

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

    void Start()
    {
        _inventory = GameManager.Instance.Inventory();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public static VentaManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }
    public static bool HasInstance() { return _instance != null; }

    /// <summary>
    /// Metodo para aumentar +1 los cultivos seleccionados (maiz).
    /// <summary>
    public void Sell(int i)
    {
        if (_sale[i] < _inventory[i])
        {
            _sale[i]++;
        }
    }

    /// <summary>
    /// Metodo para obtener el array de venta
    /// <summary>
    public int[] SellArray()
    {
        return _sale;
    }

    /// <summary>
    /// Muestra un mensaje de error en pantalla cuando el jugador intenta vender un cultivo que no tiene.
    /// </summary>
    /// <param name="mensaje">Texto del mensaje de error</param>
    public void MostrarMensajeError(string mensaje)
    {
        if (mensajeErrorTexto != null)
        {
            mensajeErrorTexto.text = mensaje;
            mensajeErrorTexto.gameObject.SetActive(true); // Asegura que el texto es visible
            CancelInvoke(nameof(OcultarMensajeError)); // Cancela cualquier temporizador previo
            Invoke(nameof(OcultarMensajeError), 2f); // Oculta el mensaje después de 2 segundos
        }
    }

    /// <summary>
    /// Oculta el mensaje de error después de un tiempo.
    /// </summary>
    private void OcultarMensajeError()
    {
        if (mensajeErrorTexto != null)
        {
            mensajeErrorTexto.gameObject.SetActive(false);
        }
    }


#endregion

// ---- MÉTODOS PRIVADOS ----
#region Métodos Privados

    private void Init()
    {
        inventory = new Dictionary<string, int>();
    }

    #endregion
}

