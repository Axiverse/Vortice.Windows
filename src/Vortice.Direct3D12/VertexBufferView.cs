﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Direct3D12
{
    /// <summary>
    /// Describes a vertex buffer view.
    /// </summary>
    public partial struct VertexBufferView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBufferView"/> struct.
        /// </summary>
        /// <param name="bufferLocation">Specifies a gpu virtual address that identifies the address of the buffer.</param>
        /// <param name="sizeInBytes">Specifies the size in bytes of the buffer.</param>
        /// <param name="strideInBytes">Specifies the size in bytes of each vertex entry.</param>
        public VertexBufferView(long bufferLocation, int sizeInBytes, int strideInBytes)
        {
            BufferLocation = bufferLocation;
            SizeInBytes = sizeInBytes;
            StrideInBytes = strideInBytes;
        }
    }
}
