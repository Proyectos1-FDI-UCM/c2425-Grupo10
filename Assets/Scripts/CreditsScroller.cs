//---------------------------------------------------------
// Script que gestiona el desplazamiento automático de los 
// créditos en pantalla mediante un ScrollRect.
// Responsable de la creación de este archivo: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Clase encargada de controlar el movimiento automático del scroll
/// que muestra los créditos del juego. Al activarse, comienza a 
/// desplazar el contenido hacia abajo hasta que llega al final. 
/// Además, proporciona un método público para cerrar la pantalla 
/// de créditos desde un botón.
/// </summary>
public class CreditsScroller : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Referencia al componente ScrollRect que contiene el contenido de los créditos.
    /// Este objeto debe tener asignado su Content correctamente configurado.
    /// </summary>
    [SerializeField] private ScrollRect ScrollRect;

    /// <summary>
    /// Velocidad de desplazamiento vertical del contenido de los créditos.
    /// Cuanto mayor sea el valor, más lento será el scroll.
    /// </summary>
    [SerializeField] private float ScrollSpeed;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Indica si el scroll de créditos se está desplazando en ese momento.
    /// </summary>
    private bool _isScrolling = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta cuando el objeto que contiene este script se activa en la escena.
    /// Coloca el scroll en la parte superior y comienza el desplazamiento.
    /// </summary>
    void OnEnable()
    {
        Debug.Log("OnEnable llamado");
        ScrollRect.verticalNormalizedPosition = 1f; // Empieza desde arriba
        _isScrolling = true;
    }

    void Start()
    {
        ScrollRect.verticalNormalizedPosition = 1f;
        _isScrolling = true;
    }


    /// <summary>
    /// Se llama cada frame mientras este componente esté habilitado.
    /// Desplaza el contenido del scroll hacia abajo de forma progresiva.
    /// </summary>
    void Update()
    {
        Debug.Log("Scroll activo: " + _isScrolling);
        if (_isScrolling)
        {
            ScrollRect.verticalNormalizedPosition -= Time.deltaTime / ScrollSpeed;

            if (ScrollRect.verticalNormalizedPosition <= 0f)
            {
                _isScrolling = false;
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Detiene el desplazamiento y oculta la pantalla de créditos.
    /// Se debe enlazar este método al botón de "Volver al menú".
    /// </summary>
    public void CloseCredits()
    {
        gameObject.SetActive(false);
    }

    #endregion  

} // class CreditsScroller
