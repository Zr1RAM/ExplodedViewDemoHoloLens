using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class SubMesh
{
    public MeshRenderer meshRenderer;
    public Vector3 originalPosition;
    public Vector3 explodedPosition;
}

public class ExplodedViewController : MonoBehaviour
{
    [SerializeField]
    List<SubMesh> SubMeshes;
    [SerializeField]
    bool isExplodedView = true;
    [SerializeField]
    float explosionLerpSpeed = 0.1f;
    [SerializeField]
    float ExplodedPositionOffset = 1.5f;
    [SerializeField]
    bool isLerping = true;


    [SerializeField]
    Transform ObjToBeViewed;
    // Start is called before the first frame update
    void Initializations()
    {
        //ObjToBeViewed.SetParent(transform,true);
        //ObjToBeViewed.localPosition = new Vector3(0,ObjToBeViewed.localPosition.y,0);

        SubMeshes = new List<SubMesh>();
        foreach (var item in ObjToBeViewed.GetComponentsInChildren<MeshRenderer>())
        {
            SubMesh mesh = new SubMesh();
            mesh.meshRenderer = item;
            mesh.originalPosition = item.transform.position;
            mesh.explodedPosition = item.bounds.center * ExplodedPositionOffset;
            SubMeshes.Add(mesh);
        }

    }
    int i; //used for the for loops in ExplodedViewFunc
    void ExplodeViewFunc()
    {
        if(isLerping)
        {
            if(isExplodedView)
            {
                //Exploded view on 
                for(i = 0; i < SubMeshes.Count; i++)
                {
                    SubMeshes[i].meshRenderer.transform.position = Vector3.Lerp(SubMeshes[i].meshRenderer.transform.position,SubMeshes[i].explodedPosition,explosionLerpSpeed);
                    if(Vector3.Distance(SubMeshes[i].meshRenderer.transform.position, SubMeshes[i].explodedPosition) < 0.0001f)
                    {
                        isLerping = false;
                        SubMeshes[i].meshRenderer.transform.position = SubMeshes[i].explodedPosition;
                    }
                }
            }
            else
            {
                //Exploded view off
                for (i = 0; i < SubMeshes.Count; i++)
                {
                    SubMeshes[i].meshRenderer.transform.position = Vector3.Lerp(SubMeshes[i].meshRenderer.transform.position, SubMeshes[i].originalPosition, explosionLerpSpeed);
                    if (Vector3.Distance(SubMeshes[i].meshRenderer.transform.position, SubMeshes[i].originalPosition) < 0.0001f)
                    {
                        isLerping = false;
                        SubMeshes[i].meshRenderer.transform.position = SubMeshes[i].originalPosition;
                    }
                }
            }
        }
    }
    void Start()
    {
        Initializations();
    }

    // Update is called once per frame
    void Update()
    {
        ExplodeViewFunc();
    }
    public void ToggleExplodedView()
    {
#if UNITY_EDITOR  // ideally an "or development build" condition should be here
        if(ObjToBeViewed == null)
        {
            Debug.LogError("Tranform to toggle exploded view on is not set");
            return;
        }
#endif
        isExplodedView = !isExplodedView;
        isLerping = true;
    }
    public void ToggleExplodedView(bool val)
    {
        isExplodedView = val;
        isLerping = true;
    }
    public void TestingHoloLEnsButtonFunctionality()
    {
        print("is button pressed?");
    }
    Animator ExplodedViewAnimator;
    public void ToggleExplodedViewWithAnimation()
    {
        if(ExplodedViewAnimator==null)
        {
            ExplodedViewAnimator = ObjToBeViewed.GetComponent<Animator>();
        }
        isExplodedView = !isExplodedView;
        if(isExplodedView)
        {
            ExplodedViewAnimator.SetFloat("ToggleExplode", 1.0f);
        }
        else
        {
            ExplodedViewAnimator.SetFloat("ToggleExplode", - 1.0f);
        }
    }
}
