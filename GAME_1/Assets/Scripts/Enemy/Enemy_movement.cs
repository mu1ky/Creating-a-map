using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //��� ����������� ������� 
using Game.Utils; //��� ������� � ������� ���������� ������ �����������
using UnityEngine.InputSystem.XR.Haptics;
using UnityEditor.ShaderKeywordFilter;
using Unity.Burst.Intrinsics;
public class Enemy_movement : MonoBehaviour
{
    //����� �������� ����� ����� �������� ��������� � ������������� NavMesh
    //private NavMeshAgent NAV_meshAgent;
    private Rigidbody2D rb;
    public event EventHandler OnEnemyAttack; //������� ��� �����
    [SerializeField] private State _startingState; //��������� ��������� �����
    [SerializeField] private float _roamingdistanceMax = 7f; //������������ ��������� ��������
    [SerializeField] private float _roamingdistanceMin = 3f; //����������� ��������� ��������
    [SerializeField] private float _roamingTimeMax = 2f; //������������ ����� ��������
    [SerializeField] private bool _isChacingEnemy = false; //����������� ����� ������������
    //����, ��������� � ���, �������� �� ���� ������������
    //[SerializeField] private bool _isRunningEnemy = false;
    [SerializeField] private bool _isAttackingEnemy = false; //����������� ����� ���������
    //����, ��������� � ���, �������� �� ���� ������������
    private enum State
    {
        Idle,
        Roaming,
        Chacing,
        Attacking,
        Death
    }//������ ���������

    [SerializeField] private float _attackingDistance = 2f; //���������� �����
    [SerializeField] private float _chacingDistance = 4f; //���������� �������������
    [SerializeField] private float _chacingSpeedMultiplaier = 2f; //��������� ��� �������������

    private NavMeshAgent _navMeshAgent;
    private State _state; //������� ��������� �����
    private float _roamingTime; //����� ��������
    private Vector2 _roamPosition; //�������� ����� �������������
    private Vector2 _startingPosition; //������� ��������������

    private float _MAINSpeed;
    private float _roamingSpeed; //�������� ��������(��������� ��������)
    private float _chacingSpeed; //�������� �������������

