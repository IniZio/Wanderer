using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWall : MazeCellEdge
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Transform wall;

	public override void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		base.Initialize(cell, otherCell, direction);
		//wall.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
	}
}
