using LeoAR.UI;
using UnityEngine;

namespace LeoAR.Core
{
    public class MainMenuFactory
    {
        #region Singleton

        private static readonly MainMenuFactory _instance = new MainMenuFactory();
        public static MainMenuFactory Instance
        {
            get { return _instance; }
        }

        static MainMenuFactory()
        {
        }

        #endregion //Singleton

        #region Public Methods

        public MainMenu CreateModel()
        {
            var model = Resources.Load<MainMenu>("States/MainMenu");
            return Object.Instantiate(model);
        }

        public MainMenuView CreateView()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/MainMenu/MainMenuView");
            return Object.Instantiate(prefab).GetComponent<MainMenuView>();
        }

        #endregion //Public Methods
    }
}
