using CodeBase.Domain.Board;
using CodeBase.Domain.Character;
using CodeBase.Domain.Field;
using UnityEngine;

namespace CodeBase.UI
{
    public class GameplayUiRoot : MonoBehaviour
    {
        [SerializeField] private BoardsPresenter _boards;
        [SerializeField] private FieldPresenter _field;
        [SerializeField] private PlayerCharacterPresenter _playerCharacter;

        public BoardsPresenter Boards => _boards;
        public FieldPresenter Field => _field;
        public PlayerCharacterPresenter Characters => _playerCharacter;
    }
}