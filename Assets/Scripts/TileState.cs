using UnityEngine;
using Random = UnityEngine.Random;

public enum EPlayerID
{
    None,
    Player1,
    Player2,
    RobotDestroyer
}

public enum EState
{
    Empty,
    Growing,
    Grown,
    Impassable,
    Overgrown,
    Burned
}

public class TileState : MonoBehaviour
{
    private EPlayerID _ownerID = EPlayerID.None;
    [SerializeField] public EState _state;
    [SerializeField] private float defaultTimerGrow;
    [SerializeField] private float defaultTimerBurn;
    private float timerGrow = 0;
    private float _timerBurn = 0;

    private bool _watered = false;

    [SerializeField] public GameObject empty;
    [SerializeField] public GameObject growing;
    [SerializeField] public GameObject growing_b;
    [SerializeField] public GameObject grown;
    [SerializeField] public GameObject grown_b;
    [SerializeField] public GameObject impassable;
    [SerializeField] public GameObject overgrown;
    [SerializeField] public GameObject burned;

    private Mesh emptyMesh;
    private Mesh growingMesh;
    private Mesh growingMesh_b;
    private Mesh grownMesh;
    private Mesh grownMesh_b;
    private Mesh impassableMesh;
    private Mesh overgrownMesh;
    private Mesh burnedMesh;
    
    private MeshFilter _myMesh;
    private Renderer _myRenderer;
    private BoxCollider _myCollider;
    
    [SerializeField] public GameObject seed1Prefab;
    [SerializeField] public GameObject seed2Prefab;
    [SerializeField] public float swingStrength = 100;    // How far seed will fly

    private bool _firstCall = true;

    public void Awake()
    {
        MeshFilter viewedModelFilter = (MeshFilter)empty.GetComponent("MeshFilter");
        emptyMesh = viewedModelFilter.sharedMesh;
        viewedModelFilter = (MeshFilter)growing.GetComponent("MeshFilter");
        growingMesh = viewedModelFilter.sharedMesh;
        viewedModelFilter = (MeshFilter)growing_b.GetComponent("MeshFilter");
        growingMesh_b = viewedModelFilter.sharedMesh;
        viewedModelFilter = (MeshFilter)grown.GetComponent("MeshFilter");
        grownMesh = viewedModelFilter.sharedMesh;
        viewedModelFilter = (MeshFilter)grown_b.GetComponent("MeshFilter");
        grownMesh_b = viewedModelFilter.sharedMesh;
        viewedModelFilter = (MeshFilter)impassable.GetComponent("MeshFilter");
        impassableMesh = viewedModelFilter.sharedMesh;
        viewedModelFilter = (MeshFilter)overgrown.GetComponent("MeshFilter");
        overgrownMesh = viewedModelFilter.sharedMesh;
        viewedModelFilter = (MeshFilter)burned.GetComponent("MeshFilter");
        burnedMesh = viewedModelFilter.sharedMesh;
    }

    public EPlayerID GetOwner()
    {
        return _ownerID;
    }

    public void Water()
    {
        if (!_watered)
        {
            _watered = true;
            _timerBurn *= 0.5f;
            timerGrow *= 0.5f;
            Debug.Log("Rain");
        }
    }
    
    private void FixedUpdate()
    {
        if (_state == EState.Burned)
        {
            _timerBurn -= Time.fixedDeltaTime;
            if (_timerBurn <= 0)
            {
                SetState(EState.Empty);

                _timerBurn = 0;
                _watered = false;
            }
        }
        else if (_state == EState.Growing)
        {
            timerGrow -= Time.fixedDeltaTime;
            if (timerGrow <= 0)
            {
                SetState(EState.Grown);
                timerGrow = 0;
                _watered = false;
            }
        }
    }

