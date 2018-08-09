using LeoAR.Input;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LeoAR.Core
{
    #region EventArgs

    public class ActionAnimationTriggeredArgs : EventArgs
    {
        public int ActionID { get; private set; }
        public ActionAnimationTriggeredArgs(int actionID) : base()
        {
            ActionID = actionID;
        }
    }

    public class SequenceAnimationTriggeredArgs : EventArgs
    {
        public int ActionID1 { get; private set; }
        public int ActionID2 { get; private set; }
        public float Timer1 { get; private set; }
        public float Timer2 { get; private set; }
        public SequenceAnimationTriggeredArgs(int actionID1, int actionID2, float timer1, float timer2) : base()
        {
            ActionID1 = actionID1;
            ActionID2 = actionID2;
            Timer1 = timer1;
            Timer2 = timer2;
        }
    }

    public class BackButtonPressedArgs : EventArgs { }

    public class VirtualJoystickArgs : EventArgs
    {
        public Vector3 InputVector { get; set; }
    }

    #endregion //EventArgs

    public class ModelPreviewView : MonoBehaviour, IObservable<ActionAnimationTriggeredArgs>,
                                                   IObservable<VirtualJoystickArgs>,
                                                   IObservable<BackButtonPressedArgs>,
                                                   IObservable<SequenceAnimationTriggeredArgs>
    {
        #region Fields

        [SerializeField] private Text _modelTitle;
        [SerializeField] private GameObject _animationButtonPrefab;
        [SerializeField] private GameObject _animationsContainer;
        [SerializeField] private GameObject _sequenceContainer;
        [SerializeField] private GameObject _textButtonsContainer;
        [SerializeField] private Button _animationsButton;
        [SerializeField] private Button _sequenceButton;
        [SerializeField] private RectTransform _animationButtonsContainer;
        [SerializeField] private VirtualJoystick _virtualJoystick;
        [SerializeField] private Button _buttonBack;
        [Header("Sequence References")]
        [SerializeField] private GameObject _sequenceAnimationsScrollView;
        [SerializeField] private RectTransform _sequenceAnimationsContainer;
        [SerializeField] private Button _sequencePlay;
        [SerializeField] private Button _sequenceTimer1Plus;
        [SerializeField] private Button _sequenceTimer1Minus;
        [SerializeField] private Text _sequenceTimer1;
        [SerializeField] private Button _sequenceTimer2Plus;
        [SerializeField] private Button _sequenceTimer2Minus;
        [SerializeField] private Text _sequenceTimer2;
        [SerializeField] private Button _sequenceAnimation1;
        [SerializeField] private Button _sequenceAnimation2;

        private int _sequenceAction1ID;
        private int _sequenceAction2ID;
        private float _sequenceTimerValue1;
        private float _sequenceTimerValue2;
        private const float TIMER_CHANGE_VALUE = 0.5f;
        private int _currentSequenceAnimationSlot;


        private Model _model3D;
        private VirtualJoystickArgs _virtualJoystickArgs;

        #endregion //Fields

        #region Events

        private event EventHandler<ActionAnimationTriggeredArgs> ActionAnimationTriggered;
        private event EventHandler<VirtualJoystickArgs> VirtualJoystick;
        private event EventHandler<BackButtonPressedArgs> BackButtonPressed;
        private event EventHandler<SequenceAnimationTriggeredArgs> SequenceAnimationTriggered;

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
            _sequenceAction1ID = -1;
            _sequenceAction2ID = -1;
            InitializeAnimationButtons();
            InitializeSequenceAnimationButtons();

            _buttonBack.onClick.RemoveAllListeners();
            _buttonBack.onClick.AddListener(OnBackButtonPressed);

            _animationsButton.onClick.RemoveAllListeners();
            _animationsButton.onClick.AddListener(OnAnimationsButtonPressed);

            _sequenceButton.onClick.RemoveAllListeners();
            _sequenceButton.onClick.AddListener(OnSequenceButtonPressed);

            _sequencePlay.onClick.RemoveAllListeners();
            _sequencePlay.onClick.AddListener(OnSequencePlayButtonPressed);

            _sequenceAnimation1.onClick.RemoveAllListeners();
            _sequenceAnimation1.onClick.AddListener(OnSequenceAnimation1Pressed);

            _sequenceTimer1Plus.onClick.RemoveAllListeners();
            _sequenceTimer1Plus.onClick.AddListener(OnSequenceTimer1PlusPressed);

            _sequenceTimer1Minus.onClick.RemoveAllListeners();
            _sequenceTimer1Minus.onClick.AddListener(OnSequenceTimer1MinusPressed);

            _sequenceAnimation2.onClick.RemoveAllListeners();
            _sequenceAnimation2.onClick.AddListener(OnSequenceAnimation2Pressed);

            _sequenceTimer2Plus.onClick.RemoveAllListeners();
            _sequenceTimer2Plus.onClick.AddListener(OnSequenceTimer2PlusPressed);

            _sequenceTimer2Minus.onClick.RemoveAllListeners();
            _sequenceTimer2Minus.onClick.AddListener(OnSequenceTimer2MinusPressed);

            _virtualJoystickArgs = new VirtualJoystickArgs();
        }

        public void ToggleSequencePlayButton(bool state)
        {
            _sequencePlay.interactable = state;
        }

        #endregion //Public Methods

        #region Private Methods

        private void InitializeAnimationButtons()
        {
            foreach (var actionID in _model3D.Animations)
            {
                var button = Instantiate(_animationButtonPrefab);
                button.transform.SetParent(_animationButtonsContainer, false);
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

        private void OnAnimationsButtonPressed()
        {
            _animationButtonsContainer.gameObject.SetActive(true);
            _sequenceContainer.gameObject.SetActive(false);
        }

        private void OnSequenceButtonPressed()
        {
            _animationButtonsContainer.gameObject.SetActive(false);
            _sequenceContainer.gameObject.SetActive(true);
        }

        #region Sequence Methods

        private void OnSequencePlayButtonPressed()
        {
            if (_sequenceAction1ID == -1 || _sequenceAction2ID == -1)
                return;

            var args = new SequenceAnimationTriggeredArgs(_sequenceAction1ID, _sequenceAction2ID, _sequenceTimerValue1, _sequenceTimerValue2);
            (this as IObservable<SequenceAnimationTriggeredArgs>).Notify(args);

            ToggleSequencePlayButton(false);
        }

        private void OnSequenceAnimation1Pressed()
        {
            _textButtonsContainer.gameObject.SetActive(false);
            _virtualJoystick.gameObject.SetActive(false);
            _sequenceAnimationsScrollView.SetActive(true);

            _currentSequenceAnimationSlot = 1;
        }

        private void OnSequenceAnimation2Pressed()
        {
            _textButtonsContainer.gameObject.SetActive(false);
            _virtualJoystick.gameObject.SetActive(false);
            _sequenceAnimationsScrollView.SetActive(true);

            _currentSequenceAnimationSlot = 2;
        }

        private void OnSequenceTimer1PlusPressed()
        {
            _sequenceTimerValue1 += TIMER_CHANGE_VALUE;

            _sequenceTimer1.text = _sequenceTimerValue1.ToString("0.0");
        }

        private void OnSequenceTimer1MinusPressed()
        {
            _sequenceTimerValue1 -= TIMER_CHANGE_VALUE;

            if (_sequenceTimerValue1 < 0)
                _sequenceTimerValue1 = 0f;

            _sequenceTimer1.text = _sequenceTimerValue1.ToString("0.0");
        }

        private void OnSequenceTimer2PlusPressed()
        {
            _sequenceTimerValue2 += TIMER_CHANGE_VALUE;
            _sequenceTimer2.text = _sequenceTimerValue2.ToString("0.0");
        }

        private void OnSequenceTimer2MinusPressed()
        {
            _sequenceTimerValue2 -= TIMER_CHANGE_VALUE;

            if (_sequenceTimerValue2 < 0)
                _sequenceTimerValue2 = 0f;

            _sequenceTimer2.text = _sequenceTimerValue2.ToString("0.0");
        }

        private void InitializeSequenceAnimationButtons()
        {
            foreach (var actionID in _model3D.Animations)
            {
                var button = Instantiate(_animationButtonPrefab);
                button.transform.SetParent(_sequenceAnimationsContainer, false);
                button.GetComponentInChildren<Text>().text = actionID.ToString();
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UpdateSequenceActionIDs((int)actionID);

                    _textButtonsContainer.gameObject.SetActive(true);
                    _virtualJoystick.gameObject.SetActive(true);
                    _sequenceAnimationsScrollView.SetActive(false);
                });
            }
        }

        private void UpdateSequenceActionIDs(int actionID)
        {
            if (_currentSequenceAnimationSlot == 1)
            {
                _sequenceAction1ID = actionID;
                _sequenceAnimation1.GetComponentInChildren<Text>().text = Enum.GetName(typeof(AnimationType), actionID);
            }
            else
            {
                _sequenceAction2ID = actionID;
                _sequenceAnimation2.GetComponentInChildren<Text>().text = Enum.GetName(typeof(AnimationType), actionID);
            }
        }

        #endregion //Sequence Methods

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

        void IObservable<SequenceAnimationTriggeredArgs>.Attach(IObserver<SequenceAnimationTriggeredArgs> observer)
        {
            SequenceAnimationTriggered += observer.OnNotified;
        }

        void IObservable<SequenceAnimationTriggeredArgs>.Detach(IObserver<SequenceAnimationTriggeredArgs> observer)
        {
            SequenceAnimationTriggered -= observer.OnNotified;
        }

        void IObservable<SequenceAnimationTriggeredArgs>.Notify(SequenceAnimationTriggeredArgs eventArgs)
        {
            if (SequenceAnimationTriggered != null)
            {
                SequenceAnimationTriggered.Invoke(this, eventArgs);
            }
        }

        #endregion //IObservable Interface Implementation
    }
}