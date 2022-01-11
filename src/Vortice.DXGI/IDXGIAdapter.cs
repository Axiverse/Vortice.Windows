// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.DXGI;

public partial class IDXGIAdapter
{
    /// <summary>
    /// Get an instance of <see cref="IDXGIAdapter"/> or null if not found.
    /// </summary>
    /// <remarks>
    /// Make sure to dispose the <see cref="IDXGIAdapter"/> instance.
    /// </remarks>
    /// <param name="index">The index to get from.</param>
    /// <returns>Instance of <see cref="IDXGIOutput"/> or null if not found.</returns>
    public IDXGIOutput GetOutput(int index)
    {
        EnumOutputs(index, out IDXGIOutput output).CheckError();
        return output;
    }

    public bool CheckInterfaceSupport<T>() where T : ComObject
    {
        return CheckInterfaceSupport(typeof(T), out _);
    }

    public bool CheckInterfaceSupport<T>(out long userModeVersion) where T : ComObject
    {
        return CheckInterfaceSupport(typeof(T), out userModeVersion);
    }

    public bool CheckInterfaceSupport(Type type, out long userModeDriverVersion)
    {
        return CheckInterfaceSupport(type.GUID, out userModeDriverVersion).Success;
    }
}
