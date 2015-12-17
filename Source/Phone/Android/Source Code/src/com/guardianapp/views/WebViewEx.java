package com.guardianapp.views;

import java.util.Map;

import android.content.Context;
import android.util.AttributeSet;
import android.webkit.URLUtil;
import android.webkit.WebView;

public class WebViewEx extends WebView
{

    public WebViewEx(Context context)
    {
        super(context);
        init(context);
    }

    public WebViewEx(Context context, AttributeSet attrs)
    {
        super(context, attrs);
        init(context);
    }

    public WebViewEx(Context context, AttributeSet attrs, int defStyle)
    {
        super(context, attrs, defStyle);
        init(context);
    }


    private void init(Context context)
    {
        mCacheRootPath = getDefaultCacheRootPath(context);
    }

    public void loadDataWithBaseURL(String baseUrl, String data, String mimeType, String encoding, String historyUrl)
    {
        super.loadDataWithBaseURL(getLoadUrl(baseUrl), data, mimeType, encoding, historyUrl);
    }

    public void loadUrl(String url)
    {
        super.loadUrl(getLoadUrl(url));
    }

    public void loadUrl(String url, Map extraHeaders)
    {
        super.loadUrl(getLoadUrl(url), extraHeaders);
    }

    public String getLoadUrl(String url)
    {
        if(isAffectedAssetUrl(url))
            return getCacheUrlFromAssetUrl(url);
        else
            return url;
    }

    public String getNonCacheUrl(String url)
    {
        if(isCacheUrl(url))
            return getAssetUrlFromCacheUrl(url);
        else
            return url;
    }

    public boolean isCacheUrl(String url)
    {
        return isAffectedOsVersion() && url != null && url.startsWith(getCacheRootUrl());
    }

    public static boolean isAffectedAssetUrl(String url)
    {
        return isAffectedOsVersion() && url != null && URLUtil.isAssetUrl(url);
    }

    public static boolean isAffectedOsVersion()
    {
        return android.os.Build.VERSION.SDK_INT >= 11 && android.os.Build.VERSION.SDK_INT < 16;
    }

    public String getCacheUrlFromAssetUrl(String url)
    {
        return url.replaceFirst("file:///android_asset/", getCacheRootUrl());
    }

    public String getAssetUrlFromCacheUrl(String url)
    {
        return url.replaceFirst(getCacheRootUrl(), "file:///android_asset/");
    }

    public String getCacheRootPath()
    {
        return mCacheRootPath;
    }

    public void setCacheRootPath(String cacheRootPath)
    {
        mCacheRootPath = cacheRootPath;
    }

    public String getCacheRootUrl()
    {
        return (new StringBuilder("file://")).append(getCacheRootPath()).toString();
    }

    public static String getDefaultCacheRootPath(Context context)
    {
        return (new StringBuilder("/data/data/")).append(context.getPackageName()).append("/webviewfix/").toString();
    }

    private static final String ANDROID_ASSET = "file:///android_asset/";
    private static final int SDK_INT_JELLYBEAN = 16;
    private String mCacheRootPath;
}