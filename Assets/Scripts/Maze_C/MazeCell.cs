﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {
    public IntVector2 coordinates;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    public MazeCellEdge GetEdge (MazeDirection direction) {
        return edges[(int) direction];
    }

    private int initializedEdgeCount;

    public bool IsFullyInitialized {
        get {
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
        edges[(int) direction] = edge;
        initializedEdgeCount += 1;
    }

    public MazeDirection RandomUninitializedDirection {
        get {
            int skips = Random.Range (0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++) {
                if (edges[i] == null) {
                    if (skips == 0) {
                        return (MazeDirection) i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException ("MazeCell has no uninitialized directions left.");
        }
    }

    public MazeRoom room;

    public void Initialize (MazeRoom room) {
        room.Add (this);
        transform.GetChild (0).GetComponent<Renderer> ().material = room.settings.floorMaterial;
    }
}