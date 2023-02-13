using Tank3DMultiplayer.Support;
using System.Collections;
using UnityEngine.Networking;
using Tank3DMultiplayer.Data;
using UnityEngine;
using UnityEngine.UI;


namespace Tank3DMultiplayer.SceneManage.Bootstrap
{
    public class LoadDataFromServer : Singleton<LoadDataFromServer>
    {
        [SerializeField]
        private Slider progressLoadingSlider;

        private bool isCompleteLoaded = false;

        private float progressLoading = 0f;
        private float maxValueLoading = 100f;

        private float checkPoint = 30;

        public bool IsCompleteLoaded { get => isCompleteLoaded;}

        private void Start()
        {
            StartCoroutine(TryGetTanksData());
        }

        

        private IEnumerator TryGetTanksData()
        {
            UnityWebRequest req = UnityWebRequest.Get(ConstValue.LOAD_DATA_TANKS);
            var handler = req.SendWebRequest();

            float startTime = 0.0f;
            while (!handler.isDone || startTime < 3f)
            {
                startTime += Time.deltaTime;
                if (progressLoading < checkPoint)
                {
                    progressLoading += Time.deltaTime * (maxValueLoading / 3.0f);
                    progressLoading = Mathf.Clamp(progressLoading, 0f, checkPoint);
                }
                else if (progressLoading == checkPoint)
                {
                    checkPoint = (maxValueLoading + checkPoint) * 2 / 3;
                    checkPoint = Mathf.Clamp(checkPoint, checkPoint, maxValueLoading);
                }

                progressLoadingSlider.value = progressLoading;

                if (startTime > 8.0f)
                {
                    break;
                }

                yield return null;
            }

            if(req.result == UnityWebRequest.Result.Success)
            {
                checkPoint = maxValueLoading;
                progressLoading = maxValueLoading;
                progressLoadingSlider.value = progressLoading;

                isCompleteLoaded = DataTanks.Instance.ParseData(req.downloadHandler.text);
            }
            else
            {
                Debug.Log("Khong the ket noi den server...");
            }

            yield return null;
        }

        
    }

}
