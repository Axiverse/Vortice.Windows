﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

namespace Vortice.DirectX.Direct2D
{
    /// <summary>
    /// Describes the pixel format and dpi of a <see cref="ID2D1Bitmap"/>.
    /// </summary>
    public partial struct BitmapProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapProperties"/> struct.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        public BitmapProperties(PixelFormat pixelFormat)
        {
            PixelFormat = pixelFormat;
            DpiX = 96.0f;
            DpiY = 96.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapProperties"/> struct.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <param name="dpiX">The bitmap dpi in the x direction.</param>
        /// <param name="dpiY">The bitmap dpi in the y direction.</param>
        public BitmapProperties(PixelFormat pixelFormat, float dpiX, float dpiY)
        {
            PixelFormat = pixelFormat;
            DpiX = dpiX;
            DpiY = dpiY;
        }
    }
}
