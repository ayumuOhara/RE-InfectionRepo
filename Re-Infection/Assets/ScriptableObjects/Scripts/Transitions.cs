using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TransitionAnimation", menuName = "Scriptable Objects/TrasitionAnimation")]
public class Transitions : ScriptableObject
{
    public List<Image> transitionList;
}
