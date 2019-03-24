using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyPieceManager : ManagerBase<FamilyPieceManager>
{
	[SerializeField]
	private GameObject piecePrefab;

	[SerializeField]
	private GameObject foodPrefab;

	[SerializeField]
	private Transform player;

	[SerializeField]
	private float distance;

	[SerializeField]
	private Transform spotParent;

	[SerializeField]
	private Transform hand;

    [SerializeField]
    private Transform familyGrave;

	[SerializeField]
	private List<GameObject> familyPhotos;

	private Transform currentPieceTranform;
	//Spawn positions for the memories pieces
	private List<Transform> spawnSpots;

	private List<Transform> foods;

	private List<Transform> minimapIcons;

	private System.Random numberGenerator;

    // Start is called before the first frame update
    void Start()
    {
		minimapIcons = new List<Transform>();
		spawnSpots = new List<Transform>();
		foods = new List<Transform>();
		foreach(Transform t in spotParent) {
			spawnSpots.Add(t);
		}
		numberGenerator = new System.Random();
		CreateNewPiece(piecePrefab, spawnSpots[numberGenerator.Next(0, spawnSpots.Count)], 0);
		GameFlowManager.Instance.OnLevelRefresh += SpawnNewMemory;
	}

	// create a new memory on the map
	public void SpawnNewMemory(LevelData data) {
		for(int i = 0; i < minimapIcons.Count; i++) {
			if(minimapIcons[i] != null) {
				currentPieceTranform.position = minimapIcons[i].position;
				Destroy(minimapIcons[i]);
			}
			minimapIcons.RemoveAt(i);
		}

        if (GameFlowManager.Instance.GetCurrentGameState() == GameState.FoundGrave)
        {
            Debug.Log("display grave on the map");
            // set the grave as the final destination 
            Transform thisSpot = familyGrave;
            CreateNewPiece(null, thisSpot, data.id);
            CreateNewFood(data.numberOfFood);
        }
        else
        {
            Transform thisSpot = spawnSpots[numberGenerator.Next(0, spawnSpots.Count)];

            while (thisSpot.position.Equals(currentPieceTranform.position))
            {
                thisSpot = spawnSpots[numberGenerator.Next(0, spawnSpots.Count)];
            }
            CreateNewPiece(piecePrefab, thisSpot, data.id);
            CreateNewFood(data.numberOfFood);
        }
	}

	public GameObject GetPhotoPiece(int num) {
		return Instantiate(familyPhotos[num]);
	}

	private void CreateNewPiece(GameObject prefab, Transform position, int level) {
        if (prefab != null) {
            GameObject o = Instantiate(prefab, position);
            currentPieceTranform = o.transform;
            FamilyPiece c = o.GetComponent<FamilyPiece>();
            c.handPosition = hand;
            c.GetComponentInChildren<InteractText>().target = player.gameObject;

            // set the piece number
            Debug.Log("family piece " + level);
            c.pieceNum = level+1;
        }
        else {
            currentPieceTranform = position; 
        }

		foreach (Transform childTrans in currentPieceTranform) {
			if (childTrans.tag == "Minimap") {
				minimapIcons.Add(childTrans);
			}
		}
	}

	private void CreateNewFood(int num) {
		for(int i = 0; i < foods.Count; i++) {
			if(foods[i] = null) {
				foods.RemoveAt(i);
			}
		}

		float xRange = currentPieceTranform.position.x - player.position.x;
		float zRange = currentPieceTranform.position.z - player.position.z;
		System.Random r = new System.Random();
		for(int i = 0; i < num-foods.Count; i++) {
			float x = ((float)r.NextDouble() * (xRange)+10) + player.position.x;
			float z = ((float)r.NextDouble() * (zRange)) + player.position.z;
			GameObject food = Instantiate(foodPrefab, new Vector3(x, 0, z), new Quaternion());
			Collectable c = food.GetComponent<Collectable>();

			if(c != null) {
				c.handPosition = hand;
			}
			foods.Add(food.transform);
		}
	}

    // Update is called once per frame
    void Update()
    {

		//update the piece information on the minimap
        foreach(Transform t in minimapIcons) {
			if(t == null) {
				continue;
			}

			if (!t.gameObject.activeSelf) continue;

			Vector3 piecesPosition2d = new Vector3(t.parent.position.x, player.position.y, t.parent.position.z);
			float distancePlayer = Vector3.Distance(player.position, piecesPosition2d);

			if(distancePlayer > distance) {
				float ratio = distance / distancePlayer;
				float x = (t.parent.position.x - player.position.x) * ratio;
				float z = (t.parent.position.z - player.position.z) * ratio;

				//add the offest to the player to show the piece within the minimap
				t.position = new Vector3(player.position.x+x, player.position.y, player.position.z+z);
			} else {
				t.position = t.parent.position;
			}

		}
    }
}
