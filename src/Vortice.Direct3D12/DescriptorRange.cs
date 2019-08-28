﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.Direct3D12
{
    /// <summary>
    /// Describes a descriptor range.
    /// </summary>
    public partial struct DescriptorRange
    {
        public const int Append = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptorRange"/> struct.
        /// </summary>
        /// <param name="rangeType">A <see cref="DescriptorRangeType"/> value that specifies the type of descriptor range.</param>
        /// <param name="numDescriptors">The number of descriptors in the range. Use -1 or UINT_MAX to specify unbounded size. Only the last entry in a table can have unbounded size.</param>
        /// <param name="baseShaderRegister">The base shader register in the range. For example, for shader-resource views (SRVs), 3 maps to ": register(t3);" in HLSL.</param>
        /// <param name="registerSpace">The register space. Can typically be 0, but allows multiple descriptor arrays of unknown size to not appear to overlap. For example, for SRVs, by extending the example in the BaseShaderRegister member description, 5 maps to ": register(t3,space5);" in HLSL.</param>
        /// <param name="offsetInDescriptorsFromTableStart">The offset in descriptors from the start of the root signature. This value can be <see cref="Append"/>, which indicates this range should immediately follow the preceding range.</param>
        public DescriptorRange(DescriptorRangeType rangeType, int numDescriptors, int baseShaderRegister, int registerSpace = 0, int offsetInDescriptorsFromTableStart = Append)
        {
            RangeType = rangeType;
            NumDescriptors = numDescriptors;
            BaseShaderRegister = baseShaderRegister;
            RegisterSpace = registerSpace;
            OffsetInDescriptorsFromTableStart = offsetInDescriptorsFromTableStart;
        }
    }
}
