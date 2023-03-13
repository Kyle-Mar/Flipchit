using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    // This could maybe be a scriptable object? This is just used to hold data about a spawn position.
    public bool IsOccupied = false;
    public ulong occupyingClientId;
}