    [SerializeField] private float _attackRate = 2f; //������������� �����
    private float _nextAttackTime = 0f; //����� ��������� �����
    public bool IsRunning
    //����� (��������), ������� ����������� ����� ���� ��� ���
    {
        get
        {
            Vector2 V = rb.velocity;
            if ((V == Vector2.zero))
            {
                return false; //��� ��������
            }
            else
            {
                return true; //���� ��������
            }
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //_navMeshAgent = GetComponent<NavMeshAgent>();
        _state = _startingState;
        /*
        _navMeshAgent.updateRotation = false; //����� �� ���� ��������
        _navMeshAgent.updateUpAxis = false; //����� ���������� ���� �� ������ �� ���������� ��������� ������� �� ��� Y
        */
        //���������� �������� ��� �������������
        //_roamingSpeed = _navMeshAgent.speed; //��������� �������� �������� (��������� ��������)
        _roamingSpeed = _MAINSpeed;
        //_chacingSpeed = _navMeshAgent.speed * _chacingSpeedMultiplaier; //���������� �������� �������������
        _chacingSpeed = _MAINSpeed * _chacingSpeedMultiplaier;
    }
    private void Update()
    {
        StateHandler();
        MovingDirectionHandle();
        //������� ��������� � �������� ��������
    }
    private void StateHandler()
    {
        switch (_state)
        {
            default:
            case State.Roaming:
                _roamingTime -= Time.deltaTime; //��������� ����� ��� ������� ����� ��������
                if (_roamingTime < 0) //���� ����� �������� ���������� ������ ����
                {
                    Roaming(); //���� ����� ����� ��� �������� � ����� �����������
                    _roamingTime = _roamingTimeMax; //��������� ����� ��������
                }
                CheckCurrentState(); //�������� ���������
                break;
            case State.Chacing:
                ChacingTarget(); //������ �������������
                CheckCurrentState(); //�������� ���������
                break;
            case State.Attacking:
                AttackingTarget(); //������ �����
                CheckCurrentState(); //�������� ���������
                break;
            case State.Death:
                break;
            case State.Idle:
                break;
        }
    }
    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime) //���� ������� ����� ������ ������� �����
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            //�������� ������� �����
            _nextAttackTime = Time.time + _attackRate;
            //������������� ����� ����� �����
        }
    }
    /*
    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
        //�������� ��������� ��� ���������
    }
    */
    private void ChacingTarget()
    {
        rb.MovePosition(Player.Instance.transform.position);
        //_navMeshAgent.SetDestination(Player.Instance.transform.position);
        //����� ����� ��� �������� ����� ��� ��������� �����
    }
    private void CheckCurrentState() //������� ��� �������� ���������
    {
        float distance_to_player = Vector2.Distance(transform.position, Player.Instance.transform.position);
        State new_state = State.Roaming;
        if (_isChacingEnemy)
        {
            if (distance_to_player <= _chacingDistance)
            {
                new_state = State.Chacing;
            }
            //��������� ��������� ���� �������� � ��������� ������������� �����
        }
        if (_isAttackingEnemy)
        {
            if (distance_to_player <= _attackingDistance)
            {
                new_state = State.Attacking;
            }
            //��������� ��������� ���� �������� � ��������� ����� �����
        }
        if (new_state != _state) //���� ��������� ����� ���������
        {
            if (new_state == State.Chacing) //�� �������������
            {
                //_navMeshAgent.ResetPath(); //����� ����� ����������� ��������
                //_navMeshAgent.speed = _chacingSpeed; //������������� �������� �������������
                //��� ��� ������ ���������� ����� ��������
                _MAINSpeed = _chacingSpeed;
            }
            else if (new_state == State.Roaming) //�� ��������
            {
                _roamingTime = 0f; //�������� ������� ������� ��������
                //_navMeshAgent.speed = _roamingSpeed; //������������� �������� ��������
                _MAINSpeed = _roamingSpeed;
            }
            else if (new_state == State.Attacking) //�� �����
            {
                //_navMeshAgent.ResetPath(); //����� ����� ����������� ��������
                //��� ��� ������ ���������� ����� ��������
            }
            _state = new_state; //������ ����� ��������� �������
        }
    }
    private void Roaming()
    {
        _startingPosition = transform.position;
        //��������� ������ ��� �������� �������
        _roamPosition = GetRoamingPosition();
        //��������� ������� ����� �������, � ������� ���� ����� ���������
        //ChangeFacingDirection(_startingPosition, _roamPosition);
        //������������� �����, ����� �� ������ �� �����
        rb.MovePosition(_roamPosition);
        //_navMeshAgent.SetDestination(_roamPosition);
        //��� ������ ���������� NavMesh ����� ��������� �������� ����� ��� ��������
    }
    private Vector2 GetRoamingPosition()
    {
        return _startingPosition + Common1.GetRandomDir() * UnityEngine.Random.Range(_roamingdistanceMin, _roamingdistanceMax);
        //������� ��� ���������� ��������� �������� �����
        //Common.GetRandomDir() - ������� ��� ����������� ����������� ��������
    }
    private void ChangeFacingDirection(Vector2 firstPosition, Vector2 targetPosition)
    {
        //������� ��� ��������� ����� � ������� ����� ��������
        if (firstPosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
            //�������� �� ��� Oy �� 180
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //��� ��������� ������
        }
    }
    private float _checkDirectionTime = 0f;
    private float _checkDirectionDeltaTime = 0.1f;
    private Vector2 _lastPosition;
    private void MovingDirectionHandle()
    {
        if (Time.time > _checkDirectionTime)
        {
            if (IsRunning)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_state == State.Attacking)
            {
                ChangeFacingDirection(_lastPosition, Player.Instance.transform.position);
            }
            _lastPosition = transform.position;
            _checkDirectionTime = Time.time * _checkDirectionDeltaTime;
        }
    }
}