    public void SetState(EState newState)
    {
        Debug.Log("Set STATE");
        if (_firstCall)
        {
            _myMesh = GetComponentInChildren<MeshFilter>();
            _myRenderer = _myMesh.GetComponent<Renderer>();
            _myCollider = GetComponent<BoxCollider>();
            _firstCall = false;
        }

        if (_state == EState.Impassable)
        {
            _myCollider.center = new Vector3(0, 0, 0);
            this.gameObject.tag = "NotAWall";
        }

        _state = newState;
        switch (_state)
        { 
            case EState.Empty: _myMesh.sharedMesh  = emptyMesh;
                //_myRenderer.material.color = new Color(255,255,255,1);
                _myRenderer.materials = empty.GetComponent<Renderer>().sharedMaterials;
                break;
            case EState.Growing: _myMesh.sharedMesh = growingMesh; SetTimerGrowing(); 
                if(_ownerID == EPlayerID.Player1)
                    //_myRenderer.material.color = new Color(0,150,150,1);
                    _myRenderer.materials = growing_b.GetComponent<Renderer>().sharedMaterials;
                if (_ownerID == EPlayerID.Player2)
                    //_myRenderer.material.color = new Color(150,0,150,1);
                    _myRenderer.materials = growing.GetComponent<Renderer>().sharedMaterials;
                break;
            case EState.Grown: _myMesh.sharedMesh = grownMesh; 
                if(_ownerID == EPlayerID.Player1)
                    //_myRenderer.material.color = new Color(0,255,255,1); 
                    _myRenderer.materials = grown_b.GetComponent<Renderer>().sharedMaterials;
                if (_ownerID == EPlayerID.Player2)
                    //_myRenderer.material.color = new Color(255,0,255,1);
                    _myRenderer.materials = grown.GetComponent<Renderer>().sharedMaterials;
                break;
            case EState.Impassable: _myMesh.sharedMesh = impassableMesh;
                _myCollider.center = new Vector3(0, 1, 0);
                this.gameObject.tag = "Wall";
                //_myRenderer.material.color = new Color(0,0,0,1); break;
                _myRenderer.materials = impassable.GetComponent<Renderer>().sharedMaterials;
                break;
            case EState.Overgrown: _myMesh.sharedMesh = overgrownMesh;
                //_myRenderer.material.color = new Color(0,0,255,1); break;
                _myRenderer.materials = empty.GetComponent<Renderer>().sharedMaterials;
                break;
            case EState.Burned: _myMesh.sharedMesh = burnedMesh; SetTimerBurned();
                //_myRenderer.material.color = new Color(255,0,0,1);
                _myRenderer.materials = burned.GetComponent<Renderer>().sharedMaterials;
                _ownerID = EPlayerID.None; break;
        }
    }
    
    public void ChangeTileState(EPlayerID playerID)
    {
        if (_ownerID == EPlayerID.None) // Tile has no owner.
        {
            switch (playerID)
            {
                case EPlayerID.Player1:
                case EPlayerID.Player2:
                    if (_state == EState.Overgrown)
                    {
                        SetState(EState.Empty);
                        SlingSeed(playerID);
                        SlingSeed(playerID);
                    }
                    else if(_state == EState.Empty)
                    {
                        _ownerID = playerID;
                        SetState(EState.Growing);                        
                    }
                    break;
            }
        }
        else if (playerID == _ownerID)   // Tile has owner. Owner of tile called.
        {
            if (_state == EState.Grown)
            {
                SlingSeed(playerID);
                SlingSeed(playerID);
                SetState(EState.Growing);
            }
        }
        else if (playerID is EPlayerID.Player1 or EPlayerID.Player2)  // Tile has owner. Enemy player called.
        {
            SetState(EState.Burned);
        }
        else if (playerID == EPlayerID.RobotDestroyer) // Tile has owner. Robot called.
        {
            SetState(EState.Burned);
        }
    }

    private void SlingSeed(EPlayerID playerID)
    {
        GameObject newSeed;
        if (playerID == EPlayerID.Player1) newSeed = Instantiate(seed1Prefab, this.transform.position + new Vector3(0,3,0), Quaternion.identity);
        else newSeed = Instantiate(seed2Prefab, this.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        Rigidbody newSeedRigidbody = newSeed.GetComponent<Rigidbody>();
        if (newSeedRigidbody != null)
        {
            int halfMaxRange = 50;
            Vector3 randomVector = new Vector3(Random.Range(-halfMaxRange * 2, halfMaxRange * 2), halfMaxRange, Random.Range(-halfMaxRange * 2, halfMaxRange * 2)).normalized;
            newSeedRigidbody.AddForce(randomVector * swingStrength);
        }
    }

    private void SetTimerGrowing()
    {
        timerGrow = defaultTimerGrow;
    }
    
    private void SetTimerBurned()
    {
        _timerBurn = defaultTimerBurn;
    }
}
