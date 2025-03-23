//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo: Julia Vera, Natalia Nita
// Nombre del juego: Roots of Life
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

public class CropSpawner : MonoBehaviour
{
    // Prefab de la semilla (asegúrate de que esté en el Inspector)
    [SerializeField] private GameObject PrefabSemilla1;  // Aquí defines el prefab
    [SerializeField] private GameObject PrefabMalasHierbas;

    private int _maxProb = 3;

    private bool b = true;


    private void Start()
    {
        
    }

    /// <summary>
    /// Reactiva la maceta para que se vuelva a poder usar, se activa desde el método cosechar - LLama al método crear malas hierbas si se cumple la probabilidad
    /// </summary>
    public void Reactivar()
    {
        b = true;
        int i = UnityEngine.Random.Range(0, _maxProb);

        if (i == 0) MalasHierbas();
    }

    /// <summary>
    /// Planta la mala hierba  si se cumplen las condiciones
    /// </summary>
    private void MalasHierbas()
    {
        GameObject planta = Instantiate(PrefabMalasHierbas, transform.position, Quaternion.identity);
        planta.transform.SetParent(transform);
        b = false;
    }

    /// <summary>
    /// Planta la semilla  si se cumplen las condiciones (Tool Correcta, Usuario pulsa usar [E], El jugador tiene Semillas)
    /// </summary>
    //private void OnTriggerStay2D()
    //{
    //    int Seeds = LevelManager.Instance.Seeds();
    //    // Si el jugador presiona la tecla E, hay semillas 
    //    if ((InputManager.Instance.UsarWasPressedThisFrame() || InputManager.Instance.UsarIsPressed()) && LevelManager.Instance.Tools() == 5 && LevelManager.Instance.Seeds() > 0 && b)
    //    {
    //        b = false;
    //        Plant(); // Llama a la función que planta la semilla en la posición determinada
    //        LevelManager.Instance.Plant(); // Llama al método que controla la cantidad de semillas

    //        //Debug.Log("Desactivar maceta");
    //        //gameObject.SetActive(false);
    //    }
        
    //}

    /// <summary>
    /// Planta la semilla y se asegura que tenga el script necesario para evolucionar
    /// </summary>
    private void Plantar()
    {

        // Instancia la planta y la guarda como Child de la carpeta Plantas
        GameObject planta = Instantiate(PrefabSemilla1, transform.position, Quaternion.identity);
        planta.transform.SetParent(transform);

        // Verifica si la planta tiene el script PlantaEvolucion
        PlantEvolution plantaEvolucion = planta.GetComponent<PlantEvolution>();
        plantaEvolucion.Plant(gameObject);

        //Huerto huerto = GetComponentInParent<Huerto>();
        //huerto.CambioPlanta(transform, planta);
    }
}
