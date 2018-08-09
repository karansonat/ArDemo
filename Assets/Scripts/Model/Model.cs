using System.Collections.Generic;
using UnityEngine;

namespace LeoAR.Core
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "LeoAR/Create Model")]
    public class Model : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _3dModel;
        [SerializeField] private string _name;
        public string ModelName
        {
            get { return _name; }
        }

        [SerializeField] private List<AnimationType> _animations;
        public List<AnimationType> Animations
        {
            get { return _animations; }
        }

        [SerializeField] float _moveSpeed = 2f;
        public float MoveSpeed
        {
            get { return _moveSpeed; }
        }
        
        #endregion // Fields

        #region Public Methods

        public GameObject Create3DView()
        {
            return Instantiate(_3dModel, Vector3.zero, Quaternion.identity);
        }

        #endregion //Public Methods

        #region Private Methods

        #endregion Private Methods
    }
}