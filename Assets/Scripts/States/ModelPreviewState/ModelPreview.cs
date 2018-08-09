using System.Collections.Generic;
using UnityEngine;

namespace LeoAR.Core
{
    [CreateAssetMenu(fileName = "NewModelPreviewState", menuName = "LeoAR/States/ModelPreview")]
    public class ModelPreview : ScriptableObject
    {
        [SerializeField] private Model _model;
        public Model Model
        {
            get { return _model; }
        }

        [SerializeField] private GameObject _environmentPrefab;
        public GameObject EnvironmentPrefab
        {
            get { return _environmentPrefab; }
        }
    }
}