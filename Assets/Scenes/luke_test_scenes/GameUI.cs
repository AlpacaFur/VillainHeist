using System;
using System.Collections;
using System.Linq;
using movement_and_Camera_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameUI : MonoBehaviour
{
    //private static GameUI Instance;
    private UIDocument _uiDocument;
    [SerializeField]
    private bool showUI = true;

    private bool _pauseMenuVisible = false;

    private PlayerController _player;
    
    public enum Modals
    {
        Welcome,
        Controls,
        TimesUp,
        Victory
    }
    
    public enum AbilityPrompts
    {
        Freeze,
        Light,
        Pickup,
        MoveWall
    }

    private Modals[] ALL_MODALS = {
        Modals.Controls, Modals.Welcome, Modals.Victory, Modals.TimesUp
    };
    
    private AbilityPrompts[] ALL_ABILITY_PROMPTS = {
        AbilityPrompts.Freeze,
        AbilityPrompts.Light,
        AbilityPrompts.Pickup,
        AbilityPrompts.MoveWall
    };
    
    // Start is called before the first frame update
    void Start()
    {
        /*if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/

        _uiDocument = GetComponent<UIDocument>();
        HideAllModals();
        HideAllAbilityPrompts();
        SetBattery(6, 6);
        if (_player != null)
        {
            SetBattery(_player.currentBattery, _player.maxBattery);
        }

        if (!showUI) HideBattery();
        
        // ShowAbilityPrompts(AbilityPrompts.Freeze);

        _uiDocument.rootVisualElement.Q<Button>("Respawn").clicked += () =>
        {
            print("Respawn");
            if (_player != null)
            {
                _player.Respawn();
            }
        };
        _uiDocument.rootVisualElement.Q<Button>("MainMenu").clicked += () =>
        {
            print("MainMenu");
            SceneManager.LoadScene("MainMenu");
        };
        _uiDocument.rootVisualElement.Q<Button>("Close").clicked += () =>
        {
            print("Hide");
            HidePauseMenu();
        };
        
    }

    public void RegisterPlayer(PlayerController player)
    {
        _player = player;
    }
    
    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     // _uiDocument.rootVisualElement.Q<VisualElement>("TimerContainer").ToggleInClassList("hidden");
        //     // _uiDocument.rootVisualElement.Q<VisualElement>("TimerContainer").ToggleInClassList("show");
        //     int modal = (int)Math.Round(Random.value * 3);
        //     ShowAbilityPrompts((AbilityPrompts) modal);
        // }
        //
        // SetTimer((int)Time.realtimeSinceStartup);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenuVisible)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }

    public void ShowVictoryPopup(Action callback)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        _uiDocument.rootVisualElement.Q<VisualElement>("VictoryBG").RemoveFromClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("VictoryBG").AddToClassList("show");
        _uiDocument.rootVisualElement.Q<Button>("Continue").clicked += () =>
        {
            print("CLICKED!");
            HideVictoryPopup();
            callback.Invoke();
        };
    }
    
    public void HideVictoryPopup()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _uiDocument.rootVisualElement.Q<VisualElement>("VictoryBG").AddToClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("VictoryBG").RemoveFromClassList("show");
    }
    
    public void ShowPauseMenu()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        _uiDocument.rootVisualElement.Q<VisualElement>("PauseBG").RemoveFromClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("PauseBG").AddToClassList("show");
        _pauseMenuVisible = true;
    }
    
    public void HidePauseMenu()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _uiDocument.rootVisualElement.Q<VisualElement>("PauseBG").AddToClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("PauseBG").RemoveFromClassList("show");
        _pauseMenuVisible = false;
    }
    
    public void HideBattery()
    {
        _uiDocument.rootVisualElement.Q<VisualElement>("BatteryFace").AddToClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("BatteryBGCont").AddToClassList("hidden");
    }
    
    public void ShowBattery()
    {
        _uiDocument.rootVisualElement.Q<VisualElement>("BatteryFace").RemoveFromClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("BatteryBGCont").RemoveFromClassList("hidden");
    }
    
    public void HideTimer()
    {
        _uiDocument.rootVisualElement.Q<VisualElement>("TimerContainer").AddToClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("TimerContainer").RemoveFromClassList("show");
    }

    private string FormatTime(int timeInSeconds)
    {
        return $"{timeInSeconds / 60}:{(timeInSeconds % 60).ToString().PadLeft(2, '0')}";
    }
    
    public void SetTimer(int timeInSeconds)
    {
        _uiDocument.rootVisualElement.Q<Label>("Timer").text = FormatTime(timeInSeconds);
        _uiDocument.rootVisualElement.Q<VisualElement>("TimerContainer").RemoveFromClassList("hidden");
        _uiDocument.rootVisualElement.Q<VisualElement>("TimerContainer").AddToClassList("show");
    }
    
    private string ModalToId(Modals modal)
    {
        return modal switch
        {
            Modals.Welcome => "Welcome",
            Modals.Controls => "Controls",
            Modals.TimesUp => "TimesUp",
            Modals.Victory => "Escaped",
            _ => throw new NotImplementedException("Invalid Enum Value!")
        };
    }
    
    private string AbilityToId(AbilityPrompts modal)
    {
        return modal switch
        {
            AbilityPrompts.Freeze => "Freeze",
            AbilityPrompts.Light => "Light",
            AbilityPrompts.Pickup => "Pickup",
            AbilityPrompts.MoveWall => "MoveWall",
            _ => throw new NotImplementedException("Invalid Enum Value!")
        };
    }

    public void HideAllAbilityPrompts()
    {
        foreach (var abilityPrompt in ALL_ABILITY_PROMPTS)
        {
            _uiDocument.rootVisualElement.Q<VisualElement>(AbilityToId(abilityPrompt)).AddToClassList("hidden");
            _uiDocument.rootVisualElement.Q<VisualElement>(AbilityToId(abilityPrompt)).RemoveFromClassList("show");
        }
    }
    
    public void ShowAbilityPrompts(AbilityPrompts targetPrompt)
    {
        foreach (var abilityPrompt in ALL_ABILITY_PROMPTS)
        {
            if (targetPrompt == abilityPrompt)
            {
                _uiDocument.rootVisualElement.Q<VisualElement>(AbilityToId(abilityPrompt)).RemoveFromClassList("hidden");
                _uiDocument.rootVisualElement.Q<VisualElement>(AbilityToId(abilityPrompt)).AddToClassList("show");
            }
            else
            {
                _uiDocument.rootVisualElement.Q<VisualElement>(AbilityToId(abilityPrompt)).AddToClassList("hidden");
                _uiDocument.rootVisualElement.Q<VisualElement>(AbilityToId(abilityPrompt)).RemoveFromClassList("show");
            }
        }
    }
    
    public void HideAllModals()
    {
        foreach (var modal in ALL_MODALS)
        {
            _uiDocument.rootVisualElement.Q<VisualElement>(ModalToId(modal)).AddToClassList("hidden");
        }
    }

    public void ShowCutsceneText()
    {
        _uiDocument.rootVisualElement.Q<VisualElement>("CutsceneText").RemoveFromClassList("hidden");
    }
    
    public void HideCutsceneText()
    {
        _uiDocument.rootVisualElement.Q<VisualElement>("CutsceneText").AddToClassList("hidden");
    }

    private void ShowForSeconds(string id, float time)
    {
        _uiDocument.rootVisualElement.Q<VisualElement>(id).RemoveFromClassList("hidden");
        StartCoroutine(HideAfterDelay(id, time));
    }
    
    public void ShowCheckpoint()
    {
        ShowForSeconds("Checkpoint", 2);
    }

    private IEnumerator HideAfterDelay(string id, float delay)
    {
        yield return new WaitForSeconds(delay);
        _uiDocument.rootVisualElement.Q<VisualElement>(id).AddToClassList("hidden");
    }
    
    public void HideModal(Modals modal)
    {
        _uiDocument.rootVisualElement.Q<VisualElement>(ModalToId(modal)).RemoveFromClassList("hidden");
    }
        
    public void ShowModal(Modals targetModal)
    {
        foreach (var modal in ALL_MODALS)
        {
            if (modal == targetModal)
            {
                _uiDocument.rootVisualElement.Q<VisualElement>(ModalToId(modal)).RemoveFromClassList("hidden");
                _uiDocument.rootVisualElement.Q<VisualElement>(ModalToId(modal)).AddToClassList("show");
            }
            else
            {
                _uiDocument.rootVisualElement.Q<VisualElement>(ModalToId(modal)).AddToClassList("hidden");
                _uiDocument.rootVisualElement.Q<VisualElement>(ModalToId(modal)).RemoveFromClassList("show");
            }
        }
    }
    
    public void SetBattery(int currentBattery, int totalBattery)
    {
        VisualElement batteryContainer = _uiDocument.rootVisualElement.Q<VisualElement>("BGStart");

        while (batteryContainer.childCount > 0)
        {
            VisualElement batteryIcon = batteryContainer.Children().First();
            batteryIcon.parent.Remove(batteryIcon);
        }

        for (int index = 1; index <= totalBattery; index += 1)
        {
            VisualElement elem = new VisualElement();
            if (index == 1) elem.AddToClassList("battery-start");
            else elem.AddToClassList("battery-next");

            if (index > currentBattery) elem.AddToClassList("off");

            if (index == totalBattery)
            {
                elem.AddToClassList("last");
            }
            batteryContainer.Add(elem);
        }
    }
}
