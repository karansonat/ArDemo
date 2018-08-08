using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeoAR.Core
{
    public class ModelController : MonoBehaviour
    {
        #region Fields

        private Model _model;
        private ModelView _view;

        #endregion //Fields

        public ModelController(Model model, ModelView view)
        {
            _model = model;
            _view = view;
        }

        #region Public Methods

        public void AnimateInstantly(string animationName)
        {

        }

        public void AnimateSequence()
        {

        }

        public void Move()
        {

        }

        #endregion //Public Methods
    }
}