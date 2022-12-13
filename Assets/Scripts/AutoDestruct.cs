using System.Collections;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField]
    private float _duration = 3;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(gameObject);
    }    
}
