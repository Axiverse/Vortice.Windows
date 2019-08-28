﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using SharpGen.Runtime;

namespace Vortice.Direct3D12
{
    public partial class ID3D12CommandQueue
    {
        public unsafe void ExecuteCommandList(ID3D12CommandList commandList)
        {
            Guard.NotNull(commandList, nameof(commandList));
            var ptr = commandList.NativePointer;
            ExecuteCommandLists(1, new IntPtr(&ptr));
        }

        public unsafe void ExecuteCommandLists(params ID3D12CommandList[] commandLists)
        {
            Guard.MustBeGreaterThan(commandLists.Length, 0, nameof(commandLists));

            var commandListsPtr = (IntPtr*)0;

            int count = commandLists.Length;
            IntPtr* tempPtr = stackalloc IntPtr[count];
            commandListsPtr = tempPtr;
            for (int i = 0; i < count; i++)
            {
                commandListsPtr[i] = (commandLists[i] == null) ? IntPtr.Zero : commandLists[i].NativePointer;
            }

            ExecuteCommandLists(count, new IntPtr(commandListsPtr));
        }

        public unsafe void ExecuteCommandLists(int count, ID3D12CommandList[] commandLists)
        {
            Guard.MustBeGreaterThan(count, 0, nameof(count));
            Guard.MustBeGreaterThan(commandLists.Length, 0, nameof(commandLists));
            var commandListsPtr = (IntPtr*)0;

            count = commandLists.Length;
            IntPtr* tempPtr = stackalloc IntPtr[count];
            commandListsPtr = tempPtr;
            for (int i = 0; i < count; i++)
            {
                commandListsPtr[i] = (commandLists[i] == null) ? IntPtr.Zero : commandLists[i].NativePointer;
            }

            ExecuteCommandLists(count, new IntPtr(commandListsPtr));
        }

        public void BeginEvent(string name)
        {
            Guard.NotNullOrEmpty(name, nameof(name));

            var handle = IntPtr.Zero;
            try
            {
                handle = Marshal.StringToHGlobalUni(name);
                BeginEvent(1, handle, name.Length);
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(handle);
                    handle = IntPtr.Zero;
                }
            }
        }

        public void SetMarker(string name)
        {
            Guard.NotNullOrEmpty(name, nameof(name));

            var handle = IntPtr.Zero;
            try
            {
                handle = Marshal.StringToHGlobalUni(name);
                SetMarker(1, handle, name.Length);
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(handle);
                    handle = IntPtr.Zero;
                }
            }
        }

        public void UpdateTileMappings(
            ID3D12Resource resource, 
            TiledResourceCoordinate[] resourceRegionStartCoordinates,
            TileRegionSize[] resourceRegionSizes,
            ID3D12Heap heap,
            TileRangeFlags[] rangeFlags, 
            int[] heapRangeStartOffsets, 
            int[] rangeTileCounts,
            TileMappingFlags flags = TileMappingFlags.None)
        {
            Guard.NotNull(resource, nameof(resource));

            UpdateTileMappings(resource,
                resourceRegionStartCoordinates.Length,
                resourceRegionStartCoordinates,
                resourceRegionSizes,
                heap,
                rangeFlags.Length,
                rangeFlags,
                heapRangeStartOffsets,
                rangeTileCounts,
                flags);
        }
    }
}
