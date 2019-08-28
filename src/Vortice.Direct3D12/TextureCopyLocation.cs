﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Vortice.Direct3D12
{
    /// <summary>
    /// Describes a portion of a texture for the purpose of texture copies.
    /// </summary>
    public partial struct TextureCopyLocation
    {
        public TextureCopyType CopyType
        {
            get => _type;
            set => _type = value;
        }

        public PlacedSubresourceFootPrint PlacedFootPrint
        {
            get => _union.PlacedFootprint;
            set => _union.PlacedFootprint = value;
        }

        public int SubresourceIndex
        {
            get => _union.SubResourceIndex;
            set => _union.SubResourceIndex = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureCopyLocation"/> struct.
        /// </summary>
        /// <param name="resource">Instance of <see cref="ID3D12Resource"/></param>
        /// <param name="subResourceIndex">Sub resource index.</param>
        public TextureCopyLocation(ID3D12Resource resource, int subResourceIndex)
            : this()
        {
            Guard.NotNull(resource, nameof(resource));

            _type = TextureCopyType.SubresourceIndex;
            _resource = resource != null ? resource.NativePointer : IntPtr.Zero;
            _union.SubResourceIndex = subResourceIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureCopyLocation"/> struct.
        /// </summary>
        /// <param name="resource">Instance of <see cref="ID3D12Resource"/></param>
        /// <param name="placedFootprint">Placed foot print.</param>
        public TextureCopyLocation(ID3D12Resource resource, PlacedSubresourceFootPrint placedFootprint)
            : this()
        {
            Guard.NotNull(resource, nameof(resource));

            _type = TextureCopyType.PlacedFootPrint;
            _resource = resource != null ? resource.NativePointer : IntPtr.Zero;
            _union.PlacedFootprint = placedFootprint;
        }

        #region Marshal
        private IntPtr _resource;
        private TextureCopyType _type;
        private Union _union;

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct Union
        {
            [FieldOffset(0)]
            public PlacedSubresourceFootPrint PlacedFootprint;

            [FieldOffset(0)]
            public int SubResourceIndex;
        }
        #endregion
    }
}
