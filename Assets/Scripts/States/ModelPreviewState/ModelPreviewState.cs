using UnityEngine;

namespace LeoAR.Core
{
    public class ModelPreviewState : IState
    {
        #region Fields

        private ModelPreview _model;
        private ModelPreviewView _view;

        #endregion //Fields

        #region Public Methods

        public void Initialize()
        {
            _model = ModelPreviewFactory.Instance.CreateModel();
            _view = ModelPreviewFactory.Instance.CreateView();
        }

        #endregion //Public Methods

        void IState.Begin()
        {
            Debug.Log("ModelPreviewState.Begin");
            Initialize();
        }

        void IState.End()
        {
            Debug.Log("ModelPreviewState.End");
        }

        void IState.Update()
        {
            Debug.Log("ModelPreviewState.Update");
        }
    }
}