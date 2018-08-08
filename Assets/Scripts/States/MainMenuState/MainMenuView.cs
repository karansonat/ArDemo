using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LeoAR.Core;
using System;

namespace LeoAR.UI
{
    public class PlayButtonPressedArgs : EventArgs { }

    public class MainMenuView : MonoBehaviour, IObservable<PlayButtonPressedArgs>
    {
        #region Fields

        [SerializeField] private Button _playButton;

        #endregion //Fields

        #region Events

        private event EventHandler<PlayButtonPressedArgs> _playButtonPressed;

        #endregion //Events

        #region Public Methods

        public void Initialize()
        {
            _playButton.onClick.RemoveAllListeners();
            _playButton.onClick.AddListener(OnPlayButtonPressed);
        }

        #endregion //Public Methods

        #region Private Methods

        private void OnPlayButtonPressed()
        {
            (this as IObservable<PlayButtonPressedArgs>).Notify(null);
        }

        #endregion //Private Methods

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