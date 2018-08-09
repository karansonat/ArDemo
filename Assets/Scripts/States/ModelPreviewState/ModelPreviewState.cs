using MalbersAnimations;
using UnityEngine;

namespace LeoAR.Core
{
    public class ModelPreviewState : IState, IObserver<ActionAnimationTriggeredArgs>
    {
        #region Fields

        private ModelPreview _model;
        private ModelPreviewView _view;
        private GameObject _environment;
        private Animal _animal;

        #endregion //Fields

        #region Public Methods

        public void Initialize()
        {
            _model = ModelPreviewFactory.Instance.CreateModel();
            _view = ModelPreviewFactory.Instance.CreateView();
            (_view as IObservable<ActionAnimationTriggeredArgs>).Attach(this as IObserver<ActionAnimationTriggeredArgs>);
            _view.Initialize(_model.Model);

            _environment = Object.Instantiate(_model.EnvironmentPrefab, Vector3.zero, Quaternion.identity);

            _animal = _model.Model.Create3DView().GetComponent<Animal>();
            _animal.transform.position = Vector3.zero;
        }

        #endregion //Public Methods

        #region IState Inteface Implementation

        void IState.Begin()
        {
            Initialize();
        }

        void IState.End()
        {
            Debug.Log("ModelPreviewState.End");
        }

        void IState.Update()
        {
            
        }

        #endregion //IState Inteface Implementation

        #region IObserver Interface Implementation

        void IObserver<ActionAnimationTriggeredArgs>.OnNotified(object sender, ActionAnimationTriggeredArgs eventArgs)
        {
            if (eventArgs != null && _animal != null)
            {
                if (eventArgs.ActionID == 0)
                {
                    _animal.SetJump();
                }
                else
                {
                    _animal.SetAttack(eventArgs.ActionID);
                }
            }
        }

        #endregion //IObserver Interface Implementation
    }
}