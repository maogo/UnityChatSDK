﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UnityChatSDK参数设置
/// </summary>
public class UnityChatSet: MonoBehaviour {

    //声音音量衰减参数
    public  float AudioVolume= 1f;
    //声音采集阈值
    public  float AudioThreshold= 0.01f;
    public VideoType VideoType = VideoType.DeviceCamera;
    //音视频分辨率
    public VideoResolution VideoResolution = VideoResolution._360P; 
    //音视频压缩质量
    public VideoQuality VideoQuality = VideoQuality.Middle;
    //视频刷新率
    [Range(5,20)]
    public int Framerate = 15;

    public bool EchoCancellation;

    [Tooltip("check video frame static")]
    public bool EnableDetection; //检测通话视频是否静帧
    /// <summary>
    /// 当选择采集Unity Camera的画面时，选择要采集的unity的摄像机
    /// </summary>
    public Camera CaptureCamera;
    /// <summary>
    /// 聊天对象的视频画面显示
    /// </summary>
    public RawImage ChatPeerRawImage;
    /// <summary>
    /// 本地采集画面的回显
    /// </summary>
    public RawImage SelfRawImage;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => UnityChatSDK.Instance != null);
        InitMic();
        InitVideo(); 
    }
    //初始化音频
    void InitMic() 
    {
        UnityChatSDK.Instance.AudioVolume=AudioVolume;
        UnityChatSDK.Instance.AudioThreshold=AudioThreshold;
        UnityChatSDK.Instance.AudioFrequency = 8000;
        UnityChatSDK.Instance.AudioSample = 2;
        UnityChatSDK.Instance.AudioLatency = 910;
        UnityChatSDK.Instance.EchoCancellation = EchoCancellation;
        //UnityChatSDK.Instance.EchoThreshold = EchoThreshold;
        //初始化音频
        UnityChatSDK.Instance.InitMic();
        print("初始化音频OK");
    }

    //初始化视频
    void InitVideo() 
    {
        UnityChatSDK.Instance.VideoRes = VideoResolution;
        UnityChatSDK.Instance.VideoQuality = VideoQuality;
        UnityChatSDK.Instance.Framerate = Framerate;
        UnityChatSDK.Instance.EnableDetection = EnableDetection;
    
        //初始化视频
        UnityChatSDK.Instance.InitVideo();

        switch (VideoType)
        {
            case VideoType.DeviceCamera:
                SetVideoCaptureType(VideoType.DeviceCamera, null);
                break;
            case VideoType.UnityCamera:
                SetVideoCaptureType(VideoType.UnityCamera, CaptureCamera);
                break;
            case VideoType.CustomMode:
                SetVideoCaptureType(VideoType.CustomMode, null);
                break;
            default:
                break;
        }
        UnityChatSDK.Instance.ChatPeerRawImage = ChatPeerRawImage;
        UnityChatSDK.Instance.SelfRawImage = SelfRawImage;

        print("streamSDK init OK!" + "--videoRes:" + UnityChatSDK.Instance.VideoRes + "--quality:" + UnityChatSDK.Instance.VideoQuality
            + "--Framerate:" + UnityChatSDK.Instance.Framerate);
    }
    /// <summary>
    /// 选择要采集的视频类型（注：未注册不支持Unity Camera）
    /// </summary>
    /// <param name="type">  DeviceCamera是外表摄像头的画面 UnityCamera是Unity Camera渲染的画面</param>
    /// <param name="captureCamera"></param>
    public void SetVideoCaptureType(VideoType type, Camera captureCamera)
    {
        UnityChatSDK.Instance.SetVideoCaptureType(type, captureCamera);
    }

    public void SetResolution180P()
    {
        UnityChatSDK.Instance.SetResolution(VideoResolution._180P);
    }
    public void SetResolution360P()
    {
        UnityChatSDK.Instance.SetResolution(VideoResolution._360P);
    }
    public void SetResolution720P()
    {
        UnityChatSDK.Instance.SetResolution(VideoResolution._720P);
    }
    bool audioEnable;
    public void SetAudioEnable()
    {
        UnityChatSDK.Instance.SetAudioEnable(audioEnable);
        audioEnable = !audioEnable;
    }
    bool videoEnable; 
    public void SetVideoEnable()
    {
        UnityChatSDK.Instance.SetVideoEnable(videoEnable);
        videoEnable = !videoEnable;
    }
    /// <summary>
    /// 当外部可用摄像头的数量>2时，如手机端前后摄像头,改变要捕捉的外部摄像头
    /// </summary>
    public void ChangeDeviceCam()
    {
        UnityChatSDK.Instance.ChangeCam();
    }
    /// <summary>
    /// 设置视频采集类型为外部摄像头捕捉的画面
    /// </summary>
    public void SetDeciveCam()
    {
        SetVideoCaptureType(VideoType.DeviceCamera, null);
    }
    /// <summary>
    /// 设置视频采集类型为Unity Camera渲染的画面
    /// </summary>
    public void SetUnityCam()
    {
        SetVideoCaptureType(VideoType.UnityCamera, CaptureCamera);
    }
    private void FixedUpdate()
    {
        if (ChatDataHandler.Instance.IsStartChat)
        {
            if (SelfRawImage!=null && SelfRawImage.transform.GetComponent<RectTransform>().rect.size != SelfRawImage.texture.texelSize)
            {
                SelfRawImage.GetComponent<AspectRatioFitter>().aspectRatio = (SelfRawImage.texture.width + 0.0f) / SelfRawImage.texture.height;
            }
            if (ChatPeerRawImage!=null && ChatPeerRawImage.transform.GetComponent<RectTransform>().rect.size != ChatPeerRawImage.texture.texelSize)
            {
                ChatPeerRawImage.GetComponent<AspectRatioFitter>().aspectRatio = (ChatPeerRawImage.texture.width + 0.0f) / ChatPeerRawImage.texture.height;
            }
        }
    }
}
