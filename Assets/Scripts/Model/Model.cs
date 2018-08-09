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
        public string Name
        {
            get { return _name; }
        }

        [SerializeField] private List<AttackAction> _animations;
        public List<AttackAction> Animations
        {
            get { return _animations; }
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