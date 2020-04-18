using UnityEngine;
using UnityEngine.UI;
public class LifeGame : MonoBehaviour
{
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] ComputeShader computeShader;
    private RenderTexture stateTexture, drawTexture;

    private int kernelInitialize, kernelUpdate, kernelDraw;
    private ThreadSize threadSizeInitialize, threadSizeUpdate, threadSizeDraw;

    struct ThreadSize
    {
        public int x;
        public int y;
        public int z;

        public ThreadSize(uint x, uint y, uint z)
        {
            this.x = (int)x;
            this.y = (int)y;
            this.z = (int)z;
        }
    }

    private void LifeGameStart()
    {
        // カーネルIdの取得
        kernelInitialize = computeShader.FindKernel("Initialize");
        kernelUpdate = computeShader.FindKernel("Update");
        kernelDraw = computeShader.FindKernel("Draw");

        // 状態を格納するテクスチャの作成
        stateTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.RInt);
        stateTexture.wrapMode = TextureWrapMode.Repeat;
        stateTexture.enableRandomWrite = true;
        stateTexture.Create();
        // レンダリング用のテクスチャの取得
        drawTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
        drawTexture.filterMode = FilterMode.Point;
        drawTexture.enableRandomWrite = true;
        drawTexture.Create();

        // スレッド数の取得
        uint threadSizeX, threadSizeY, threadSizeZ;
        computeShader.GetKernelThreadGroupSizes(kernelInitialize, out threadSizeX, out threadSizeY, out threadSizeZ);
        threadSizeInitialize = new ThreadSize(threadSizeX, threadSizeY, threadSizeZ);
        computeShader.GetKernelThreadGroupSizes(kernelUpdate, out threadSizeX, out threadSizeY, out threadSizeZ);
        threadSizeUpdate = new ThreadSize(threadSizeX, threadSizeY, threadSizeZ);
        computeShader.GetKernelThreadGroupSizes(kernelDraw, out threadSizeX, out threadSizeY, out threadSizeZ);
        threadSizeDraw = new ThreadSize(threadSizeX, threadSizeY, threadSizeZ);

        // LifeGameの状態の初期化
        computeShader.SetFloat("keikaTime", Time.time);
        computeShader.SetTexture(kernelInitialize, "stateTexture", stateTexture);
        computeShader.Dispatch(kernelInitialize, Mathf.CeilToInt(stateTexture.width / threadSizeInitialize.x), Mathf.CeilToInt(stateTexture.height / threadSizeInitialize.y), 1);
    }

    private void LifeGameUpdate()
    {
        // LifeGameの状態の更新
        computeShader.SetFloat("keikaTime", Time.time);
        computeShader.SetTexture(kernelUpdate, "stateTexture", stateTexture);
        computeShader.Dispatch(kernelUpdate, Mathf.CeilToInt(stateTexture.width / threadSizeUpdate.x), Mathf.CeilToInt(stateTexture.height / threadSizeUpdate.y), 1);

        // LifeGameの状態をもとにレンダリング用のテクスチャを作成
        computeShader.SetTexture(kernelDraw, "stateTexture", stateTexture);
        computeShader.SetTexture(kernelDraw, "drawTexture", drawTexture);
        computeShader.Dispatch(kernelDraw, Mathf.CeilToInt(stateTexture.width / threadSizeDraw.x), Mathf.CeilToInt(stateTexture.height / threadSizeDraw.y), 1);
        _renderer.material.mainTexture = drawTexture;
    }
}