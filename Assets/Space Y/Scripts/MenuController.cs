using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    private const float CLOSED_POS = -400.0f;

    [SerializeField]
    private RectTransform satMenu;
    [SerializeField]
    private RectTransform contractMenu;
    [SerializeField]
    private RectTransform stationMenu;
    [SerializeField]
    private ScrollRect scrollMenu;
    [SerializeField]
    private Button satButton;
    [SerializeField]
    private Button contractButton;
    [SerializeField]
    private Button stationButton;

    private Animator anim;

    private bool menuIsOpen;
    private RectTransform activeMenu;

    public RectTransform SatMenu { get => satMenu; set => satMenu = value; }
    public RectTransform ContractMenu { get => contractMenu; set => contractMenu = value; }
    public RectTransform StationMenu { get => stationMenu; set => stationMenu = value; }
    public ScrollRect ScrollMenu { get => scrollMenu; set => scrollMenu = value; }
    public Animator Anim { get => anim; set => anim = value; }
    

    private void Awake() {
        
        if (!satMenu) {
            Debug.LogError("Satellite menu is null");
        }

        if (!contractMenu) {
            Debug.LogError("Contract menu is null");
        }

        if (!stationMenu) {
            Debug.LogError("Station menu is null");
        }

        if (!scrollMenu) {
            Debug.LogError("Scroll view is null");
        }

        anim = GetComponent<Animator>();

        if (!anim) {
            Debug.LogError("Animation is null");
        }

        menuIsOpen = false;
        activeMenu = null;
    }

    public void ToggleButtons(int _state) {
        if (_state == 0) {
            satButton.interactable = false;
            contractButton.interactable = false;
            stationButton.interactable = false;
        } else {
            satButton.interactable = true;
            contractButton.interactable = true;
            stationButton.interactable = true;
        }
    }

    public void ActivateMenu(RectTransform _menu) {
        satMenu.gameObject.SetActive(false);
        contractMenu.gameObject.SetActive(false);
        stationMenu.gameObject.SetActive(false);

        if (_menu == activeMenu) {
            activeMenu = null;
            anim.SetBool("IsOpen", false);
            return;
        }

        
        if (activeMenu == null) {
            activeMenu = _menu;
            anim.SetBool("IsOpen", true);
        }

        activeMenu = _menu;
        _menu.gameObject.SetActive(true);
        scrollMenu.content = _menu;
    }

}
