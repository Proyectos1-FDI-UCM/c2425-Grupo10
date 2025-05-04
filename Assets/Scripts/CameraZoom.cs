using UnityEngine;
using Cinemachine;

public class CinemachineZoom : MonoBehaviour
{
    #region Serialize
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private float StartZoom = 10f;
    [SerializeField] private float TargetZoom = 5f;
    [SerializeField] private float ZoomDuration = 2f;
    #endregion

    #region Privates
    private float _elapsed = 0f;
    private bool _zooming = true;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        if (VirtualCamera != null)
        {
            VirtualCamera.m_Lens.OrthographicSize = StartZoom;
        }
    }

    private void Update()
    {
        if (_zooming && VirtualCamera != null)
        {
            _elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsed / ZoomDuration);
            VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(StartZoom, TargetZoom, t);

            if (t >= 1f)
            {
                _zooming = false;
            }
        }
    }
    #endregion

    #region Métodos públicos
    // Si necesitas activar el zoom desde otro script, descomenta esto:
    /*
    public void IniciarZoom()
    {
        _elapsed = 0f;
        _zooming = true;
        if (VirtualCamera != null)
        {
            VirtualCamera.m_Lens.OrthographicSize = StartZoom;
        }
    }
    */
    #endregion

    #region Métodos privados
    // Puedes agregar lógica privada aquí si hace falta en el futuro
    #endregion
}