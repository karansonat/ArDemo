using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeoAR.UI;

namespace LeoAR.Core
{
    public enum StateType
    {
        MainMenuState = 0,
        ModelPreviewState
    }
    
    public class StateController : MonoBehaviour, IObserver<PreviewModelButtonPressedArgs>, IObserver<BackButtonPressedArgs>
    {
        #region Fields

        private IState _activeState;
        private Model _selectedModel;

        #endregion //Fields

        #region Unity Methods

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            //Start with default state
            SwitchState(StateType.MainMenuState);
        }

        private void Update()
        {
            // Update current state each frame
            if (_activeState != null)
            {
                _activeState.Update();
            }
        }

        #endregion //Unity Methods

        #region Private Methods

        private void SwitchState(StateType state)
        {
            // End state routine
            if (_activeState != null)
            {
                _activeState.End();
            }

            _activeState = null;

            // Create new state
            switch (state)
            {
                case StateType.MainMenuState:
                    _activeState = StateFactory.Instance.CreateMainMenuState();
                    (_activeState as IObservable<PreviewModelButtonPressedArgs>).Attach(this as IObserver<PreviewModelButtonPressedArgs>);
                    break;
                case StateType.ModelPreviewState:
                    _activeState = StateFactory.Instance.CreateModelPreviewState();
                    (_activeState as IObservable<BackButtonPressedArgs>).Attach(this as IObserver<BackButtonPressedArgs>);
                    (_activeState as ModelPreviewState).Initialize(_selectedModel);
                    break;
            }

            // Begin state routine
            if (_activeState != null)
            {
                _activeState.Begin();
            }
        }

        #endregion //Private Methods

        #region IObserver Interface Implementation

        void IObserver<PreviewModelButtonPressedArgs>.OnNotified(object sender, PreviewModelButtonPressedArgs eventArgs)
        {
            _selectedModel = eventArgs.SelectedModel;
            SwitchState(StateType.ModelPreviewState);
        }

        void IObserver<BackButtonPressedArgs>.OnNotified(object sender, BackButtonPressedArgs eventArgs)
        {
            SwitchState(StateType.MainMenuState);
        }

        #endregion IObserver Interface Implementation
    }
}