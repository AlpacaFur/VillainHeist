using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordPiece : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 5f;
    [SerializeField] private int pieceNum = -1;
    [SerializeField] private GameObject child;
    private static HashSet<int> piecesFound;

    // Start is called before the first frame update
    void Awake()
    {
        if (piecesFound == null)
        {
            piecesFound = new HashSet<int>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        spin();
    }

    private void spin()
    {
        child.transform.eulerAngles += new Vector3(0, spinSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            piecesFound.Add(pieceNum);
            Debug.Log("Found " + pieceNum + " star piece");
            Destroy(gameObject);
        }
    }

    public static bool hasBeenFound(int num)
    {
        return piecesFound.Contains(num);
    }
}
