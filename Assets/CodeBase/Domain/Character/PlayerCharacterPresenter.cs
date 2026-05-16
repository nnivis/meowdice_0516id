using CodeBase.Domain.Character.CatSkin;
using CodeBase.Domain.Match.Data;
using UnityEngine;

namespace CodeBase.Domain.Character
{
    public class PlayerCharacterPresenter : MonoBehaviour
    {
        [SerializeField] private CatSkinView _catSkinViewPrefab;
        [SerializeField] private RectTransform _localPlayerRoot;
        [SerializeField] private RectTransform _playerOpponentRoot;

        private CatSkinView _localPlayerView;
        private CatSkinView _opponentPlayerView;

        public void Bind(MatchPlayerContext context)
        {
            Unbind();

            _localPlayerView = Instantiate(_catSkinViewPrefab, _localPlayerRoot);
            _localPlayerView.Apply(context.LocalPlayer.SkinId, context.LocalPlayer.Name);

            _opponentPlayerView = Instantiate(_catSkinViewPrefab, _playerOpponentRoot);
            _opponentPlayerView.Apply(context.OpponentPlayer.SkinId, context.OpponentPlayer.Name);
        }

        public void Unbind()
        {
            if (_localPlayerView != null)
                Destroy(_localPlayerView.gameObject);

            if (_opponentPlayerView != null)
                Destroy(_opponentPlayerView.gameObject);

            _localPlayerView = null;
            _opponentPlayerView = null;
        }
    }
}