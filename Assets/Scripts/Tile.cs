using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public MeshRenderer widthHolder;
    public float Width { get => widthHolder.bounds.extents.z * 2; }
}
