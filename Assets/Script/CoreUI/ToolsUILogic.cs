using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Windows.Forms;

public class ToolsUILogic : MonoBehaviour {

    public UnityEngine.UI.Button btn;
	// Use this for initialization
	void Start () {

        btn.onClick.AddListener(()=> {
            //Win32MessageBox.MessageBox(System.IntPtr.Zero, "this is message", "title", 0);
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "==============";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                Debug.Log(folder.SelectedPath);
            }
        });

    }
	
}
