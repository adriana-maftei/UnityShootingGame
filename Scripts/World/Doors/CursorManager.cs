using UnityEngine;

	public class CursorManager : MonoBehaviour {

		public static CursorManager instance;
	    private UnityEngine.UI.Image img;

		public Sprite defaultCursor;
		public Sprite lockedCursor;
		public Sprite doorCursor;

		void Awake () 
	    {
			instance = this;
			img = GetComponent<UnityEngine.UI.Image> ();						
		}

		public void SetCursorToLocked() => img.sprite = lockedCursor;

		public void SetCursorToDoor() => img.sprite = doorCursor;

		public void SetCursorToDefault() => img.sprite = defaultCursor; 
	}
