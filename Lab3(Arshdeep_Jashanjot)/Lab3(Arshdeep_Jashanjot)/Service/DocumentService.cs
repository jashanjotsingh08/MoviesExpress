using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_Arshdeep_Jashanjot_.Service
{
    public class DocumentService
    {
        private const string bucketName = "moviesnet";
        private const string accessKey = "AKIAZRP7NAAVXIVECSX4";
        private const string secretKey = "SdfBccnRhVvstVe2NaR/Y3L2jCbIwrv19d/Gw3JV";

        // Specify your bucket region here
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        private static IAmazonS3 s3Client;

  
        public async Task<bool> UploadDocument(Stream stream, string fileName)
        {
            try
            {
                s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
                TransferUtility transferUtility = new TransferUtility(s3Client);
                //Set partSize to 5 MB
                long partSize = 5 * 1024 * 1024;
                TransferUtilityUploadRequest transferUtilityUploadRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = stream,
                    StorageClass = S3StorageClass.Standard,
                    CannedACL = S3CannedACL.Private,
                    PartSize = partSize,
                    Key = fileName
                };

                await transferUtility.UploadAsync(transferUtilityUploadRequest);
                transferUtility.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //Log exception
                return false;
            }
        }

        public async Task<bool> DownloadDocument(string fileName)
        {
            try
            {
                string directoryPath = @"D:\AWSDocuments\";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                };

                using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
                {
                    string filePath = directoryPath + fileName;
                    await response.WriteResponseStreamToFileAsync(filePath, false, CancellationToken.None);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Log exception
            }
            return false;
        }
    }
}
