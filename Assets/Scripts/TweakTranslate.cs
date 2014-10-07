using UnityEngine;
using System.Collections;

public class TweakTranslate : MonoBehaviour {
	//保存魔方的九个面，每个面使用3*3的矩阵表示，保存每个obj的假名（依据位置的命名）
	string[,,] tweak = new string[9,3,3];
	//真名与假名的对应表，tweakName[i,0]为真名，其余为假名
	string[,] tweakName = new string[28,4];
	Vector2 mousePositionDown;
	Vector2 mousePositionUp;
	//旋转角度，每次只能旋转一个面
	int turnAngle;
	//是否处于旋转中
	bool isTurn;
	//是否反向
	bool isDown;
	//选中面,left , up or front
	int turnType;
	//选中面的某行或某列
	int turnNum;
	string hitName;
	bool isTouched;
	bool canMoved;
	int accelerate;
	void Awake()
	{
		GameObject btnLeft = GameObject.Find("uiRoot/Camera/leftHor");
		GameObject btnRight = GameObject.Find("uiRoot/Camera/rightHor");
		GameObject btnMiddle = GameObject.Find("uiRoot/Camera/middleVer");
		GameObject btnReturn = GameObject.Find("uiRoot/Camera/return");
		GameObject btnBegin = GameObject.Find("uiRoot/Camera/begin");
		GameObject btnChoose = GameObject.Find("uiRoot/Camera/choose");
		//设置这个按钮的监听，指向本类的ButtonClick方法中。
		UIEventListener.Get(btnLeft).onClick = ButtonClick;
		UIEventListener.Get(btnRight).onClick = ButtonClick;
		UIEventListener.Get(btnMiddle).onClick = ButtonClick;
		UIEventListener.Get(btnReturn).onClick = ButtonClick;
		UIEventListener.Get(btnBegin).onClick = ButtonClick;
		UIEventListener.Get(btnChoose).onClick = ButtonClick;
	}

