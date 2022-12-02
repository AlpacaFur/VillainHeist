using System.Collections;
using System.Collections.Generic;
using movement_and_Camera_Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInputs : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private bool running;
    [Header("Ability Start")]
    [SerializeField] [Tooltip("Probably the player transform")]
    private Transform abilitySelectionStartTransform;
    [SerializeField]
    private string buttonToActivate = "Fire1";
    [SerializeField]
    private string buttonToSelect = "Fire1";
    [Header("Abilities")]
    private Ability[] abilities;
    public enum AbilityType { Clickable, Immediate, Shootable };
    public enum AbilityTarget { Position, Object}
    [Header("Ability Selection")]
    private GameObject curSelectedObj;
    private GameObject hitLocation;
    //[SerializeField] private LayerMask abilityLayers;
    //[SerializeField] private float abilityRaycastDistance = 300f;
    [SerializeField]
    private GameObject activeCanvas;
    [SerializeField]
    private Transform activeCanvasContent;
    [SerializeField]
    private Button abilityButtonPreFab;
    private IEnumerator curAbilitySelRoutine;
    // This is the current ability selection routine
    private AbilityType curType = AbilityType.Clickable;
    private AbilityTarget curTarget = AbilityTarget.Position;
    private int abilityIndexUsed = 0;
    [Header("Clickable Selection")]
    private bool movingSelection;
    [SerializeField]
    private float selectionTimeScale = 0f;
    private Vector3 curSelectionLoc;
    [SerializeField]
    private float selectionMoveSpeed = 2f;
    [SerializeField]
    private float mouseSelectionMoveSpeed = 1f;
    [SerializeField]
    private float minMouseMove = .1f;
    [SerializeField]
    private GameObject selectionPrefab;
    [SerializeField]
    private float selectObjYOffest = 20f;
    private GameObject clickableSelectionObj;
    [Header("Immediate Selection")]
    [SerializeField]
    private GameObject immediateCanvas;
    private bool selectedImmediateOption;
    private bool useImmediate;
    [SerializeField]
    private string immediateCanvasQuestion = "Use [Ability]?";
    [SerializeField]
    private Text immediateCanvasText;
    [Header("Shootable Selection")]
    private bool selectedShootInput;
    private bool useShootable;
    
    //[SerializeField] private string selectableTag = "Selectable";
    [SerializeField]
    private GameObject shootableCanvas;
    [Header("Popups")]
    [SerializeField]
    private GameObject selectionMissedPopupObj;
    [SerializeField]
    private float popupShowTime;

    [Tooltip("Insert Battery Storage Object")]
    public Battery bat;
    public int cost = 1;

    // Start is called before the first frame update
    void Start()
    {
        selectedImmediateOption = false;
        running = false;
        movingSelection = false;
        ResetAbilityComponents();
        hitLocation = new GameObject("Hit Location");

    }

    // Update is called once per frame
    void Update()
    {
        CheckStart();
        ShootableUpdate();
    }

    private void CheckStart()
    {
        Debug.Log("Running ability system, running = " + running + ", button = " + Input.GetButtonDown(buttonToActivate) + ", bat has left = " + bat.hasLeft(this.cost));
        if (!running && Input.GetButtonDown(buttonToActivate) && bat.hasLeft(this.cost))
        {
            Debug.Log("Using ability");
            ResetAbilityComponents();
            AbilitySystemStart();
            bat.subFromCurrent(this.cost);
            activeCanvas.SetActive(true);
        }
        if (running)
        {
            SwitchAbilities();
        }
    }

    private void AbilitySystemStart()
    {
        AbilityType prevType = curType;
        curTarget = abilities[abilityIndexUsed].abilityTarget();
        if (!player.isFirstPov) // in third person
        {
            curType = abilities[abilityIndexUsed].abilityType3rdPerson();
        }
        else
        {
            curType = abilities[abilityIndexUsed].abilityType1stPerson();
        }
        if (!running || prevType != curType)
        {
            running = true;
            if (prevType == AbilityType.Clickable)
            {
                ClickableStop();
            }
            else if (prevType == AbilityType.Immediate)
            {
                ImmediateStop();
            }
            else if (prevType == AbilityType.Shootable)
            {
                ShootableStop();
            }

            if (curType == AbilityType.Clickable)
            {
                curAbilitySelRoutine = ClickableRoutine();
            }
            else if (curType == AbilityType.Immediate)
            {
                curAbilitySelRoutine = ImmediateRoutine();
            }
            else if (curType == AbilityType.Shootable)
            {
                curAbilitySelRoutine = ShootableRoutine();
            }
            StartCoroutine(curAbilitySelRoutine);
        }
    }


    private IEnumerator ShootableRoutine()
    {
        yield return new WaitUntil(() => Input.GetButtonUp(buttonToSelect));
        // This will prevent the user from accidentally immediatly selecting
        // a location.
        ShowShootableCanvas(true);
        useShootable = true;
        // By default, this will use the shootable, unless the stop method is
        // called to override this
        yield return new WaitUntil(() => Input.GetButton(buttonToSelect));
        ShowShootableCanvas(false);
        if (useShootable)
        {
            ApplyAbility();
            AbilitySystemEnd();
        }
    }

	public void refill() {
		this.bat.currentBat = this.bat.maxBat;
	}

    private void ApplyAbility()
    {
        if (curTarget == AbilityTarget.Object && !(abilities[abilityIndexUsed] is SelectableObjectAbility))
        {
            if (curSelectedObj != null)
            {
                abilities[abilityIndexUsed].ApplyTo(curSelectedObj);
            }
        }
        else if (abilities[abilityIndexUsed] is SelectableObjectAbility)
        {
            SelectableObjectAbility curSelectableAbility = (SelectableObjectAbility)abilities[abilityIndexUsed];
            if (curSelectedObj != null && curSelectedObj.CompareTag(curSelectableAbility.selectableTag()))
            {
                curSelectableAbility.ApplyTo(curSelectedObj);
            }
            else
            {
                StartCoroutine(SelectionMissedPopUp());
            }
        }
        else if (curTarget == AbilityTarget.Position)
        {
            abilities[abilityIndexUsed].ApplyTo(hitLocation);
        }
    }

    private IEnumerator SelectionMissedPopUp()
    {
        selectionMissedPopupObj.SetActive(true);
        yield return new WaitForSecondsRealtime(popupShowTime);
        selectionMissedPopupObj.SetActive(false);
    }

    private void ShootableUpdate()
    {
        if (running)
        {
            if (curType == AbilityType.Shootable)
            {
                var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);//Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (curTarget == AbilityTarget.Object || abilities[abilityIndexUsed] is SelectableObjectAbility)
                    {
                        var selection = hit.transform;
                        SelectObject(selection.gameObject);
                    }
                    else if (curTarget == AbilityTarget.Position)
                    {
                        hitLocation.transform.position = hit.point;
                    }
                }
            }
        }
    }

    private void SelectObject(GameObject selection)
    {
        DeselectPrevObject();
        curSelectedObj = selection;
        if (abilities[abilityIndexUsed] is SelectableObjectAbility)
        {
            SelectableObjectAbility curSelectableAbility = (SelectableObjectAbility)abilities[abilityIndexUsed];
            if (curSelectedObj.CompareTag(curSelectableAbility.selectableTag()))
            {
                curSelectedObj.BroadcastMessage("select");
            }
        }
    }

    private void DeselectPrevObject()
    {
        if (abilities[abilityIndexUsed] is SelectableObjectAbility && curSelectedObj != null)
        {
            SelectableObjectAbility curSelectableAbility = (SelectableObjectAbility)abilities[abilityIndexUsed];
            if (curSelectedObj.CompareTag(curSelectableAbility.selectableTag()))
            {
                curSelectedObj.BroadcastMessage("deselect");
            }
        }
        curSelectedObj = null;
    }

    private void ShowShootableCanvas(bool showIt)
    {
        shootableCanvas.SetActive(showIt);
    }

    private void ShootableStop()
    {
        DeselectPrevObject();
        selectedShootInput = true;
        useShootable = false;
    }

    private IEnumerator ImmediateRoutine()
    {
        ShowImmediateCanvas();
        yield return new WaitUntil(() => selectedImmediateOption);
        immediateCanvas.SetActive(false);
        if (useImmediate)
        {
            abilities[abilityIndexUsed].ApplyTo(null);
        }
        AbilitySystemEnd();
    }

    private void ImmediateStop()
    {
        selectedImmediateOption = true;
        useImmediate = false;
        //immediateCanvas.SetActive(false);
    }

    private void ShowImmediateCanvas()
    {
        selectedImmediateOption = false;
        immediateCanvas.SetActive(true);
        int abilityNameStart = immediateCanvasQuestion.IndexOf("[Ability]");
        int abilityNameEnd = immediateCanvasQuestion.IndexOf("[Ability]") + "[Ability]".Length;
        string curQuestion = immediateCanvasQuestion.Substring(0, abilityNameStart)
            + abilities[abilityIndexUsed].GetName()
            + immediateCanvasQuestion.Substring(abilityNameEnd);
        immediateCanvasText.text = curQuestion;
    }

    public void selectImmediateOption(bool yes)
    {
        useImmediate = yes;
        selectedImmediateOption = true;
    }

    private IEnumerator ClickableRoutine()
    {
        yield return new WaitUntil(() => Input.GetButtonUp(buttonToSelect));
        // This will prevent the user from accidentally immediatly selecting
        // a location.
        float oldTimeScale = Time.timeScale;
        Time.timeScale = selectionTimeScale;
        curSelectionLoc = abilitySelectionStartTransform.position;
        StartCoroutine(MoveSelection());
        yield return new WaitUntil(() => !movingSelection);
        Time.timeScale = oldTimeScale;
        /*AbilitySystemEnd();*/
    }

    private IEnumerator MoveSelection()
    {
        movingSelection = true;
        while (movingSelection)
        {
            Vector3 input;
            if (MouseHasMoved())
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitMouseLoc;
                Physics.Raycast(mouseRay, out hitMouseLoc);
                if (hitMouseLoc.collider != null)
                {
                    curSelectionLoc = hitMouseLoc.point;
                }
                //input = new Vector3(Input.GetAxisRaw("Mouse X"), 0, Input.GetAxisRaw("Mouse Y"));
                //input *= mouseSelectionMoveSpeed;
            }
            else
            {
                input = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                input *= selectionMoveSpeed;
                curSelectionLoc += input * Time.unscaledDeltaTime;
            }
            
            // This will find the gameobject associated with this location
            RaycastHit findObjHere;
            Physics.Raycast(new Vector3(curSelectionLoc.x, curSelectionLoc.y + selectObjYOffest, curSelectionLoc.z),
                Vector3.down, out findObjHere);
            if (findObjHere.collider != null)
            {
                SelectObject(findObjHere.collider.gameObject);
                //curSelectedObj = findObjHere.collider.gameObject;
            }
            ShowSelectionWithObj();
            if (Input.GetButton(buttonToSelect))
            {
                movingSelection = false;
            }

            yield return new WaitForEndOfFrame();
        }
        if (curType == AbilityType.Clickable)
        {
            RemoveSelectionObj();
            //GameObject applyLocation = new GameObject();
            //applyLocation.transform.position = curSelectionLoc;
            hitLocation.transform.position = curSelectionLoc;
            ApplyAbility();
            AbilitySystemEnd();
        }
    }

    private void ClickableStop()
    {
        RemoveSelectionObj();
        movingSelection = false;
    }

    private void ShowSelectionWithObj()
    {
        if (clickableSelectionObj == null)
        {
            clickableSelectionObj = Instantiate(selectionPrefab);
        }
        clickableSelectionObj.transform.position = curSelectionLoc;
    }

    private void RemoveSelectionObj()
    {
        Destroy(clickableSelectionObj);
    }

    private bool MouseHasMoved()
    {
        return new Vector3(Input.GetAxisRaw("Mouse X"), 0, Input.GetAxisRaw("Mouse Y")).magnitude > minMouseMove;
    }

    private void ResetAbilityComponents()
    {
        abilities = GetComponents<Ability>();
        abilityIndexUsed = 0;
        ResetActiveCanvasButtons();
    }

    private void ResetActiveCanvasButtons()
    {
        ClearActiveCanvasButtons();
        for (int i = 0; i < abilities.Length; i++)
        {
            GameObject abilityButtonObj = Instantiate(abilityButtonPreFab.gameObject, activeCanvasContent);
            Button abilityButton = abilityButtonObj.GetComponent<Button>();
            int abilityNum = i;
            abilityButton.onClick.AddListener(() => { SwitchAbilityTo(abilityNum); });
            // Adds the function to this button to change the current ability to
            // this button's ability
            abilityButtonObj.transform.GetChild(0).GetComponent<Image>().sprite = abilities[i].GetIcon();
            abilityButtonObj.GetComponentInChildren<Text>().text = "" + (abilityNum + 1);
            // This will work as long as the only child object of the ability button is its sprite
        }
    }

    private void ClearActiveCanvasButtons()
    {
        for (int index = 0; index < activeCanvasContent.childCount; index++)
        {
            Destroy(activeCanvasContent.GetChild(index).gameObject);
        }
    }

    private void SwitchAbilities()
    {
        string input = Input.inputString;
        for (int i = 0; i < abilities.Length; i++)
        {
            if (input != null && input.Equals("" + (i + 1)))
            {
                SwitchAbilityTo(i);
            }
        }
    }

    private void SwitchAbilityTo(int i)
    {
        if (i < 0 || i >= abilities.Length)
        {
            throw new AbilityIndexOutOfBoundsException();
        }
        abilityIndexUsed = i;
        AbilitySystemStart();
    }

    private void AbilitySystemEnd()
    {
        running = false;
        activeCanvas.SetActive(false);
        DeselectPrevObject();
    }
}

// Thrown when you try changing the ability index to an index that does not exist
class AbilityIndexOutOfBoundsException : System.Exception {

}

public interface Ability
{
    /**
     * Where this Ability is to be used.
     */
    public void ApplyTo(GameObject spot);

    public AbilityInputs.AbilityType abilityType3rdPerson();

    public AbilityInputs.AbilityType abilityType1stPerson();

    public AbilityInputs.AbilityTarget abilityTarget();

    public string GetName();

    public Sprite GetIcon();
}

public abstract class SelectableObjectAbility : MonoBehaviour, Ability
{
    public AbilityInputs.AbilityTarget abilityTarget()
    {
        return AbilityInputs.AbilityTarget.Object;
    }

    public abstract string selectableTag();

    public abstract AbilityInputs.AbilityType abilityType1stPerson();

    public abstract AbilityInputs.AbilityType abilityType3rdPerson();

    public abstract void ApplyTo(GameObject spot);

    public abstract Sprite GetIcon();

    public abstract string GetName();
}