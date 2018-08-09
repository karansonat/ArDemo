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
        public PreviewModelButtonPressedArgs(Model selectedModel) : base()
        {
            SelectedModel = selectedModel;
        }
    }

    public class MainMenuView : MonoBehaviour, IObservable<PreviewModelButtonPressedArgs>
    {
        #region Fields

        [SerializeField] private GameObject _modelButtonPrefab;
        [SerializeField] private RectTransform _buttonContainer;
        private PreviewModelButtonPressedArgs _previewModelButtonPressedArgs;

        #endregion //Fields

        #region Events

        private event EventHandler<PreviewModelButtonPressedArgs> _previewModelButtonPressed;

        #endregion //Events

        #region Public Methods

        public void Initialize(List<Model> availableModels)
        {
            foreach (var model in availableModels)
            {
                var button = Instantiate(_modelButtonPrefab);
                button.transform.SetParent(_buttonContainer, false);
                button.GetComponentInChildren<Text>().text = model.ModelName;
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    (this as IObservable<PreviewModelButtonPressedArgs>).Notify(new PreviewModelButtonPressedArgs(model));
                });
            }
        }

        #endregion //Public Methods

        #region Private Methods

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