using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LevelBuilder : MonoBehaviour {
    Graph _g;
    Graph _Graph { get { if (!_g) _g = GetComponent<Graph>(); return _g; } }

    public bool generateDynamically = false;

    public GameObject islandTile;
    public Tile roadTile;
    public DynamicGeneration content;
    public float offset = 1;

    void OnEnable () {
        _Graph.onGraphModified += GraphModifiedHandler;
    }

    public void GraphModifiedHandler () {
        if (generateDynamically) {
            Build();
        }
    }

    public void Build () {
        content.Clear();
        Dictionary<Pair, bool> built = new Dictionary<Pair, bool>();

        for (int i=0; i<_Graph.edges.Count; i++) {
            foreach (int link in _Graph.edges[i].links) {
                if (!built.ContainsKey(new Pair(i, link)) &&
                    !built.ContainsKey(new Pair(link, i))) {
                    BuildTiles(_Graph.ToWorldPoint(_Graph.vertex[i]),
                               _Graph.ToWorldPoint(_Graph.vertex[link]));
                    built[new Pair(i, link)] = true;
                }
            }
        }

        CreateIslands();
    }

    public void CreateIslands () {
        foreach (Vector3 vertex in _Graph.vertex) {
            GameObject created = Util.Instantiate(islandTile);
            created.transform.position = _Graph.ToWorldPoint(vertex);
            created.transform.parent = content.Dynamic;
        }
    }

    public void BuildTiles (Vector3 origin, Vector3 destination) {
        float originalLevel = origin.y;
        float destinationLevel = destination.y;

        Vector3 difference = destination - origin;

        origin += difference.normalized * offset;
        origin.y = originalLevel;

        destination -= difference.normalized * offset;
        destination.y = destinationLevel;

        difference = destination - origin;

        int amount = (int) (difference.magnitude / roadTile.Width);
        float scale = (difference.magnitude / amount) / roadTile.Width;

        for (int i=0; i<amount; i++) {
            Tile created = Util.Instantiate(roadTile).GetComponent<Tile>();
            created.transform.position =
                origin + i * difference.normalized * (difference.magnitude/(float)amount);
            created.transform.forward = difference;
            created.transform.localScale = new Vector3(1,1, scale);
            created.transform.parent = content.Dynamic;
        }
    }
}
