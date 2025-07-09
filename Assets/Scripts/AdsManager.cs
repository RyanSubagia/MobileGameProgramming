using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    private const string _androidGameId = "5895606";
    private const string _iOSGameId = "5895606";
    private const string _rewardedAdUnitId = "Rewarded_ContinueRun";
    private const bool _isTestMode = true;

    private string _gameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
        _gameId = _androidGameId;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _isTestMode, this);
        }
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(_rewardedAdUnitId, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Inisialisasi Unity Ads berhasil.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Inisialisasi Unity Ads GAGAL: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_rewardedAdUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Rewarded Ad selesai ditonton. Memberikan reward...");
            LivesManager.instance.RefillLivesFromAd();
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Gagal menampilkan iklan untuk AdUnit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log("Mulai menampilkan iklan: " + adUnitId);
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("Iklan di-klik: " + adUnitId);
    }
}