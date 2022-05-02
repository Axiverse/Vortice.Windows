// Copyright © Aaron Sun and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Vortice.Direct3D12;

namespace HelloMnist;

internal class ReadbackBuffer<T> : IDisposable
    where T : unmanaged
{
    public Trainer Trainer;
    public int Count;
    public ID3D12Resource Resource;
    public ID3D12Resource ReadbackResource;

    public unsafe ReadbackBuffer(Trainer trainer, int count)
    {
        Trainer = trainer;
        Count = count;

        var size = (ulong)(count * sizeof(T));
        Resource = trainer.D3D12Device.CreateCommittedResource(
            HeapProperties.DefaultHeapProperties, HeapFlags.None, ResourceDescription.Buffer(size, ResourceFlags.AllowUnorderedAccess), ResourceStates.UnorderedAccess);
        ReadbackResource = trainer.D3D12Device.CreateCommittedResource(
            HeapProperties.ReadbackHeapProperties, HeapFlags.None, ResourceDescription.Buffer(size), ResourceStates.CopyDest);
    }

    public T[] Download(Trainer trainer, int count)
    {
        if (count > Count) throw new ArgumentOutOfRangeException(nameof(count));

        trainer.D3D12CommandList.ResourceBarrierTransition(Resource, ResourceStates.UnorderedAccess, ResourceStates.CopySource);
        trainer.D3D12CommandList.CopyResource(ReadbackResource, Resource);

        trainer.CloseExecuteResetWait();

        trainer.D3D12CommandList.ResourceBarrierTransition(Resource, ResourceStates.CopySource, ResourceStates.UnorderedAccess);

        var data = new T[count];
        ReadbackResource.GetData<T>(data);

        return data;
    }

    public void Dispose()
    {
        Resource.Dispose();
        ReadbackResource.Dispose();
    }
}
