// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Julia Vera Ruiz
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase con las variables:
/// PlayerPosition, Inventory, ActivePlants, Garden;
/// </summary>
[System.Serializable]
public class SaveData
{
    public Vector3 PlayerPosition;
    public int[] Inventory;
    public int ActivePlants;
    public Plant[] Garden = new Plant[36];
    public float timer;
    public int money;
    public int tutorialPhase;
    public int waterUpdate;
    public int gardenUpdate;
    public bool[] notifications;
    public string[] textNotifications;
    public bool[] checks;

} // class SaveData 
// namespace
