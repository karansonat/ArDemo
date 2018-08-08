using System.Collections.Generic;
using UnityEngine;

namespace LeoAR.Core
{
    public class Model : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _3dModel;
        [SerializeField] private List<AnimationClip> _animations;

        #endregion // Fields

        #region Public Methods

        public void Init()
        {

        }

        #endregion //Public Methods

        #region Private Methods

        private void Animate(AnimationClip animationClip)
        {

        }

        #endregion Private Methods
    }
}