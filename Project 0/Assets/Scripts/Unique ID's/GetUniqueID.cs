using Vexe.Runtime.Types;

public class GetUniqueID : BaseBehaviour
{
    [Comment("This excludes the specific gamobject from getting an ID as long as this is set to true", helpButton: true)]
    public bool dontGiveID;
    public bool hasBeenUsed;

    private void Start()
    {
        Name = this.gameObject.name;
    }

    public string ReceiveID
    {
        set
        {
            ID = value;
        }
    }

    public string Name { get; private set; }

    public bool hasID { get; set; }

    [Comment("Displays this specific gameobjects given ID.", helpButton: true)]
    [Show]
    public string ID { get; private set; }
}
