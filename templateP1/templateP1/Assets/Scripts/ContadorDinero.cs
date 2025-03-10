//---------------------------------------------------------
// Este script maneja el Contador del dinero del jugador
// Javier Librada Jerez
// Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Clase que gestiona el contador de dinero del jugador, permitiendo la compra de semillas,
/// la venta de cultivos y la actualización de la interfaz de usuario.
/// </summary>
public class ContadorDinero : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI textoDinero;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private int Contador = 0;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        // Obtiene el GameManager automáticamente si no ha sido asignado en el Inspector
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }

        Contador += 100;
        ActualizarContador();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ComprarSemillasLechuga();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            LechugaVendida();
        }
        ActualizarContador();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void ComprarSemillasLechuga()
    {
        if (Contador >= 15)
        {
            Contador -= 15;
            gameManager.AñadirObjeto("Lechuga", 5);
        }
    }

    public void LechugaVendida()
    {
        if (gameManager.RemoverObjeto("Lechuga", 1))
        {
            Contador += 20;
        }
    }

    public void ComprarSemillasFresas()
    {
        if (Contador >= 30)
        {
            Contador -= 30;
            gameManager.AñadirObjeto("Fresa", 5);
        }
    }

    public void FresaVendida()
    {
        if (gameManager.RemoverObjeto("Fresa", 1))
        {
            Contador += 40;
        }
    }

    public void ComprarSemillasZanahoria()
    {
        if (Contador >= 50)
        {
            Contador -= 50;
            gameManager.AñadirObjeto("Zanahoria", 5);
        }
    }

    public void ZanahoriaVendida()
    {
        if (gameManager.RemoverObjeto("Zanahoria", 1))
        {
            Contador += 65;
        }
    }

    public void ComprarSemillasMaiz()
    {
        if (Contador >= 70)
        {
            Contador -= 70;
            gameManager.AñadirObjeto("Maiz", 5);
        }
    }

    public void MaizVendido()
    {
        if (gameManager.RemoverObjeto("Maiz", 1))
        {
            Contador += 90;
        }
    }

    public void ComprarAbono()
    {
        if (Contador >= 70)
        {
            Contador -= 70;
            gameManager.AñadirObjeto("Abono", 5);
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void ActualizarContador()
    {
        if (textoDinero != null)
        {
            textoDinero.text = "x" + Contador;
        }
    }

    #endregion
}
