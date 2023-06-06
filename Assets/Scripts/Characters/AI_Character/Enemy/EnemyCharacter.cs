using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EnemyCharacter : Character
{

    [SerializeField] private float aggroRange = 8f;
    [SerializeField] private float distanceToPerformAttack;
    [SerializeField] private Ability basicAttack;
    [SerializeField] private float projectileSize = 1f;
    
    [Range(0,1f)]
    [SerializeField] private float dropChance;
    [SerializeField] private DropTable dropTable;
    
    public event Action<GameObject> OnEnemyDied;
    
    private GameObject _player;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    public bool isTurret = false;

    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        
        CombatSystem=GetComponent<CombatSystem>();
        CharacterMovement = GetComponent<CharacterMovement>();
        AttributeSet = GetComponent<AttributeSet>();
        AttributeSet.currentHealth.OnValueChanged += HealthChanged;
        _player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
        _animator = GetComponent<Animator>();
        basicAttack = Instantiate(basicAttack);
        basicAttack.Initialize(gameObject);
    }

    private void OnEnable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        _player = player;
    }

    private void HealthChanged(ModifiableAttributeValue health, float prevValue)
    {
        if (health.CurrentValue <= 0)
        {
            OnEnemyDied?.Invoke(gameObject);
            Destroy(gameObject);

            if (Random.Range(0, 1f) <= dropChance)
            {
                ItemData item = dropTable.GetDrop();
                ItemObject.SpawnItemObject(transform.position, item);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTurret)
        {
            PerformBasicAttack(transform.right);
            return;
        }

        if (!_player)
        {
            Debug.LogWarning(gameObject.name + " has no value set for _player. Taking no action.");
            return;
        }
        var playerDirection = _player.transform.position - transform.position;
        float sqrDistance = playerDirection.sqrMagnitude;
        
        //if outside of aggro range do nothing
        if (sqrDistance > aggroRange * aggroRange)
        {
            _animator.SetFloat(Speed, 0);
            return;
        }
        
        //check what is between this character and the player
        //RaycastHit2D hitResult = Physics2D.Raycast(transform.position, playerDirection, playerDirection.magnitude, LayerMask.GetMask("Player", "Default"));

        RaycastHit2D hitResult = Physics2D.CircleCast(transform.position, projectileSize, playerDirection,playerDirection.magnitude,
            LayerMask.GetMask("Player", "Default"));
        //if we got the player we can see them
        if (hitResult.transform.gameObject.CompareTag("Player")) 
        {
            //since we can see them, either  attack, or move closer directly towards them
            if (sqrDistance < distanceToPerformAttack * distanceToPerformAttack)
            {
                PerformBasicAttack(playerDirection);
            }
            else
            {
                hitResult = Physics2D.CircleCast(transform.position, projectileSize, playerDirection,playerDirection.magnitude,
                    LayerMask.GetMask("Player", "Pit"));
                //if there is no pit between us and the player
                //move directly towrards them
                if (hitResult.transform.gameObject.CompareTag("Player"))
                {
                    MoveInDirection(playerDirection);
                }
                else
                {
                    MoveTowardsPosition(_player.transform.position);
                }
                
            }
        }
        else //if we can't see the player, find a path to them
        {
            MoveTowardsPosition(_player.transform.position);
        }
    }

    private void MoveTowardsPosition(Vector3 transformPosition)
    {
        var tilemapDictionary = ServiceLocator.Instance.Get<LevelSceneManager>().GetTilemapDictionary();
        var playerPosition = tilemapDictionary["Walls"].WorldToCell(transformPosition);
        var position = tilemapDictionary["Walls"].WorldToCell(transform.position);

        List<Tilemap> collidableTilemaps = new List<Tilemap>();
        collidableTilemaps.Add(tilemapDictionary["Walls"]);
        collidableTilemaps.Add(tilemapDictionary["Pit"]);
        var path = TilemapUtils.Astar(position, playerPosition, collidableTilemaps);

        //Move via Astar
        if (path != null && path.Count > 0)
        {
            Vector3 destination = tilemapDictionary["Walls"].CellToWorld(path[1]);
            destination += new Vector3(.5f, .5f, 0);
            Vector3 direction = destination - transform.position;
            MoveInDirection(direction.normalized);
        }
    }

    private void MoveInDirection(Vector3 playerDirection)
    {
        //face the direction
        playerDirection.Normalize();
        CharacterMovement.Rotate(playerDirection);
        //move
        CharacterMovement.Move(playerDirection);
    }

    private void PerformBasicAttack(Vector3 direction)
    {
        //face the player
        direction.Normalize();
        CharacterMovement.Rotate(direction);

        //set animator move speed
        _animator.SetFloat(Speed, 0);

        //setup targetdata
        AbilityTargetData targetData = new AbilityTargetData();
        targetData.sourceCharacterDirection = direction;
        targetData.sourceCharacterLocation = transform.position;
        targetData.targetLocation = _player.transform.position;
        targetData.targetGameObject = _player;

        //activate ability
        basicAttack.TryActivate(targetData);
    }
}
