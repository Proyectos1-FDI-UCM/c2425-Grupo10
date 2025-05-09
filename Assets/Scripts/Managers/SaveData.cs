// Guardar los datos al salir del juego
// Julia Vera Ruiz
// Roots of Life
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
    // ---- ATRIBUTOS SERIALIZABLES ----
    #region Atributos Públicos
    [SerializeField]private Vector3 PlayerPosition;
    [SerializeField] private int[] Inventory;
    [SerializeField] private int ActivePlants;
    [SerializeField] private Plant[] Garden = new Plant[36];
    [SerializeField] private float timer;
    [SerializeField] private int money;
    [SerializeField] private int tutorialPhase;
    [SerializeField] private int tutorialPhaseEscena;
    [SerializeField] private int tutorialPhaseBanco;
    [SerializeField] private int tutorialPhaseMejora;
    [SerializeField] private int waterUpdate;
    [SerializeField] private int gardenUpdate;
    [SerializeField] private bool[] notifications;
    [SerializeField] private string[] textNotifications;
    [SerializeField] private bool[] checks;
    [SerializeField] private bool[] unlockedCrops = new bool[4]; // 0: Lechuga, 1: Zanahoria, 2: Fresa, 3: Maíz

    #endregion

    // ---- MÉTODOS GET ----
    #region Métodos Get
    public Vector3 GetPlayerPosition() {  return PlayerPosition; }
    public int[] GetInventory() { return Inventory; }
    public int GetActivePlants() { return ActivePlants; }
    public Plant[] GetGarden() { return Garden; }
    public float GetTimer() { return timer; }
    public int GetMoney() { return money; }
    public int GetTutorialPhase() { return tutorialPhase; }
    public int GetTutorialPhaseEscena() { return tutorialPhaseEscena; }
    public int GetTutorialPhaseBanco() { return tutorialPhaseBanco; }
    public int GetTutorialPhaseMejora() {return tutorialPhaseMejora; }
    public int GetwaterUpdate() { return waterUpdate; }
    public int GetGardenUpdate() { return gardenUpdate; }
    public bool[] Getnotification() { return notifications;  }
    public string[] GettextNotifications() { return textNotifications; }
    public bool[] Getchecks () { return checks; }
    public bool[] GetUnlockedCrops() { return unlockedCrops; }
    #endregion

    // ---- MÉTODOS SET ----
    #region Métodos Set
    public void SetPlayerPosition(Vector3 playerPosition) { PlayerPosition = playerPosition; }
    public void SetInventory(int[] inventory) { Inventory = inventory; }
    public void SetActivePlants(int activePlants) { ActivePlants = activePlants; }
    public void SetGarden(Plant[] garden) { Garden = garden; }
    public void SetTimer(float Timer) { timer = Timer; }
    public void SetMoney(int Money) { money = Money; }
    public void SetTutorialPhase(int TutorialPhase) { tutorialPhase = TutorialPhase; }
    public void SetTutorialPhaseEscena(int TutorialPhaseEscena) { tutorialPhaseEscena = TutorialPhaseEscena; }
    public void SetTutorialPhaseBanco(int TutorialPhaseBanco) { tutorialPhaseBanco = TutorialPhaseBanco; }
    public void SetTutorialPhaseMejora(int TutorialPhaseMejora) { tutorialPhaseMejora = TutorialPhaseMejora; }
    public void SetwaterUpdate(int WaterUpdate) { waterUpdate = WaterUpdate; }
    public void SetGardenUpdate(int GardenUpdate) { gardenUpdate = GardenUpdate; }
    public void Setnotification(bool[] Notifications) { notifications = Notifications; }
    public void SettextNotifications(string[] TextNotifications) { textNotifications = TextNotifications; }
    public void Setchecks(bool[] Checks) { checks = Checks; }
    public void SetUnlockedCrops(bool[] value) { unlockedCrops = value; }

    #endregion
} // class SaveData 
// namespace