	void Start () {

		turnAngle = 0;
		isTurn = false;
		isDown = false;
		isTouched = false;
		canMoved = false;
		hitName = "";
		turnType = 1;
		turnNum = 1;
		accelerate = 3;
		tweakName [0, 0] = "Cube_1_1";
		tweakName [0, 1] = "cubic_1_1";
		tweakName [0, 2] = "cubic_2_3";
		tweakName [0, 3] = "cubic_3_7";

		tweakName [1, 0] = "Cube_1_2";
		tweakName [1, 1] = "cubic_1_2";
		tweakName [1, 2] = "cubic_3_8";
		tweakName [1, 3] = "";
	
		tweakName [2, 0] = "Cube_1_3";
		tweakName [2, 1] = "cubic_1_3";
		tweakName [2, 2] = "cubic_3_9";
		tweakName [2, 3] = "cubic_4_1";

		tweakName [3, 0] = "Cube_2_1";
		tweakName [3, 1] = "cubic_1_4";
		tweakName [3, 2] = "cubic_2_6";
		tweakName [3, 3] = "";

		tweakName [4, 0] = "Cube_2_2";
		tweakName [4, 1] = "cubic_1_5";
		tweakName [4, 2] = "";
		tweakName [4, 3] = "";

		tweakName [5, 0] = "Cube_2_3";
		tweakName [5, 1] = "cubic_1_6";
		tweakName [5, 2] = "cubic_4_4";
		tweakName [5, 3] = "";

		tweakName [6, 0] = "Cube_3_1";
		tweakName [6, 1] = "cubic_1_7";
		tweakName [6, 2] = "cubic_2_9";
		tweakName [6, 3] = "cubic_5_1";

		tweakName [7, 0] = "Cube_3_2";
		tweakName [7, 1] = "cubic_1_8";
		tweakName [7, 2] = "cubic_5_2";
		tweakName [7, 3] = "";

		tweakName [8, 0] = "Cube_3_3";
		tweakName [8, 1] = "cubic_1_9";
		tweakName [8, 2] = "cubic_4_7";
		tweakName [8, 3] = "cubic_5_3";
		
		tweakName [9, 0] = "Cube_1_7";
		tweakName [9, 1] = "cubic_6_1";
		tweakName [9, 2] = "cubic_2_1";
		tweakName [9, 3] = "cubic_3_1";

		tweakName [10, 0] = "Cube_1_8";
		tweakName [10, 1] = "cubic_6_2";
		tweakName [10, 2] = "cubic_3_2";
		tweakName [10, 3] = "";

		tweakName [11, 0] = "Cube_1_9";
		tweakName [11, 1] = "cubic_6_3";
		tweakName [11, 2] = "cubic_3_3";
		tweakName [11, 3] = "cubic_4_3";

		tweakName [12, 0] = "Cube_2_7";
		tweakName [12, 1] = "cubic_6_4";
		tweakName [12, 2] = "cubic_2_4";
		tweakName [12, 3] = "";

		tweakName [13, 0] = "Cube_2_8";
		tweakName [13, 1] = "cubic_6_5";
		tweakName [13, 2] = "";
		tweakName [13, 3] = "";

		tweakName [14, 0] = "Cube_2_9";
		tweakName [14, 1] = "cubic_6_6";
		tweakName [14, 2] = "cubic_4_6";
		tweakName [14, 3] = "";

		tweakName [15, 0] = "";
		tweakName [15, 1] = "";
		tweakName [15, 2] = "";
		tweakName [15, 3] = "";

		tweakName [16, 0] = "Cube_3_7";
		tweakName [16, 1] = "cubic_6_7";
		tweakName [16, 2] = "cubic_2_7";
		tweakName [16, 3] = "cubic_5_7";
		
		tweakName [17, 0] = "Cube_3_8";
		tweakName [17, 1] = "cubic_6_8";
		tweakName [17, 2] = "cubic_5_8";
		tweakName [17, 3] = "";
		
		tweakName [18, 0] = "Cube_3_9";
		tweakName [18, 1] = "cubic_6_9";
		tweakName [18, 2] = "cubic_4_9";
		tweakName [18, 3] = "cubic_5_9";

		tweakName [19, 0] = "Cube_1_4";
		tweakName [19, 1] = "cubic_2_2";
		tweakName [19, 2] = "cubic_3_4";
		tweakName [19, 3] = "";

		tweakName [20, 0] = "Cube_3_4";
		tweakName [20, 1] = "cubic_2_8";
		tweakName [20, 2] = "cubic_5_4";
		tweakName [20, 3] = "";

		tweakName [21, 0] = "Cube_1_6";
		tweakName [21, 1] = "cubic_3_6";
		tweakName [21, 2] = "cubic_4_2";
		tweakName [21, 3] = "";

		tweakName [22, 0] = "Cube_3_6";
		tweakName [22, 1] = "cubic_4_8";
		tweakName [22, 2] = "cubic_5_6";
		tweakName [22, 3] = "";

		tweakName [23, 0] = "Cube_1_5";
		tweakName [23, 1] = "cubic_3_5";
		tweakName [23, 2] = "";
		tweakName [23, 3] = "";

		tweakName [24, 0] = "Cube_2_4";
		tweakName [24, 1] = "cubic_2_5";
		tweakName [24, 2] = "";
		tweakName [24, 3] = "";

		tweakName [25, 0] = "Cube_2_6";
		tweakName [25, 1] = "cubic_4_5";
		tweakName [25, 2] = "";
		tweakName [25, 3] = "";

		tweakName [26, 0] = "Cube_3_5";
		tweakName [26, 1] = "cubic_5_5";
		tweakName [26, 2] = "";
		tweakName [26, 3] = "";
		
		tweakName [27, 0] = "Cube_2_5";
		tweakName [27, 1] = "cubic_7_1";
		tweakName [27, 2] = "";
		tweakName [27, 3] = "";

		tweak [0,0,0] = "cubic_1_1";
		tweak [0,0,1] = "cubic_1_2";
		tweak [0,0,2] = "cubic_1_3";
		tweak [0,1,0] = "cubic_1_4";
		tweak [0,1,1] = "cubic_1_5";
		tweak [0,1,2] = "cubic_1_6";
		tweak [0,2,0] = "cubic_1_7";
		tweak [0,2,1] = "cubic_1_8";
		tweak [0,2,2] = "cubic_1_9";

		tweak [1,0,0] = "cubic_3_4";
		tweak [1,0,1] = "cubic_3_5";
		tweak [1,0,2] = "cubic_3_6";
		tweak [1,1,0] = "cubic_2_5";
		tweak [1,1,1] = "cubic_7_1";
		tweak [1,1,2] = "cubic_4_5";
		tweak [1,2,0] = "cubic_5_4";
		tweak [1,2,1] = "cubic_5_5";
		tweak [1,2,2] = "cubic_5_6";

		tweak [2,0,0] = "cubic_6_1";
		tweak [2,0,1] = "cubic_6_2";
		tweak [2,0,2] = "cubic_6_3";
		tweak [2,1,0] = "cubic_6_4";
		tweak [2,1,1] = "cubic_6_5";
		tweak [2,1,2] = "cubic_6_6";
		tweak [2,2,0] = "cubic_6_7";
		tweak [2,2,1] = "cubic_6_8";
		tweak [2,2,2] = "cubic_6_9";

		tweak [3,0,0] = "cubic_2_1";
		tweak [3,0,1] = "cubic_2_2";
		tweak [3,0,2] = "cubic_2_3";
		tweak [3,1,0] = "cubic_2_4";
		tweak [3,1,1] = "cubic_2_5";
		tweak [3,1,2] = "cubic_2_6";
		tweak [3,2,0] = "cubic_2_7";
		tweak [3,2,1] = "cubic_2_8";
		tweak [3,2,2] = "cubic_2_9";

		tweak [4,0,0] = "cubic_3_2";
		tweak [4,0,1] = "cubic_3_5";
		tweak [4,0,2] = "cubic_3_8";
		tweak [4,1,0] = "cubic_6_5";
		tweak [4,1,1] = "cubic_7_1";
		tweak [4,1,2] = "cubic_1_5";
		tweak [4,2,0] = "cubic_5_8";
		tweak [4,2,1] = "cubic_5_5";
		tweak [4,2,2] = "cubic_5_2";

		tweak [5,0,0] = "cubic_4_7";
		tweak [5,0,1] = "cubic_4_8";
		tweak [5,0,2] = "cubic_4_9";
		tweak [5,1,0] = "cubic_4_4";
		tweak [5,1,1] = "cubic_4_5";
		tweak [5,1,2] = "cubic_4_6";
		tweak [5,2,0] = "cubic_4_1";
		tweak [5,2,1] = "cubic_4_2";
		tweak [5,2,2] = "cubic_4_3";

		tweak [6,0,0] = "cubic_3_1";
		tweak [6,0,1] = "cubic_3_2";
		tweak [6,0,2] = "cubic_3_3";
		tweak [6,1,0] = "cubic_3_4";
		tweak [6,1,1] = "cubic_3_5";
		tweak [6,1,2] = "cubic_3_6";
		tweak [6,2,0] = "cubic_3_7";
		tweak [6,2,1] = "cubic_3_8";
		tweak [6,2,2] = "cubic_3_9";

		tweak [7,0,0] = "cubic_6_4";
		tweak [7,0,1] = "cubic_6_5";
		tweak [7,0,2] = "cubic_6_6";
		tweak [7,1,0] = "cubic_2_5";
		tweak [7,1,1] = "cubic_7_1";
		tweak [7,1,2] = "cubic_4_5";
		tweak [7,2,0] = "cubic_2_6";
		tweak [7,2,1] = "cubic_1_5";
		tweak [7,2,2] = "cubic_4_4";

		tweak [8,0,0] = "cubic_5_7";
		tweak [8,0,1] = "cubic_5_8";
		tweak [8,0,2] = "cubic_5_9";
		tweak [8,1,0] = "cubic_5_4";
		tweak [8,1,1] = "cubic_5_5";
		tweak [8,1,2] = "cubic_5_6";
		tweak [8,2,0] = "cubic_5_1";
		tweak [8,2,1] = "cubic_5_2";
		tweak [8,2,2] = "cubic_5_3";
	}

