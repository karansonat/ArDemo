using UnityEngine;

namespace LeoAR.Core
{
    [CreateAssetMenu(fileName = "NewModelPreviewState", menuName = "LeoAR/States/ModelPreview")]
    public class ModelPreview : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _environmentPrefab;
        public GameObject EnvironmentPrefab
        {
            get { return _environmentPrefab; }
        }

        #endregion //Fields
    }
}