using System.Collections.Generic;
using UnityEngine;

namespace LeoAR.Core
{
    [CreateAssetMenu(fileName = "NewModelPreviewState", menuName = "LeoAR/States/ModelPreview")]
    public class ModelPreview : ScriptableObject
    {
        [SerializeField] private GameObject _model;
    }
}