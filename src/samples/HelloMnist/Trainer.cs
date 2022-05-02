// Copyright © Aaron Sun and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Vortice.Direct3D12;
using Vortice.Direct3D12.Debug;
using Vortice.DirectML;
using Vortice.DXGI;

namespace HelloMnist;
internal class Trainer : IDisposable
{
    public IDXGIFactory4 DXGIFactory;
    public ID3D12Device2 D3D12Device;
    public ID3D12CommandQueue D3D12CommandQueue;
    public ID3D12CommandAllocator D3D12CommandAllocator;
    public ID3D12GraphicsCommandList4 D3D12CommandList;
    public IDMLDevice DMLDevice;

    public ID3D12Fence D3D12Fence;
    public EventWaitHandle FenceWaitHandle = new(false, EventResetMode.AutoReset);
    public ulong FenceValue = 0;

    public IdxFile TrainingData;
    public IdxFile TrainingLabels;
    public IdxFile TestData;
    public IdxFile TestLabels;

    public UploadBuffer<byte> TrainingDataBuffer;
    public UploadBuffer<byte> TrainingLabelsBuffer;
    public UploadBuffer<byte> TestDataBuffer;
    public UploadBuffer<byte> TestLabelsBuffer;

    public int[] Layers = new int[] { 300 }; // Prepended with input size, postpended with output size.


    public Trainer(IdxFile trainingData, IdxFile trainingLabels, IdxFile testData, IdxFile testLabels)
    {
        TrainingData = trainingData;
        TrainingLabels = trainingLabels;
        TestData = testData;
        TestLabels = testLabels;
    }

    public void Initialize()
    {

        if (!IsSupported())
        {
            throw new InvalidOperationException("Direct3D12 is not supported on current OS");
        }

        var validation = false;

        if (D3D12.D3D12GetDebugInterface(out ID3D12Debug? debug).Success)
        {
            debug!.EnableDebugLayer();
            debug!.Dispose();
            validation = true;
        }

        DXGIFactory = DXGI.CreateDXGIFactory2<IDXGIFactory4>(validation);

        for (int adapterIndex = 0; DXGIFactory.EnumAdapters1(adapterIndex, out IDXGIAdapter1 adapter).Success; adapterIndex++)
        {
            AdapterDescription1 desc = adapter.Description1;

            // Don't select the Basic Render Driver adapter.
            if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
            {
                adapter.Dispose();

                continue;
            }

            if (D3D12.D3D12CreateDevice(adapter, Vortice.Direct3D.FeatureLevel.Level_11_0, out D3D12Device).Success)
            {
                adapter.Dispose();

                break;
            }
        }

        if (D3D12Device == null)
        {
            throw new InvalidOperationException("Direct3D12 device could not be created");
        }

        var commandQueueDesc = new CommandQueueDescription
        {
            Type = CommandListType.Direct,
            Flags = CommandQueueFlags.None,
        };

        D3D12CommandQueue = D3D12Device.CreateCommandQueue(commandQueueDesc);
        D3D12CommandAllocator = D3D12Device.CreateCommandAllocator(CommandListType.Direct);
        D3D12CommandList = D3D12Device.CreateCommandList<ID3D12GraphicsCommandList4>(CommandListType.Direct, D3D12CommandAllocator);
        D3D12Fence = D3D12Device.CreateFence();

        var createFlags = CreateDeviceFlags.Debug;
        DML.DMLCreateDevice(D3D12Device, createFlags, out DMLDevice);

        var fl = DMLDevice.CheckFeatureLevelsSupport(FeatureLevel.Level1_0, FeatureLevel.Level3_1, FeatureLevel.Level3_0, FeatureLevel.Level2_0, FeatureLevel.Level4_0);
        Console.WriteLine(fl);

        // Load data.
        TrainingDataBuffer = new(this, TrainingData.Values);
        TrainingLabelsBuffer = new(this, TrainingLabels.Values);
        TestDataBuffer = new(this, TestData.Values);
        TestLabelsBuffer = new(this, TestLabels.Values);

        // Upload buffers.
        CloseExecuteResetWait();

        // Create graph.
    }

    public void TrainEpoch()
    {

    }

    public void TestError()
    {

    }


    public void CloseExecuteResetWait()
    {
        D3D12CommandList.Close();
        D3D12CommandQueue.ExecuteCommandList(D3D12CommandList);

        D3D12CommandQueue.Signal(D3D12Fence, ++FenceValue);
        D3D12Fence.SetEventOnCompletion(1, FenceWaitHandle);

        FenceWaitHandle.WaitOne();

        D3D12CommandAllocator.Reset();
        D3D12CommandList.Reset(D3D12CommandAllocator);
    }

    public void Dispose()
    {
        TrainingDataBuffer.Dispose();
        TrainingLabelsBuffer.Dispose();
        TestDataBuffer.Dispose();
        TestLabelsBuffer.Dispose();

        D3D12CommandList.Dispose();
        D3D12CommandAllocator.Dispose();
        D3D12CommandQueue.Dispose();

        DMLDevice.Dispose();
        D3D12Device.Dispose();
        DXGIFactory.Dispose();
    }

    public bool IsSupported() => D3D12.IsSupported(Vortice.Direct3D.FeatureLevel.Level_12_0);
}
