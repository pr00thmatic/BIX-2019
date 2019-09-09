using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour {
    public event System.Action onGraphModified;

    [SerializeField]
    Transform _pov = null;
    public Transform PoV { get { if (!_pov) _pov = transform; return _pov; } }

    public List<Vector3> vertex;
    public List<Edge> edges;

    public Vector3 ToWorldPoint (Vector3 point) {
        return PoV.TransformPoint(point);
    }

    public Vector3 ToLocalPoint (Vector3 point) {
        return PoV.InverseTransformPoint(point);
    }

    public void SetVertex (int index, Vector3 pos) {
        vertex[index] = pos;
    }

    public void AddVertex (Vector3 pos) {
        vertex.Add(pos);

        while (edges.Count < vertex.Count) {
            edges.Add(new Edge());
        }

        FireGraphModified();
    }

    public void DeleteVertex (int index) {
        for (int i=0; i<edges.Count; i++) {
            for (int j=0; j<edges[i].links.Count; j++) {
                if (edges[i].links[j] > index) {
                    edges[i].links[j]--;
                } else if (edges[i].links[j] == index) {
                    edges[i].links[j] = -1;
                }
            }
        }

        vertex.RemoveAt(index);
        edges.RemoveAt(index);
        foreach (Edge edge in edges) {
            edge.links.RemoveAll((int a) => a < 0);
        }

        FireGraphModified();
    }

    public void Connect (int origin, int destination,
                         bool directed = true) {
        edges[origin].links.Add(destination);

        if (false == directed) {
            Connect(destination, origin);
        }

        FireGraphModified();
    }

    public void Disconnect (int origin, int destination,
                            bool directed = false) {
        if (false == IsConnected(origin, destination)) return;

        int index = edges[origin].links.IndexOf(destination);

        if (index >= 0) {
            edges[origin].links.RemoveAt(index);
        }

        if (false == directed) {
            Disconnect(destination, origin);
        }

        FireGraphModified();
    }

    public bool IsConnected (int origin, int destination) {
        foreach (int link in edges[origin].links) {
            if (link == destination) return true;
        }

        return false;
    }

    public void FireGraphModified () {
        if (onGraphModified != null) onGraphModified();
    }
}
