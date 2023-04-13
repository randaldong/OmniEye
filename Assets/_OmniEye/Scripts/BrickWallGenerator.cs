using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWallGenerator : MonoBehaviour
{
    public GameObject brickPrefab_1;
    public GameObject brickPrefab_2;
    public GameObject brickPrefab_3;
	[Tooltip("The number of bricks in one layer")] // information about public variable in Inspector
	public int wallCircumference = 36;
    public int wallHeight = 10;
    private float offsetAngle = 0f; // make crossing bricks

    // Start is called before the first frame update
    void Start()
    {
        for (int layer = 0; layer< wallHeight; layer++)
        {
            for (int i=0; i< wallCircumference; i++)
            {
                GameObject brick;
				int brickNum = Random.Range(1, 4);
                switch (brickNum) {
                    case 1:
						brick = Instantiate(brickPrefab_1);
                        break;
                    case 2:
						brick = Instantiate(brickPrefab_2);
						break;
                    case 3:
						brick = Instantiate(brickPrefab_3);
						break;
                    default:
						brick = Instantiate(brickPrefab_1);
						break;
				}
				Vector3 brickSize = brick.GetComponent<MeshRenderer>().bounds.size;
				float deltaAngle = 2.0f * Mathf.PI / wallCircumference;
				float wallRadius = wallCircumference * (brickSize.x + 0.02f) / (2.0f * Mathf.PI);
				if (layer % 2 == 1) 
                {
                    offsetAngle = deltaAngle / 2.0f;
                }
                else
                {
					offsetAngle = 0f;
                }
                brick.transform.position = new Vector3(
                    transform.position.x + wallRadius * Mathf.Sin(deltaAngle * i + offsetAngle),
					transform.position.y + layer * brickSize.y,
					transform.position.z + wallRadius * Mathf.Cos(deltaAngle * i + offsetAngle));
                brick.transform.Rotate(0, i * deltaAngle * 180f / Mathf.PI, 0, Space.Self);
                brick.transform.parent = gameObject.transform; // add current brick instance as a child of the wall
			}
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
