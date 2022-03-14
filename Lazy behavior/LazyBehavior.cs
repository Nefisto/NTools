// ReSharper disable InconsistentNaming
// When building compile thrown warning about the new keyword not being necessary, and this will be based if the field is used or not 
#pragma warning disable 108,109, 114

using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NTools
{
    public abstract class LazyBehavior : MonoBehaviour
    {
        #region Basic

        [NonSerialized]
        private SpriteRenderer _spriteRenderer;

        public SpriteRenderer spriteRenderer => _spriteRenderer
            ? _spriteRenderer
            : _spriteRenderer = GetComponent<SpriteRenderer>();


        [NonSerialized]
        private Animator _animator;

        public Animator animator => _animator
            ? _animator
            : _animator = GetComponent<Animator>();

		[NonSerialized]
        private RectTransform _rectTransform;

        public RectTransform rectTransform => _rectTransform
            ? _rectTransform
            : _rectTransform = GetComponent<RectTransform>();
			
        #endregion

        #region Physics

        [NonSerialized]
        private Rigidbody _rigidbody;

        public new Rigidbody rigidbody => _rigidbody
            ? _rigidbody
            : _rigidbody = GetComponent<Rigidbody>();

        [NonSerialized]
        private Rigidbody2D _rigidbody2D;

        public new Rigidbody2D rigidbody2D => _rigidbody2D
            ? _rigidbody2D
            : _rigidbody2D = GetComponent<Rigidbody2D>();

        [NonSerialized]
        private BoxCollider2D _boxCollider2D;

        public BoxCollider2D boxCollider2D => _boxCollider2D
            ? _boxCollider2D
            : _boxCollider2D = GetComponentInChildren<BoxCollider2D>();

        [NonSerialized]
        private CircleCollider2D _circleCollider2D;

        public CircleCollider2D circleCollider2D => _circleCollider2D
            ? _circleCollider2D
            : _circleCollider2D = GetComponentInChildren<CircleCollider2D>();

        [NonSerialized]
        private SphereCollider _sphereCollider;

        public SphereCollider sphereCollider => _sphereCollider
            ? _sphereCollider
            : _sphereCollider = GetComponent<SphereCollider>();
        
        #endregion

        #region UI

        [NonSerialized]
        private Button _button;

        public Button button => _button
            ? _button
            : _button = GetComponent<Button>();

        [NonSerialized]
        private Image _image;

        public Image image => _image ? _image : _image = GetComponent<Image>();

        [NonSerialized]
        private Text _text;

        public Text text => _text ? _text : _text = GetComponent<Text>();

        #endregion
        
        [NonSerialized]
        private Renderer _renderer;
        public new Renderer renderer => _renderer ? _renderer : _renderer = GetComponent<Renderer>();
        
        [NonSerialized]
        private Light _light;
        public new Light light => _light ? _light : _light = GetComponent<Light>();

        [NonSerialized]
        private NavMeshAgent _navMeshAgent;
        public NavMeshAgent navMeshAgent => _navMeshAgent
            ? _navMeshAgent
            : _navMeshAgent = GetComponent<NavMeshAgent>();

        #region Event system

        [NonSerialized]
        private EventTrigger _eventTrigger;

        public EventTrigger eventTrigger => _eventTrigger
            ? _eventTrigger
            : _eventTrigger = GetComponent<EventTrigger>();

        #endregion
    }
}