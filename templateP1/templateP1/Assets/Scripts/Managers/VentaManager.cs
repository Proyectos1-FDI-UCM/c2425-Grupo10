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
public class VentaManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int Herramienta; // Herramientas - Guantes = 1, Semillas = 5
    [SerializeField] int CantidadSemillas = 100; // Semillas
    [SerializeField] int AguaRegadera = 50; // Regadera (lleno)
    [SerializeField] GameObject PrefabSemilla1;
    [SerializeField] GameManager GameManager;
    [SerializeField] ToolManager ToolManager;

    int[] _venta = { 0, 0, 0, 0 };

    #endregion
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static VentaManager _instance;

    /// <summary>
    /// Inventario de cultivos recolectados.
    /// </summary>
    private Dictionary<string, int> inventario = new Dictionary<string, int>();
    private int[] _inventario;

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
        if (GameManager == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoTexto != null)
            {
                GameManager = ObjetoTexto.GetComponent<GameManager>();
            }
        }
    }

    void start()
    {
        _inventario = GameManager.Instance.Inventario();
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
    public void Vender(int i)
    {
        if (_venta[i] < _inventario[i])
        {
            _venta[i]++;
        }
    }

    /// <summary>
    /// Metodo para obtener el array de venta
    /// <summary>
    public int[] VenderArray()
    {
        return _venta;
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

