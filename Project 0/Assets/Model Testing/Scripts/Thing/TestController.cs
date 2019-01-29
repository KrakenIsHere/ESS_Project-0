using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class TestController : MonoBehaviour {

    Process process = new Process();

	// Use this for initialization
	void Start ()
    {
        process.StartInfo.FileName = @"Assets\Model Testing\Scripts\MasterH-8000.exe";
        process.Start();
    }
	
}
