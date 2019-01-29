using Vexe.Runtime.Types;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEditor;

public class IDController : BaseBehaviour
{
    private GetUniqueID GetUniqueID;    // Connection between the IDController script and the GetUniqueID Script.
    private GetUniqueID[] guidList;    // A public list to see all available Gameobjects with the GetUniqueID Scrip onto it.
    private string fileName;    // Used to fetch the filepath of jsonfiles in the GetJsonFiles Method.
    private JsonData data;    // Used to serialize & Deserialize strings for save/load files.

    [Comment("The asset which the Scene names will be saved in, can be found in the root assets folder named SceneList by default.", helpButton: true)]
    public SceneList list;

    private int maxGeneratedIds = 0;    // Maximum number of Generated Unique IDs.

    [Comment("Maximum number of Generated Unique IDs." + "\n\n" +
             "(This value cannot be changed from the inspector since it's controlled automatically)", helpButton: true)]
    [Show] public int MaxGUIDS
    {
        get
        {
            return maxGeneratedIds;
        }
    }

    [iMin(0)]
    private int i;    // Used in the GenerateGUIDs method as the minimum value.

    private string ID;     // Used as temporary string to add IDs to objects.
    private bool hasID;    // Checks if the object has an id.

    [Comment("List of currently Generated Unique ID's", helpButton: true)]
    public List<string> uniqueIDList = new List<string>();
    
    private void Start()
    {
        createSceneDirectories();    // Used to create directories IDs and add a new folder for each scene in the build settings if the directories does not already exsist.

        list = (SceneList)AssetDatabase.LoadAssetAtPath("Assets/SceneList.asset", typeof(SceneList));

        GetUniqueID = new GetUniqueID();

        maxGeneratedIds = getObjects().Length + 5;    // Sets the Maximum number of GUIDs and will add 5 spare GUIDs just in case if this malfunction.
        GetJsonFiles();    // Method to fetch all json files in the chosen directory and assign unique ids to Gameobjects which already has received an id.
        GenerateGUIDs();    // Methods that will be executed from the start.
    }

    void Update ()
    {
        guidList = getObjects();    // Fills the public GetUniqueID list.
        GiveIDs();    // Assign ids to new Gamobjects with the GetUniqueID Script attached
    }

    // Gets all Gameobjects and returns every Gameobject with the GetUniqueID script Attached.
    public GetUniqueID[] getObjects()
    {
        GetUniqueID[] scripts = FindObjectsOfType<GetUniqueID>();
        GetUniqueID[] objects = new GetUniqueID[scripts.Length];
        for (int i = 0; i < objects.Length; i++)
            objects[i] = scripts[i].gameObject.GetComponent<GetUniqueID>();
        return objects;
    }

    void GetJsonFiles()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        DirectoryInfo dataFolder = new DirectoryInfo(Application.persistentDataPath + "/IDs/Scene[" + currentScene + "]");

        foreach (FileInfo curFile in dataFolder.GetFiles("*.json"))
        {
            fileName = curFile.FullName;
            fileName = fileName.Replace('\\', '/');

            string jsonString = File.ReadAllText(fileName);
            data = JsonMapper.ToObject(jsonString);

            foreach (GetUniqueID guid in getObjects())
            {
                if (guid.name == data["Name"].ToString())
                {
                    guid.ReceiveID = data["ID"].ToString();
                    guid.hasID = data["hasID"].IsBoolean;
                    maxGeneratedIds--;
                }
            }
        }
    }

    void GiveIDs()
    {
        var rguid = uniqueIDList[UnityEngine.Random.Range(0, uniqueIDList.Count)];

        foreach (GetUniqueID guid in getObjects())
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (guid.ID == null && guid.dontGiveID != true)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                JsonWriter writer = new JsonWriter(builder);
                writer.PrettyPrint = true;
                writer.IndentValue = 0;
                hasID = true;
                guid.ReceiveID = uniqueIDList[UnityEngine.Random.Range(0, uniqueIDList.Count)];
                uniqueIDList.Remove(guid.ID);
                SaveIDs.gameObjectsWithID savedIds = new SaveIDs.gameObjectsWithID(guid.name, guid.ID, hasID);
                JsonMapper.ToJson(savedIds, writer);
                data = builder.ToString();
                File.WriteAllText(Application.persistentDataPath + "/IDs/Scene[" + currentScene + "]/" + guid.name + ".json", data.ToString());
                Debug.Log(guid.name + " Has received the ID: " + guid.ID);
            }
            else if (guid.ID != null && guid.dontGiveID == true)
            {
                return;
            }
        }
    }

    private void createSceneDirectories()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (!Directory.Exists(Application.persistentDataPath + "/IDs"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/IDs");
            ConsoleProDebug.LogToFilter("Directory IDs has been created!", "Directory");
        }
        
        if(!Directory.Exists(Application.persistentDataPath + "/IDs/Scene[" + currentScene + "]"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/IDs/Scene[" + currentScene + "]");
            ConsoleProDebug.LogToFilter("Directory Scene[" + currentScene + "] has been created!", "Directory");
        }
        
        Regex regex = new Regex(@"([^/]*/)*([\w\d\-]*)\.unity");
        for (int i = 0; i < list.sceneNames.Length; i++)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/IDs/Scene[" + regex.Replace(list.sceneNames[i], "$2") + "]"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/IDs/Scene[" + regex.Replace(list.sceneNames[i], "$2") + "]");
                ConsoleProDebug.LogToFilter("Directory Scene[" + regex.Replace(list.sceneNames[i], "$2") + "] has been created!", "Directory");
            }
        }
    }

    private void GenerateGUIDs()
    {
        while (i < maxGeneratedIds)
        {
            ID = Guid.NewGuid().ToString();
            uniqueIDList.Add(ID);
            i++;
        }
    }
}
