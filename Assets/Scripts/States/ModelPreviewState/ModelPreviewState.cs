using MalbersAnimations;
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeoAR.Core
{
    public class ModelPreviewState : IState, IObserver<ActionAnimationTriggeredArgs>,
                                             IObserver<VirtualJoystickArgs>, 
                                             IObserver<BackButtonPressedArgs>, IObservable<BackButtonPressedArgs>,
                                             IObserver<SequenceAnimationTriggeredArgs>
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
            (_view as IObservable<SequenceAnimationTriggeredArgs>).Attach(this as IObserver<SequenceAnimationTriggeredArgs>);
        }

        private void UnRegisterEvents()
        {
            (_view as IObservable<ActionAnimationTriggeredArgs>).Detach(this as IObserver<ActionAnimationTriggeredArgs>);
            (_view as IObservable<VirtualJoystickArgs>).Detach(this as IObserver<VirtualJoystickArgs>);
            (_view as IObservable<BackButtonPressedArgs>).Detach(this as IObserver<BackButtonPressedArgs>);
            (_view as IObservable<SequenceAnimationTriggeredArgs>).Detach(this as IObserver<SequenceAnimationTriggeredArgs>);
        }

        private void DestroyGameObjects()
        {
            Object.Destroy(_environment);
            Object.Destroy(_animal.gameObject);
            Object.Destroy(_model);
            Object.Destroy(_view.gameObject);
        }

        private IEnumerator SequenceAnimationRoutine(int actionID1, int actionID2, float delay1, float delay2)
        {
            //First Delay
            yield return new WaitForSeconds(delay1);

            //Animation 1
            PlayAnimation(actionID1);

            //Wait until animation state begin.
            while (_animal.CurrentAnimState == AnimTag.Idle)
            {
                yield return null;
            }

            //Wait until finished
            while (_animal.CurrentAnimState != AnimTag.Idle)
                yield return null;

            //Second Delay
            yield return new WaitForSeconds(delay2);

            //Animation 2
            PlayAnimation(actionID2);

            //Wait until animation state begin.
            while (_animal.CurrentAnimState == AnimTag.Idle)
            {
                yield return null;
            }

            //Wait until finished
            while (_animal.CurrentAnimState != AnimTag.Idle)
                yield return null;

            //Make play sequence button interactable
            _view.ToggleSequencePlayButton(true);
        }

        private void PlayAnimation(int id)
        {
            if (id == 0)
            {
                _animal.SetJump();
            }
            else
            {
                _animal.SetAttack(id);
            }
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
                PlayAnimation(eventArgs.ActionID);
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

        void IObserver<SequenceAnimationTriggeredArgs>.OnNotified(object sender, SequenceAnimationTriggeredArgs eventArgs)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(SequenceAnimationRoutine(eventArgs.ActionID1,
                                                                                  eventArgs.ActionID2,
                                                                                  eventArgs.Timer1,
                                                                                  eventArgs.Timer2));
        }

        #endregion //IObserver Interface Implementation
    }
}