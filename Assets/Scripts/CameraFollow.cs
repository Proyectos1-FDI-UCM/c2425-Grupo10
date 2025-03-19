//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Este archivo se encarga de mover la cámara para seguir al jugador en el espacio 2D.
// Responsable de la creación de este archivo: [Tu Nombre]
// Nombre del juego: [Nombre del juego]
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Esta clase permite que la cámara siga al objetivo (normalmente el jugador) 
/// mientras se mueve por el mundo 2D. La cámara se mueve suavemente con un 
/// movimiento interpolado para crear una transición fluida.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// La velocidad de seguimiento de la cámara. Controla qué tan rápido la cámara 
    /// se mueve hacia la posición del objetivo. Se puede ajustar desde el Inspector.
    /// </summary>
    public float FollowSpeed = 2f;

    /// <summary>
    /// El objetivo que la cámara debe seguir, generalmente el jugador.
    /// Se debe asignar desde el Inspector de Unity.
    /// </summary>
    public Transform target;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama una vez por cada fotograma. Actualiza la posición de la cámara
    /// para que siga al objetivo suavemente.
    /// </summary>
    void Update()
    {
        // Calcula la nueva posición de la cámara, manteniendo la misma coordenada Z (-10 para 2D).
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);

        // Interpola suavemente entre la posición actual de la cámara y la nueva posición deseada.
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

    #endregion
} // class CameraFollow
// namespace

