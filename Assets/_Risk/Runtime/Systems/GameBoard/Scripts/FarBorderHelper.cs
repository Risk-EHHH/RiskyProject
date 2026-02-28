using System;
using Risk.Runtime.GameBoard;
using Risk.Runtime.Utils;
using UnityEngine;

public class FarBorderHelper : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    

    #region MonoBehaviour

    void OnValidate()
    {
        DependencyValidator.NotNull(_lineRenderer, this);
        DependencyValidator.NotNull(_startPoint, this);
        DependencyValidator.NotNull(_endPoint, this);
        
        _startPoint.localPosition = _lineRenderer.GetPosition(0);
        _endPoint.localPosition = _lineRenderer.GetPosition(1);
    }
    

    #endregion
}
