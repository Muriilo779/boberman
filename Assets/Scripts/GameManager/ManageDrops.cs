using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

public class ManageDrops : NetworkBehaviour
{
     public static ManageDrops Instance {get; private set;}
     
    [Header("Tilemaps")]
    [SerializeField] private Transform _wall;
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private Tile _spawnTile;
    [SerializeField] private Tile _spawnBoundariesTile;
    [SerializeField] private Tile[] _ground;
    [SerializeField] private Tile[] _indestructible;

    [Header("Grid")]
    private Grid<BackgroundTile> _walls;
    private GridStruct _bombs; 
    public Vector3Int Size { get; private set; }
    public Vector3Int origin;

    [Header("Game Variables")]
    [Range(0f, 1f)]
    [SerializeField] private float _bricksPercentage;
    [Range(0f, 1f)]
    [SerializeField] private float _powerUpPercentage;

    [Header("Items")]
    [SerializeField] private Speed _speedPw;
    [SerializeField] private Blast _blastPw;
    [SerializeField] private Bomb _bombPw;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void CreateInfo()
    {
        //Maybe revert this to a normal grid class?
        _tileMap = GameObject.FindWithTag("Grid").GetComponentInChildren<Tilemap>();
        Size = _tileMap.size;
        origin = _tileMap.origin;
        
        _bombs = new GridStruct(Size.x, Size.y); 
        _walls = new Grid<BackgroundTile>(Size.x, Size.y);
    }
    
    #region Walls
    public void CreateWalls()
    {
        if (!IsServer)
            return;
        
        for (var i = 0; i < Size.x - 1; i++)
            for (var j = 0; j < Size.y; j++)
            {
                var wallChances = Random.Range(0f, 1f);
                var powerUpChances = Random.Range(0f, 1f);

                var cellPos = new Vector3Int(i + origin.x, j + origin.y, origin.z);

                if (CanCreateWall(i, j, cellPos, wallChances))
                {
                    var worldPos = _tileMap.GetCellCenterWorld(cellPos);
                    Transform wallInstance = Instantiate(_wall, worldPos, Quaternion.identity);
                    wallInstance.name = "wall";
                    UpdateGridWall(true, wallInstance.gameObject, i, j);
                    _walls.gridArray[i, j].Item = GetPowerUp(powerUpChances);
                    wallInstance.GetComponent<NetworkObject>().Spawn(true);
                }
            }
    }

    public void RemoveWalls()
    {
        if (!IsServer)
            return;

        for (var i = 0; i < Size.x-1; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var wall = _walls.gridArray[i, j].Wall;

                if (wall == null)
                    continue;
                
                //Activating the animation before the wall destroy
                wall.GetComponent<NetworkObject>().Despawn(true);
                UpdateGridWall(false, null, i, j);
            }
        }
    }
    public void RemoveWall(Vector2 pos)
    {
        if (!IsServer)
            return;

        var gridPos = WorldToGridIndex(pos);
        
        var wall = _walls.gridArray[gridPos.x, gridPos.y].Wall;
        var item = _walls.gridArray[gridPos.x, gridPos.y].Item;

        //Activating the animation before the wall destroy
        wall.GetComponent<Animator>().SetBool("Destroy", true);

        UpdateGridWall(false, null, gridPos.x, gridPos.y);

        if (item == null)
            return;

        var powerUp = Instantiate(item, pos, Quaternion.identity);
        powerUp.GetComponent<NetworkObject>().Spawn(true);
    }

    private GameObject GetPowerUp(float chance)
    {
        if (chance >= _powerUpPercentage)
            return null;

        var index = Random.Range(0, 3);
        switch (index)
        {
            case 0:
                return _speedPw.prefab;
            case 1:
                return _blastPw.prefab;
            case 2:
                return _bombPw.prefab;

            default:
                return null;
        }
    }
    #endregion

    #region Tiles

    public void CreateTiles()
    {
        if (!IsServer)
            return;

        //Create the tiles for all the grid, and set the isUsable variable to be true or false
        for (var i = 0; i < Size.x - 1; i++)
            for (var j = 0; j < Size.y; j++)
            {
                var pos = new Vector3Int(i + origin.x, j + origin.y, origin.z);
                _walls.gridArray[i, j] = new BackgroundTile(UsableTile(pos), false, null, null, pos.x, pos.y);
                _bombs.gridArray[i, j] = new BackgroundBomb(UsableTile(pos), false, pos.x, pos.y);
            }
    }

    private bool UsableTile(Vector3Int pos)
    {
        var tile = _tileMap.GetTile(pos) as Tile;

        return _indestructible.All(t => tile != t);
    }

    private bool CheckTile(Vector3Int pos)
    {
        var tile = _tileMap.GetTile(pos) as Tile;


        if (tile == _spawnTile || tile == _spawnBoundariesTile)
            return false;

        return _ground.Any(t => tile == t);
    }

    public Vector3 GetCellCenterWorld(Vector3 worldPos)
    {
        Vector3Int cellPosition = _tileMap.WorldToCell(worldPos);
        return _tileMap.GetCellCenterWorld(cellPosition);
    }

    public Vector2Int WorldToGridIndex(Vector3 worldPos)
    {
        Vector3Int cellPosition = _tileMap.WorldToCell(worldPos);
        return new Vector2Int(cellPosition.x - origin.x, cellPosition.y - origin.y);
    }
    #endregion

    #region UpdateGrid
    public void UpdateGridBomb(bool hasBomb, int x, int y)
    {
        _bombs.gridArray[x, y].hasBomb = hasBomb;
    }

    private void UpdateGridWall(bool hasWall, GameObject wall, int x, int y)
    {
        _walls.gridArray[x, y].HasWall = hasWall;
        _walls.gridArray[x, y].Wall = wall;
    }
    #endregion

    #region BooleanChecks
    public bool CheckBombs(int x, int y) => _bombs.gridArray[x, y].hasBomb;
    public bool CheckForUsableTiles(int x, int y) => !_walls.gridArray[x, y].IsUsable;
    public bool CheckForWalls(int x, int y) => _walls.gridArray[x, y].HasWall;
    private bool CanCreateWall(int i, int j, Vector3Int pos, float chance) => _walls.gridArray[i, j].IsUsable
                                                                              && CheckTile(pos)
                                                                              && chance < _bricksPercentage;
    #endregion
}
