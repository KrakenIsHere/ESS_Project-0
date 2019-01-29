using Vexe.Runtime.Types;
using UnityEngine;
using System;

[SerializeField]
public class SaveIDs : BaseBehaviour
{
    [Serializable]
    public class gameObjectsWithID
    {
        public string Name;
        public string ID;
        public bool hasID;

        public gameObjectsWithID(string Name, String ID, bool hasID)
        {
            this.Name = Name;
            this.ID = ID;
            this.hasID = hasID;
        }
    }
}