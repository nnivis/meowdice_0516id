using CodeBase.Domain.Match.Data;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Domain.Match
{
    public class MatchPresenter : MonoBehaviour
    {
        [SerializeField] private GameplayUiRoot _uiRoot;

        private IMatchReadModel _model;
        private MatchPlayerContext _playerContext;

        public void StartMatch(IMatchReadModel model, MatchPlayerContext playerContext)
        {
            StopMatch();

            _model = model;
            _playerContext = playerContext;

            _uiRoot.Boards.Bind(_model, _playerContext);
            _uiRoot.Field.Bind(_model, _playerContext);
            _uiRoot.Characters.Bind(_playerContext);
        }

        public void StopMatch()
        {
            _uiRoot.Boards.Unbind();
            _uiRoot.Field.Unbind();
            _uiRoot.Characters.Unbind();

            _model = null;
            _playerContext = null;
        }

        private void OnDestroy() => StopMatch();
    }
}