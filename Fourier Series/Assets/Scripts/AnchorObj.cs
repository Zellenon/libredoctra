using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnchorObj : MonoBehaviour, IDragHandler
{

    [SerializeField] private CustomFuncSelector _customFunc;

    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {       
    }



    public void OnDrag(PointerEventData eventdata)
    {
        // Debug.Log("On Drag handler");

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(eventdata.position);
        transform.position = mousePos;

        _customFunc.UpdateAnchorPoints();
        // UnityEngine.Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }



}
