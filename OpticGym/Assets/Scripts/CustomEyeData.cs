using System.Collections.Generic;

public static class CustomEyeData
{

    public enum EyePositionStates
    {
        Center,
        MiddleRight,
        MiddleLeft,
        MiddleTop,
        MiddleBottom,
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft
    }


    public static Dictionary<EyePositionStates, float[]> EyePositions =
        new Dictionary<EyePositionStates, float[]>()
    {
		// 0 -> lower x
		// 1 -> lower y
		// 2 -> upper x
		// 3 -> upper y
		{EyePositionStates.Center,new float[]{0.4f,0.4f,0.6f,0.6f}},
        {EyePositionStates.MiddleRight,new float[]{0.7f,0.4f,1.0f,0.6f}},
        {EyePositionStates.MiddleLeft,new float[]{-0.5f,0.4f,0.4f,0.6f}},
        {EyePositionStates.MiddleTop,new float[]{0.4f,0.8f,0.6f,1.2f}},
        {EyePositionStates.MiddleBottom,new float[]{0.4f,0.0f,0.6f,0.3f}},
        {EyePositionStates.TopRight,new float[]{0.7f,0.8f,1.1f,1.4f}},
        {EyePositionStates.TopLeft,new float[]{-0.2f,0.8f,0.4f,1.4f}},
        {EyePositionStates.BottomRight,new float[]{0.5f,0.0f,1.1f,0.3f}},
        {EyePositionStates.BottomLeft,new float[]{-2f,0.0f,0.4f,0.3f}},
    };

    public static Dictionary<EyePositionStates, int[]> EyeStateToPositions =
    new Dictionary<EyePositionStates, int[]>()
{

                {EyePositionStates.Center,new int[]{0,0}},
                {EyePositionStates.MiddleRight,new int[]{1,0}},
                {EyePositionStates.MiddleLeft,new int[]{-1,0}},
                {EyePositionStates.MiddleTop,new int[]{0,1}},
                {EyePositionStates.MiddleBottom,new int[]{0,-1}},
                {EyePositionStates.TopRight,new int[]{1,1}},
                {EyePositionStates.TopLeft,new int[]{-1,1}},
                {EyePositionStates.BottomRight,new int[]{1,-1}},
                {EyePositionStates.BottomLeft,new int[]{-1,-1}},
    };
}