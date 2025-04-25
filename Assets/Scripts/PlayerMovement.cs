//---------------------------------------------------------
// Este archivo contiene el comportamiento de movimiento del jugador
// Responsable de la creación de este archivo: Natalia Nita
// Nombre del juego: Roots Of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    ///<summary>
    ///Energia maxima del jugador
    /// </summary>
    [SerializeField] private int maxEnergy = 100;

    ///<summary>
    ///energia actual del jugador
    /// </summary>
    [SerializeField] private float _currentEnergy;

    ///<summary>
    ///Energia que se resta al jugador
    /// </summary>
    [SerializeField] private float _fadingEnergy;

    ///<summary>
    ///ref al uimanager
    /// </summary>
    [SerializeField] private UIManager UIManager;

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
    [SerializeField] private bool _isMovementEnabled;

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

    ///<summary>
    ///Movimiento en x e y
    /// </summary>
    private float moveX;
    private float moveY;

    ///<summary>
    ///booleano para saber si estas cansado
    /// </summary>
    private bool _isTired = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama al comenzar el juego. Inicializa los componentes del jugador.
    /// </summary>
    void Start()
    {
        _isMovementEnabled = true;
        // Obtiene la referencia al Rigidbody2D del jugador.
        _playerRb = GetComponent<Rigidbody2D>();

        // Obtiene la referencia al Animator del jugador.
        _playerAnimator = GetComponent<Animator>();

        // Inicializa los estados de animación.
        _playerAnimator.SetBool("Watering", false);
        _playerAnimator.SetBool("Harvesting", false);

        // Obtiene la referencia al SpriteRenderer del jugador.
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (SceneManager.GetActiveScene().name == "Escena_build")
        {
            // Obtiene la referencia a la mano del jugador.
            _hand = gameObject.transform.GetChild(0);
        }

        UpdateEnergy();
        _currentEnergy = maxEnergy;
        UIManager = FindObjectOfType<UIManager>();
    }

    /// <summary>
    /// Se llama una vez por cada fotograma. Se utiliza para capturar la entrada del jugador.
    /// </summary>
    void Update()
    {
        if (_isMovementEnabled)
        {
            // Si el movimiento está habilitado, entonces procesamos la entrada
            moveX = InputManager.Instance.MovementVector.x;
            moveY = InputManager.Instance.MovementVector.y;
            _moveInput = InputManager.Instance.MovementVector;

            // Actualiza el vector de dirección solo si te estás moviendo
            if (_moveInput.magnitude >= 0.2f)
            {
                _lastMoveDirection = _moveInput.normalized;
                _playerAnimator.SetFloat("Horizontal", _moveInput.x);
                _playerAnimator.SetFloat("Vertical", _moveInput.y);
            }
            else
            {
                // Redondea dirección cuando estás quieto
                Vector2 cleanDirection = RoundToCardinal(_lastMoveDirection);
                _playerAnimator.SetFloat("Horizontal", cleanDirection.x);
                _playerAnimator.SetFloat("Vertical", cleanDirection.y);
            }

            _playerAnimator.SetFloat("Speed", _moveInput.sqrMagnitude);
            UpdateEnergy();
        }
        else
        {
            // Si el movimiento está deshabilitado, no proceses el input.
            _moveInput = Vector2.zero;
            _playerAnimator.SetFloat("Speed", 0);
            _playerAnimator.SetFloat("Horizontal", _lastMoveDirection.x);
            _playerAnimator.SetFloat("Vertical", _lastMoveDirection.y);
        }

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    ///<summary>
    ///Metodo para activar el movimiento
    /// </summary>
    public void EnablePlayerMovement()
    {
        _isMovementEnabled = true;
        Debug.Log("Movimiento Activado");
    }

    ///<summary>
    ///Metodo para desactivar el movimiento
    /// </summary>
    public void DisablePlayerMovement()
    {
        _isMovementEnabled = false;
        Debug.Log("Movimiento desactivado");
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
        if (_isMovementEnabled)
        {
            // Mueve al jugador según la entrada y la velocidad definida, ajustada al tiempo de cada frame.
            _playerRb.MovePosition(_playerRb.position + _moveInput * Speed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Actualizar la energia del jugador  y mandar datos al uimanager
    /// </summary>
    private void UpdateEnergy()
    {
        // Si se puede mover y hay input, gasta energía
        if (_isMovementEnabled && _moveInput != Vector2.zero)
        {
            if (_currentEnergy > 0)
            {
                _currentEnergy -= _fadingEnergy;
            }
        }
        // Si no puede moverse (o está quieto), recupera energía
        else if (_currentEnergy < maxEnergy)
        {
            _currentEnergy += (10f * Time.deltaTime);
        }

        // Si llega a 0, se cansa
        if (_currentEnergy <= 0f && !_isTired)
        {
            _isTired = true;
            UIManager.ShowNotification("Te has cansado,\n recupera energía.", "NoCounter", 0, "Energy");
            DisablePlayerMovement();
        }
        // Si recupera al menos 30, se activa
        else if (_isTired && _currentEnergy >= 30f)
        {
            _isTired = false;
            UIManager.HideNotification("Energy");
            EnablePlayerMovement();
        }

        // Actualiza la barra de energía
        UIManager.UpdateEnergyBar(_currentEnergy, maxEnergy);
    }

    /// <summary>
    /// Normaliza la entrada de movimiento: aplica un umbral para convertir la entrada en -1, 0 o 1.
    /// </summary>
    private Vector2 NormalizeInput(Vector2 input)
    {
        
            float threshold = 0.2f;
            float absX = Mathf.Abs(input.x);
            float absY = Mathf.Abs(input.y);

            // Si ambos son menores al threshold → quedarse quieto
            if (absX < threshold && absY < threshold)
                return Vector2.zero;

            // Determinar dirección dominante
            if (absX > absY)
                return new Vector2(Mathf.Sign(input.x), 0f); // Horizontal
            else
                return new Vector2(0f, Mathf.Sign(input.y)); // Vertical
        
    }

    private Vector2 RoundToCardinal(Vector2 dir)
    {
        
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                return new Vector2(Mathf.Sign(dir.x), 0f);
            else
                return new Vector2(0f, Mathf.Sign(dir.y));
        
    }

    #endregion
    //-----EVENTOS-----
    #region

    #endregion

} // class PlayerMovement 
  // namespace