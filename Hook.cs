using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Hook : MonoBehaviour
{
    public enum HookState
    {
        Rotation,// Start State
        Shoot,// Shoot State
        Pull, // After Shoot
    }

    HookState hookState = HookState.Rotation;

    #region Serializefield
    [SerializeField] private float _rotateSpeed = 100;
    [SerializeField] private float _maxAngle = 60;
    [SerializeField] private float _minAngle = -60;
    [SerializeField] private float _shootSpeed =5;
    [SerializeField] private float _maxYPull = 3;
    [SerializeField] private float _maxXPull = 4;
    [SerializeField] private float _coolDownTime = 10;
    [SerializeField] private Text _goldNumber;
    [SerializeField] private Text _gameTimeText;
    [SerializeField] private GameObject _3thWin;
    [SerializeField] private GameObject _2thWin;
    [SerializeField] private GameObject _1thWin;
    #endregion

    #region Private
    private SpriteRenderer _spriteRenderer;
    private float _angle;
    private Vector3 _originPosition;
    private float _slowDown;
    private int _money;
    private float _currentTime;
    private Transform _thing;

    #endregion
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (hookState == HookState.Pull) return;
        _thing = col.transform;
        _slowDown = col.GetComponent<Thing>()._slowSpeed;
        _money += col.GetComponent<Thing>()._value;
        _thing.SetParent(transform);
        hookState = HookState.Pull;
    }
    private void Awake()
    {
        _currentTime = _coolDownTime;
        _originPosition = transform.position;
        _spriteRenderer = transform.root.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        _gameTimeText.text = _currentTime.ToString("00");
        if(_currentTime > 0)
        {
          _currentTime -= Time.deltaTime;
        }
        if (_currentTime <= 0)
        {
            _3thWin.SetActive(true);
        }
        if (_currentTime <= 0 && _money >= 700 && _money <= 1999)
        {
            _2thWin.SetActive(true);
        }
        if (_currentTime <= 0 && _money >= 2000)
        {
            _1thWin.SetActive(true);
        }
        switch (hookState)
        {
            case HookState.Rotation:
                Rotation();
                break;
            case HookState.Shoot:
                transform.Translate(Vector3.down* _shootSpeed* Time.deltaTime);

                if (Mathf.Abs(transform.position.x) > _maxXPull  || Mathf.Abs(transform.position.y) > _maxYPull)
                {
                    hookState = HookState.Pull;
                }
                break;
            case HookState.Pull:
                transform.Translate(Vector3.up* (_shootSpeed - _slowDown)* Time.deltaTime);
                if(Mathf.Floor(transform.position.x) == Mathf.Floor(_originPosition.x) && Mathf.Floor(transform.position.y) == Mathf.Floor(_originPosition.y))
                {
                    transform.position = _originPosition;
                    hookState = HookState.Rotation;
                    if(_thing != null)
                    {
                        _slowDown = 0;
                        Destroy(_thing.gameObject);
                        SetGold(_money);
                    }
                }
                break;
        }
        
    }
    private void SetGold(int gold)
    {
        _goldNumber.text = (" Money: " + gold);
    }
    private void Rotation()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 1) && _currentTime > 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                hookState = HookState.Shoot;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                hookState = HookState.Shoot;
            }
        }
        if (_angle > _maxAngle || _angle < _minAngle)
            _rotateSpeed *= -1;

        _angle += _rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }
}
