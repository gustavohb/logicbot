using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(GridLayoutGroup))]
public class BackgroundGridUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color _startColor = Color.clear;
    [SerializeField] private Color _endColor = Color.white;
    
    [SerializeField] private int _gridCols = 16;
    [SerializeField] private int _gridRows = 12;
    [SerializeField] private int _cellSpacing = 7;

    [SerializeField] private float _delayFactor = 0.1f;
    
    [SerializeField] private float _animateTileDuration = 0.5f;
    [SerializeField] private float _animateSingleTileTime = 1f;

    [SerializeField] private int _minBlinkTileCol = 4;
    [SerializeField] private int _minBlinkTileRow = 4;
    
    [SerializeField] private int _maxBlinkTileCol = 10;
    [SerializeField] private int _maxBlinkTileRow = 10;
    
    [Header("References")]
    
    [SerializeField] private CanvasScaler _canvasScaler;
    [SerializeField] private Image _tileImagePrefab;
    
    [Header("Events")]
    [SerializeField] private GameEvent _levelCompletedGameEvent;

    private List<Image> _gridTileImageList = new List<Image>();
    private GridLayoutGroup _gridLayoutGroup;
    private Vector2 _referenceScreenResolution;
    private float _showSingleTileTimer; 
    private float _showSingleTileDuration;
    
    private void Awake()
    {
        _referenceScreenResolution = _canvasScaler.referenceResolution;
        
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _gridLayoutGroup.spacing = new Vector2(_cellSpacing, _cellSpacing);
        _gridLayoutGroup.padding = new RectOffset(_cellSpacing, _cellSpacing, _cellSpacing, _cellSpacing);
        _gridLayoutGroup.cellSize = CalculateCellSize();

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

    private void Start()
    {
        _showSingleTileTimer = _animateSingleTileTime;
    }

    private void Update()
    {
        BlinkTiles();
    }

    private void BlinkTiles()
    {
        if (_showSingleTileTimer > 0)
        {
            _showSingleTileTimer -= Time.deltaTime;
            if (_showSingleTileTimer <= 0)
            {
                int col = UnityEngine.Random.Range(_minBlinkTileCol, _maxBlinkTileCol);
                int row = UnityEngine.Random.Range(_minBlinkTileRow, _maxBlinkTileRow);
                
                
                BlinkTiles(0.01f, col, row);
                
                col = UnityEngine.Random.Range(_minBlinkTileCol, _maxBlinkTileCol);
                row = UnityEngine.Random.Range(_minBlinkTileRow, _maxBlinkTileRow);
                
                BlinkTiles(0.15f, col, row);
            }
        }
    }

    private void BlinkTiles(float delay, int col, int row)
    {
        BlinkSingleTile(0, col, row);
        BlinkRandomNeighborTiles(1f, col, row);
    }

    private void BlinkRandomNeighborTiles(float delay, int col, int row)
    {
        int rand = UnityEngine.Random.Range(0, 5);
        switch (rand)
        {
            case 0:
                return;
            case 1:
                if (row + 1 < _gridRows)
                {
                    BlinkSingleTile(delay, col, row + 1);
                }
                break;
            case 2:
                if (col - 1 >= 0)
                {
                    BlinkSingleTile(delay + 0.1f, col - 1, row);
                    if (col - 2 >= 0)
                    {
                        BlinkSingleTile(delay + 0.15f, col - 2, row);
                    }
                }
                break;
            case 3:
                if (col + 1 < _gridCols)
                {
                    BlinkSingleTile(delay + 0.1f, col + 1, row);
                }
                break;
            case 4:
                if (col + 1 < _gridCols)
                {
                    BlinkSingleTile(delay + 0.1f, col + 1, row);
                    if (row + 1 < _gridRows)
                    {
                        BlinkSingleTile(delay + 0.15f, col + 1, row + 1);
                    }
                }
                break;
        }
    }

    private void BlinkSingleTile(float delay, int col, int row)
    {
        Image currentTileImage = _gridTileImageList[row * _gridCols + col];
        AnimateTile(delay, _animateTileDuration, currentTileImage, _endColor, () =>
        {
            AnimateTile(0.01f, _animateTileDuration, currentTileImage, _startColor, () =>
            {
                _showSingleTileTimer = _animateSingleTileTime;
            });
        });
    }
    
    private Vector2 CalculateCellSize()
    {
        float screenWidth = _referenceScreenResolution.x;
        float screenHeight = _referenceScreenResolution.y;
        
        float horizontalSpacing = (_gridCols + 1) * _cellSpacing;
        float verticalSpacing = (_gridRows + 1) * _cellSpacing;

        float availableHorizontalTileSpace = screenWidth - horizontalSpacing;
        float availableVerticalTileSpace = screenHeight - verticalSpacing;

        Vector2 cellSize =
            new Vector2(availableHorizontalTileSpace / _gridCols, availableVerticalTileSpace / _gridRows);

        return cellSize;
    }
    
    public void ShowBackgroundGrid()
    {
        int x = UnityEngine.Random.Range(0, _gridCols);
        int y = UnityEngine.Random.Range(0, _gridRows);

        int maxDist = 0;
        
        for (int col = 0; col < _gridCols; col++) {
            for (int row = 0; row < _gridRows; row++)
            {
                int dist = Mathf.Abs(x - col) + Mathf.Abs(y - row); // calculate distance
                if (maxDist < dist)
                {
                    maxDist = dist;
                }
                Image currentTileImage = _gridTileImageList[row * _gridCols + col];
                if (currentTileImage != null)
                {
                    AnimateTile((dist) * _delayFactor, _animateTileDuration, currentTileImage, _endColor);
                }
            }
        }
        
        this.Wait((float)maxDist / 1.4f * _delayFactor, () =>
        {
            HideBackgroundGrid(x, y);
        });
    }

    public void HideBackgroundGrid(int x, int y)
    {
        Image currentTileImage;
        
        for (int col = 0; col < _gridCols; col++)
        {
            for (int row = 0; row < _gridRows; row++)
            {
                int dist = Mathf.Abs(x - col) + Mathf.Abs(y - row); // calculate distance

                currentTileImage = _gridTileImageList[row * _gridCols + col];
                if (currentTileImage != null)
                {
                    AnimateTile((dist) * _delayFactor, _animateTileDuration, currentTileImage, _startColor);
                }
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void AnimateTile(float delay, float duration, Image tileImage, Color endColor, Action onComplete = null)
    {
        tileImage.DOColor(endColor, duration).SetDelay(delay).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
