using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSmoother : MonoBehaviour 
{
	public static Vector3[] SmoothLine( List<Vector3> inputPoints, float segmentSize )
	{
		//create curves
		AnimationCurve curveX = new AnimationCurve();
		AnimationCurve curveY = new AnimationCurve();
		AnimationCurve curveZ = new AnimationCurve();

		//create keyframe sets
		Keyframe[] keysX = new Keyframe[inputPoints.Count];
		Keyframe[] keysY = new Keyframe[inputPoints.Count];
		Keyframe[] keysZ = new Keyframe[inputPoints.Count];

		//set keyframes
		for( int i = 0; i < inputPoints.Count; i++ )
		{
			keysX[i] = new Keyframe( i, inputPoints[i].x );
			keysY[i] = new Keyframe( i, inputPoints[i].y );
			keysZ[i] = new Keyframe( i, inputPoints[i].z );
		}

		//apply keyframes to curves
		curveX.keys = keysX;
		curveY.keys = keysY;
		curveZ.keys = keysZ;

		//smooth curve tangents
		for( int i = 0; i < inputPoints.Count; i++ )
		{
			curveX.SmoothTangents( i, 0 );
			curveY.SmoothTangents( i, 0 );
			curveZ.SmoothTangents( i, 0 );
		}

		//list to write smoothed values to
		List<Vector3> lineSegments = new List<Vector3>();

		//find segments in each section
		for( int i = 0; i < inputPoints.Count; i++ )
		{
			//add first point
			lineSegments.Add( inputPoints[i] );

			//make sure within range of array
			if( i+1 < inputPoints.Count )
			{
				//find distance to next point
				float distanceToNext = Vector3.Distance(inputPoints[i], inputPoints[i+1]);

				//number of segments
				int segments = (int)(distanceToNext / segmentSize);

				//add segments
				for( int s = 1; s < segments; s++ )
				{
					//interpolated time on curve
					float time = ((float)s/(float)segments) + (float)i;

					//sample curves to find smoothed position
					Vector3 newSegment = new Vector3( curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time) );

					//add to list
					lineSegments.Add( newSegment );
				}
			}
		}

		return lineSegments.ToArray();
	}

}
