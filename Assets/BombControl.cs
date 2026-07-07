using Unity.Netcode;
using UnityEngine;

public class BombControl : NetworkBehaviour
{
    [SerializeField] private Explosion _explosionStartPrefab;
    [SerializeField] private Explosion _explosionMiddlePrefab;
    [SerializeField] private Explosion _explosionEndPrefab;

    [SerializeField] private ManageDrops _manageDrops;
    [SerializeField] private PlayerBomb _playerBomb;

    [SerializeField] private int _explosionRadius;
    [SerializeField] private float _bombFuseTime = 3f;
    [SerializeField] private Vector3 _worldPos;
    [SerializeField] private Vector2Int _gridPos;
    private void Awake()
    {
        _manageDrops = ManageDrops.Instance;
    }

    public void Initialize(int explosionRadius, PlayerBomb playerBomb, Vector3 worldPos, Vector2Int gridPos)
    {
        _explosionRadius = explosionRadius;
        _playerBomb = playerBomb;
        _worldPos = worldPos;
        _gridPos = gridPos;
    }

    private void Update()
    {
        if (_bombFuseTime < 0)
        {
            StartExplosionServerRpc();
            DestroyBombServerRpc();
        }
        else
            _bombFuseTime -= Time.deltaTime;
    }
    #region Explosions

    [Rpc(SendTo.Server)]
    private void StartExplosionServerRpc()
    {
        var explosion = Instantiate(_explosionStartPrefab, _worldPos, Quaternion.identity);
        explosion.GetComponent<NetworkObject>().Spawn(true);

        ExplodeServerRpc(_worldPos, Vector2.up, _explosionRadius);
        ExplodeServerRpc(_worldPos, Vector2.down, _explosionRadius);
        ExplodeServerRpc(_worldPos, Vector2.left, _explosionRadius);
        ExplodeServerRpc(_worldPos, Vector2.right, _explosionRadius);
    }

    [Rpc(SendTo.Server)]
    private void ExplodeServerRpc(Vector2 position, Vector2 direction, int length)
    {
        if (length < 1)
            return;

        position += direction;

        var pos = _manageDrops.WorldToGridIndex(new Vector3(position.x, position.y, _worldPos.z));

        if (_manageDrops.CheckForUsableTiles(pos.x, pos.y))
            return;

        if (_manageDrops.CheckForWalls(pos.x, pos.y)) 
        {
            _manageDrops.RemoveWall(position);
            return;
        }

        var isLast = length > 1 ? _explosionMiddlePrefab : _explosionEndPrefab; 
        var explosion = Instantiate(isLast, position, Quaternion.identity);
        explosion.GetComponent<NetworkObject>().Spawn(true);
        explosion.SetDirectionClientRpc(direction);

        ExplodeServerRpc(position, direction, length - 1);
    }

    #endregion

    [Rpc(SendTo.Server)]
    private void DestroyBombServerRpc()
    {
        _manageDrops.UpdateGridBomb(false, _gridPos.x, _gridPos.y);

        _playerBomb.GetBombServerRpc();
        GetComponent<NetworkObject>().Despawn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
