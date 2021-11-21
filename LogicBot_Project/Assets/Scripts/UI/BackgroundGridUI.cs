using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(GridLayoutGroup))]
public class BackgroundGridUI : MonoBehaviour
{
    [SerializeField] private Color _startColor = Color.clear;
    [SerializeField] private Color _endColor = Color.white;

    [SerializeField] private float _delayFactor = 0.1f;
    
    [SerializeField] private int _gridCols = 16;
    [SerializeField] private int _gridRows = 12;

    [SerializeField] private Image _tileImagePrefab;
    
    [SerializeField] private GameEvent _levelCompletedGameEvent;
    
    private List<Image> _gridTileImageList = new List<Image>();
    
    private void Awake()
    {
        for (int i = 0; i < _gridRows; i++)
        {
            for (int j = 0; j < _gridCols; j++)
            {
               Image newTileImage = Instantiate(_tileImagePrefab, transform);
               newTileImage.color = _startColor;
               _gridTileImageList.Add(newTileImage);
            }
        }
        _levelCompletedGameEvent.AddListener(ShowBackgroundGrid);
    }
    
    [ContextMenu("Animate Background Grid")]
    public void ShowBackgroundGrid()
    {
        int n = _gridCols;
        int m = _gridRows;
        
        int row = 0, col = 0;
        bool up = true;
        
        float delayFactor = 0.01f;

        while (row < m && col < n)
        {
            Image currentTileImage = null;
            if (up)
            {
                while (row > 0 && col < n - 1)
                {
                    currentTileImage = _gridTileImageList[row * n + col];
                    if (currentTileImage != null)
                    {
                        AnimateTile((row + col) * _delayFactor, currentTileImage, _endColor);
                    }
                    row--;
                    col++;
                }
                currentTileImage = _gridTileImageList[row * n + col];
                if (currentTileImage != null)
                {
                    AnimateTile((row + col) * _delayFactor, currentTileImage, _endColor);
                }
                if (col == n - 1)
                {
                    row++;
                }
                else
                {
                    col++;
                }
            }
            else
            {
                while (col > 0 && row < m - 1)
                {
                    currentTileImage = _gridTileImageList[row * n + col];
                    if (currentTileImage != null)
                    {
                        AnimateTile((row + col) * _delayFactor, currentTileImage, _endColor);
                    }
                    row++;
                    col--;
                }
                currentTileImage = _gridTileImageList[row * n + col];
                if (currentTileImage != null)
                {
                    AnimateTile((row + col) * _delayFactor, currentTileImage, _endColor);
                }
                if (row == m - 1)
                {
                    col++;
                }
                else
                {
                    row++;
                }
            }
            up = !up;
        }
        this.Wait((n + m) / 1.5f * _delayFactor, HideBackgroundGrid);
    }
    
    public void HideBackgroundGrid()
    {
        int n = _gridCols;
        int m = _gridRows;
        
        int row = 0, col = 0;
        bool up = true;
        
        float delayFactor = 0.01f;

        while (row < m && col < n)
        {
            Image currentTileImage = null;
            if (up)
            {
                while (row > 0 && col < n - 1)
                {
                    currentTileImage = _gridTileImageList[row * n + col];
                    if (currentTileImage != null)
                    {
                        AnimateTile((row + col) * _delayFactor, currentTileImage, _startColor);
                    }
                    row--;
                    col++;
                }
                currentTileImage = _gridTileImageList[row * n + col];
                if (currentTileImage != null)
                {
                    AnimateTile((row + col) * _delayFactor, currentTileImage, _startColor);
                }
                if (col == n - 1)
                {
                    row++;
                }
                else
                {
                    col++;
                }
            }
            else
            {
                while (col > 0 && row < m - 1)
                {
                    currentTileImage = _gridTileImageList[row * n + col];
                    if (currentTileImage != null)
                    {
                        AnimateTile((row + col) * _delayFactor, currentTileImage, _startColor);
                    }
                    row++;
                    col--;
                }
                currentTileImage = _gridTileImageList[row * n + col];
                if (currentTileImage != null)
                {
                    AnimateTile((row + col) * _delayFactor, currentTileImage, _startColor);
                }
                if (row == m - 1)
                {
                    col++;
                }
                else
                {
                    row++;
                }
            }
            up = !up;
        }
    }
    
    private void AnimateTile(float delay, Image tileImage, Color endColor)
    {
        tileImage.DOColor(endColor, 0.5f).SetDelay(delay);
    }
}
