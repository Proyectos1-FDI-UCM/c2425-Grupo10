//---------------------------------------------------------
// Script de movimiento horizontal con colisiones para NPCs
// Javi
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Clase que gestiona el movimiento horizontal de los NPCs, 
/// incluyendo comportamientos de espera, cambio de dirección 
/// al colisionar y animación de comer.
/// </summary>
public class NPCMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    [Header("Movimiento")]

    /// <summary>
    /// Velocidad de desplazamiento horizontal del NPC.
    /// </summary>
    [SerializeField] private float Speed = 1.5f;

    /// <summary>
    /// Duración en segundos del movimiento continuo antes de detenerse.
    /// </summary>
    [SerializeField] private float MoveDuration = 2f;

    /// <summary>
    /// Duración en segundos que el NPC permanece quieto antes de volver a moverse.
    /// </summary>
    [SerializeField] private float WaitDuration = 3f;

    
    [Header("Comportamiento")]

    /// <summary>
    /// Probabilidad (0-1) de que el NPC realice la animación de comer durante la espera.
    /// </summary>
    [SerializeField] private float EatChance = 0.3f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    /// <summary>
    /// Dirección actual del movimiento: -1 izquierda, 1 derecha, 0 quieto.
    /// </summary>
    private int _direction = 0; // -1 izq, 1 der, 0 quieto

    /// <summary>
    /// Referencia al componente Animator para controlar las animaciones del NPC.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Referencia al componente Rigidbody2D para aplicar movimiento físico.
    /// </summary>
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// Temporizador que controla la duración de los estados de movimiento y espera.
    /// </summary>
    private float _timer;

    /// <summary>
    /// Indica si el NPC está actualmente en movimiento (true) o esperando (false).
    /// </summary>
    private bool _isMoving = false;

    /// <summary>
    /// Indica si el NPC está realizando la animación de comer.
    /// </summary>
    private bool _isEating = false;

    #endregion

    // ---- MÉTODOS DE UNITY ----
    #region Métodos de Unity

    /// <summary>
    /// Se llama al iniciar el script. Obtiene referencias a componentes
    /// e inicia el comportamiento de espera.
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        StartWaiting();
    }

    /// <summary>
    /// Se llama cada frame. Gestiona el temporizador, el movimiento del NPC
    /// y decide cuándo cambiar entre estados de movimiento y espera.
    /// </summary>
    void Update()
    {
        _timer -= Time.deltaTime;

        if (_isMoving)
        {
            _rigidbody.velocity = new Vector2(_direction * Speed, 0f);

            if (_timer <= 0f)
                StartWaiting();
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;

            if (_timer <= 0f)
                StartMoving();
        }

        UpdateAnimator();
    }

    /// <summary>
    /// Se llama cuando el NPC colisiona con otro objeto.
    /// Invierte la dirección del movimiento si está en movimiento.
    /// </summary>
    /// <param name="collision">Información sobre la colisión ocurrida.</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Cambiar dirección si choca con algo mientras se mueve
        if (_isMoving)
        {
            _direction *= -1;
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Inicia el estado de movimiento con una dirección aleatoria.
    /// </summary>
    private void StartMoving()
    {
        _isMoving = true;
        _timer = MoveDuration;
        _direction = Random.value < 0.5f ? -1 : 1;
        _isEating = false;
    }

    /// <summary>
    /// Inicia el estado de espera y determina aleatoriamente
    /// si el NPC realizará la animación de comer.
    /// </summary>
    private void StartWaiting()
    {
        _isMoving = false;
        _timer = WaitDuration;
        _direction = 0;
        _isEating = Random.value < EatChance;
    }

    /// <summary>
    /// Actualiza los parámetros del Animator para reflejar
    /// el estado actual del NPC (movimiento, dirección, comiendo).
    /// </summary>
    private void UpdateAnimator()
    {
        if (_animator != null)
        {
            _animator.SetFloat("Horizontal", _direction);
            _animator.SetBool("IsMoving", _isMoving);
            _animator.SetBool("IsEating", _isEating);
        }
    }

    #endregion
}