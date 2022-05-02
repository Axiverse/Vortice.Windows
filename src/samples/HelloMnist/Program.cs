// Copyright © Aaron Sun and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.IO.Compression;
using System.Net;

namespace HelloMnist;

internal static class Program
{
    /// <summary>
    /// http://yann.lecun.com/exdb/mnist/
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        DownloadAndExtract("https://storage.googleapis.com/cvdf-datasets/mnist/train-images-idx3-ubyte.gz", "train-images-idx3-ubyte.bin");
        DownloadAndExtract("https://storage.googleapis.com/cvdf-datasets/mnist/train-labels-idx1-ubyte.gz", "train-labels-idx1-ubyte.bin");
        DownloadAndExtract("https://storage.googleapis.com/cvdf-datasets/mnist/t10k-images-idx3-ubyte.gz", "t10k-images-idx3-ubyte.bin");
        DownloadAndExtract("https://storage.googleapis.com/cvdf-datasets/mnist/t10k-labels-idx1-ubyte.gz", "t10k-labels-idx1-ubyte.bin");

        var trainingData = new IdxFile("train-images-idx3-ubyte.bin");
        var trainingLabels = new IdxFile("train-labels-idx1-ubyte.bin");
        var testData = new IdxFile("t10k-images-idx3-ubyte.bin");
        var testLabels = new IdxFile("t10k-labels-idx1-ubyte.bin");

        using var trainer = new Trainer(trainingData, trainingLabels, testData, testLabels);
        
        var running = true;
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("Termination requested.");
            running = false;
            e.Cancel = true;
        };

        Console.WriteLine("Initializing trainer.");

        trainer.Initialize();

        while (running)
        {
            Console.WriteLine("Training neural network.");
            trainer.TrainEpoch();

            Console.WriteLine("Testing neural network.");
            trainer.TestError();
        }

        Console.WriteLine("Terminating.");
    }

    static void DownloadAndExtract(string url, string fileName)
    {
        if (File.Exists(fileName))
        {
            Console.WriteLine($"{fileName} exists, skipping download.");
            return;
        }

        Console.WriteLine($"Downloading {fileName} from {url}.");

        using var client = new WebClient();
        var data = client.DownloadData(url);

        Console.WriteLine($"Extracting {fileName}.");

        using var inStream = new MemoryStream(data);
        using var decompStream= new GZipStream(inStream, CompressionMode.Decompress);
        using var outStream = File.Create(fileName);

        decompStream.CopyTo(outStream);

        Console.WriteLine($"Completed downloading {fileName}.");
    }
}
