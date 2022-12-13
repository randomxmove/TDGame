using UnityEngine;

public class Board : MonoBehaviour
{
    [System.Serializable]
    public struct AirPath
    {
        public Transform[] Pivots;
    }

    [SerializeField]
    private Transform[] _groundPathPivots = null;

    [SerializeField]
    private AirPath[] _airPathsPivots = null;

    [SerializeField]
    private Transform _playerCorePivot = null;

    private Vector3[] ConvertPivotsToPath(Transform[] pivots)
    {
        var result = new Vector3[pivots.Length];
        for (int i = 0; i < pivots.Length; i++)
        {
            result[i] = pivots[i].position;
        }
        return result;
    }

    public Vector3[] GetGroundPath()
    {
        return ConvertPivotsToPath(_groundPathPivots);
    }

    public Vector3[] GetAirPath()
    {
        var randomAirPathPivotsArray = _airPathsPivots[Random.Range(0, _airPathsPivots.Length)];
        return ConvertPivotsToPath(randomAirPathPivotsArray.Pivots);
    }

    public Vector3 GetPlayerCorePosition()
    {
        return _playerCorePivot.position;
    }
}