	//通过假名获取真名
	string getCubeByName( string str )
	{
		int i = 0, j = 0;
		for (i = 0; i < 28; i++ ) {
			for (j = 0 ; j < 4 ; j++ )
			{
				if ( tweakName[i,j] == str )
				{
					return tweakName[i,0];
				}
			}
		}
		return "";
	}

	//通过假名获取真名位置
	int getCubeByLayer( string str )
	{
		int i = 0, j = 0;
		for (i = 0; i < 28; i++ ) {
			for (j = 0 ; j < 4 ; j++ )
			{
				if ( tweakName[i,j] == str )
				{
					return i;
				}
			}
		}
		return -1;
	}

	//设置真名
	void setCubeByName( string str , int layer )
	{
		tweakName [layer, 0] = str;
	}

	///
	//旋转并刷新魔方
	//@type : 1: 水平面 ; 2 : 左切面; 3 : 右切面
	//@turn : 0: 第一行或第一列 , 1 , 2
	//@dir  : 是否反向
	bool turn( int type , int turn , bool dir )
	{
		int layer = 0;
		int angle = 1;
		Vector3 turnDir = Vector3.left;
		//type
		// 1 , 0 , 1 , 2 
		// 2 , 3 , 4 , 5
		// 3 , 6 , 7 , 8
		if (type == 1) {
			layer = turn - 1;
			if (dir)
			{
				turnDir = Vector3.up;
			}
			else
			{
				turnDir = Vector3.down;
			}
		} else if (type == 2) {
			layer = type + turn;
			if (dir)
			{
				turnDir = Vector3.forward;
			}
			else
			{
				turnDir = Vector3.back;
			}
		}
		else if (type == 3) {
			layer = type * 2 + turn - 1;
			if (dir)
			{
				turnDir = Vector3.left;
			}
			else
			{
				turnDir = Vector3.right;
			}
		}

		if (layer >= 0 && layer <= 8) {
			///旋转每个obj
			Transform center = transform.FindChild( getCubeByName( tweak[layer,1,1] ) );
			int i = 0, j = 0;
			for( i = 2 ; i >= 0 ; i-- )
			{
				for ( j = 0 ; j < 3 ; j++ )
				{
					Transform tt = transform.FindChild( getCubeByName( tweak[layer,i,j]));
					if (tt)
					{
//						Debug.Log ("tt is Exist ");
						tt.RotateAround( center.position , turnDir , angle );
					}
				}
			}
			turnAngle+= angle;
			if ( turnAngle >= 90 )
			{
				//刷新
				refresh( layer , dir );
				turnAngle = 0;
				return true;
			}
		}
		return false;
	}


