//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MostrarInventario : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    // Prefabs cultivos
    [SerializeField] private GameObject MaizPrefab;
    [SerializeField] private GameObject LechugaPrefab;
    [SerializeField] private GameObject ZanahoriaPrefab;
    [SerializeField] private GameObject FresasPrefab;

    
    [SerializeField] private GameObject[] Casillas = new GameObject [24];
    [SerializeField] private int [] Cultivo = new int [24]; // Maiz(0), Lechuga (1), Zanahoria(2), Fresas(3)
    [SerializeField] private int [] Cantidad = new int [24];
    [SerializeField] private TextMeshProUGUI [] CantidadTexto = new TextMeshProUGUI [24];

    
    //private struct Inventario
    //{
    //    private GameObject[] Casillas;
    //    private int Cultivo; // Maiz(0), Lechuga (1), Zanahoria(2), Fresas(3)
    //    private int Cantidad;
    //    private TextMeshProUGUI CantidadTexto;
    //};



    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    
    private bool[] _casillaLlena = new bool [24];

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        // GetInventario del GameManager
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Falta el método para pasar el inventario a este script
        AgregarAlInventario(Casillas, Cultivo, Cantidad, CantidadTexto);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Instancia el prefab del cultivo
    /// </summary>
    private void InstanciarPrefab(int []cultivo)
    {
        for (int i = 0; i < cultivo.Length; i++)
        {
            if (cultivo[i] == 0) { Instantiate(MaizPrefab); }
            if (cultivo[i] == 1) { Instantiate(LechugaPrefab); }
            if (cultivo[i] == 2) { Instantiate(ZanahoriaPrefab); }
            if (cultivo[i] == 3) { Instantiate(FresasPrefab); }
        }
    }

    /// <summary>
    /// Añade el texto mostrando la cantidad
    /// </summary>
    private void MostrarTextoCantidad(int[] cantidad, TextMeshProUGUI[] CantidadTexto)
    {
        for (int i = 0; i < cantidad.Length; i++)
        {
            CantidadTexto[i].text = cantidad[i].ToString();
        }
    }


    /// <summary>
    /// Busca la primera casilla que no esté llena y muestra el cultivo
    /// </summary>
    private void AgregarAlInventario(GameObject [] casillas, int[] cultivo, int[] cantidad, TextMeshProUGUI[] texto)
    {
        bool casillaLlena = false;
        int i = 0;
        while (i < casillas.Length && !casillaLlena)
        {
            casillaLlena = IsCasillaLlena(casillas, cultivo, cantidad);
            InstanciarPrefab(cultivo);
            MostrarTextoCantidad(cantidad, texto);
            i++;
        }
    }

    private bool IsCasillaLlena(GameObject [] casillas, int[] cultivo, int[] cantidad)
    {
        bool casillaLlena = false;
        for (int i = 0; i < casillas.Length; i++)
        {
            if (cantidad[i] >= 10) casillaLlena = true;
        }
        return casillaLlena;
    }

    #endregion   

} // class MostrarInventario 
// namespace
