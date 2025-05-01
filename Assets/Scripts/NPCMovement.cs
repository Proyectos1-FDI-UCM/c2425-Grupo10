//---------------------------------------------------------
// Script de movimiento horizontal con colisiones para NPCs
// Javi
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector

    [Header("Movimiento")]
    [SerializeField] private float Speed = 1.5f;
    [SerializeField] private float MoveDuration = 2f;
    [SerializeField] private float WaitDuration = 3f;

    [Header("Comportamiento")]
    [SerializeField] private float EatChance = 0.3f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados

    private int _direction = 0; // -1 izq, 1 der, 0 quieto
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private float _timer;
    private bool _isMoving = false;
    private bool _isEating = false;

    #endregion

    // ---- MÉTODOS DE UNITY ----
    #region Métodos de Unity

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        StartWaiting();
    }

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

    private void StartMoving()
    {
        _isMoving = true;
        _timer = MoveDuration;
        _direction = Random.value < 0.5f ? -1 : 1;
        _isEating = false;
    }

    private void StartWaiting()
    {
        _isMoving = false;
        _timer = WaitDuration;
        _direction = 0;
        _isEating = Random.value < EatChance;
    }

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