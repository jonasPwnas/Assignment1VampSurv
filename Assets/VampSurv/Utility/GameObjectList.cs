using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectList", menuName = "Scriptable Objects/GameObjectList")]
public class GameObjectList : ScriptableObject
{
    [SerializeField] public List<GameObject> gameObjects;
}
