using MalbersAnimations;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeoAR.Core
{
    public class ModelPreviewState : IState, IObserver<ActionAnimationTriggeredArgs>, IObserver<VirtualJoystickArgs>, IObserver<BackButtonPressedArgs>, IObservable<BackButtonPressedArgs>
    {
        #region Fields

        private Model _selectedModel;
        private ModelPreview _model;
        private ModelPreviewView _view;
        private GameObject _environment;
        private Animal _animal;
        private FollowCameraController _followCameraController;

        #endregion //Fields

        #region Events

        private event EventHandler<BackButtonPressedArgs> BackButtonPressed;

        #endregion //Events

        #region Public Methods

        public void Initialize(Model selectedModel)
        {
            _selectedModel = selectedModel;
            _model = ModelPreviewFactory.Instance.CreateModel();
            _view = ModelPreviewFactory.Instance.CreateView();
            _view.Initialize(_selectedModel);
            _environment = Object.Instantiate(_model.EnvironmentPrefab, Vector3.zero, Quaternion.identity);
            _animal = _selectedModel.Create3DView().GetComponent<Animal>();
            _animal.transform.position = Vector3.zero;
            var camera = _environment.GetComponentInChildren<Camera>().transform;
            _followCameraController = FollowCameraFactory.Instance.CreateController();
            _followCameraController.Initialize(_animal.transform, camera);
        }

        #endregion //Public Methods

        #region Private Methods

        private void RegisterEvents()
        {
            (_view as IObservable<ActionAnimationTriggeredArgs>).Attach(this as IObserver<ActionAnimationTriggeredArgs>);
            (_view as IObservable<VirtualJoystickArgs>).Attach(this as IObserver<VirtualJoystickArgs>);
            (_view as IObservable<BackButtonPressedArgs>).Attach(this as IObserver<BackButtonPressedArgs>);
        }

        private void UnRegisterEvents()
        {
            (_view as IObservable<ActionAnimationTriggeredArgs>).Detach(this as IObserver<ActionAnimationTriggeredArgs>);
            (_view as IObservable<VirtualJoystickArgs>).Detach(this as IObserver<VirtualJoystickArgs>);
            (_view as IObservable<BackButtonPressedArgs>).Detach(this as IObserver<BackButtonPressedArgs>);
        }

        private void DestroyGameObjects()
        {
            Object.Destroy(_environment);
            Object.Destroy(_animal.gameObject);
            Object.Destroy(_model);
            Object.Destroy(_view.gameObject);
        }

        #endregion //Private Methods

        #region IState Inteface Implementation

        void IState.Begin()
        {
            RegisterEvents();
        }

        void IState.End()
        {
            UnRegisterEvents();
            DestroyGameObjects();
        }

        void IState.Update()
        {
            if (_animal != null)
            {
                _followCameraController.Follow();
            }
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

        void IObserver<VirtualJoystickArgs>.OnNotified(object sender, VirtualJoystickArgs eventArgs)
        {
            if (eventArgs != null && _animal != null)
            {
                _animal.Move(-eventArgs.InputVector * _selectedModel.MoveSpeed * Time.deltaTime);
            }
        }

        void IObservable<BackButtonPressedArgs>.Attach(IObserver<BackButtonPressedArgs> observer)
        {
            BackButtonPressed += observer.OnNotified;
        }

        void IObservable<BackButtonPressedArgs>.Detach(IObserver<BackButtonPressedArgs> observer)
        {
            BackButtonPressed -= observer.OnNotified;
        }

        void IObservable<BackButtonPressedArgs>.Notify(BackButtonPressedArgs eventArgs)
        {
            if (BackButtonPressed != null)
            {
                BackButtonPressed.Invoke(this, eventArgs);
            }
        }

        void IObserver<BackButtonPressedArgs>.OnNotified(object sender, BackButtonPressedArgs eventArgs)
        {
            (this as IObservable<BackButtonPressedArgs>).Notify(null);
        }

        #endregion //IObserver Interface Implementation
    }
}