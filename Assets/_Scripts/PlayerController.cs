using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
    /// <summary>
    /// Velocidad máxima
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// Aceleración de movimiento horizontal
    /// </summary>
    public float accel = 1.0f;

    /// <summary>
    /// Fuerza de salto
    /// </summary>
    public float jumpForce = 10.0f;


    bool jump = false;
    bool landed;
    float moveX = 1f;
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        landed = Mathf.Abs(rb.velocity.y)<0.01f;
        jump = Input.GetButtonDown("Jump");
        moveX = Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// Llamado justo antes de la simulación física, cambia
    /// la velocidad del objeto en base al eje horizontal de
    /// la entrada.
    /// </summary>
	void FixedUpdate () {

        // Somos flexibles en el movimiento horizontal del PlayerController,
        // siempre que seamos coherentes:
        // 1. Se puede modificar la velocidad del jugador de manera instantánea, 
        //    con cuidado de no perder la velocidad en Y
        // 2. Se puede aplicar una fuerza para acelerarlo y limitar la velocidad máxima.
        //    En este caso, es necesario que haya una variable de aceleración
        //    (no tiene sentido usar la misma que la velocidad)
        
        // TRUCO: Si quitas una / a la siguiente línea cambia el código comentado ;-)
        //*
        rb.velocity = new Vector2(speed*moveX, rb.velocity.y);
        /*/
        if (Mathf.Abs(rb.velocity.x) < speed)
        {
            // Al igual que haremos con el salto, lo deberíamos limitar a una vez por frame
            rb.AddForce(new Vector2(accel*moveX, 0));
        }
        /**/
        
        // La fuerza de salto solo se debe de aplicar una vez por frame para evitar
        // saltos incontrolados
        if (landed && jump && rb != null) {
            rb.AddForce(new Vector2(0f,jumpForce), ForceMode2D.Impulse);
            jump = false;
        }
	}
}
