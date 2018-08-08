using UnityEngine;

namespace LeoAR.Core
{
    public class ModelPreviewFactory
    {
        #region Singleton

        private static readonly ModelPreviewFactory _instance = new ModelPreviewFactory();
        public static ModelPreviewFactory Instance
        {
            get { return _instance; }
        }

        static ModelPreviewFactory()
        {
        }

        #endregion //Singleton

        #region Public Methods

        public ModelPreview CreateModel()
        {
            var model = Resources.Load<ModelPreview>("States/ModelPreview");
            return Object.Instantiate(model);
        }

        public ModelPreviewView CreateView()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/ModelPreview/ModelPreviewView");
            return Object.Instantiate(prefab).GetComponent<ModelPreviewView>();
        }

        #endregion //Public Methods
    }
}
