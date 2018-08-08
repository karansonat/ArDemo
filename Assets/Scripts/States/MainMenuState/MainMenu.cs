using System.Collections.Generic;
using UnityEngine;

namespace LeoAR.Core
{
    [CreateAssetMenu(fileName = "NewMainMenuState", menuName = "LeoAR/States/MainMenu")]
    public class MainMenu : ScriptableObject
    {
        [SerializeField] private List<GameObject> _models;
    }
}