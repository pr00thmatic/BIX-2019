using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Util {
    public static T Instantiate<T> (T prototype) where T: Object {
        #if UNITY_EDITOR
        if (Application.isPlaying) {
            return Instantiate(prototype);
        } else {
            return PrefabUtility.InstantiatePrefab(prototype) as T;
        }
        #else
        return Instantiate(prototype);
        #endif
    }

    public static void Destroy (GameObject thing) {
        #if UNITY_EDITOR
        if (Application.isPlaying) {
            GameObject.Destroy(thing);
        } else {
            GameObject.DestroyImmediate(thing);
        }
        #endif
    }
}
