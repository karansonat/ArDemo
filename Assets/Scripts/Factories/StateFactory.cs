using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeoAR.Core
{
    public class StateFactory
    {
        #region Singleton

        private static readonly StateFactory _instance = new StateFactory();
        public static StateFactory Instance
        {
            get { return _instance; }
        }

        static StateFactory()
        {
        }

        #endregion //Singleton

        #region Public Methods

        public MainMenuState CreateMainMenuState()
        {
            var state = new MainMenuState();
            return state;
        }

        public ModelPreviewState CreateModelPreviewState()
        {
            var state = new ModelPreviewState();
            return state;
        }

        #endregion //Public Methods
    }
}
