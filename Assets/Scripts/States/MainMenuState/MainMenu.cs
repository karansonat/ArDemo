using System.Collections.Generic;
using UnityEngine;

namespace LeoAR.Core
{
    [CreateAssetMenu(fileName = "NewMainMenuState", menuName = "LeoAR/States/MainMenu")]
    public class MainMenu : ScriptableObject
    {
        [SerializeField] private List<Model> _models;
        public List<Model> Models
        {
            get { return _models; }
        }
    }
}