// Copyright © Aaron Sun, Amer Koleci, and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.DirectML;

public partial struct Slice1OperatorDescription : IOperatorDescription, IOperatorDescriptionMarshal
{
    public OperatorType OperatorType => OperatorType.Slice1;

    public TensorDescription InputTensor { get; set; }

    public TensorDescription OutputTensor { get; set; }

    public uint DimensionCount { get; set; }

    public uint[] InputWindowOffsets { get; set; }

    public uint[] InputWindowSizes { get; set; }

    public int[] InputWindowStrides { get; set; }

    #region Marshal
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    internal struct __Native
    {
        public IntPtr InputTensor;
        public IntPtr OutputTensor;
        public uint DimensionCount;
        public IntPtr InputWindowOffsets;
        public IntPtr InputWindowSizes;
        public IntPtr InputWindowStrides;
    }

    unsafe IntPtr IOperatorDescriptionMarshal.__MarshalAlloc()
    {
        __Native* @ref = UnsafeUtilities.Alloc<__Native>();

        @ref->InputTensor = InputTensor.__MarshalAlloc();
        @ref->OutputTensor = OutputTensor.__MarshalAlloc();
        @ref->DimensionCount = DimensionCount;
        @ref->InputWindowOffsets = new(UnsafeUtilities.AllocWithData(InputWindowOffsets));
        @ref->InputWindowSizes = new(UnsafeUtilities.AllocWithData(InputWindowSizes));
        @ref->InputWindowStrides = new(UnsafeUtilities.AllocWithData(InputWindowStrides));

        return new(@ref);
    }

    unsafe void IOperatorDescriptionMarshal.__MarshalFree(ref IntPtr pDesc)
    {
        var @ref = (__Native*)pDesc;

        InputTensor.__MarshalFree(ref @ref->InputTensor);
        OutputTensor.__MarshalFree(ref @ref->OutputTensor);
           UnsafeUtilities.Free(@ref->InputWindowOffsets);
           UnsafeUtilities.Free(@ref->InputWindowSizes);
           UnsafeUtilities.Free(@ref->InputWindowStrides);

        UnsafeUtilities.Free(@ref);
    }
    #endregion

    public static implicit operator OperatorDescription(Slice1OperatorDescription description)
    {
        return new(description);
    }
}