	//刷新真名，假名不变
	void refresh( int layer , bool dir )
	{
		int i = 0, j = 0;
		string[,] temp = new string[3,3];
		string[,] tempObj = new string[3, 3];
		for(i = 2 ; i >= 0 ; i-- )
		{
			for ( j = 0 ; j < 3 ; j++ )
			{
				temp[i,j] = tweak[layer,i,j];
				tempObj[i,j] = getCubeByName( tweak[layer,i,j]);
			}
		}
		//deal after turn
		if ( !dir )
		{
			int new_i = 0;
			for( i = 2 ; i >= 0 ; i-- )
			{
				int new_j = 0;
				for ( j = 0 ; j < 3 ; j++ )
				{
					int tweakLayer = getCubeByLayer(tweak[layer,new_j,new_i]);
					if ( tweakLayer >= 0 )
					{
						setCubeByName(tempObj[i,j] , tweakLayer );
					}
					new_j = new_j + 1;
				}
				new_i = new_i + 1;
			}
		}
		else
		{
			int new_i = 0;
			for( i = 0 ; i < 3 ; i++ )
			{
				int new_j = 0;
				for ( j = 2 ; j >= 0 ; j-- )
				{
					int tweakLayer = getCubeByLayer(tweak[layer,new_j,new_i]);
					if ( tweakLayer >= 0 )
					{
						setCubeByName(tempObj[i,j] , tweakLayer );
					}
					new_j = new_j + 1;
				}
				new_i = new_i + 1;
			}
		}
	}

