/// Copyright © Aaron Sun and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Vortice.Direct3D12;

namespace HelloMnist;

internal class UploadBuffer<T> : IDisposable
    where T : unmanaged
{
    public Trainer Trainer;
    public int Count;
    public ID3D12Resource Resource;
    public ID3D12Resource UploadResource;

    public UploadBuffer(Trainer trainer, T[] data) : this(trainer, data.Length)
    {
        Upload(data);
    }

    public unsafe UploadBuffer(Trainer trainer, int count)
    {
        Trainer = trainer;
        Count = count;

        var size = (uint)(count * sizeof(T));
        Resource = trainer.D3D12Device.CreateCommittedResource(
            HeapProperties.DefaultHeapProperties, HeapFlags.None, ResourceDescription.Buffer(size, ResourceFlags.AllowUnorderedAccess), ResourceStates.CopyDest);
        UploadResource = trainer.D3D12Device.CreateCommittedResource(
            HeapProperties.UploadHeapProperties, HeapFlags.None, ResourceDescription.Buffer(size), ResourceStates.GenericRead);
    }

    public unsafe void Upload(T[] data)
    {
        if (data.Length > Count) throw new ArgumentOutOfRangeException(nameof(data));

        UploadResource.SetData(data);

        Trainer.D3D12CommandList.CopyResource(Resource, UploadResource);
        Trainer.D3D12CommandList.ResourceBarrierTransition(Resource, ResourceStates.CopyDest, ResourceStates.UnorderedAccess);
    }

    public void Dispose()
    {
        Resource.Dispose();
        UploadResource.Dispose();
    }
}
