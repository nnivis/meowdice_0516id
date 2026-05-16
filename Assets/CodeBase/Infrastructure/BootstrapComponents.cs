using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services;
using CodeBase.Services.StateMachine;
using UnityEngine;
using VContainer;

namespace CodeBase.Infrastructure
{
    public class BootstrapComponents : MonoBehaviour
    {
        [SerializeField] private MainSceneMode _mainSceneMode;
        [SerializeField] private GameCoordinator _gameCoordinator;

        private IDataProvider _dataProvider;
        private IPersistentData _persistentData;
        
        [Inject]
        public void Construct(IDataProvider dataProvider, IPersistentData persistentData)
        {
            _dataProvider = dataProvider;
            _persistentData = persistentData;
            
            InitData();
        }
        
        public void Run()
        {
            _mainSceneMode.Bootstrap(); 
            _mainSceneMode.GoToMain();
        }
        
        private void InitData() => _gameCoordinator.InitData(_dataProvider, _persistentData);
        
    }
    
   
}
