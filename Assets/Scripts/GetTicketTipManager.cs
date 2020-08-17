using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTicketTipManager : MonoBehaviour
{
    public static GetTicketTipManager Instance;
    public Text TextTip;
    public GameObject GetTicketTip;
    public GameObject NotGetTicketTip;
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
        if (ticketName == "None")
        {
            NotGetTicketTip.SetActive(true);
        }
        else
        {
            GetTicketTip.SetActive(true);
            TextTip.text = ticketName;
        }
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
