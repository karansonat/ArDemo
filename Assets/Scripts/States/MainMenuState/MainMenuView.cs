using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LeoAR.Core;
using System;

namespace LeoAR.UI
{
    public class PreviewModelButtonPressedArgs : EventArgs
    {
        public Model SelectedModel { get; private set; }
    }

    public class MainMenuView : MonoBehaviour, IObservable<PreviewModelButtonPressedArgs>
    {
        #region Fields

        [SerializeField] private Button _playButton;
        private PreviewModelButtonPressedArgs _previewModelButtonPressedArgs;

        #endregion //Fields

        #region Events

        private event EventHandler<PreviewModelButtonPressedArgs> _previewModelButtonPressed;

        #endregion //Events

        #region Public Methods

        public void Initialize()
        {
            _previewModelButtonPressedArgs = new PreviewModelButtonPressedArgs();

            _playButton.onClick.RemoveAllListeners();
            _playButton.onClick.AddListener(OnPlayButtonPressed);
        }

        #endregion //Public Methods

        #region Private Methods

        private void OnPlayButtonPressed()
        {
            (this as IObservable<PreviewModelButtonPressedArgs>).Notify(null);
        }

        #endregion //Private Methods

        #region IObservable Interface Implementation

        void IObservable<PreviewModelButtonPressedArgs>.Attach(IObserver<PreviewModelButtonPressedArgs> observer)
        {
            _previewModelButtonPressed += observer.OnNotified;
        }

        void IObservable<PreviewModelButtonPressedArgs>.Detach(IObserver<PreviewModelButtonPressedArgs> observer)
        {
            _previewModelButtonPressed -= observer.OnNotified;
        }

        void IObservable<PreviewModelButtonPressedArgs>.Notify(PreviewModelButtonPressedArgs eventArgs)
        {
            if (_previewModelButtonPressed != null)
            {
                _previewModelButtonPressed.Invoke(this, eventArgs);
            }
        }

        #endregion IObservable Interface Implementation
    }
}