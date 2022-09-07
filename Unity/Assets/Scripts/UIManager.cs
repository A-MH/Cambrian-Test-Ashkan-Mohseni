using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMPro.TMP_InputField fileLocationIF;

    public NetworkManager networkManager;

    public void OnLoadDataClicked()
    {
        StartCoroutine(networkManager.DownloadCubesInfo(fileLocationIF.text));
    }
}
