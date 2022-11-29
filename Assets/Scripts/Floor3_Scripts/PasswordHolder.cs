using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordHolder : MonoBehaviour
{
    [SerializeField] private List<int> pieceNums;
    [SerializeField] private LaserDoor lockedDoor;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(unlockDoor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator unlockDoor()
    {
        yield return new WaitUntil(() => foundAllPieces());
        //lockedDoor.unlock();
        //Debug.Log("Unlocked Door");
        lockedDoor.setOpen(true);
    }

    private bool foundAllPieces()
    {
        foreach (int num in pieceNums)
        {
            if (!PasswordPiece.hasBeenFound(num))
            {
                return false;
            }
        }
        return true;
    }
}
