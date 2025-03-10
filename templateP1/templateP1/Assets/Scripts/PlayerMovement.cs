//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Este archivo contiene el comportamiento de movimiento del jugador
// Responsable de la creación de este archivo: Natalia Nita
// Nombre del juego: Roots Of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using

/// <summary>
/// Clase que controla el movimiento del jugador en el juego.
/// Utiliza un Rigidbody2D para mover al jugador de acuerdo con la entrada del usuario.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Velocidad a la que el jugador se mueve en el mundo 2D.
    /// Se puede ajustar desde el Inspector de Unity.
    /// </summary>
    [SerializeField] private float speed = 3f;

    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Referencia al Rigidbody2D del jugador para manejar la física.
    /// </summary>
    private Rigidbody2D playerRb;
    public bool enablemovement = true;

    /// <summary>
    /// Vector que contiene la entrada del jugador para el movimiento 
    /// </summary>
    private Vector2 moveInput;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama al comenzar el juego. Inicializa los componentes del jugador.
    /// </summary>
    void Start()
    {
        // Obtiene la referencia al Rigidbody2D del jugador.
        playerRb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Se llama una vez por cada fotograma. Se utiliza para capturar la entrada del jugador.
    /// </summary>
    void Update()
    {
        // Captura la entrada del jugador para los ejes horizontal y vertical.
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        // Normaliza la entrada para evitar movimiento diagonal más rápido.
        moveInput = new Vector2(moveX, moveY).normalized;

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Si en el futuro se añaden métodos públicos, deben ser documentados aquí.

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se llama en intervalos fijos de tiempo. Se utiliza para mover al jugador con base en la entrada.
    /// </summary>
    private void FixedUpdate()
    {
       if(enablemovement)
        {
            // Mueve al jugador según la entrada y la velocidad definida, ajustada al tiempo de cada frame.

            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);

        }
    }

    #endregion
} // class PlayerMovement 
// namespace
