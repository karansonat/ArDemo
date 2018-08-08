using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeoAR.UI;
using System;

namespace LeoAR.Core
{
    public class MainMenuState : IState, IObserver<PlayButtonPressedArgs>, IObservable<PlayButtonPressedArgs>
    {
        #region Fields

        private MainMenu _model;
        private MainMenuView _view;

        #endregion //Fields

        #region Events

        private event EventHandler<PlayButtonPressedArgs> _playButtonPressed;

        #endregion //Events

        #region Public Methods

        //TODO: must be public?
        public void Initialize()
        {
            _model = MainMenuFactory.Instance.CreateModel();
            
            _view = MainMenuFactory.Instance.CreateView();
            _view.Initialize();

            (_view as IObservable<PlayButtonPressedArgs>).Attach(this as IObserver<PlayButtonPressedArgs>);
        }

        #endregion Public Methods

        void IObserver<PlayButtonPressedArgs>.OnNotified(object sender, PlayButtonPressedArgs eventArgs)
        {
            (this as IObservable<PlayButtonPressedArgs>).Notify(null);
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
            Debug.Log("MainMenuState.Update");
        }

        #endregion //IState Interface Implementation

        #region IObservable Interface Implementation

        void IObservable<PlayButtonPressedArgs>.Attach(IObserver<PlayButtonPressedArgs> observer)
        {
            _playButtonPressed += observer.OnNotified;
        }

        void IObservable<PlayButtonPressedArgs>.Detach(IObserver<PlayButtonPressedArgs> observer)
        {
            _playButtonPressed -= observer.OnNotified;
        }

        void IObservable<PlayButtonPressedArgs>.Notify(PlayButtonPressedArgs eventArgs)
        {
            if (_playButtonPressed != null)
            {
                _playButtonPressed.Invoke(this, eventArgs);
            }
        }

        #endregion IObservable Interface Implementation
    }
}