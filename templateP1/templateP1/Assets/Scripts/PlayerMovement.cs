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
    private Rigidbody2D _playerRb;
    private Animator _playerAnimator;
    private SpriteRenderer _spriteRenderer;
    public bool enablemovement = true;

    /// <summary>
    /// Vector que contiene la entrada del jugador para el movimiento 
    /// </summary>
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    private bool facingRight = true;
    private bool handfacingRight = true;
    private Transform Hand;

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
        _playerAnimator = GetComponent<Animator>();
        _playerAnimator.SetBool("Watering", false);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        Hand = gameObject.transform.GetChild(0);

        
    }

    /// <summary>
    /// Se llama una vez por cada fotograma. Se utiliza para capturar la entrada del jugador.
    /// </summary>
    void Update()
    {

        float moveX = InputManager.Instance.MovementVector.x;
        float moveY = InputManager.Instance.MovementVector.y;

        Vector2 moveInput = InputManager.Instance.MovementVector.normalized;


        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
            _playerAnimator.SetFloat("Horizontal", moveX);
            _playerAnimator.SetFloat("Vertical", moveY);

            if ((moveX < 0 && facingRight) || (moveX > 0 && !facingRight))
            {
                Flip();
                FlipHand();
            }
        }

        if (moveInput == Vector2.zero)
        {
            _playerAnimator.SetFloat("Horizontal", lastMoveDirection.x);
            _playerAnimator.SetFloat("Vertical", lastMoveDirection.y);
        }

        _playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);

        int Regadera = LevelManager.Instance.Regadera();
        if ((InputManager.Instance.UsarIsPressed() || InputManager.Instance.UsarWasPressedThisFrame()) && LevelManager.Instance.Herramientas() == 2 && Regadera > 0)
        {
            _playerAnimator.SetBool("Watering", true);
            Hand.gameObject.SetActive(false);
            Invoke("NotWatering", 1f);
        }
        

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void CambioHerramienta()
    {
        //
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se llama en intervalos fijos de tiempo. Se utiliza para mover al jugador con base en la entrada.
    /// </summary>
    private void FixedUpdate()
    {
        if (enablemovement)
        {
            // Mueve al jugador según la entrada y la velocidad definida, ajustada al tiempo de cada frame.

            _playerRb.MovePosition(_playerRb.position + InputManager.Instance.MovementVector * speed * Time.fixedDeltaTime);

        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;

        //Vector3 scale = Hand.localScale;
        //scale.x *= -1;
        //Hand.localPosition = scale;
    }
    private void FlipHand()
    {
        Vector3 scale = Hand.localScale;
        Vector3 position = Hand.localPosition;
        position.x *= -1;
        scale.x *= -1;
        Hand.localPosition = position;
        Hand.localScale = scale;    
    }

    private void NotWatering() 
    { 
        _playerAnimator.SetBool("Watering", false); 
        Hand.gameObject.SetActive(true);
    }
      


    #endregion
} // class PlayerMovement 
// namespace
