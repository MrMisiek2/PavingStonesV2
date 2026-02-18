using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneratorScript : MonoBehaviour
{
    public GameObject tilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        for(int x=-10; x<10; x++)
        {
            for (int y=-10; y<10; y++)
            {
                Vector3 position = new Vector3(x , -0.25f, y);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.parent = transform;
                tile.name = $"Tile_{x}_{y}";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
