// Copyright © Aaron Sun, Amer Koleci, and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.DirectML;

public partial struct TopK1OperatorDescription : IOperatorDescription, IOperatorDescriptionMarshal
{
    public OperatorType OperatorType => OperatorType.TopK1;

    public TensorDescription InputTensor { get; set; }

    public TensorDescription OutputValueTensor { get; set; }

    public TensorDescription OutputIndexTensor { get; set; }

    public uint Axis { get; set; }

    public uint K { get; set; }

    public AxisDirection AxisDirection { get; set; }

    #region Marshal
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    internal struct __Native
    {
        public IntPtr InputTensor;
        public IntPtr OutputValueTensor;
        public IntPtr OutputIndexTensor;
        public uint Axis;
        public uint K;
        public AxisDirection AxisDirection;
    }

    unsafe IntPtr IOperatorDescriptionMarshal.__MarshalAlloc()
    {
        __Native* @ref = UnsafeUtilities.Alloc<__Native>();

        @ref->InputTensor = InputTensor.__MarshalAlloc();
        @ref->OutputValueTensor = OutputValueTensor.__MarshalAlloc();
        @ref->OutputIndexTensor = OutputIndexTensor.__MarshalAlloc();
        @ref->Axis = Axis;
        @ref->K = K;
        @ref->AxisDirection = AxisDirection;

        return new(@ref);
    }

    unsafe void IOperatorDescriptionMarshal.__MarshalFree(ref IntPtr pDesc)
    {
        var @ref = (__Native*)pDesc;

        InputTensor.__MarshalFree(ref @ref->InputTensor);
        OutputValueTensor.__MarshalFree(ref @ref->OutputValueTensor);
        OutputIndexTensor.__MarshalFree(ref @ref->OutputIndexTensor);

        UnsafeUtilities.Free(@ref);
    }
    #endregion

    public static implicit operator OperatorDescription(TopK1OperatorDescription description)
    {
        return new(description);
    }
}
