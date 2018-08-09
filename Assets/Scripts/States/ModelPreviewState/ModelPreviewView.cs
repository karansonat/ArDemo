using LeoAR.Input;
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

    public class BackButtonPressedArgs : EventArgs { }

    public class VirtualJoystickArgs : EventArgs
    {
        public Vector3 InputVector { get; set; }
    }

    public class ModelPreviewView : MonoBehaviour, IObservable<ActionAnimationTriggeredArgs>, IObservable<VirtualJoystickArgs>, IObservable<BackButtonPressedArgs>
    {
        #region Fields

        [SerializeField] private Text _modelTitle;
        [SerializeField] private GameObject _animationButtonPrefab;
        [SerializeField] private RectTransform _animationsContainer;
        [SerializeField] private VirtualJoystick _virtualJoystick;
        [SerializeField] private Button _buttonBack;

        private Model _model3D;
        private VirtualJoystickArgs _virtualJoystickArgs;

        #endregion //Fields

        #region Events

        private event EventHandler<ActionAnimationTriggeredArgs> ActionAnimationTriggered;
        private event EventHandler<VirtualJoystickArgs> VirtualJoystick;
        private event EventHandler<BackButtonPressedArgs> BackButtonPressed;

        #endregion //Events

        #region Unity Methods

        private void Update()
        {
            if (_virtualJoystick != null)
            {
                _virtualJoystickArgs.InputVector = _virtualJoystick.InputVector;
                (this as IObservable<VirtualJoystickArgs>).Notify(_virtualJoystickArgs);
            }
        }

        #endregion //Unity Methods

        #region Public Methods

        public void Initialize(Model model)
        {
            _model3D = model;
            _modelTitle.text = _model3D.ModelName;
            InitializeAnimationButtons();

            _buttonBack.onClick.RemoveAllListeners();
            _buttonBack.onClick.AddListener(OnBackButtonPressed);

            _virtualJoystickArgs = new VirtualJoystickArgs();
        }

        #endregion //Public Methods

        #region Private Methods

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

        private void OnBackButtonPressed()
        {
            (this as IObservable<BackButtonPressedArgs>).Notify(null);
        }

        #endregion //Private Methods

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

        void IObservable<VirtualJoystickArgs>.Attach(IObserver<VirtualJoystickArgs> observer)
        {
            VirtualJoystick += observer.OnNotified;
        }

        void IObservable<VirtualJoystickArgs>.Detach(IObserver<VirtualJoystickArgs> observer)
        {
            VirtualJoystick -= observer.OnNotified;
        }

        void IObservable<VirtualJoystickArgs>.Notify(VirtualJoystickArgs eventArgs)
        {
            if (VirtualJoystick != null)
            {
                VirtualJoystick.Invoke(this, eventArgs);
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

        #endregion //IObservable Interface Implementation
    }
}