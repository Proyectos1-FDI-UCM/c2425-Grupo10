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
    [SerializeField] GameManager GameManager;
    [SerializeField] ToolManager ToolManager;

    [SerializeField] int ActualMaiz = 0; // Maiz que quieres vender
    [SerializeField] int ActualLechuga = 0;
    [SerializeField] int ActualZanahoria = 0;
    [SerializeField] int ActualFresas = 0;

    [SerializeField] int MaxMaiz = 0; // Cantidad de maiz que hay en el inventario
    [SerializeField] int MaxLechuga = 0;
    [SerializeField] int MaxZanahoria = 0;
    [SerializeField] int MaxFresas = 0;
    [SerializeField] int MaxAgua;
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
        if (GameManager == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("GameManager");
            if (ObjetoTexto != null)
            {
                GameManager = ObjetoTexto.GetComponent<GameManager>();
            }
        }
        GetMejorasRegadera();
        ToolManager.BarraAgua(AguaRegadera, MaxAgua);
        AguaRegadera = GameManager.Instance.LastWaterAmount();

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
    public void LlenarRegadera(int i)
    {
        AguaRegadera = i;
        ToolManager.BarraAgua(AguaRegadera, MaxAgua);
        GameManager.Instance.UpdateWaterAmount();
    }
    public void Plantar() { CantidadSemillas--; }
    public void Regar() 
    { 
        AguaRegadera--;
        ToolManager.BarraAgua(AguaRegadera, MaxAgua);
        GameManager.Instance.UpdateWaterAmount();
    }
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
       GuardarCultivos(nombrePlanta, inventario[nombrePlanta]);
        Debug.Log(nombrePlanta + " añadida al inventario. Cantidad: " + inventario[nombrePlanta]);
    }

    public Dictionary <string, int> GetInventario()
    {
        return inventario;
    }

       public (string, int) GuardarCultivos(string nombre, int num)
       {
           return (nombre, num);
    
      }


    public int GetVenderMaiz() { return ActualMaiz; }
    public int GetVenderLechuga() { return ActualLechuga; }
    public int GetVenderZanahoria() { return ActualZanahoria; }
    public int GetVenderFresas() { return ActualMaiz; }
    public int GetMaxAgua() { return MaxAgua; }


    /// <summary>
    /// Metodo para aumentar +1 los cultivos seleccionados (maiz).
    /// <summary>
    public void VenderMaiz()
    {
        if (ActualMaiz < MaxMaiz)
        {
            ActualMaiz++;
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 los cultivos seleccionados (lechuga).
    /// <summary>
    public void VenderLechuga()
    {
        if (ActualLechuga < MaxLechuga)
        {
            ActualLechuga++;
        }
    }
    /// <summary>
    /// Metodo para aumentar +1 los cultivos seleccionados (zanahoria).
    /// <summary>
    public void VenderZanahoria()
    {
        if (ActualZanahoria < MaxZanahoria)
        {
            ActualZanahoria++;
        }
    }
    /// <summary>
    /// Metodo para aumentar +1 los cultivos seleccionados (fresas).
    /// <summary>
    public void VenderFresas()
    {
        if (ActualFresas < MaxFresas)
        {
            ActualFresas++;
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 los cultivos seleccionados (fresas).
    /// <summary>
    public void Mejora0Regadera()
    {
        MaxAgua = 6;
    }
    public void Mejora1Regadera()
    {
        MaxAgua = 9;
    }
    public void Mejora2Regadera()
    {
        MaxAgua = 15;
    }
    public void Mejora3Regadera()
    {
        MaxAgua = 20;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void Init()
    {
        inventario = new Dictionary<string, int>();
    }
    private void GetMejorasRegadera()
    {
        if((GameManager.GetMejorasRegadera() == 0)) 
        {
            Mejora0Regadera();
        }
        if ((GameManager.GetMejorasRegadera() == 1))
        {
            Mejora1Regadera();
        }
        if ((GameManager.GetMejorasRegadera() == 2))
        {
            Mejora2Regadera();
        }
        if ((GameManager.GetMejorasRegadera() == 3))
        {
            Mejora3Regadera();
        }
    }
    #endregion
}
