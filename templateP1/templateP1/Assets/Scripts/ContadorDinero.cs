//---------------------------------------------------------
// Este script maneja el Contador del dinero del jugador
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ContadorDinero : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI textoDinero;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private int Contador = 0;
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
        Contador += 100000;

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    ComprarSemillasLechuga();
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    LechugaVendida();
        //}


        ActualizarContador();
        //Debug.Log(Contador);
        if (textoDinero == null)
        {
            GameObject ObjetoTexto = GameObject.FindGameObjectWithTag("TextoContador");
            if (ObjetoTexto != null)
            {
                textoDinero = ObjetoTexto.GetComponent<TextMeshProUGUI>();
            }
        }

    }
    #endregion
    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public int GetContadorDinero()
    { 
        return Contador; 
    }
    public void ComprarSemillasLechuga()
    {
        if (Contador >= 15)
        {
            Contador -= 15;
            //aqui tengo que añadir que se añadan 5 al inventario


        }
    }
    public void LechugaVendida(int cantidad)
    {
        int total = cantidad * 20;
        Contador += total;
        ActualizarContador();
    }
    //-------------------------
    public void ComprarSemillasFresas()
    {
        if (Contador >= 30)
        {
            Contador -= 30;
            //aqui tengo que añadir que se añadan 5 al inventario

        }
    }
    public void FresaVendida(int cantidad)
    {
        int total = cantidad * 40;
        Contador += total;
        ActualizarContador();
    }
    //-------------------------
    public void ComprarSemillasZanahoria()
    {
        if (Contador >= 50)
        {
            Contador -= 50;
            //aqui tengo que añadir que se añadan 5 al inventario

        }
    }
    public void ZanahoriaVendida(int cantidad)
    {
        int total = cantidad * 65;
        Contador += total;
        ActualizarContador();
    }
    //-------------------------
    public void ComprarSemillasMaiz()
    {
        if (Contador >= 70)
        {
            Contador -= 70;
            //aqui tengo que añadir que se añadan 5 al inventario
        }
    }
    public void MaizVendido(int cantidad)
    {
        int total = cantidad * 90;
        Contador += total;
        ActualizarContador();
    }
    //-------------------------
    public void ComprarAbono()
    {
        if (Contador >= 70)
        {
            Contador -= 70;
            //aqui tengp que añadir que se añadan 5 al inventario
        }
        else
        {

        }
    }
    //------------------------
    public void Mejora1Regadera()
    {
        if (Contador >= 1000)
        {
            Contador -= 1000;
        }
    }
    public void Mejora2Regadera()
    {
        if (Contador >= 5000)
        {
            Contador -= 5000;
        }
    }
    public void Mejora3Regadera()
    {
        if (Contador >= 10000)
        {
            Contador -= 10000;
        }
    }


    public void ActualizarContador() // Cambiado a public
    {
        textoDinero.text = "x" + Contador; // Actualiza el texto con el valor del contador
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ContadorDinero 
  // namespace