using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeoAR.UI;
using System;

namespace LeoAR.Core
{
    public class MainMenuState : IState, IObserver<PreviewModelButtonPressedArgs>, IObservable<PreviewModelButtonPressedArgs>
    {
        #region Fields

        private MainMenu _model;
        private MainMenuView _view;

        #endregion //Fields

        #region Events

        private event EventHandler<PreviewModelButtonPressedArgs> _playButtonPressed;

        #endregion //Events

        #region Public Methods

        //TODO: must be public?
        public void Initialize()
        {
            _model = MainMenuFactory.Instance.CreateModel();            
            _view = MainMenuFactory.Instance.CreateView();
            _view.Initialize(_model.Models);

            (_view as IObservable<PreviewModelButtonPressedArgs>).Attach(this as IObserver<PreviewModelButtonPressedArgs>);
        }

        #endregion Public Methods

        void IObserver<PreviewModelButtonPressedArgs>.OnNotified(object sender, PreviewModelButtonPressedArgs eventArgs)
        {
            (this as IObservable<PreviewModelButtonPressedArgs>).Notify(eventArgs);
        }

        #region IState Interface Implementation

        void IState.Begin()
        {
            Debug.Log("MainMenuState.Begin");
            Initialize();
        }

        void IState.End()
        {
            Debug.Log("MainMenuState.End");
            UnityEngine.Object.Destroy(_model);
            UnityEngine.Object.Destroy(_view.gameObject);
        }

        void IState.Update()
        {
            
        }

        #endregion //IState Interface Implementation

        #region IObservable Interface Implementation

        void IObservable<PreviewModelButtonPressedArgs>.Attach(IObserver<PreviewModelButtonPressedArgs> observer)
        {
            _playButtonPressed += observer.OnNotified;
        }

        void IObservable<PreviewModelButtonPressedArgs>.Detach(IObserver<PreviewModelButtonPressedArgs> observer)
        {
            _playButtonPressed -= observer.OnNotified;
        }

        void IObservable<PreviewModelButtonPressedArgs>.Notify(PreviewModelButtonPressedArgs eventArgs)
        {
            if (_playButtonPressed != null)
            {
                _playButtonPressed.Invoke(this, eventArgs);
            }
        }

        #endregion IObservable Interface Implementation
    }
}