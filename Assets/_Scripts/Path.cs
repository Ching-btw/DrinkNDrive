using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path {

    [SerializeField, HideInInspector]
    List<Vector2> points;

    public Path(Vector2 center) {

    }
}
