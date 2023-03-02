using UnityEngine;

namespace DevBoost.DataTool
{
    public abstract class DataContainerBase : ScriptableObject
    {
        [SerializeField] 
        [HideInInspector]
        public string documentID;
    }
}