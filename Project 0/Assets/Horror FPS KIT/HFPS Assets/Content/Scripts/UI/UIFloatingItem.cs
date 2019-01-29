using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIFloatingItem : MonoBehaviour {

	private UIManager uiManager;

	[Header("Raycasting")]
	public LayerMask Layer;
	public bool addAllGrabItems;
	public List<GameObject> CustomItemsList = new List<GameObject>();

	[HideInInspector]
	public List<GameObject> AllItemsList = new List<GameObject>();
	private List<GameObject> ItemsInDistance = new List<GameObject>();
	private List<GameObject> ItemsFloatingCache = new List<GameObject>();

	[Header("UI")]
	[Tooltip("Must be in \"Resources\" Folder!!")]
	public GameObject FloatingIconPrefab;
	public Transform FloatingIconUI;

	private KeyCode UseKey;

	private Image FloatingIcon;
	private Sprite DefaultFloatingIcon;
	private Vector2 DefaultFloatingSize;

	[Header("Distance")]
	public float distanceShow = 3;
	public float distanceDraw = 4;

	private float distance;

    private bool isCleared;
    private bool isVisible;

	void Start()
	{
		uiManager = GetComponent<UIManager> ();

		if (addAllGrabItems) {
			GameObject[] GrabArray = GameObject.FindGameObjectsWithTag ("Grab");
			AllItemsList = GrabArray.ToList ();
		}

		if (CustomItemsList.Count > 0) {
			if (CustomItemsList [0] == null) {
				CustomItemsList.Clear ();
				Debug.LogError ("UIFloatingItem script in " + this.name + " have null elements in Custom Items List that need to be removed!");
			} else {
				for (int i = 0; i < CustomItemsList.Count; i++) {
					AllItemsList.Add (CustomItemsList [i]);
				}
			}
		}

        isVisible = true;
        isCleared = true;
    }

	void Update () {
		if (isCleared && AllItemsList != null) {
            for (int i = 0; i < AllItemsList.Count; i++)
            {
                float currentDistance = Vector3.Distance(uiManager.Player.transform.position, AllItemsList[i].transform.position);
                if (currentDistance <= distanceDraw && !ItemsInDistance.Contains(AllItemsList[i]))
                {
                    ItemsInDistance.Add(AllItemsList[i]);
                }
                if (currentDistance >= distanceDraw && ItemsInDistance.Contains(AllItemsList[i]))
                {
                    ItemsInDistance.Remove(AllItemsList[i]);
                    ItemsFloatingCache.Remove(GetAndDestroyFloatingIcon(AllItemsList[i]));
                }
            }

            if(ItemsInDistance != null) {
                for (int i = 0; i < ItemsInDistance.Count; i++)
                {
                    if (GetItemVisible(ItemsInDistance[i]) && isVisible)
                    {
                        if (!ContainsFloatingIcon(ItemsInDistance[i]))
                        {
                            InstantiateFloatingIcon(ItemsInDistance[i]);
                        }
                        if (ContainsFloatingIcon(ItemsInDistance[i]))
                        {
                            GetFloatingIcon(ItemsInDistance[i]).GetComponent<FloatingItemInfo>().SetVisible(true);
                        }
                    }
                    else
                    {
                        if (ContainsFloatingIcon(ItemsInDistance[i]))
                        {
                            GetFloatingIcon(ItemsInDistance[i]).GetComponent<FloatingItemInfo>().SetVisible(false);
                        }
                    }
                }
            }
        }
	}

	void InstantiateFloatingIcon(GameObject FollowObject)
	{
		GameObject icon = Instantiate (FloatingIconPrefab);
		ItemsFloatingCache.Add (icon);
		icon.transform.SetParent (FloatingIconUI);
		icon.GetComponent<FloatingItemInfo> ().ObjectToFollow = FollowObject;
	}

	bool GetItemVisible(GameObject Object)
	{
		GameObject camera = Camera.main.gameObject;
		RaycastHit hit;
		if (Physics.Linecast (camera.transform.position, Object.transform.position, out hit, Layer)) {
			if(!(hit.collider.gameObject == Object))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		return false;
	}

    public void SetItemVisible(GameObject Object, bool Visible)
    {
        switch(Visible)
        {
            case true:
                isVisible = true;
                GetFloatingIcon(Object).GetComponent<FloatingItemInfo>().SetVisible(true);
                break;
            case false:
                isVisible = false;
                GetFloatingIcon(Object).GetComponent<FloatingItemInfo>().SetVisible(false);
                break;
        }
    }

    public void RemoveFloatingObject(GameObject Object)
    {
        isCleared = false;
		if (AllItemsList.Contains (Object)) {
			CustomItemsList.Remove (Object);
			AllItemsList.Remove (Object);
			ItemsInDistance.Clear ();
			ItemsFloatingCache.Clear ();
		}
		Destroy (Object);
        isCleared = true;
    }

	GameObject GetFloatingIcon(GameObject FollowObject)
	{
		for (int i = 0; i < ItemsFloatingCache.Count; i++) {
			if (ItemsFloatingCache [i].GetComponent<FloatingItemInfo> ().ObjectToFollow == FollowObject) {
				return ItemsFloatingCache [i];
			}
		}
		return null;
	}

	GameObject GetAndDestroyFloatingIcon(GameObject FollowObject)
	{
		for (int i = 0; i < ItemsFloatingCache.Count; i++) {
			if (ItemsFloatingCache [i].GetComponent<FloatingItemInfo> ().ObjectToFollow == FollowObject) {
				Destroy (ItemsFloatingCache [i]);
				return ItemsFloatingCache [i];
			}
		}
		return null;
	}

	bool ContainsFloatingIcon(GameObject FollowObject)
	{
		for (int i = 0; i < ItemsFloatingCache.Count; i++) {
			if (ItemsFloatingCache.Count <= 0) {
				return false;
			} else if (ItemsFloatingCache [i].GetComponent<FloatingItemInfo> ().ObjectToFollow == FollowObject) {
				return true;
			}
		}
		return false;
	}
}
