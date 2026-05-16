using System;
using System.Collections.Generic;
using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;
using TMPro;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    public class FieldColumnView : MonoBehaviour
    {
        public RectTransform CellRoot => _cellRoot;
        public int CellsCount => _cells.Count;

        [SerializeField] private RectTransform _cellRoot;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private List<CellView> _cells = new();

        private const int UpTextValue = 49; // это нужно менять т.к адаптива нормального не будет
        private const int DownTextValue = -447;

        public void Build(List<CellView> cells, ScoreTextPosition scoreTextPosition)
        {
            _cells = cells;
            ApplyScoreTextPosition(scoreTextPosition);
        }

        public void PlaceDiceView(DiceView diceView, int row)
        {
            if (row < 0 || row >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(row));

            _cells[row].SetDiceView(diceView);
        }

        public void RemoveDiceView(int row)
        {
            if (row < 0 || row >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(row));

            _cells[row].ClearDiceView();
        }

        public CellView GetCell(int row)
        {
            if (row < 0 || row >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(row));

            return _cells[row];
        }

        public void Clear()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i] != null)
                    Destroy(_cells[i].gameObject);
            }

            _cells.Clear();
        }

        public void SetScore(int score)
        {
            if (_scoreText != null)
                _scoreText.text = score.ToString();
        }

        private void ApplyScoreTextPosition(ScoreTextPosition position)
        {
            if (_scoreText == null)
                return;

            Vector2 anchoredPosition = _scoreText.rectTransform.anchoredPosition;

            anchoredPosition.y = position == ScoreTextPosition.Top
                ? UpTextValue
                : DownTextValue;

            _scoreText.rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}