//---------------------------------------------------------
// Este script maneja el Contador del dinero del jugador
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using System.Collections;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MoneyManager : MonoBehaviour
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
    [SerializeField] private TextMeshProUGUI mensajeErrorVenta; // Texto de error para mostrar mensajes

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private int _contador = 0;
    private int[] _inventario;

    private Coroutine _mensajeCoroutine; // Variable para manejar la visibilidad del mensaje
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
        _contador += 100000;
        _inventario = GameManager.Instance.Inventario();

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
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
        return _contador; 
    }

    public void RestarDinero (float cantidad)
    {
        _contador -= Convert.ToInt32(cantidad);
    }

    public void ComprarSemillasLechuga(int cantidad)
    {
        int precioTotal = cantidad * 15;
        if (_contador >= precioTotal)
        {
            _contador -= precioTotal;
            _inventario[1] += cantidad;
            ActualizarContador();
        }
    }

    public void LechugaVendida(int cantidad)
    {
        if (_inventario[1] >= cantidad)
        {
            int total = cantidad * 20;
            _contador += total;
            _inventario[1] -= cantidad;
            ActualizarContador();
        }
        else
        {
            VentaManager.Instance.MostrarMensajeError("No puedes vender este cultivo, no tienes suficientes.");
        }
    }
    
    public void ComprarSemillasFresas(int cantidad)
    {
        int precioTotal = cantidad * 30;
        if (_contador >= precioTotal)
        {
            _contador -= precioTotal;
            _inventario[3] += cantidad;
            ActualizarContador();
        }
    }

    public void FresaVendida(int cantidad)
    {
        if (_inventario[3] >= cantidad)
        {
            int total = cantidad * 40;
            _contador += total;
            _inventario[3] -= cantidad;
            ActualizarContador();
        }

        else
        {
            VentaManager.Instance.MostrarMensajeError("No puedes vender este cultivo, no tienes suficientes.");
        }
    }

    public void ComprarSemillasZanahoria(int cantidad)
    {
        int precioTotal = cantidad * 50;
        if (_contador >= precioTotal)
        {
            _contador -= precioTotal;
            _inventario[2] += cantidad;
            ActualizarContador();
        }
    
    }
    public void ZanahoriaVendida(int cantidad)
    {
        if (_inventario[2] >= cantidad)
        {
            int total = cantidad * 65;
            _contador += total;
            _inventario[2] -= cantidad;
            ActualizarContador();
        }

        else
        {
            VentaManager.Instance.MostrarMensajeError("No puedes vender este cultivo, no tienes suficientes.");
        }
    }

    public void ComprarSemillasMaiz(int cantidad)
    {
        int precioTotal = cantidad * 70;
        if (_contador >= precioTotal)
        {
            _contador -= precioTotal;
            _inventario[0] += cantidad;
            ActualizarContador();
        }
    }

    public void MaizVendido(int cantidad)
    {
        if (_inventario[0] >= cantidad)
        {
            int total = cantidad * 90;
            _contador += total;
            _inventario[0] -= cantidad;
            ActualizarContador();
        }

        else
        {
            VentaManager.Instance.MostrarMensajeError("No puedes vender este cultivo, no tienes suficientes.");
        }
    }
   
    public void ComprarAbono()
    {
        if (_contador >= 70)
        {
            _contador -= 70;
            _inventario[4] += 5;
        }
    }
   
    public void Mejora1Regadera()
    {
        if (_contador >= 1000)
        {
            _contador -= 1000;
        }
    }
    public void Mejora2Regadera()
    {
        if (_contador >= 5000)
        {
            _contador -= 5000;
        }
    }
    public void Mejora3Regadera()
    {
        if (_contador >= 10000)
        {
            _contador -= 10000;
        }
    }

    public void Comprar(int cantidad)
    {
        _contador -= cantidad;
        ActualizarContador();
    }


    public void ActualizarContador() // Cambiado a public
    {
        textoDinero.text = "x" + _contador; // Actualiza el texto con el valor del contador
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void MostrarMensajeError(string mensaje)
    { 
        if (_mensajeCoroutine != null)
        {
            StopCoroutine(_mensajeCoroutine);
        }
        _mensajeCoroutine = StartCoroutine(MostrarMensajeTemporal(mensaje));
    }

    private IEnumerator MostrarMensajeTemporal(string mensaje)
    {
        mensajeErrorVenta.text = mensaje;
        mensajeErrorVenta.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); // El mensaje se muestra por 2 segundos
        mensajeErrorVenta.gameObject.SetActive(false);
    }
    #endregion

} // class ContadorDinero 
  // namespace