using UnityEngine;
using System.Collections;
using LSL;

namespace Assets.LSL4Unity.Scripts
{
	[HelpURL("https://github.com/xfleckx/LSL4Unity/wiki#using-a-marker-stream")]
	public class LSLIntMarkerStream : MonoBehaviour
	{
		private const string unique_source_id = "IntMarkerTestSource";

		public string lslStreamName = "Unity_<Paradigma_Name_here>";
		public string lslStreamType = "LSL_Marker_Strings";

		private liblsl.StreamInfo lslStreamInfo;
		private liblsl.StreamOutlet lslOutlet;
		private int lslChannelCount = 1;

		//Assuming that markers are never send in regular intervalls
		private double nominal_srate = liblsl.IRREGULAR_RATE;

		private const liblsl.channel_format_t lslChannelFormat = liblsl.channel_format_t.cf_int32;

		private int[] sample;

		void Awake()
		{
			sample = new int[lslChannelCount];

			lslStreamInfo = new liblsl.StreamInfo(
				lslStreamName,
				lslStreamType,
				lslChannelCount,
				nominal_srate,
				lslChannelFormat,
				unique_source_id);

			lslOutlet = new liblsl.StreamOutlet(lslStreamInfo);
		}

		public void Write(int marker)
		{
			sample[0] = marker;
			lslOutlet.push_sample(sample);
		}

		public void Write(int marker, double customTimeStamp)
		{
			sample[0] = marker;
			lslOutlet.push_sample(sample, customTimeStamp);
		}

		public void Write(int marker, float customTimeStamp)
		{
			sample[0] = marker;
			lslOutlet.push_sample(sample, customTimeStamp);
		}

		public void WriteBeforeFrameIsDisplayed(int marker)
		{
			StartCoroutine(WriteMarkerAfterImageIsRendered(marker));
		}

		IEnumerator WriteMarkerAfterImageIsRendered(int pendingMarker)
		{
			yield return new WaitForEndOfFrame();

			Write(pendingMarker);

			yield return null;
		}

	}
}