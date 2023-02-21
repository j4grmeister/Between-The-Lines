using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Might move this to ClickableArea to reduce the number of GetComponent() calls
//       Definitely need to do this if we start to handle mouse hovers and highlighting areas; can't be doing this every single frame
public class ClickListener : Singleton<ClickListener>
{

    private ClickableArea clickedArea;

    void Update()
    {
        // Check for clicks on any ClickableAreas
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);

            if(hit.collider != null)
            {
                // Save clicked area for later. A click only counts if both MouseDown and MouseUp occur on the same area
                hit.transform.TryGetComponent<ClickableArea>(out clickedArea);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);

            if(clickedArea != null && hit.collider != null && hit.transform.GetComponent<ClickableArea>() == clickedArea)
            {
                // The area was clicked
                clickedArea.Invoke();
            }
        }
    }
}