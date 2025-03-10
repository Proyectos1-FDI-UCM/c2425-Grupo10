//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín, Alexia
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para manejar la UI del contador de dinero

/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Numero de mejoras activas de la Regadera.
    /// <summary>
    [SerializeField] int MejorasRegadera = 0;

    /// <summary>
    /// Numero de mejoras activas del Huerto.
    /// <summary>
    [SerializeField] int MejorasHuerto = 0;

    /// <summary>
    /// Numero de mejoras activas del Inventario.
    /// <summary>
    [SerializeField] int MejorasInventario = 0;

    /// <summary>
    /// Referencia al texto de la UI que muestra el dinero del jugador.
    /// </summary>
    [SerializeField] private Text contadorDineroText;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;

    /// <summary>
    /// Numero de máximo de mejoras para la Regadera.
    /// <summary>
    private int _maxMejorasRegadera = 3;

    /// <summary>
    /// Numero de máximo de mejoras para el Huerto.
    /// <summary>
    private int _maxMejorasHuerto = 4;

    /// <summary>
    /// Numero de máximo de mejoras para el Inventario.
    /// <summary>
    private int _maxMejorasInventario = 3;

    /// <summary>
    /// Numero de máximo de mejoras para el Inventario.
    /// <summary>
    private int _maxVenderMaiz = 3;

    /// <summary>
    /// Cantidad de dinero del jugador.
    /// </summary>
    private int dinero = 100; // Valor inicial de dinero

    private List<string> inventario = new List<string>();
    private int capacidadMaxima = 20;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al GameManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            TransferSceneState();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
        
        
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    /// <summary>
    /// Metodo para obtener la cantidad de mejoras que tiene la Regadera/Huerto/Inventario.
    /// <summary>
    public int GetMejorasRegadera() { return MejorasRegadera; }
    public int GetMejorasHuerto() { return MejorasHuerto; }
    public int GetMejorasInventario() { return MejorasInventario; }

    /// <summary>
    /// Metodo para aumentar +1 la mejora del Huerto.
    /// <summary>
    public void MejorarHuerto()
    {
        if (MejorasHuerto < _maxMejorasHuerto)
        {
            MejorasHuerto++;
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora del Inventario.
    /// <summary>
    public void MejorarInventario()
    {
        if (MejorasInventario < _maxMejorasInventario)
        {
            MejorasInventario++;
        }
    }

    /// <summary>
    /// Metodo para aumentar +1 la mejora de la Regadera.
    /// <summary>
    public void MejorarRegadera()
    {
        if (MejorasRegadera < _maxMejorasRegadera)
        {
            MejorasRegadera++;
        }
    }

    public bool Cosechado()
    {
        bool _cosechado = true;
        return _cosechado;
    }

    /// <summary>
    /// Obtiene la cantidad actual de dinero del jugador.
    /// </summary>
    /// <returns>Cantidad de dinero.</returns>
    public int GetDinero()
    {
        return dinero;
    }

    /// <summary>
    /// Añade una cantidad de dinero al jugador y actualiza la UI.
    /// </summary>
    /// <param name="cantidad">Cantidad de dinero a añadir.</param>
    public void AñadirDinero(int cantidad)
    {
        dinero += cantidad;
        ActualizarUI();
    }

    /// <summary>
    /// Resta una cantidad de dinero al jugador si tiene suficiente y actualiza la UI.
    /// </summary>
    /// <param name="cantidad">Cantidad de dinero a restar.</param>
    /// <returns>True si la operación fue exitosa, false si no había suficiente dinero.</returns>
    public bool RestarDinero(int cantidad)
    {
        if (dinero >= cantidad)
        {
            dinero -= cantidad;
            ActualizarUI();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Añade un objeto al inventario si hay espacio disponible.
    /// </summary>
    /// <param name="objeto">El objeto a añadir.</param>
    /// <returns>True si se añadió correctamente, false si el inventario está lleno.</returns>
    public bool AñadirObjeto(string objeto, int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (inventario.Count >= capacidadMaxima)
                return false; // Inventario lleno

            inventario.Add(objeto);
        }
        return true;
    }

    /// <summary>
    /// Remueve una cantidad específica de un objeto del inventario.
    /// </summary>
    /// <param name="objeto">El objeto a remover.</param>
    /// <param name="cantidad">La cantidad de objetos a remover.</param>
    /// <returns>True si se eliminaron los objetos correctamente, false si no hay suficientes.</returns>
    public bool RemoverObjeto(string objeto, int cantidad)
    {
        int contador = 0;

        // Intenta eliminar la cantidad solicitada del inventario
        for (int i = 0; i < cantidad; i++)
        {
            if (inventario.Contains(objeto))
            {
                inventario.Remove(objeto);
                contador++;
            }
            else
            {
                return false; // Si en algún momento no hay más objetos, la operación falla
            }
        }

        return contador == cantidad; // Retorna true solo si eliminó la cantidad completa
    }


    /// <summary>
    /// Actualiza la UI del contador de dinero.
    /// </summary>
    private void ActualizarUI()
    {
        if (contadorDineroText != null)
        {
            contadorDineroText.text = "$" + dinero.ToString();
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        ActualizarUI();
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    #endregion
} // class GameManager 
// namespace