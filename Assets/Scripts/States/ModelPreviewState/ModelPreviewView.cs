using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeoAR.Core
{
    public class ActionAnimationTriggeredArgs : EventArgs
    {
        public int ActionID { get; private set; }
        public ActionAnimationTriggeredArgs(int actionID) : base()
        {
            ActionID = actionID;
        }
    }

    public class ModelPreviewView : MonoBehaviour, IObservable<ActionAnimationTriggeredArgs>
    {
        #region Fields

        [SerializeField] private GameObject _animationButtonPrefab;
        [SerializeField] private RectTransform _animationsContainer;

        private Model _model3D;

        #endregion //Fields

        #region Events

        private event EventHandler<ActionAnimationTriggeredArgs> ActionAnimationTriggered;

        #endregion //Events

        #region Public Methods

        public void Initialize(Model model)
        {
            _model3D = model;
            InitializeAnimationButtons();
        }

        #endregion //Public Methods

        private void InitializeAnimationButtons()
        {
            foreach (var actionID in _model3D.Animations)
            {
                var button = Instantiate(_animationButtonPrefab);
                button.transform.SetParent(_animationsContainer, false);
                button.GetComponentInChildren<Text>().text = actionID.ToString();
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    (this as IObservable<ActionAnimationTriggeredArgs>).Notify(new ActionAnimationTriggeredArgs((int)actionID));
                });
            }
        }

        #region IObservable Interface Implementation

        void IObservable<ActionAnimationTriggeredArgs>.Attach(IObserver<ActionAnimationTriggeredArgs> observer)
        {
            ActionAnimationTriggered += observer.OnNotified;
        }

        void IObservable<ActionAnimationTriggeredArgs>.Detach(IObserver<ActionAnimationTriggeredArgs> observer)
        {
            ActionAnimationTriggered -= observer.OnNotified;
        }

        void IObservable<ActionAnimationTriggeredArgs>.Notify(ActionAnimationTriggeredArgs eventArgs)
        {
            if (ActionAnimationTriggered != null)
            {
                ActionAnimationTriggered.Invoke(this, eventArgs);
            }
        }

        #endregion //IObservable Interface Implementation
    }
}