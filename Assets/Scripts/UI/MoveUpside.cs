using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [RequireComponent(typeof(RectTransform))]
    public class MoveUpside : MonoBehaviour
    {
        [SerializeField] [Range(0,1)] private float _offset; 
        
        private RectTransform _rect;
        
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            Move();
        }

        private void Move()
        {
            _rect.offsetMin = new Vector2(_rect.offsetMin.x, _rect.offsetMin.y - ((Screen.height - Screen.safeArea.height) * _offset));
            _rect.offsetMax = new Vector2(_rect.offsetMax.x, _rect.offsetMax.y + ((Screen.height - Screen.safeArea.height) * (1 - _offset)));
        }
    }
}