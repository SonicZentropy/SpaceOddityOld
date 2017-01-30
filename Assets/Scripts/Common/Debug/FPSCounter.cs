// /** 
//  * FPSCounter.cs
//  * Will Hart
//  * 20161209
// */

namespace Zen.Common.Debug
{
    #region Dependencies

    using UnityEngine;

    #endregion

    public class FpsCounter : MonoBehaviour
    {
        private static readonly string[] StringsFrom00To99 =
        {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
        };

        private int[] _fpsBuffer;
        private int _fpsBufferIndex;

        public int FrameRange = 60;
        public UILabel InstFpsLabel;
        public UILabel AvgFpsLabel;
        public int AverageFps { get; private set; }
        public int InstFps { get; private set; }

        private void Awake()
        {
            if (!InstFpsLabel) InstFpsLabel = GameObject.Find("InstFPSLabel").GetComponent<UILabel>();
            if (!AvgFpsLabel) AvgFpsLabel = GameObject.Find("AvgFPSLabel").GetComponent<UILabel>();
        }

        private void Update()
        {
            if ((_fpsBuffer == null) || (_fpsBuffer.Length != FrameRange))
            {
                InitializeBuffer();
            }

            InstFps = (int) (1f/Time.unscaledDeltaTime);

            UpdateBuffer();
            CalculateFps();

            InstFpsLabel.text = StringsFrom00To99[Mathf.Clamp(InstFps, 0, 99)];
            AvgFpsLabel.text = StringsFrom00To99[Mathf.Clamp(AverageFps, 0, 99)];
        }

        private void CalculateFps()
        {
            var sum = 0;

            for (var i = 0; i < FrameRange; i++)
            {
                var fps = _fpsBuffer[i];
                sum += fps;
            }
            AverageFps = sum/FrameRange;
        }

        private void InitializeBuffer()
        {
            if (FrameRange <= 0)
            {
                FrameRange = 1;
            }
            _fpsBuffer = new int[FrameRange];
            _fpsBufferIndex = 0;
        }

        private void UpdateBuffer()
        {
            _fpsBuffer[_fpsBufferIndex++] = InstFps;
            if (_fpsBufferIndex >= FrameRange)
            {
                _fpsBufferIndex = 0;
            }
        }
    }
}