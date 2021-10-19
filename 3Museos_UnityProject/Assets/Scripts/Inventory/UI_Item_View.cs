using Museos;
using Museos.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_Item_View : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    public Item StartItem;
    private Item _item = null;
    public Item CurrentItem
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            LoadItem(_item);
        }
    }

    private Vector3 _originalPosition = default;
    private Transform _originalParent = default;
    private Transform _canvasParent = default;
    private Image _raycastImage = default;
    private Image _childImage = default;
    private bool _hasInitialized = false;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (_hasInitialized)
            return;

        _hasInitialized = true;

        _originalParent = gameObject.transform.parent;
        _originalPosition = transform.position;
        _canvasParent = gameObject.GetComponentInParent<Canvas>().gameObject.transform;

        _raycastImage = gameObject.GetComponent<Image>();
        _childImage = transform.GetChild(0).GetComponent<Image>();
        _childImage.raycastTarget = false;

        if (StartItem != null)
            CurrentItem = StartItem;
    }

    public void LoadItem(Item item)
    {
        if(item != null)
        {
            _childImage.sprite = item.Icon;
            _childImage.gameObject.SetActive(true);

            GameSave.CurrentSave.SaveInventory.AddItem(item);
            _item = item;
        }
        else
        {
            _childImage.sprite = null;
            _childImage.gameObject.SetActive(false);
            GameSave.CurrentSave.SaveInventory.TakeItem(_item);
            _item = null;
        }
    }

    #region Interfaces
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item == null)
            return;

        _originalPosition = transform.position;
        eventData.selectedObject = this.gameObject;
        _raycastImage.raycastTarget = false;

        transform.SetParent(_canvasParent);
        transform.SetAsLastSibling();

        Museos.GameLoop.Instance.StateMachine.CurrentState?.SelectItem(_item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_item == null)
            return;

        transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.selectedObject == null)
            return;
        if (eventData.selectedObject.GetComponent<UI_Item_View>().CurrentItem == CurrentItem)
            return;

        var otherUIView = eventData.selectedObject.GetComponent<UI_Item_View>();
        var tempItem = otherUIView.CurrentItem;

        otherUIView.CurrentItem = CurrentItem;
        CurrentItem = tempItem;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_originalParent);
        transform.position = _originalPosition;
        eventData.selectedObject = null;
        _raycastImage.raycastTarget = true;

        Museos.GameLoop.Instance.StateMachine.CurrentState?.SelectItem(null);
    }
    #endregion

    /*
    #region Old Logic
    
    //Just uncomment to get it all back 
    //(also don't forget to remove IEndDrag and put IDrop back in)
    
    public Item StartItem;
    [HideInInspector]
    public Item _thisItem;
    private Image _thisVisuals;


    protected static UI_Item_View _currentDrag;
    private Vector3 _startDragPosition;

    private RectTransform _rTransform;
    private Transform _currentParent;

    private void Start()
    {
        _currentParent = transform.parent;
        _rTransform = GetComponent<RectTransform>();
        _startDragPosition = _rTransform.position;

        if (StartItem != null)
            LoadItem(StartItem);

        _thisVisuals = GetComponent<Image>();
    }

    public void LoadItem(Item item)
    {
        _thisItem = item;
        _thisVisuals.sprite = item.Icon;

        if (_thisItem == null)
        {
            _thisVisuals.gameObject.SetActive(false);
        }
        else
        {
            _thisVisuals.gameObject.SetActive(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_currentDrag != null)
        {
            _currentDrag.ResetPosition();
        }

        _currentDrag = this;
        _currentDrag._startDragPosition = _currentDrag._rTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentDrag == this)
        {
            transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform, true);
            _currentDrag._rTransform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(_currentDrag == this)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        _rTransform.position = _startDragPosition;
        transform.SetParent(_currentParent, true);
    }
    
    #endregion
    */
}
