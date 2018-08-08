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
    
    public class StateController : MonoBehaviour, IObserver<PlayButtonPressedArgs>
    {
        #region Fields

        private IState _activeState;

        #endregion //Fields

        #region Unity Methods

        private void Awake()
        {
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
                    (_activeState as IObservable<PlayButtonPressedArgs>).Attach(this as IObserver<PlayButtonPressedArgs>);
                    break;
                case StateType.ModelPreviewState:
                    _activeState = StateFactory.Instance.CreateModelPreviewState();
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

        void IObserver<PlayButtonPressedArgs>.OnNotified(object sender, PlayButtonPressedArgs eventArgs)
        {
            SwitchState(StateType.ModelPreviewState);
        }

        #endregion IObserver Interface Implementation
    }
}