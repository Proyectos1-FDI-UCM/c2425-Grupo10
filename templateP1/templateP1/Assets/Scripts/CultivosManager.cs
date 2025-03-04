//---------------------------------------------------------
// Script para gestionar la selección y deselección de herramientas.
// Permite al jugador cambiar entre diferentes herramientas usando las teclas asignadas.
// Responsable: Alexia Pérez Santana
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase encargada de gestionar la selección de herramientas 
/// en el inventario del jugador. Al pulsar las teclas específicas, 
/// se activa una herramienta y se desactiva la anterior.
/// </summary>
public class CultivosManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    
    [SerializeField]public Sprite spriteMaiz;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Referencia a la herramienta actualmente seleccionada.
    /// Solo una herramienta puede estar activa a la vez.
    /// </summary>
    private bool _espaciovacio;
    private int _count = 0;
    public CultivosLista[] inventario = new CultivosLista[10];  // Arreglo fijo con 10 casillas
    public int maxItemsPorCasilla = 10;  // Número máximo de items por casilla

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Inicializa el estado de las herramientas al inicio del juego,
    /// asegurándose de que ninguna esté activada.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Escucha la entrada del jugador en cada fotograma para cambiar de herramienta.
    /// </summary>
    
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    // Método para añadir un cultivo al inventario
    public void AñadirCultivo(CultivosLista nuevoCultivo)
    {
        // Verificar si el cultivo ya existe en alguna casilla
        for (int i = 0; i < inventario.Length; i++)
        {
            if (inventario[i] != null && inventario[i].nombre == nuevoCultivo.nombre)
            {
                // Si el cultivo ya existe y no supera el límite de cantidad
                if (inventario[i].cantidad < maxItemsPorCasilla)
                {
                    inventario[i].cantidad++;
                    return;
                }
            }
        }

        // Si no se encontró el cultivo, busca una casilla vacía
        for (int i = 0; i < inventario.Length; i++)
        {
            if (inventario[i] == null)
            {
                inventario[i] = nuevoCultivo;  // Asigna el cultivo en la casilla vacía
                return;
            }
        }

        // Si no hay espacio (todo el inventario está lleno), destruye el cultivo
        Destroy(nuevoCultivo);
        #endregion
    }

        // ---- MÉTODOS PRIVADOS ----
        #region Métodos Privados

        /// <summary>
        /// Activa la herramienta seleccionada o la deselecciona si ya está activa.
        /// </summary>
       
   


    /// <summary>
    /// Deselecciona la herramienta actualmente en uso, dejando las manos vacías.
    /// </summary>
   
    #endregion
}
 // class ToolManager 
// namespace
