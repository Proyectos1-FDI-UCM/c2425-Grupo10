//---------------------------------------------------------
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
    [SerializeField] private float Speed = 3f;

    /// <summary>
    /// Referencia al Watering Can Manager
    /// </summary>
    [SerializeField] WateringCanManager WateringCanManager;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Referencia al Rigidbody2D del jugador para manejar la física.
    /// </summary>
    private Rigidbody2D _playerRb;

    /// <summary>
    /// Referencia al Animator del jugador para manejar las animaciones.
    /// </summary>
    private Animator _playerAnimator;

    /// <summary>
    /// Referencia al SpriteRenderer del jugador para manejar la representación gráfica.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Booleano que indica si el movimiento del jugador está habilitado.
    /// </summary>
    private bool _movementenabled = true;

    /// <summary>
    /// Vector que contiene la entrada del jugador para el movimiento 
    /// </summary>
    private Vector2 _moveInput;

    /// <summary>
    /// Vector que guarda la última dirección de movimiento del jugador.
    /// </summary>
    private Vector2 _lastMoveDirection = Vector2.down;

    /// <summary>
    /// Booleano que indica si el jugador está mirando a la derecha.
    /// </summary>
    private bool _facingRight = true;

    /// <summary>
    /// Booleano que indica si la mano del jugador está mirando a la derecha.
    /// </summary>
    private bool _handfacingRight = true;

    /// <summary>
    /// Transform que representa la mano del jugador.
    /// </summary>
    private Transform _hand;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama al comenzar el juego. Inicializa los componentes del jugador.
    /// </summary>
    void Start()
    {
        // Obtiene la referencia al Rigidbody2D del jugador.
        _playerRb = GetComponent<Rigidbody2D>();

        // Obtiene la referencia al Animator del jugador.
        _playerAnimator = GetComponent<Animator>();

        // Inicializa los estados de animación.
        _playerAnimator.SetBool("Watering", false);
        _playerAnimator.SetBool("Sicklering", false);

        // Obtiene la referencia al SpriteRenderer del jugador.
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Obtiene la referencia a la mano del jugador.
        _hand = gameObject.transform.GetChild(0);

        
    }

    /// <summary>
    /// Se llama una vez por cada fotograma. Se utiliza para capturar la entrada del jugador.
    /// </summary>
    void Update()
    {
        // Captura la entrada del jugador para el movimiento en el eje X 
        float moveX = InputManager.Instance.MovementVector.x;
        float moveY = InputManager.Instance.MovementVector.y;

        // Normaliza la entrada de movimiento.
        _moveInput = InputManager.Instance.MovementVector.normalized;

        // Si hay entrada de movimiento, actualiza la dirección y animaciones.
        if (_moveInput != Vector2.zero)
        {
            _lastMoveDirection = _moveInput;
            _playerAnimator.SetFloat("Horizontal", moveX);
            _playerAnimator.SetFloat("Vertical", moveY);

            // Lógica para voltear al jugador si es necesario.
            if ((moveX < 0 && _facingRight) || (moveX > 0 && !_facingRight))
            {
                //Flip();
               // FlipHand();
            }
        }

        // Si no hay entrada de movimiento, mantiene la última dirección.
        if (_moveInput == Vector2.zero)
        {
            _playerAnimator.SetFloat("Horizontal", _lastMoveDirection.x);
            _playerAnimator.SetFloat("Vertical", _lastMoveDirection.y);
        }

        // Actualiza la velocidad de la animación.
        _playerAnimator.SetFloat("Speed", _moveInput.sqrMagnitude);

        

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Método para cambiar la herramienta del jugador.
    /// </summary>
    public void ChangeTool()
    {
        //
    }

    ///<summary>
    ///Metodo para activar el movimiento
    /// </summary>
    public void EnablePlayerMovement()
    {
        _movementenabled = true;
    }

    ///<summary>
    ///Metodo para desactivar el movimiento
    /// </summary>
    public void DisablePlayerMovement()
    {
        _movementenabled = false;
    }


    /// <summary>
    /// Método para obtener la última dirección de movimiento.
    /// </summary>
    public Vector2 GetLastMoveDirection()
    {
        return _lastMoveDirection;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se llama en intervalos fijos de tiempo. Se utiliza para mover al jugador con base en la entrada.
    /// </summary>
    private void FixedUpdate()
    {
        if (_movementenabled)
        {
            // Mueve al jugador según la entrada y la velocidad definida, ajustada al tiempo de cada frame.
            _playerRb.MovePosition(_playerRb.position + InputManager.Instance.MovementVector * Speed * Time.fixedDeltaTime);

        }
    }

    /// <summary>
    /// Método para voltear al jugador.
    /// </summary>
    private void Flip()
    {
        _facingRight = !_facingRight;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;

        //Vector3 scale = Hand.localScale;
        //scale.x *= -1;
        //Hand.localPosition = scale;
    }

    /// <summary>
    /// Método para voltear al jugador.
    /// </summary>
    private void FlipHand()
    {
        Vector3 scale = _hand.localScale;
        Vector3 position = _hand.localPosition;
        position.x *= -1;
        scale.x *= -1;
        _hand.localPosition = position;
        _hand.localScale = scale;    
    }

    #endregion
    //-----EVENTOS-----
    #region
    
    #endregion

} // class PlayerMovement 
// namespace
