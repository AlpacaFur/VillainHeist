using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordHolder : MonoBehaviour
{
    [SerializeField] private List<int> pieceNums;
    [SerializeField] private LaserDoor lockedDoor;
    [SerializeField] private string hintText = "Forgot Password Hint: I hid the password all in the surrounding rooms... Find them";
    [SerializeField] private string solvedText = "Correct Password! \nYou've unlocked the door to the top-secret ultimate weapon";
    [SerializeField] private Text computerText;
    private bool solved;

    // Start is called before the first frame update
    void Start()
    {
        solved = false;
        StartCoroutine(unlockDoor());
    }

    // Update is called once per frame
    void Update()
    {
        updateUI();
    }

    private IEnumerator unlockDoor()
    {
        yield return new WaitUntil(() => foundAllPieces());
        //lockedDoor.unlock();
        Debug.Log("Unlocked Door");
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
        solved = true;
        return true;
    }

    private void updateUI()
    {
        if (solved)
        {
            computerText.text = "Password: " + makePasswordText() + "\n" + solvedText;
        }
        else
        {
            computerText.text = "Password: " + makePasswordText() + "\n" + hintText;
        }
    }

    private string makePasswordText()
    {
        string passwordText = "";
        for (int i = 0; i < pieceNums.Count; i++)
        {
            if (PasswordPiece.hasBeenFound(pieceNums[i]))
            {
                passwordText += "*";
            }
            else
            {
                passwordText += "_";
            }
        }
        return passwordText;
    }
}
