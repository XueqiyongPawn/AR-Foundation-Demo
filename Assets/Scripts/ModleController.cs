using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModleController : MonoBehaviour
{
    private Animation mAnimation;

    private bool mIsHitModel = false;
    private AniState mCurAniState = AniState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        mAnimation = this.GetComponent<Animation>();
        SetAniState(AniState.Idle);
    }

    private void SetAniState(AniState state)
    {
        mCurAniState = state;
        string aniName = "";
        switch (state)
        {
            case AniState.Idle:
                aniName = "Stay";
                break;
            case AniState.GetTicket:
                aniName = "Give";
                break;
            case AniState.NotGetTicket:
                aniName = "Take 001";
                break;
            default:
                break;
        }
        
        mAnimation.Play(aniName);
    }


    private void Update()
    {
        if (mIsHitModel && mCurAniState != AniState.Idle)
        {
            if (!mAnimation.isPlaying)
            {
                Debug.Log("animation stop");

                if (mCurAniState == AniState.GetTicket)
                {
                    GetTicketTipManager.Instance.SetTicketName("XXXXXXXXX");
                }
                else if (mCurAniState == AniState.NotGetTicket)
                {
                    GetTicketTipManager.Instance.SetTicketName("None");
                }
                SetAniState(AniState.Idle);

            }

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetAniState(AniState.GetTicket);
            mIsHitModel = true;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetAniState(AniState.NotGetTicket);
            mIsHitModel = true;
        }

        if (mIsHitModel || Input.touchCount == 0)
            return;
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<ModleController>())
                {
                    GetCoupon();
                    mIsHitModel = true;
                    PlaneManager.Instance.SetClickModelState(false);
                }
            }
        }
    }

    private void GetCoupon()
    {
        int r = Random.Range(0, 10);
        //30%抽不中，70%抽中
        if (r >= 0 && r <= 2)
        {//0,1,2
            SetAniState(AniState.NotGetTicket);
        }
        else
        {
            SetAniState(AniState.GetTicket);
        }
    }

    public enum AniState
    {
        Idle = 0,
        GetTicket,
        NotGetTicket
    }
}
