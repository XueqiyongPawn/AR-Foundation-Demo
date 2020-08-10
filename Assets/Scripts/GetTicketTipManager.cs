using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTicketTipManager : MonoBehaviour
{
    public static GetTicketTipManager Instance;
    public Text TextTip;
    public GameObject WithCameraPanel;
    private bool isWebRequestSccess = false;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }

    public void SetTicketName(string ticketName)
    {
        TextTip.text = "恭喜您获得\n" + ticketName + "\n优惠券！";
        isWebRequestSccess = true;
        this.gameObject.SetActive(true);
    }

    public void BgClick()
    {
        if (isWebRequestSccess)
        {
            this.gameObject.SetActive(false);
            WithCameraPanel.SetActive(true);

        }
    }


}