	//计算按钮的点击事件
	void ButtonClick(GameObject button)
	{
		if (!isTurn) 
		{
			switch (button.transform.name) {
			case "leftHor":
					turnType = 3;
					break;
			case "rightHor":
					turnType = 2;
					break;
			case "middleVer":
					turnType = 1;
					break;
			case "return":
					isDown = !isDown;
					break;
			case "begin":
					isTurn = true;
					break;
			case "choose":
					turnNum += 1;
					if (turnNum > 3)
							turnNum = 1;
					break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
//		print(Input.touchCount);	
//		if (Input.touchCount > 0) 
//		{
//			string name = "";
//			print (Input.touchCount);
//			if (Input.GetTouch (0).phase == TouchPhase.Began) 
//			{
//				RaycastHit hit;
//				// 从目前的触摸坐标，构建射线
//				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
//				
//				if (Physics.Raycast (ray, out hit, 100)) {
//					isTouched = true;
//					hitName = hit.transform.name;
//					mousePositionDown = Input.GetTouch (0).position;
//					print ( "getTouch down " + name + " " + mousePositionDown.ToString() );
//				}
//			}
//			if (Input.GetTouch (0).phase == TouchPhase.Moved) 
//			{
//				canMoved = true;
//				print ( "getTouch moved " );
//			}
//			if (Input.GetTouch (0).phase == TouchPhase.Ended && canMoved && isTouched ) 
//			{
//				print ( "getTouch Ended " );
//				mousePositionUp = Input.GetTouch (0).position;
//				float angle = 0;
//				Vector3 from = new Vector3( mousePositionUp.x , mousePositionUp.y , 0);
//				Vector3 to 	 = new Vector3( mousePositionDown.x , mousePositionDown.y , 0); 
//				if ( Vector3.Cross( from , to ).z > 0 )
//				{
//					print ("mouse angle is " + Vector2.Angle( mousePositionUp , mousePositionDown ).ToString() );
//					angle =  Vector2.Angle( mousePositionUp , mousePositionDown );
//				}
//				else
//				{
//					print ("mouse angle is " + (- Vector2.Angle( mousePositionUp , mousePositionDown )).ToString() );
//					angle = -Vector2.Angle( mousePositionUp , mousePositionDown );
//				}
//				print ( "getTouch Ended " + mousePositionUp.ToString() + "  " + mousePositionDown.ToString() );
//				print ( "getTouch Ended " + angle.ToString() + "  " + angle.ToString() );
//				turnByName( hitName , angle );
//				canMoved = false;
//				isTouched = false;
//				hitName = "";
//				mousePositionDown = mousePositionUp;
//			}
//		}
		if (isTurn) {
			if ( accelerate > 0 )
			{
				for( int i = 0 ; i < accelerate ; i++ )
				{
					if (turn ( turnType , turnNum , isDown ) )
					{
						isTurn = false;
						break;
					}
				}
			}
		}
	}
}
