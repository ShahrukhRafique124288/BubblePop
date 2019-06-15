using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseViewController {

	void Open (GameObject obj = null, object viewModel = null);
	void Close ();
}
	