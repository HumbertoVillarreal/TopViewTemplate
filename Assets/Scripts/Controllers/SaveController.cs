using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController invController;
    private HotBarController hotbarController;
    private KeyInventoryController keyInvController;
    private Chest[] chests;
    public static string playerName = "";

    public static int SelectedSlot = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeComponents();

        LoadGame();
    }



    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, $"save_slot_{SelectedSlot}.json");
        invController = FindAnyObjectByType<InventoryController>();
        hotbarController = FindAnyObjectByType<HotBarController>();
        keyInvController = FindAnyObjectByType<KeyInventoryController>();
        chests = FindObjectsOfType<Chest>();
    }



    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerName = playerName,
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundry = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name,
            inventorySaveData = invController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            keyInvSaveData = keyInvController.GetKeyItems(),
            chestSaveData = GetChestState()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private List<ChestSaveData> GetChestState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach(Chest chest in chests){
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened
            };
            chestStates.Add(chestSaveData);
        }

        return chestStates;
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            //Debug.Log(saveData);
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            PolygonCollider2D savedMapBoundry = GameObject.Find(saveData.mapBoundry).GetComponent<PolygonCollider2D>();
            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = savedMapBoundry;
            MapController_Manual.Instance?.HighlightArea(saveData.mapBoundry);
            MapController_Dynamic.Instance?.GenerateMap(savedMapBoundry);

            //Debug.Log(string.Join(", ", saveData.inventorySaveData));
            invController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);
            keyInvController.SetKeyItems(
                saveData.keyInvSaveData ?? new List<KeyInventorySaveData>()
            );

            //Load Chests
            LoadChestStates(saveData.chestSaveData);

            //Load Bg music 
            Debug.Log("MapBoundry: " + saveData.mapBoundry);
            BgMusicManager.instance.PlayMusicByAreaName(saveData.mapBoundry);

        }
        else
        {

            SaveGame();

            invController.SetInventoryItems(new List<InventorySaveData>());
            hotbarController.SetHotbarItems(new List<InventorySaveData>());

            MapController_Dynamic.Instance?.GenerateMap();
        }
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if(chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }

    public static bool SaveExists(int slotFile)
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            $"save_slot_{slotFile}.json"
            );

        return File.Exists(path);
    }


    public static void DeleteSaveFile(int slot)
    {
        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save deleted: " + path);
        }
    }


    public static string GetSavePath(int slot)
    {
        return Application.persistentDataPath + "/save_slot_" + slot + ".json";
    }


    public static void CopySaveFile(int fromSlot, int toSlot)
    {
        string fromPath = GetSavePath(fromSlot);
        string toPath = GetSavePath(toSlot);

        if (!File.Exists(fromPath))
        {
            Debug.LogWarning("Source save does not exist");
            return;
        }

        File.Copy(fromPath, toPath, true);

        Debug.Log($"Copied save from {fromSlot} to {toSlot}");
    }


    public static string GetPlayerName(int slot)
    {
        string path = GetSavePath(slot);

        if (!File.Exists(path))
        {
            return "- - -";
        }

        SaveData saveData =
            JsonUtility.FromJson<SaveData>(
                File.ReadAllText(path)
            );

        return saveData.playerName;
    }

}
