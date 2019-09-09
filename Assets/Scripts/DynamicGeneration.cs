using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicGeneration : MonoBehaviour {
    public string contentName = "_Dynamic";
    public Transform Dynamic { get { if (!transform.Find(contentName)) BuildRoot(); return transform.Find(contentName); } }

    public void Clear () {
        if (transform.Find(contentName)) {
            Util.Destroy(transform.Find(contentName).gameObject);
        }
    }

    public void BuildRoot () {
        Transform root = new GameObject(contentName).transform;
        root.parent = transform;
        root.localPosition = Vector3.zero;
        root.localScale = Vector3.one;
        root.localRotation = Quaternion.identity;
    }
}
