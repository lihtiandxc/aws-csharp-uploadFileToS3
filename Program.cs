using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Util;

namespace TestConsoleApp
{
    class Program
    {
        private const string bucketName = "<bucket_name>"; // "This name will be given by AWS admin" e.g: MyBucketName
        private const string keyName = "<file_name_to_be_saved_in_S3>"; // e.g: MyFlatFile.csv
        private const string filePath = "<file_path>"; // absolute file path e.g: string = @"C:\temp\Myestimate.csv"
        private static readonly Amazon.RegionEndpoint bucketRegion = Amazon.RegionEndpoint.APSoutheast1; // Region where the S3 Bucket located AWS admin has the info. Default is Singapore APSoutheast1
        private static IAmazonS3 s3Client; 
        private const string accessKey = "<AWS programmatic access key>"; // The IAM user with the programmatic access and enough policy to put file into S3 Bucket. For testing purpose, you may request AWS admin to assign S3 full permission on the specify bucket that you want to upload the file
        private const string secretKey = "<AWS programmatic secret key>";
        
        public static void Main()
        {
            s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
            UploadFileAsync().Wait();
        }

        private static async Task UploadFileAsync()
        {
            try
            {
                bool bucketExists = await AmazonS3Util.DoesS3BucketExistAsync(s3Client, bucketName);

                if (!bucketExists)
                {
                    // you can implement logging if needed
                    Console.WriteLine("Bucket doesn't exist! Please verify with AWS Cloudops administrator.");
                }

                var fileTransferUtility = new TransferUtility(s3Client);
                Console.WriteLine("Uploading in progress");
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                Console.WriteLine("Upload completed");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server.Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}
